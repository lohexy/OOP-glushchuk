using Xunit;
using lab30v4;

namespace lab30v4.Tests
{
    public class ValidatorTests
    {
        private readonly Validator _validator;

        public ValidatorTests()
        {
            _validator = new Validator();
        }

        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("user.name@domain.co.ua", true)]
        [InlineData("invalid-email", false)]
        [InlineData("test@.com", false)]
        [InlineData("@domain.com", false)]
        public void IsValidEmail_ShouldValidateCorrectly(string email, bool expected)
        {
            bool result = _validator.IsValidEmail(email);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsValidEmail_NullOrEmpty_ReturnsFalse()
        {
            Assert.False(_validator.IsValidEmail(null));
            Assert.False(_validator.IsValidEmail(string.Empty));
            Assert.False(_validator.IsValidEmail("   "));
        }


        [Theory]
        [InlineData("+380991234567", true)]
        [InlineData("380991234567", true)]
        [InlineData("123456789", false)]
        [InlineData("+38099abc4567", false)]
        public void IsValidPhone_ShouldValidateCorrectly(string phone, bool expected)
        {
            bool result = _validator.IsValidPhone(phone);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsValidPhone_NullInput_ReturnsFalse()
        {
            Assert.False(_validator.IsValidPhone(null));
        }

        [Theory]
        [InlineData("StrongPass1!", true)]
        [InlineData("weak", false)]
        [InlineData("onlylowercase1!", false)]
        [InlineData("ONLYUPPERCASE1!", false)]
        [InlineData("NoDigitsHere!", false)]
        [InlineData("NoSpecialChars123", false)]
        public void IsStrongPassword_ShouldValidateCorrectly(string password, bool expected)
        {
            bool result = _validator.IsStrongPassword(password);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsStrongPassword_EmptyString_ReturnsFalse()
        {
            Assert.False(_validator.IsStrongPassword(""));
        }
        
        [Fact]
        public void IsStrongPassword_JustUnder8Chars_ReturnsFalse()
        {
            Assert.False(_validator.IsStrongPassword("Pass1!a"));
        }
    }
}