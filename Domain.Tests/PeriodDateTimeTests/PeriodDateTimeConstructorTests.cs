using Domain.ValueObjects;

namespace Domain.Tests.PeriodDateTimeTests;

public class PeriodDateTimeConstructorTests
{
    /**
    * Test method for constructor that receives two DateOnly
    * Happy Path -> dates are valid 
    * Object has to be instatiated with no exceptions
    */
    public static IEnumerable<object[]> GetPeriodDates_ValidFields()
    {
        yield return new object[] { DateTime.Now, DateTime.Now.AddYears(1) };
        yield return new object[] { DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(12) };
        yield return new object[] { DateTime.Now.AddDays(1), DateTime.Now.AddDays(12) };
    }

    [Theory]
    [MemberData(nameof(GetPeriodDates_ValidFields))]
    public void WhenPassingValidDates_ThenObjectIsInstatiated(DateTime initDate, DateTime finalDate)
    {
        // Arrange

        // Act
        new PeriodDateTime(initDate, finalDate);

        // Assert
    }

    /**
    * Test method for constructor that receives two DateOnly
    * Should Throw Exception -> Dates are invalid 
    * Init date after end date
    */
    public static IEnumerable<object[]> GetPeriodDates_InvalidFields()
    {
        yield return new object[] { DateTime.Now, DateTime.Now.AddYears(-1) };
        yield return new object[] { DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(-12) };
        yield return new object[] { DateTime.Now.AddDays(1), DateTime.Now.AddDays(-12) };
    }

    [Theory]
    [MemberData(nameof(GetPeriodDates_InvalidFields))]
    public void WhenPassingInvalidDates_ThenThrowException(DateTime initDate, DateTime finalDate)
    {
        // Arrange

        // Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() =>
            // Act
            new PeriodDateTime(initDate, finalDate)
        );

        Assert.Equal("Invalid Arguments", exception.Message);
    }

    /**
    * Test method for constructor that receives a periodDate
    * Conversion from DateOnly to DateTime
    * If PeriodDate is instatiated then dates are valid
    */
    [Fact]
    public void WhenPassingPeriodDate_ThenObjectIsInstatiatedWithItsDates()
    {
        // Arrange
        var startDate = DateOnly.FromDateTime(DateTime.Today);
        var endDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        PeriodDate inPeriod = new PeriodDate(startDate, endDate);

        // Act
        PeriodDateTime periodDateTime = new PeriodDateTime(inPeriod);

        // Assert
        Assert.NotNull(periodDateTime);
        Assert.Equal(startDate.ToDateTime(TimeOnly.MinValue), periodDateTime._initDate);
        Assert.Equal(endDate.ToDateTime(TimeOnly.MinValue), periodDateTime._finalDate);
    }

}
