#  agile-actors-assignment

A .NET 9 Web API that aggregates data from multiple external APIs including OpenWeather, GitHub, and News API.

## Getting Started

### Prerequisites

- .NET 9 SDK
- API keys for external services (see Configuration section)

### Configuration

Before running the project, you must create an `appsettings.Secrets.json` file in the root directory of the project with your API keys:

```json
{
  "ExternalApis": {
    "OpenWeather": {
      "ApiKey": "your_openweather_api_key_here"
    },
    "NewsApi": {
      "ApiKey": "your_news_api_key_here"
    }
  }
}
```

**Note:** Replace the placeholder values with your actual API keys:
- Get OpenWeather API key from: https://openweathermap.org/api
- Get News API key from: https://newsapi.org/

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Create the `appsettings.Secrets.json` file with your API keys
4. Run the application:

```bash
dotnet run
```

The application will start and be available at:
- **HTTP:** `http://localhost:5077`
- **HTTPS:** `https://localhost:7149` or `http://localhost:5077`

### Using the API

#### Option 1: Swagger UI
Swagger UI will automatically open, allowing you to:
- View available endpoints
- Test the API directly from the browser interface. Use the sample request body shown.

#### Option 2: HTTP File (Recommended)
Use the included `assignment.http` file to test the endpoints.

### Available Endpoints

- `POST /api/aggregation/location` - Retrieve aggregated data for a specific location
- `GET /api/aggregation/health` - Health check endpoint

### Testing

Run the unit tests with:

```bash
dotnet test
```
