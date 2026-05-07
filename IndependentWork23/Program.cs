using System;

public interface IUserAuthenticator
{
    bool Authenticate(string user, string pass);
}

public class LdapAuthenticator
{
    public bool AuthenticateUser(string user, string pass)
    {
        return user == "admin" && pass == "password123";
    }
}

public class LdapAuthAdapter : IUserAuthenticator
{
    private readonly LdapAuthenticator _ldapAuthenticator = new LdapAuthenticator();

    public bool Authenticate(string user, string pass)
    {
        return _ldapAuthenticator.AuthenticateUser(user, pass);
    }
}

public class UserService
{
    public void CreateUser(string user) => Console.WriteLine($"User {user} created.");
}

public class RoleService
{
    public void AssignRole(string user, string role) => Console.WriteLine($"Role {role} assigned to {user}.");
}

public class PermissionService
{
    public void GrantPermission(string user, string permission) => Console.WriteLine($"Permission {permission} granted to {user}.");
}

public class SecurityFacade
{
    private readonly UserService _userService = new UserService();
    private readonly RoleService _roleService = new RoleService();
    private readonly PermissionService _permissionService = new PermissionService();

    public void SetupNewUser(string user, string role, string permission)
    {
        _userService.CreateUser(user);
        _roleService.AssignRole(user, role);
        _permissionService.GrantPermission(user, permission);
    }
}

public interface IUserAccess
{
    string GetUser(string user);
}

public class RealUserAccess : IUserAccess
{
    public string GetUser(string user) => $"Sensitive data for user {user}";
}

public class SecurityUserAccessProxy : IUserAccess
{
    private readonly RealUserAccess _realUserAccess = new RealUserAccess();
    private readonly string _currentUserRole;

    public SecurityUserAccessProxy(string role)
    {
        _currentUserRole = role;
    }

    public string GetUser(string user)
    {
        if (_currentUserRole == "Admin")
        {
            return _realUserAccess.GetUser(user);
        }
        return "Access Denied: Insufficient permissions.";
    }
}

class Program23
{
    static void Main()
    {
        IUserAuthenticator auth = new LdapAuthAdapter();
        Console.WriteLine($"LDAP Auth Result: {auth.Authenticate("admin", "password123")}");

        var facade = new SecurityFacade();
        facade.SetupNewUser("new_employee", "Staff", "Read_Only");

        IUserAccess proxy = new SecurityUserAccessProxy("Staff");
        Console.WriteLine(proxy.GetUser("new_employee"));

        IUserAccess adminProxy = new SecurityUserAccessProxy("Admin");
        Console.WriteLine(adminProxy.GetUser("new_employee"));
    }
}