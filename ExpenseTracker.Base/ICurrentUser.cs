
namespace ExpenseTracker.Base
{
    public interface ICurrentUser
    {
        public Guid Id { get; }
        public string UserName { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public UserRoles Role { get; }
    }
}
