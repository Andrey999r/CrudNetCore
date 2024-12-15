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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Уникальная база для каждого теста
                .Options;
            _context = new ApplicationDbContext(options);
            _authService = new AuthService(_context);
        }

        [Fact]
        public void Register_ValidUser_ShouldReturnUser()
        {
            // Arrange
            string username = "testuser";
            string password = "Test@123";

            // Act
            var user = _authService.Register(username, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(username, user.Username);
            Assert.NotNull(user.PasswordHash);
            Assert.True(BCrypt.Net.BCrypt.Verify(password, user.PasswordHash));
            Assert.Single(_context.Users);
            Assert.Equal(username, _context.Users.First().Username);
        }

        [Fact]
        public void Register_ExistingUsername_ShouldThrowException()
        {
            // Arrange
            string username = "existinguser";
            string password = "Test@123";
            var existingUser = new ApplicationUser { Id = 1, Username = username, PasswordHash = "hashedpassword" };
            _context.Users.Add(existingUser);
            _context.SaveChanges();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _authService.Register(username, password));
            Assert.Equal("Username already exists.", exception.Message);
            Assert.Single(_context.Users); // Не должно добавиться нового пользователя
        }

        [Fact]
        public void Login_ValidCredentials_ShouldReturnUser()
        {
            // Arrange
            string username = "loginuser";
            string password = "Login@123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new ApplicationUser { Id = 2, Username = username, PasswordHash = hashedPassword };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var result = _authService.Login(username, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public void Login_InvalidPassword_ShouldReturnNull()
        {
            // Arrange
            string username = "loginuser2";
            string password = "Login@123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new ApplicationUser { Id = 3, Username = username, PasswordHash = hashedPassword };
            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var result = _authService.Login(username, "WrongPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Login_NonExistingUser_ShouldReturnNull()
        {
            // Arrange
            string username = "nonexistinguser";
            string password = "SomePassword";

            // Act
            var result = _authService.Login(username, password);

            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }