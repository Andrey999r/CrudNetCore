using Smth.Data;

namespace Smth.Interfaces;

public interface IAuth
{
    ApplicationUser Register(string username, string password);
    ApplicationUser Login(string username, string password);
}