
using System.Text.Json.Serialization;

namespace ExpenseTracker.Base;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRoles
{
    Admin,
    Personel
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExpenseStatus
{
    Pending,
    Approved,
    Rejected
}