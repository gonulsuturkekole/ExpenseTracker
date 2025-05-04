using ExpenseTracker.Base;

namespace ExpenseTracker.Schema;

public class AccountRequest : BaseRequest
{
    public long CustomerId { get; set; }
    public string Name { get; set; }
    public string CurrencyCode { get; set; }
    public string IBAN { get; set; } 
}

public class AccountResponse : BaseResponse
{
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int CustomerNumber { get; set; }
    public string Name { get; set; }
    public int AccountNumber { get; set; }
    public string IBAN { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyCode { get; set; }
    public DateTime OpenDate { get; set; }
    public DateTime? CloseDate { get; set; }

   
   //public virtual List<AccountTransactionResponse> AccountTransactions { get; set; }
}