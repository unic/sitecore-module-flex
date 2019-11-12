namespace Unic.Flex.Implementation.Tests.Validators
{
    using Definitions;
    using Implementation.Validators;
    using NodaTime.Testing;
    using NUnit.Framework;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using Model.Specifications;
    using NodaTime;

    [TestFixture]
    public class DateIntervalTests
    {
        public FakeClock Clock { get; set; }

        public DateTime TestDate { get; set; }

        private IEnumerable<TestCaseData> ValidDateTestCases
        {
            get
            {
                this.SetUp();

                yield return new TestCaseData(1000, Enums.TimeIntervalTypes.Day, Enums.TimeCompareTypes.Past);
                yield return new TestCaseData(11, Enums.TimeIntervalTypes.Month, Enums.TimeCompareTypes.Future);
                yield return new TestCaseData(1, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.EqualPast);
                yield return new TestCaseData(4, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.PastOrEqual);
                yield return new TestCaseData(5, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.FutureOrEqual);
            }
        }

        private IEnumerable<TestCaseData> InvalidDateTestCases
        {
            get
            {
                this.SetUp();

                yield return new TestCaseData(1, Enums.TimeIntervalTypes.Day, Enums.TimeCompareTypes.EqualPast);
                yield return new TestCaseData(4, Enums.TimeIntervalTypes.Month, Enums.TimeCompareTypes.Past);
                yield return new TestCaseData(3, Enums.TimeIntervalTypes.Month, Enums.TimeCompareTypes.EqualFuture);
                yield return new TestCaseData(1, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.EqualFuture);

            }
        }

        [SetUp]
        public void SetUp()
        {
            this.Clock = new FakeClock(Instant.FromDateTimeOffset(DateTime.Now));
            this.TestDate = this.Clock.GetCurrentInstant().ToDateTimeUtc().Date.AddMonths(-12);
        }

        [Test]
        [TestCaseSource(nameof(ValidDateTestCases))]
        public void CurrentDateIsInInterval_Returns_True_For_Valid_Dates(int amount, Enums.TimeIntervalTypes intervalType, Enums.TimeCompareTypes compareType)
        {
            var dateIntervalValidator = new DateIntervalValidator
            {
                TimeAmount = amount,
                TimeIntervalType = new Specification { Value = intervalType.ToString() },
                TimeCompareType = new Specification { Value = compareType.ToString() }
            };

            var result = dateIntervalValidator.IsValid(this.TestDate);

            result.ShouldBeTrue();
        }

        [Test]
        [TestCaseSource(nameof(InvalidDateTestCases))]
        public void CurrentDateIsInInterval_Returns_False_For_Invalid_Dates(int amount, Enums.TimeIntervalTypes intervalType, Enums.TimeCompareTypes compareType)
        {
            var dateIntervalValidator = new DateIntervalValidator
            {
                TimeAmount = amount,
                TimeIntervalType = new Specification { Value = intervalType.ToString() },
                TimeCompareType = new Specification { Value = compareType.ToString() }
            };

            var result = dateIntervalValidator.IsValid(this.TestDate);

            result.ShouldBeFalse();
        }
    }
}
