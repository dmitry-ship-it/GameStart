using System.Security.Cryptography;
using System.Text;

namespace GameStart.OrderingService.Application.Services
{
    public class GameKeyGeneratorService : IGameKeyGeneratorService
    {
        private const int Blocks = 3;
        private const int BlockSize = 5;
        private const char Separator = '-';
        private const int SeparatorCount = Blocks - 1;

        public string Generate()
        {
            var keyBase = GetRandomString(Blocks * BlockSize).AsSpan();

            var builder = new StringBuilder();

            var pointer = 0;
            for (var i = 0; i < SeparatorCount; i++)
            {
                builder.Append(keyBase.Slice(pointer, BlockSize));
                builder.Append(Separator);
                pointer += BlockSize;
            }

            builder.Append(keyBase[pointer..]);

            return builder.ToString();
        }

        private static string GetRandomString(int size)
        {
            const string charPool =
                "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "0123456789";

            var result = new char[size];

            for (var i = 0; i < size; i++)
            {
                result[i] = charPool[RandomNumberGenerator.GetInt32(charPool.Length)];
            }

            return new string(result);
        }
    }
}
