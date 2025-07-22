using Application;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace InterfaceAdapters.IntegrationTests;

public class ResultExtensionsTests
{
    [Fact]
    public void ToActionResult_WhenResultIsSuccess_ReturnsOkResult()
    {
        // Arrange
        Result successResult = Result.Success();

        // Act
        var actionResult = successResult.ToActionResult();

        // Assert
        Assert.IsType<OkResult>(actionResult);
    }

    [Fact]
    public void ToActionResult_WhenResultIsBadRequest_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var errorMessage = "Invalid request payload.";
        Result badRequestResult = Result.Failure(Error.BadRequest(errorMessage));

        // Act
        var actionResult = badRequestResult.ToActionResult();

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        Assert.Equal(400, badRequestObjectResult.StatusCode);
        Assert.Equal(errorMessage, badRequestObjectResult.Value);
    }

    [Fact]
    public void ToActionResult_WhenResultIsUnauthorized_ReturnsUnauthorizedObjectResult()
    {
        // Arrange
        var errorMessage = "Authentication credentials missing.";
        Result unauthorizedResult = Result.Failure(Error.Unauthorized(errorMessage));

        // Act
        var actionResult = unauthorizedResult.ToActionResult();

        // Assert
        var unauthorizedObjectResult = Assert.IsType<UnauthorizedObjectResult>(actionResult);
        Assert.Equal(401, unauthorizedObjectResult.StatusCode);
        Assert.Equal(errorMessage, unauthorizedObjectResult.Value);
    }

    [Fact]
    public void ToActionResult_WhenResultIsForbidden_ReturnsObjectResultWith403StatusCode()
    {
        // Arrange
        var errorMessage = "You do not have permission to access this resource.";
        Result forbiddenResult = Result.Failure(Error.Forbidden(errorMessage));

        // Act
        var actionResult = forbiddenResult.ToActionResult();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        Assert.Equal(403, objectResult.StatusCode);
        Assert.Equal(errorMessage, objectResult.Value);
    }

    [Fact]
    public void ToActionResult_WhenResultIsNotFound_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var errorMessage = "The requested item was not found.";
        Result notFoundResult = Result.Failure(Error.NotFound(errorMessage));

        // Act
        var actionResult = notFoundResult.ToActionResult();

        // Assert
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actionResult);
        Assert.Equal(404, notFoundObjectResult.StatusCode);
        Assert.Equal(errorMessage, notFoundObjectResult.Value);
    }

    [Fact]
    public void ToActionResult_WhenResultIsInternalServerError_ReturnsObjectResultWith500StatusCode()
    {
        // Arrange
        var errorMessage = "An unexpected error occurred on the server.";
        Result internalServerErrorResult = Result.Failure(Error.InternalServerError(errorMessage));

        // Act
        var actionResult = internalServerErrorResult.ToActionResult();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(actionResult);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal(errorMessage, objectResult.Value);
    }

    [Fact]
    public void ToActionResultGeneric_WhenResultIsSuccess_ReturnsOkObjectResultWithValue()
    {
        // Arrange
        var expectedValue = new TestData { Id = 1, Name = "Test Item" };
        Result<TestData> successResult = Result<TestData>.Success(expectedValue);

        // Act
        var actionResult = successResult.ToActionResult();

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result); // Access .Result for ActionResult<T>
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedValue, okObjectResult.Value);
    }

    [Fact]
    public void ToActionResultGeneric_WhenResultIsBadRequest_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var errorMessage = "Missing required fields.";
        Result<TestData> badRequestResult = Result<TestData>.Failure(Error.BadRequest(errorMessage));

        // Act
        var actionResult = badRequestResult.ToActionResult();

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.Equal(400, badRequestObjectResult.StatusCode);
        Assert.Equal(errorMessage, badRequestObjectResult.Value);
    }

    [Fact]
    public void ToActionResultGeneric_WhenResultIsNotFound_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var errorMessage = "User not found.";
        Result<TestData> notFoundResult = Result<TestData>.Failure(Error.NotFound(errorMessage));

        // Act
        var actionResult = notFoundResult.ToActionResult();

        // Assert
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal(404, notFoundObjectResult.StatusCode);
        Assert.Equal(errorMessage, notFoundObjectResult.Value);
    }

    [Fact]
    public void ToActionResultGeneric_WhenResultIsForbidden_ReturnsObjectResultWith403StatusCode()
    {
        // Arrange
        var errorMessage = "Insufficient permissions.";
        Result<TestData> forbiddenResult = Result<TestData>.Failure(Error.Forbidden(errorMessage));

        // Act
        var actionResult = forbiddenResult.ToActionResult();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
        Assert.Equal(403, objectResult.StatusCode);
        Assert.Equal(errorMessage, objectResult.Value);
    }

    [Fact]
    public void ToActionResultGeneric_WhenResultIsInternalServerError_ReturnsObjectResultWith500StatusCode()
    {
        // Arrange
        var errorMessage = "Database connection error.";
        Result<TestData> internalServerErrorResult = Result<TestData>.Failure(Error.InternalServerError(errorMessage));

        // Act
        var actionResult = internalServerErrorResult.ToActionResult();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal(errorMessage, objectResult.Value);
    }

    // Helper class for generic tests
    private class TestData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Override Equals and GetHashCode for proper object comparison in Assert.Equal
        public override bool Equals(object? obj)
        {
            return obj is TestData other && Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}
