using EasyNetQ;
using EasyNetQ.Topology;
using ExpenseTracker.Base.Consumers;
using ExpenseTracker.Business.Services;
using ExpenseTracker.Persistence;
using ExpenseTracker.Persistence.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpenseTracker.Business.Consumers;

public class ApprovedExpenseConsumer : BackgroundService
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;
    private Queue approvedExpenseEventQueue;

    public ApprovedExpenseConsumer(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var exchange = await _bus.Advanced.ExchangeDeclareAsync("expense_tracker", "topic", true, false);
        approvedExpenseEventQueue = await _bus.Advanced.QueueDeclareAsync("approved_expense_subscriber", true, false, false);
        await _bus.Advanced.BindAsync(exchange, approvedExpenseEventQueue, "approved_expense");

        await base.StartAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.Advanced.Consume(approvedExpenseEventQueue, async (body, propertis, info) =>
        {
            var message = System.Text.Json.JsonSerializer.Deserialize<ApprovedExpenseConsumerModel>(body.Span);

            var refNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);
            var feeAmount = message.Amount * 0.02m;

            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                var accountService = scope.ServiceProvider.GetService<IAccountService>();

                var currentUser = await unitOfWork.UserRepository.GetByIdAsync(message.ApprovedUserId, "Account");

                var responseOutgoing = await accountService.CreateOutgoingAccountTransaction(currentUser.Account.AccountNumber, message.Amount, feeAmount, "Masraf onaylandı", refNumber);
                if (!responseOutgoing.Success)
                {
                    throw new Exception(responseOutgoing.Message);
                }

                var expenseOwnerUser = await unitOfWork.UserRepository.GetByIdAsync(message.ExpenseOwnerUserId, "Account");
                var responseIncoming = await accountService.CreateIncomingAccountTransaction(expenseOwnerUser.Account.AccountNumber, message.Amount, "Masraf onaylandı", refNumber);
                if (!responseIncoming.Success)
                {
                    throw new Exception(responseIncoming.Message);
                }

                await unitOfWork.MoneyTransferRepository.AddAsync(new MoneyTransfer()
                {
                    Id = Guid.NewGuid(),
                    Amount = message.Amount,
                    FeeAmount = feeAmount,
                    FromAccountId = currentUser.Account.Id,
                    ToAccountId = expenseOwnerUser.Account.Id,
                    ReferenceNumber = refNumber,
                    TransactionDate = DateTimeOffset.UtcNow,
                    InsertedDate = DateTimeOffset.UtcNow,
                    InsertedUser = message.ApprovedUserId,
                });

                await unitOfWork.CompleteAsync();
            }
        });
        return Task.CompletedTask;
    }
}
