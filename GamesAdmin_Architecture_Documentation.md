# GamesAdmin System Architecture Documentation

## Overview

GamesAdmin is a comprehensive gaming administration system built with **ASP.NET Core** using **Clean Architecture** principles. The system manages casino games, player data, and administrative operations through a sophisticated layered architecture.

## Architecture Overview

### Solution Structure

```
GamesAdmin/
├── src/
│   ├── GamesAdmin.Api/          # Backend API Layer
│   ├── GamesAdmin.Site/         # MVC Web Application
│   ├── GamesAdmin.Core/         # Domain Models & Business Logic
│   └── GamesAdmin.Database/     # Data Access Layer
├── Tests/                       # Unit Test Projects
└── docs/                        # Documentation
```

### Architectural Patterns

- **CQRS with MediatR** - Commands and queries separation
- **Clean Architecture** - Clear separation between layers
- **Feature-Based Organization** - Code organized by business features
- **Repository Pattern** - Generic repository for data access
- **Dependency Injection** - Extensive DI container usage

---

## Core Components

### 1. GamesAdmin.Core (Domain Layer)

#### Game Definitions

**Static Games** (Defined in `GameId.cs` enum):
```csharp
public enum GameId
{
    BigSmall = 1,
    SeDie = 2,
    // ... 148+ game IDs defined
    SeDieMultiplayer = 148,
}
```

**Dynamic Games** (Defined in `game-settings.json`):
```json
[
  {
    "GameId": 159,
    "GameName": "plinkox",
    "DisplayName": "Plinko X",
    "BetChoices": [],
    "Odds": [],
    "OfferedSystem": ["SC1", "VG2"]
  }
]
```

#### Game Types

```csharp
public enum GameType
{
    AutoRNG = 1,        // Standard RNG games
    Slots,              // Traditional slot machines
    CascadingSlots,     // Modern cascading slots
    SlotRNG,           // RNG-based slots
    Crash,             // Crash games
    DynamicOdds,       // Games with changing odds
    Marble,            // Marble-based games
    CrashTurnBase,     // Turn-based crash games
    GuaranteedWin,     // Guaranteed win mechanics
    AutoRNGIndex,      // Indexed RNG games
    Plinko             // Plinko drop games
}
```

#### Game Configuration Models

**GameConfig** - Main game configuration:
```csharp
public class GameConfig
{
    public GameInfo Info { get; set; }
    public Dictionary<string, double> Odds { get; set; }
    public EnableSetting EnableSetting { get; set; }
    public BotSetting BotSettings { get; set; }
    public BetLimit BetLimit { get; set; }
    public PlinkoConfig PlinkoConfig { get; set; } // Game-specific config
    // ... additional properties
}
```

**PlinkoConfig** - Plinko-specific configuration:
```csharp
public class PlinkoConfig
{
    public Dictionary<string, Dictionary<string, double[]>> Odds { get; set; }
    // Odds organized by RiskLevel -> RowCount -> Multipliers array
}
```

### 2. GamesAdmin.Database (Data Access Layer)

#### Entity Structure

**GameConfigEntity** - MongoDB document:
```csharp
[BsonCollection("game_configs")]
public class GameConfigEntity : MiniGameBaseEntity
{
    public byte Type { get; set; }           // GameType enum value
    public string Name { get; set; }         // Internal game name
    public string DisplayName { get; set; }  // Display name
    public int GameId { get; set; }          // Numeric game ID
    public bool Enabled { get; set; }        // Game enabled status
    public PlinkoConfig PlinkoConfig { get; set; } // Game-specific config
    // ... 80+ additional properties
}
```

#### Repository Pattern

**Generic Repository**:
```csharp
public interface IGenericRepository<T> where T : IDocument
{
    Task InsertOneAsync(T document);
    Task<T> GetByIdAsync(string id);
    IQueryable<T> AsQueryable();
    Task UpdateOneAsync(T document);
    Task DeleteOneAsync(string id);
}
```

**Specialized Repositories**:
- `JackpotRepository` - Jackpot operations
- `VoidLogRepository` - Void/cancel operations
- `SiteRepository` - Site-specific data

### 3. GamesAdmin.Api (Application Layer)

#### CQRS with MediatR Implementation

**Request/Response Pattern**:
```csharp
// Request
public class CreateGameRequest : IRequest<GameCreationResult>
{
    public GameConfig Game { get; set; }
}

// Handler
public class GameSettingsHandler :
    IRequestHandler<CreateGameRequest, GameCreationResult>
{
    public async Task<GameCreationResult> Handle(
        CreateGameRequest request, CancellationToken cancellationToken)
    {
        // Business logic implementation
        return await gameService.Create(request.Game);
    }
}
```

#### Game Creation Flow

```
UI Form → GamesHandler (Site) → IGameSettingApi (Refit)
    ↓
API Controller → GameSettingsHandler → IGameSettingsService
    ↓
GameSettingsService → Repository.InsertOneAsync(GameConfigEntity)
```

#### Key Services

**GameSettingsService** - Main game management:
```csharp
public interface IGameSettingsService
{
    Task<GameCreationResult> Create(GameConfig game);
    Task<IEnumerable<GameConfig>> Get();
    Task<GameConfig> GetByName(string name);
    Task<bool> UpdateStatus(string[] names, bool enabled);
    // ... 15+ additional methods
}
```

**VoidGame Service** - Game cancellation/void operations:
```csharp
public interface IVoidGameService
{
    Task<bool> VoidTicket(string obCustId, string partnerId,
        string siteId, long gameRoundId, int gameTypeId, bool freeBet);
}
```

### 4. GamesAdmin.Site (Presentation Layer)

#### MVC Architecture

**Controller Structure**:
```csharp
[Route("game_settings")]
public class GameSettingsController : Controller
{
    private readonly IMediator mediator;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var viewModel = await mediator.Send(new GetGamesRequest());
        return View(viewModel);
    }
}
```

#### View Models

**EditViewModel** - Game editing interface:
```csharp
public class EditViewModel
{
    public GameType Type { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public PlinkoConfig PlinkoConfig { get; set; }
    public string DetailsView { get; set; } // Dynamic view selection
    // ... 30+ properties
}
```

#### Dynamic View Selection

```csharp
private string GetDetailsView(GameType type)
{
    switch (type)
    {
        case GameType.Plinko:
            return "GameDetails_Plinko";
        case GameType.AutoRNG:
        case GameType.SlotRNG:
            return "GameDetails_RNG";
        default:
            return $"GameDetails_{type}";
    }
}
```

---

## Game Creation Process

### 1. UI Form Submission

**AddGameSettings.cshtml** provides:
- Game name selection (dropdown or custom input)
- Display name input
- Game ID input
- Game type selection
- Configuration options (Enabled, BotEnabled, etc.)
- Bet limits and odds configuration

### 2. Validation & Processing

**GamesHandler.CreateRequest**:
```csharp
public Task<GameCreationResult> Handle(CreateRequest request)
{
    var gameConfig = new GameConfig(request.GameInfo, request.Odds, ...);
    return gameService.Add(gameConfig);
}
```

### 3. Database Persistence

**GameSettingsService.Create**:
```csharp
public async Task<GameCreationResult> Create(GameConfig game)
{
    var entity = new GameConfigEntity
    {
        Name = game.Info.Name,
        Type = (byte)game.Info.Type,
        GameId = game.Info.GameId,
        PlinkoConfig = game.PlinkoConfig,
        // ... map all properties
    };

    await repository.InsertOneAsync(entity);
    return GameCreationResult.CreateSuccess();
}
```

### 4. Notification & Caching

- Notifications sent for new game creation
- Cache invalidation for game lists
- Client reload triggered if required

---

## Game Type-Specific Implementations

### Plinko Games

**Configuration**:
- Risk levels (0, 1, 2) representing difficulty
- Row counts (8-16) representing board size
- Multiplier arrays for each risk/row combination

**Specialized Views**:
- `GameDetails_Plinko.cshtml` - Plinko-specific settings
- `OddsSettings_Plinko.cshtml` - Plinko odds configuration
- JavaScript: `initializePlinkoRTP()` for RTP calculations

**Void Operations**:
- `MovePlinkoGameRound<BasePlinkoRoundEntity>()` for cancellation
- Round-based ticket management
- Archive handling for historical data

### Slot Games

**Configurations**:
- `SlotsConfig` - Traditional slot settings
- `CascadingSlotsConfig` - Modern cascading mechanics
- `SlotRNGConfig` - RNG-based slot parameters

**Special Features**:
- RTP verification system
- Symbol weight management
- Jackpot configuration

### Crash Games

**Configurations**:
- `CrashConfig` - Crash game parameters
- Multiplier calculations
- House edge management

---

## Data Flow Architecture

### Request Flow

```
User Action → Controller → MediatR Handler → Service → Repository → Database
                      ↓
                View Model ← Mapper ← Entity
```

### Game Configuration Flow

```
UI Form → Validation → GameConfig DTO → GameConfigEntity → MongoDB
                            ↓
                      AutoMapper → ViewModel → UI Display
```

### Void/Cancel Operations

```
Void Request → VoidGameService → TicketHandler → Database Update
                      ↓
                Notification → Client Reload → Cache Invalidation
```

---

## Key Design Patterns

### 1. CQRS Implementation

**Commands** (Write Operations):
```csharp
public class CreateGameRequest : IRequest<GameCreationResult>
{
    // Properties for game creation
}

public class UpdateGameStatusRequest : IRequest<bool>
{
    public string[] Names { get; set; }
    public bool Enabled { get; set; }
}
```

**Queries** (Read Operations):
```csharp
public class GetGamesRequest : IRequest<GamesViewModel>
{
    // Parameters for filtering
}
```

### 2. Repository Pattern

**Generic Repository**:
```csharp
public class GenericRepository<T> : IGenericRepository<T> where T : IDocument
{
    private readonly IMongoCollection<T> collection;

    public async Task InsertOneAsync(T document)
    {
        await collection.InsertOneAsync(document);
    }

    public IQueryable<T> AsQueryable()
    {
        return collection.AsQueryable();
    }
}
```

### 3. Factory Pattern

**Service Factory**:
```csharp
public class ReportServiceFactory
{
    public IGameReportService Create(GameType gameType)
    {
        return gameType switch
        {
            GameType.Slots => new SlotReportService(),
            GameType.Plinko => new PlinkoReportService(),
            _ => new DefaultReportService()
        };
    }
}
```

### 4. Strategy Pattern

**Game-Specific Logic**:
```csharp
public interface IGameProcessor
{
    Task ProcessGame(GameConfig game);
}

public class PlinkoGameProcessor : IGameProcessor
{
    public async Task ProcessGame(GameConfig game)
    {
        // Plinko-specific processing logic
    }
}
```

---

## Database Schema

### Collections

1. **game_configs** - Game configurations
2. **game_rounds** - Game round data
3. **bets** - Player bets
4. **jackpots** - Jackpot information
5. **void_logs** - Cancellation logs
6. **notifications** - System notifications

### Entity Relationships

```
GameConfigEntity (1) ──── (Many) GameRoundEntity
    │                           │
    └── PlinkoConfig            └── BetEntity
                                    │
                                    └── PlayerEntity
```

---

## API Endpoints

### Game Management

```
GET    /api/game_settings           # Get all games
POST   /api/game_settings           # Create new game
GET    /api/game_settings/{name}    # Get game by name
PUT    /api/game_settings/status    # Update game status
POST   /api/game_settings/odds      # Update game odds
```

### Void Operations

```
POST   /api/void_game               # Void/cancel game round
POST   /api/void_running_ticket     # Void running ticket
```

### Reporting

```
GET    /api/report/games            # Game reports
GET    /api/report/bets             # Bet reports
GET    /api/report/players          # Player reports
```

---

## Client-Side Architecture

### JavaScript Modules

**Game Management** (`game-status.js`):
- Game enable/disable operations
- Status monitoring
- Client reload management

**Game Creation** (`add-new-game.js`):
- Form validation
- Dynamic field management
- Game type-specific logic

**Plinko Specific** (`game-status.js`):
- RTP calculations
- Odds validation
- Risk level management

### CSS/SCSS Structure

```
wwwroot/
├── css/
│   ├── game-management.min.css
│   ├── adjust-bot.min.css
│   └── game-status.min.css
├── scss/
│   ├── pages/
│   ├── components/
│   └── utilities/
```

---

## Testing Strategy

### Unit Test Structure

```
Tests/
├── GamesAdmin.Api.UnitTests/
│   ├── VoidGame/
│   └── Controllers/
└── GamesAdmin.Site.UnitTests/
    ├── Features/
    └── Extensions/
```

### Test Categories

1. **Unit Tests** - Individual method testing
2. **Integration Tests** - API endpoint testing
3. **UI Tests** - Frontend functionality testing

---

## Deployment & Configuration

### Environment Configuration

**appsettings.json** hierarchy:
- `appsettings.json` (base)
- `appsettings.{Environment}.json` (environment-specific)
- `appsettings.AWS.json` (AWS-specific)

### Docker Configuration

```dockerfile
# AdminApi.Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
COPY . /app
WORKDIR /app
EXPOSE 80
ENTRYPOINT ["dotnet", "GamesAdmin.Api.dll"]
```

---

## Monitoring & Observability

### Error Tracking

**Sentry Integration**:
```csharp
public class SentryClient : ISentryClient
{
    public void CaptureException(Exception ex)
    {
        SentrySdk.CaptureException(ex);
    }
}
```

### Logging

**Structured Logging**:
```csharp
logger.LogInformation("Game created: {GameName}, ID: {GameId}",
    game.Info.Name, game.Info.GameId);
```

### Health Checks

**Endpoint Monitoring**:
```csharp
public class HealthCheckController : Controller
{
    [HttpGet("/health")]
    public IActionResult Health() => Ok("Healthy");
}
```

---

## Security Considerations

### Authentication

- ASP.NET Core Identity
- JWT Bearer tokens for API
- Cookie authentication for web UI

### Authorization

- Role-based access control
- Game-specific permissions
- Admin-only operations

### Input Validation

- Data annotations on models
- Custom validation attributes
- Server-side validation before database operations

---

## Performance Optimizations

### Caching Strategy

- In-memory caching for game configurations
- Distributed caching for multi-instance deployments
- Cache invalidation on configuration changes

### Database Optimization

- Indexed queries on frequently accessed fields
- Projection for large result sets
- Async operations throughout the stack

### Frontend Optimization

- Minified CSS/JS bundles
- Lazy loading for heavy components
- Efficient DOM manipulation

---

## Conclusion

The GamesAdmin system demonstrates a robust, scalable architecture for managing complex gaming operations. Key strengths include:

1. **Modular Design** - Clear separation of concerns across layers
2. **Extensibility** - Easy addition of new game types and features
3. **Maintainability** - Well-structured code with consistent patterns
4. **Performance** - Optimized data access and caching strategies
5. **Testability** - Comprehensive unit test coverage
6. **Monitoring** - Built-in error tracking and logging

The system successfully handles the complexity of managing multiple game types while maintaining clean, maintainable code through adherence to SOLID principles and established architectural patterns.
