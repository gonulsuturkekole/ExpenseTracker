
using System.Text.Json.Serialization;

namespace ExpenseTracker.Base;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRoles
{
    Admin,
    Personel
}
