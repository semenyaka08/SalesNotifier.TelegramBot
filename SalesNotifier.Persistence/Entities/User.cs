namespace SalesNotifier.Persistence.Entities;

public class User
{
    public long Id { get; set; }

    public string UserName { get; set; } = string.Empty;
    
    public UserType? UserType { get; set; }
}

public enum UserType
{
    Admin
}