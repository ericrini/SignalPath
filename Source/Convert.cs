using System;
using System.Text;

namespace SignalPath
{
    internal static class Convert
    {
        public static string HexToBase64(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException(nameof(hex));
            }

            // The most common way to interperet hexadecimal is using every 2 characters as a byte, so the string
            // "5162" is [0x51, 0x62]. This implies that "516" is an invalid input as they gave you the byte 0x51 and
            // then 6 which is ambiguous what that means.
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentOutOfRangeException($"The input string should consist of an even number of characters.");
            }

            // We've established that we need one byte per 2 characters.
            byte[] bytes = new byte[(int)hex.Length / 2];

            // Were going to iterate through the string 2 elements at a time.
            for (int hexIndex = 0, bytesIndex = 0; hexIndex < hex.Length; hexIndex += 2, bytesIndex += 1)
            {
                // The first character become the 4 high bits the second character becomes the 4 low bits of the byte.
                byte high4 = ParseHexChar(hex[hexIndex]);
                byte low4 = ParseHexChar(hex[hexIndex + 1]);
                bytes[bytesIndex] = (byte)(high4 << 4 | low4);
            }

            StringBuilder builder = new StringBuilder();

            // A base64 character can encode the range 0-63 which is 6 bits. We're going to iterate through the buffer
            // 3 bytes at a time. This will give us an even 24-bits which we will convert into 4 base64 encoded
            // characters.
            for (int i = 0; i < bytes.Length; i += 3)
            {
                byte one = bytes[i];
                builder.Append(GetBase64Char(one >> 2)); // Highest 6 bits in the first byte.

                if (i + 2 > bytes.Length)
                {
                    // Use the remaining 2 low bits of the first byte as the 2 high bits in the next sextet. Confusing!
                    builder.Append(GetBase64Char((one & 0x03) << 4));
                    builder.Append("==");
                    break;
                }

                byte two = bytes[i + 1];
                // Lowest 2 bits in the first byte and highest 4 bits in the second byte.
                builder.Append(GetBase64Char(((one & 0x3) << 4) | (two >> 4)));

                if (i + 3 > bytes.Length)
                {
                    // Use the remaining 2 low bits of the first byte as the 2 high bits in the next sextet. Confusing!
                    builder.Append(GetBase64Char((one & 0x03) << 4));
                    builder.Append("=");
                    break;
                }

                byte three = bytes[i + 2];
                builder.Append(GetBase64Char(((two & 0xF) << 2) | (three >> 6))); // Lowest 4 bits in the first byte and highest 2 bits in the third byte.
                builder.Append(GetBase64Char(three & 0x3F)); // Lowest 6 bits in the third byte.
            }

            return builder.ToString();
        }

        private static byte ParseHexChar(char current)
        {
            // Character codes 48 (0) to 57 (9) convert to value of 0 to 9.
            if (current >= '0' && current <= '9')
            {
                return (byte)(current - 48);
            }

            // Character codes 65 (A) to 70 (F) convert to value of 10 to 15.
            if (current >= 'A' && current <= 'F')
            {
                return (byte)(current - 55);
            }

            // Character codes 97 (a) to 102 (f) convert to value of 10 to 15.
            if (current >= 'a' && current <= 'f')
            {
                return (byte)(current - 87);
            }

            throw new ArgumentOutOfRangeException($"The hexadecimal character '{current}' is invalid.");
        }

        private static char GetBase64Char(int value)
        {
            if (value >= 0 && value <= 25)
            {
                return (char)(value + 65); // Range of A (86) to Z (90).
            }

            if (value >= 26 && value <= 51)
            {
                return (char)(value + 71);  // Range of a (97) to z (122).
            }

            if (value >= 52 && value <= 61)
            {
                return (char)(value - 4); // Range of 0 (48) to 9 (57).
            }

            if (value == 62)
            {
                return '+';
            }

            if (value == 63)
            {
                return '/';
            }

            throw new ArgumentOutOfRangeException($"The value '{value}' can't be represented in base64.");
        }
    }
}
