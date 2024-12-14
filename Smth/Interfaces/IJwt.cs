using Smth.Data;

namespace Smth.Interfaces;

public interface IJwt
{
    string GenerateToken(ApplicationUser user);
}