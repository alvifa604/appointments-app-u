using BC = BCrypt.Net.BCrypt;

namespace Application.Core
{
    public static class PasswordService
    {
        public static string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public static bool IsPasswordValid(string password, string hashedPassword)
        {
            return BC.Verify(password, hashedPassword);
        }
    }
}