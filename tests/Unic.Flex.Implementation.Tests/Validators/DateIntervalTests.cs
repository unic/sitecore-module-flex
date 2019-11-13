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
    using Moq;
    using NodaTime;
    using Unic.Flex.Core.Logging;

    [TestFixture]
    public class DateIntervalTests
    {
        public FakeClock Clock { get; set; }

        public DateTime TestDate { get; set; }

        public DateIntervalValidator DateIntervalValidator { get; set; }

        private IEnumerable<TestCaseData> ValidDateTestCases
        {
            get
            {
                this.SetUp();

                yield return new TestCaseData(1, Enums.TimeIntervalTypes.Day, Enums.TimeCompareTypes.NotNewerThan);
                yield return new TestCaseData(1000, Enums.TimeIntervalTypes.Day, Enums.TimeCompareTypes.NotOlderThan);
                yield return new TestCaseData(1, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.NotNewerThan);
                yield return new TestCaseData(4, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.PastAndNotOlderThan);
                yield return new TestCaseData(5, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.NotOlderThan);
                yield return new TestCaseData(1, Enums.TimeIntervalTypes.Day, Enums.TimeCompareTypes.NotNewerThan);
            }
        }

        private IEnumerable<TestCaseData> InvalidDateTestCases
        {
            get
            {
                this.SetUp();

                yield return new TestCaseData(4, Enums.TimeIntervalTypes.Month, Enums.TimeCompareTypes.FutureAndNotNewerThan);
                yield return new TestCaseData(3, Enums.TimeIntervalTypes.Month, Enums.TimeCompareTypes.PastAndNotOlderThan);
                yield return new TestCaseData(1, Enums.TimeIntervalTypes.Year, Enums.TimeCompareTypes.NotOlderThan);
            }
        }

        [SetUp]
        public void SetUp()
        {
            var loggerMock = new Mock<ILogger>();

            this.DateIntervalValidator = new DateIntervalValidator(loggerMock.Object);
            this.Clock = new FakeClock(Instant.FromDateTimeOffset(DateTime.Now));
            this.TestDate = this.Clock.GetCurrentInstant().ToDateTimeUtc().Date.AddMonths(-12);

        }

        [Test]
        [TestCaseSource(nameof(ValidDateTestCases))]
        public void CurrentDateIsInInterval_Returns_True_For_Valid_Dates(int amount, Enums.TimeIntervalTypes intervalType, Enums.TimeCompareTypes compareType)
        {
            this.DateIntervalValidator.TimeAmount = amount;
            this.DateIntervalValidator.TimeIntervalType = new Specification { Value = intervalType.ToString() };
            this.DateIntervalValidator.TimeCompareType = new Specification { Value = compareType.ToString() };

            var result = this.DateIntervalValidator.IsValid(this.TestDate);

            result.ShouldBeTrue();
        }

        [Test]
        [TestCaseSource(nameof(InvalidDateTestCases))]
        public void CurrentDateIsInInterval_Returns_False_For_Invalid_Dates(int amount, Enums.TimeIntervalTypes intervalType, Enums.TimeCompareTypes compareType)
        {
            this.DateIntervalValidator.TimeAmount = amount;
            this.DateIntervalValidator.TimeIntervalType = new Specification { Value = intervalType.ToString() };
            this.DateIntervalValidator.TimeCompareType = new Specification { Value = compareType.ToString() };

            var result = this.DateIntervalValidator.IsValid(this.TestDate);

            result.ShouldBeFalse();
        }
    }
}
