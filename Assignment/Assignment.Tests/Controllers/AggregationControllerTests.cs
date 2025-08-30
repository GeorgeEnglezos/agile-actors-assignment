using Assignment.Controllers;
using Assignment.Interfaces;
using Assignment.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Assignment.Tests.Controllers
{
    public class AggregationControllerTests
    {
        private readonly Mock<ILogger<AggregationController>> _mockLogger;
        private readonly Mock<IAggregatedDataService> _mockAggregatedDataService;
        private readonly AggregationController _controller;

        public AggregationControllerTests()
        {
            _mockLogger = new Mock<ILogger<AggregationController>>();
            _mockAggregatedDataService = new Mock<IAggregatedDataService>();
            _controller = new AggregationController(_mockLogger.Object, _mockAggregatedDataService.Object);
        }

        [Fact]
        public async Task GetLocation_WithValidQuery_ReturnsOkResult()
        {
            // Arrange
            var query = new AggregationQuery
            {
                City = "Athens",
                Country = "GR",
                PageSize = 5,
                NewsKeyword = "Tech"
            };

            var expectedResponse = new AggregationDto
            {
                GithubResponse = new GithubApiDTO(),
                OpenWeatherResponse = new List<OpenWeatherDTO>
                {
                    new OpenWeatherDTO()
                },
                NewsResponse = new NewsApiFullDto()
            };

            _mockAggregatedDataService
                .Setup(x => x.GetAggregatedDataAsync(It.IsAny<AggregationQuery>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetLocation(query);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(expectedResponse);
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetLocation_WithEmptyCity_ReturnsBadRequest()
        {
            // Arrange
            var query = new AggregationQuery
            {
                City = "", // Empty city
                Country = "GR"
            };

            // Act
            var result = await _controller.GetLocation(query);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("City and country parameters are required.");
        }

        [Fact]
        public async Task GetLocation_WithEmptyCountry_ReturnsBadRequest()
        {
            // Arrange
            var query = new AggregationQuery
            {
                City = "Athens",
                Country = "" // Empty country
            };

            // Act
            var result = await _controller.GetLocation(query);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
            badRequestResult.Value.Should().Be("City and country parameters are required.");
        }

        [Fact]
        public async Task GetLocation_WithNullCity_ReturnsBadRequest()
        {
            // Arrange
            var query = new AggregationQuery
            {
                City = null,
                Country = "GR"
            };

            // Act
            var result = await _controller.GetLocation(query);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetLocation_WithWhitespaceCity_ReturnsBadRequest()
        {
            // Arrange
            var query = new AggregationQuery
            {
                City = "   ", // Whitespace only
                Country = "GR"
            };

            // Act
            var result = await _controller.GetLocation(query);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetLocation_CallsAggregatedDataService_WithCorrectParameters()
        {
            // Arrange
            var query = new AggregationQuery
            {
                City = "Athens",
                Country = "GR",
                PageSize = 10
            };

            var expectedResponse = new AggregationDto();
            _mockAggregatedDataService
                .Setup(x => x.GetAggregatedDataAsync(It.IsAny<AggregationQuery>()))
                .ReturnsAsync(expectedResponse);

            // Act
            await _controller.GetLocation(query);

            // Assert
            _mockAggregatedDataService.Verify(
                x => x.GetAggregatedDataAsync(It.Is<AggregationQuery>(q =>
                    q.City == query.City &&
                    q.Country == query.Country &&
                    q.PageSize == query.PageSize)),
                Times.Once);
        }

        [Fact]
        public async Task GetLocation_LogsInformation_WithCityAndCountry()
        {
            // Arrange
            var query = new AggregationQuery
            {
                City = "Athens",
                Country = "GR"
            };

            _mockAggregatedDataService
                .Setup(x => x.GetAggregatedDataAsync(It.IsAny<AggregationQuery>()))
                .ReturnsAsync(new AggregationDto());

            // Act
            await _controller.GetLocation(query);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Athens") && v.ToString().Contains("GR")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void Health_ReturnsOkResult_WithHealthyStatus()
        {
            // Act
            var result = _controller.Health();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);

            // Check that the response contains Status and Timestamp
            var response = okResult.Value;
            response.Should().NotBeNull();

            // You can use reflection to check the anonymous object properties
            var statusProperty = response.GetType().GetProperty("Status");
            var timestampProperty = response.GetType().GetProperty("Timestamp");

            statusProperty.Should().NotBeNull();
            statusProperty.GetValue(response).Should().Be("Healthy");

            timestampProperty.Should().NotBeNull();
            timestampProperty.GetValue(response).Should().BeOfType<DateTime>();
        }

        [Fact]
        public void Health_ReturnsCurrentTimestamp()
        {
            // Arrange
            var beforeCall = DateTime.UtcNow;

            // Act
            var result = _controller.Health();

            // Assert
            var afterCall = DateTime.UtcNow;
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var response = okResult.Value;
            var timestampProperty = response.GetType().GetProperty("Timestamp");
            var timestamp = (DateTime)timestampProperty.GetValue(response);

            // Verify the timestamp is between before and after the call
            timestamp.Should().BeOnOrAfter(beforeCall).And.BeOnOrBefore(afterCall);
        }
    }
}