using System.Linq;

namespace TopGames.Infrastructure
{
    public class Helpers
    {
        private static string ExtractNumber(string text)
        {
            return new string(text.Where(c => char.IsDigit(c)).ToArray());
        }

        public static int ExtractIntNumber(string text)
        {
            int.TryParse(ExtractNumber(text), out int number);
            return number;
        }

        public static long ExtractLongNumber(string text)
        {
            long.TryParse(ExtractNumber(text), out long number);
            return number;
        }
    }
}
