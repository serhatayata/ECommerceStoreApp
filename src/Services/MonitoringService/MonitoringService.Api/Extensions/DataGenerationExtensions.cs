using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text;

namespace MonitoringService.Api.Extensions;

public static class DataGenerationExtensions
{
    private static readonly Random _random = new Random();
    private readonly static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    #region RandomNumber
    // Generates a random number within a range.      
    public static int RandomNumber(int min, int max)
    {
        return _random.Next(min, max);
    }
    #endregion
    #region RandomString
    // Generates a random string with a given size.    
    public static string RandomString(int size, bool lowerCase = false)
    {
        var builder = new StringBuilder(size);

        // Unicode/ASCII Letters are divided into two blocks
        // (Letters 65–90 / 97–122):   
        // The first group containing the uppercase letters and
        // the second group containing the lowercase.  

        // char is a single Unicode character  
        char offset = lowerCase ? 'a' : 'A';
        const int lettersOffset = 26; // A...Z or a..z: length = 26  

        for (var i = 0; i < size; i++)
        {
            var @char = (char)_random.Next(offset, offset + lettersOffset);
            builder.Append(@char);
        }

        return lowerCase ? builder.ToString().ToLower() : builder.ToString();
    }
    #endregion
    #region RandomCode
    public static string RandomCode(int number)
    {
        var stringChars = new char[number];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var finalString = new String(stringChars);
        return finalString;
    }
    #endregion
    #region RandomPassword
    // Generates a random password.  
    // 4-LowerCase + 4-Digits + 2-UpperCase  
    public static string RandomPassword()
    {
        var passwordBuilder = new StringBuilder();

        // 4-Letters lower case   
        passwordBuilder.Append(RandomString(4, true));

        // 4-Digits between 1000 and 9999  
        passwordBuilder.Append(RandomNumber(1000, 9999));

        // 2-Letters upper case  
        passwordBuilder.Append(RandomString(2));
        return passwordBuilder.ToString();
    }
    #endregion
    #region GenerateLinkTitle
    public static string GenerateLinkData(params string[] data)
    {
        if (data == null || data?.Count() == 0)
            return string.Empty;

        string returnValue = string.Empty;
        Array.ForEach(data, d =>
        {
            d.ToLower().Trim();
            d = GetNormalizedStringData(d);
            d = ReplaceWhiteSpacesUsingRegex(d);
            returnValue += d;
        });

        return string.Join("-", returnValue);
    }
    #endregion

    #region GetNormalizedStringData
    public static string GetNormalizedStringData(string text)
    {
        return String.Join("", text.Normalize(NormalizationForm.FormD)
                     .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
                     .Replace("ı", "i");
    }
    #endregion
    #region ReplaceWhiteSpacesUsingRegex
    public static string ReplaceWhiteSpacesUsingRegex(string source)
    {
        return Regex.Replace(source, @"\s+", "-");
    }
    #endregion
    #region RemoveWhitespacesUsingRegex
    public static string RemoveWhitespacesUsingRegex(string source)
    {
        return Regex.Replace(source, @"\s", string.Empty);
    }
    #endregion
    #region COMPRESS / DECOMPRESS
    public static string CompressString(string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        var memoryStream = new MemoryStream();
        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
        {
            gZipStream.Write(buffer, 0, buffer.Length);
        }

        memoryStream.Position = 0;

        var compressedData = new byte[memoryStream.Length];
        memoryStream.Read(compressedData, 0, compressedData.Length);

        var gZipBuffer = new byte[compressedData.Length + 4];
        Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
        Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
        return Convert.ToBase64String(gZipBuffer);
    }

    public static string DecompressString(string compressedText)
    {
        byte[] gZipBuffer = Convert.FromBase64String(compressedText);
        using (var memoryStream = new MemoryStream())
        {
            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

            var buffer = new byte[dataLength];

            memoryStream.Position = 0;
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                gZipStream.Read(buffer, 0, buffer.Length);
            }

            return Encoding.UTF8.GetString(buffer);
        }
    }
    #endregion
}
