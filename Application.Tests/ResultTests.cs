using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Application.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ReturnsSuccessResult_WithNoErrors()
    {
        // Act
        Result result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ReturnsFailureResult_WithError()
    {
        // Arrange
        // Example error - BadRequest
        var expectedError = Error.BadRequest("Invalid input for operation.");

        // Act
        Result result = Result.Failure(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(expectedError.Message, result.Error.Message);
        Assert.Equal(expectedError.StatusCode, result.Error.StatusCode);
    }

    [Fact]
    public void Success_Generic_ReturnsSuccessResult_WithValueAndNoErrors()
    {
        // Arrange
        var expectedValue = "Operation successful data";

        // Act
        Result<string> result = Result<string>.Success(expectedValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
        Assert.Equal(expectedValue, result.Value);
    }

    [Fact]
    public void Failure_Generic_ReturnsFailureResult_WithErrorAndDefaultValueType()
    {
        // Arrange
        var expectedError = Error.NotFound("Item with ID 5 not found.");

        // Act
        Result<int> result = Result<int>.Failure(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(expectedError.Message, result.Error.Message);
        Assert.Equal(expectedError.StatusCode, result.Error.StatusCode);
        Assert.Equal(default(int), result.Value); // For int, default is 0
    }

    [Fact]
    public void Failure_Generic_ReturnsFailureResult_WithErrorAndDefaultReferenceType()
    {
        // Arrange
        var expectedError = Error.Unauthorized("Access token expired.");

        // Act
        Result<MyTestObject> result = Result<MyTestObject>.Failure(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(expectedError.Message, result.Error.Message);
        Assert.Equal(expectedError.StatusCode, result.Error.StatusCode);
        Assert.Null(result.Value);
    }

    // An example fake class to test on the generic behaviour
    private class MyTestObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
