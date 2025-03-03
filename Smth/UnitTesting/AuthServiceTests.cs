using Microsoft.EntityFrameworkCore;
using Smth.Data;
using Smth.Services;
using Xunit;

public class AuthServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _authService = new AuthService(_context);
    }

    [Fact]
    public void Register_ValidUser_ShouldReturnUser()
    {
        // Arrange
        string username = "testuser";
        string email = "test@example.com";
        string password = "Test@123";

        // Act
        var user = _authService.Register(username, email, password);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(username, user.Username);
        Assert.Equal(email, user.Email);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
        Assert.Single(_context.Users);
    }

    [Fact]
    public void Register_ExistingUsername_ShouldThrowException()
    {
        // Arrange
        string username = "existinguser";
        string email = "existing@example.com";
        string password = "Test@123";
        
        _authService.Register(username, "other@email.com", password);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _authService.Register(username, email, password));
        
        Assert.Equal("Логин уже занят.", exception.Message);
        Assert.Single(_context.Users);
    }

    [Fact]
    public void Register_ExistingEmail_ShouldThrowException()
    {
        // Arrange
        string email = "existing@example.com";
        string password = "Test@123";
        
        _authService.Register("user1", email, password);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _authService.Register("user2", email, password));
        
        Assert.Equal("Email уже зарегистрирован.", exception.Message);
        Assert.Single(_context.Users);
    }

    [Fact]
    public void Login_ByUsername_ValidCredentials_ShouldReturnUser()
    {
        // Arrange
        string username = "loginuser";
        string email = "login@example.com";
        string password = "Login@123";
        _authService.Register(username, email, password);

        // Act
        var result = _authService.Login(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
    }

    [Fact]
    public void Login_ByEmail_ValidCredentials_ShouldReturnUser()
    {
        // Arrange
        string username = "loginuser";
        string email = "login@example.com";
        string password = "Login@123";
        _authService.Register(username, email, password);

        // Act
        var result = _authService.Login(email, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
    }

    [Fact]
    public void Login_InvalidPassword_ShouldReturnNull()
    {
        // Arrange
        string username = "loginuser2";
        string email = "login2@example.com";
        string password = "Login@123";
        _authService.Register(username, email, password);

        // Act
        var result = _authService.Login(username, "WrongPassword");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Login_NonExistingLogin_ShouldReturnNull()
    {
        // Act
        var result = _authService.Login("nonexisting", "SomePassword");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Login_InvalidEmailFormat_ShouldReturnNull()
    {
        // Act
        var result = _authService.Login("notanemail", "SomePassword");

        // Assert
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}