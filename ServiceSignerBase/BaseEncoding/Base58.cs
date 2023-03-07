using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceSignerBase.BaseEncoding
{
    public  class Base58
    {
        private const string Base58Characters = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
        private static readonly char[] Base58CharsArray = Base58Characters.ToCharArray();
        private static readonly int Base58Length = Base58Characters.Length;

        public static string Encode(byte[] data)
        {
            // Count leading zeroes
            int zeroes = 0;
            while (zeroes < data.Length && data[zeroes] == 0)
            {
                zeroes++;
            }

            // Allocate enough space in big-endian base58 representation
            byte[] temp = new byte[data.Length * 2];
            int length = 0;

            // Convert big-endian bytes to little-endian base58 characters
            for (int i = zeroes; i < data.Length; i++)
            {
                int carry = (int)data[i];

                for (int j = 0; j < length; j++)
                {
                    carry += 256 * temp[j];
                    temp[j] = (byte)(carry % Base58Length);
                    carry /= Base58Length;
                }

                while (carry > 0)
                {
                    temp[length++] = (byte)(carry % Base58Length);
                    carry /= Base58Length;
                }
            }

            // Skip leading zeroes in base58 result
            int resultLength = zeroes + length;
            char[] result = new char[resultLength];

            for (int i = 0; i < zeroes; i++)
            {
                result[i] = Base58Characters[0];
            }

            for (int i = 0; i < length; i++)
            {
                result[zeroes + i] = Base58CharsArray[temp[length - 1 - i]];
            }

            return new string(result);
        }
        public static byte[] Decode(string encoded)
        {
            // Convert the string to a byte array
            byte[] inputBytes = new byte[encoded.Length];
            for (int i = 0; i < encoded.Length; i++)
            {
                char c = encoded[i];
                int value = Array.IndexOf(Base58CharsArray, c);
                if (value == -1)
                {
                    throw new FormatException($"Invalid character '{c}' at position {i}");
                }
                inputBytes[i] = (byte)value;
            }

            // Count leading zeroes
            int zeroes = 0;
            while (zeroes < inputBytes.Length && inputBytes[zeroes] == 0)
            {
                zeroes++;
            }

            // Allocate enough space in big-endian byte array
            byte[] temp = new byte[inputBytes.Length * 2];
            int length = 0;

            // Convert little-endian base58 characters to big-endian bytes
            for (int i = zeroes; i < inputBytes.Length; i++)
            {
                int carry = (int)inputBytes[i];

                for (int j = 0; j < length; j++)
                {
                    carry += Base58Length * temp[j];
                    temp[j] = (byte)(carry % 256);
                    carry /= 256;
                }

                while (carry > 0)
                {
                    temp[length++] = (byte)(carry % 256);
                    carry /= 256;
                }
            }

            // Skip leading zeroes in result
            int resultLength = zeroes + length;
            byte[] result = new byte[resultLength];

            for (int i = 0; i < zeroes; i++)
            {
                result[i] = 0;
            }

            for (int i = 0; i < length; i++)
            {
                result[zeroes + i] = temp[length - 1 - i];
            }

            return result;
        }

    }
}
