using FluentValidation;
using System.Text.RegularExpressions;

namespace IdentityServer.Api.Extensions
{
    public static class RuleBuilderExtensions
    {
        #region CustomMessage
        public static IRuleBuilderOptions<T, string> CustomMessage<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder.With(InternationalPhoneNumber);

            return options;
        }
        #endregion
        #region PasswordWithoutMessage
        public static IRuleBuilderOptions<T, string> PasswordWithoutMessage<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .Must(IsAUpperCharacter)
                .Must(IsALowerCharacter)
                .Must(IsANumber)
                .Must(IsASpecialCharacter)
                .Must(IsConsecutiveNumber)
                .Must(IsRepetitiveNumber);
            return options;
        }
        #endregion
        #region InternationalPhoneNumber
        public static IRuleBuilderOptions<T, string> InternationalPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder.Must(InternationalPhoneNumber);

            return options;
        }
        #endregion
        #region PhoneNumber
        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder.Must(PhoneNumber);

            return options;
        }
        #endregion
        #region IsAUpperCharacter
        public static bool IsAUpperCharacter(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                @"[A-ZÇĞİÖÜ]{1,}");
        }
        #endregion
        #region IsALowerCharacter
        public static bool IsALowerCharacter(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                "[a-zçğıöü]{1,}");
        }
        #endregion
        #region IsANumber
        public static bool IsANumber(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                "[0-9]{1,}");
        }
        #endregion
        #region IsJustNumber
        //Yalnızca sayı içerir
        public static bool IsJustNumber(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                "^[0-9]*$");
        }
        #endregion
        #region IsConsecutiveNumber
        //Ardışık sayı içerir
        public static bool IsConsecutiveNumber(string arg)
        {
            return !string.IsNullOrEmpty(arg) && !Regex.IsMatch(arg,
                "(012|123|234|345|456|567|678|789|890|098|987|876|765|654|543|432|321|210)");
        }
        #endregion
        #region IsValidMonth
        //Ayları içerir
        public static bool IsValidMonth(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                "(1|2|3|4|5|6|7|8|9|10|11|12)");
        }
        #endregion
        #region IsValidOverTheYear
        public static bool IsValidOverTheYear(string arg)
        {
            int year;
            var conv = int.TryParse(arg, out year);
            return !string.IsNullOrEmpty(arg) && conv && year > DateTime.Now.Year;
        }
        #endregion
        #region IsRepetitiveNumber
        //Tekrarlı sayı içerir
        public static bool IsRepetitiveNumber(string arg)
        {
            return !string.IsNullOrEmpty(arg) && !Regex.IsMatch(arg,
                "([0-9])\\1\\1");
        }
        #endregion
        #region IsASpecialCharacter
        public static bool IsASpecialCharacter(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                "[#?!@$%^&*-+]{1,}");
        }
        #endregion
        #region IsLetter
        public static bool IsLetter(string arg)//Sadece harf ve Boşluk
        {
            return !string.IsNullOrEmpty(arg) && !Regex.IsMatch(arg,
                "([^a-zA-Z çÇğĞıİöÖşŞüÜ])");
        }
        #endregion
        #region IsLetterWithoutEmpty
        public static bool IsLetterWithoutEmpty(string arg)//Sadece harf ve Boşluk
        {
            return !Regex.IsMatch(arg,
                "([^a-zA-Z çÇğĞıİöÖşŞüÜ])");
        }
        #endregion
        #region InternationalPhoneNumber
        private static bool InternationalPhoneNumber(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                "^([\\+]?90[-]?|[0])?[1-9][0-9]{8}$");
        }
        #endregion
        #region PhoneNumber
        private static bool PhoneNumber(string arg)
        {
            return !string.IsNullOrEmpty(arg) && Regex.IsMatch(arg,
                "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        }
        #endregion
    }
}
