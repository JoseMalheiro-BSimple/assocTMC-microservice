namespace Application.Tests;

public class ErrorTests
{
    [Fact]
    public void NotFound_CreatesErrorWithCorrectMessageAndStatusCode()
    {
        // Arrange
        string message = "Resource not found for ID 123.";

        // Act
        Error error = Error.NotFound(message);

        // Assert
        Assert.Equal(message, error.Message);
        Assert.Equal(404, error.StatusCode);
    }

    [Fact]
    public void BadRequest_CreatesErrorWithCorrectMessageAndStatusCode()
    {
        // Arrange
        string message = "Invalid input data provided.";

        // Act
        Error error = Error.BadRequest(message);

        // Assert
        Assert.Equal(message, error.Message);
        Assert.Equal(400, error.StatusCode);
    }

    [Fact]
    public void Unauthorized_CreatesErrorWithCorrectMessageAndStatusCode()
    {
        // Arrange
        string message = "Authentication required.";

        // Act
        Error error = Error.Unauthorized(message);

        // Assert
        Assert.Equal(message, error.Message);
        Assert.Equal(401, error.StatusCode);
    }

    [Fact]
    public void Forbidden_CreatesErrorWithCorrectMessageAndStatusCode()
    {
        // Arrange
        string message = "Access to this resource is forbidden.";

        // Act
        Error error = Error.Forbidden(message);

        // Assert
        Assert.Equal(message, error.Message);
        Assert.Equal(403, error.StatusCode);
    }

    [Fact]
    public void InternalServerError_CreatesErrorWithCorrectMessageAndStatusCode()
    {
        // Arrange
        string message = "An unexpected server error occurred.";

        // Act
        Error error = Error.InternalServerError(message);

        // Assert
        Assert.Equal(message, error.Message);
        Assert.Equal(500, error.StatusCode);
    }
}
