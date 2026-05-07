using System;
using Xunit;

public class IntegrationTests24
{
    [Fact]
    public void LdapAuthAdapter_ValidCredentials_ReturnsTrue()
    {
        IUserAuthenticator auth = new LdapAuthAdapter();
        var result = auth.Authenticate("admin", "password123");
        Assert.True(result);
    }

    [Fact]
    public void LdapAuthAdapter_InvalidCredentials_ReturnsFalse()
    {
        IUserAuthenticator auth = new LdapAuthAdapter();
        var result = auth.Authenticate("user", "wrongpass");
        Assert.False(result);
    }

    [Fact]
    public void SecurityFacade_SetupNewUser_ExecutesWithoutExceptions()
    {
        var facade = new SecurityFacade();
        
        Action act = () => facade.SetupNewUser("testUser", "Role", "Perm");
        var exception = Record.Exception(act);
        
        Assert.Null(exception);
    }

    [Fact]
    public void SecurityUserAccessProxy_AdminRole_ReturnsData()
    {
        IUserAccess proxy = new SecurityUserAccessProxy("Admin");
        var result = proxy.GetUser("testUser");
        Assert.Contains("Sensitive data", result);
    }

    [Fact]
    public void SecurityUserAccessProxy_NonAdminRole_ReturnsAccessDenied()
    {
        IUserAccess proxy = new SecurityUserAccessProxy("User");
        var result = proxy.GetUser("testUser");
        Assert.Contains("Access Denied", result);
    }
}

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
    public void CreateUser(string user) { }
}

public class RoleService
{
    public void AssignRole(string user, string role) { }
}

public class PermissionService
{
    public void GrantPermission(string user, string permission) { }
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

// --- PROXY ---
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