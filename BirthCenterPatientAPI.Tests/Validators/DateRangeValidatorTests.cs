using FluentValidation.TestHelper;
using BirthCenterPatientAPI.Models;
using BirthCenterPatientAPI.Validators;

namespace BirthCenterPatientAPI.Tests.Validators
{
    public class DateRangeValidatorTests
    {
        private DateRangeValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new DateRangeValidator();
        }

        [Test]
        public void DateRangeValidator_WhenDateFromIsGreaterThanDateTo_ShouldHaveValidationError()
        {
            var dateRange = new DateRange
            {
                DateFrom = new DateTime(2023, 01, 01),
                DateTo = new DateTime(2022, 12, 31)
            };

            var result = _validator.TestValidate(dateRange);

            result.ShouldHaveValidationErrorFor(x => x.DateFrom);
        }

        [Test]
        public void DateRangeValidator_WhenDateFromEqualsDateTo_ShouldNotHaveValidationError()
        {
            var dateRange = new DateRange
            {
                DateFrom = new DateTime(2023, 01, 01),
                DateTo = new DateTime(2023, 01, 01)
            };

            var result = _validator.TestValidate(dateRange);

            result.ShouldNotHaveValidationErrorFor(x => x.DateFrom);
        }

        [Test]
        public void DateFromGreaterThanDateTo_ShouldFailValidation()
        {
            var dateRange = new DateRange
            {
                DateFrom = DateTime.Now.AddDays(1),
                DateTo = DateTime.Now
            };

            var validationResult = _validator.Validate(dateRange);

            Assert.That(validationResult.IsValid, Is.False);
        }

        [Test]
        public void DateFromEqualToDateTo_ShouldPassValidation()
        {
            var dateRange = new DateRange
            {
                DateFrom = DateTime.Now,
                DateTo = DateTime.Now
            };

            var validationResult = _validator.Validate(dateRange);

            Assert.That(validationResult.IsValid);
        }
    }
}
