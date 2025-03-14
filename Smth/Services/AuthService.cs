﻿using Smth.Data;
using Smth.Interfaces;

namespace Smth.Services
{
    public class AuthService : IAuth
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationUser Register(string username, string email, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Все поля обязательны для заполнения.");

            if (_context.Users.Any(u => u.Username == username))
                throw new InvalidOperationException("Логин уже занят.");

            if (_context.Users.Any(u => u.Email == email))
                throw new InvalidOperationException("Email уже зарегистрирован.");

            var user = new ApplicationUser
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                
            };
            
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public ApplicationUser Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => 
        u.Username == username || u.Email == username);
        var users = _context.Users.ToList(); 
        Console.WriteLine($"В БД пользователей: {users.Count}");
        foreach (var u in users)
        {
            Console.WriteLine($"User: {u.Username}, Email: {u.Email}");
        }

        Console.WriteLine("Найден пользователь: " + user.Username);
        Console.WriteLine("Хранимый хеш: " + user.PasswordHash);
        Console.WriteLine("Введенный пароль: " + password);
        if (user == null)
        {
            Console.WriteLine("Пользователь не найден: " + username);
            return null;
        }
    

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            Console.WriteLine("Пароль не прошел верификацию");
            return null;
        }

        Console.WriteLine("Успешный вход");
        return user;
        }
    }
}


