namespace Application.Utils
{
    public static class StringUtils
    {
        public static string Hash(this string input) => BCrypt.Net.BCrypt.HashPassword(input);

        public static bool CheckPassword(this string password, string hashPassword) => BCrypt.Net.BCrypt.Verify(password, hashPassword);

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}
