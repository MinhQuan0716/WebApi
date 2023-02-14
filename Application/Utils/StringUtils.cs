namespace Application.Utils
{
    public static class StringUtils
    {
        public static string Hash(this string input) => BCrypt.Net.BCrypt.HashPassword(input);

        public static bool CheckPassword(this string password, string hashPassword)=> BCrypt.Net.BCrypt.Verify(password,hashPassword);

    }
}
