# Admin Site Development Guidelines (Cursor Rules)

This document describes the development guidelines for `GamesAdmin.Site` project, ensuring code consistency and architectural correctness.

## Architecture Flow

```
Browser (JavaScript)
    ↓ AJAX Call
Controller (GameSettingsController)
    ↓ mediator.Send(SiteRequest)
Handler (GamesHandler)
    ↓ gameService.Method(SiteRequest)
Service (GameSettingService)
    ↓ Create ApiRequest, call Refit API
Refit API Interface (IXxxApi)
    ↓ HTTP Request
Backend API (GamesAdmin.Api)
```

## 1. Controller Guidelines

### 1.1 Route Definition
- Site Controller routes **do NOT need** `/api` prefix
- Frontend JavaScript calls Site Controller directly

```csharp
// ✅ Correct: Site Controller route
[HttpPost, Audit]
[Route("/shooting_blitz_rtp")]
public async Task<IActionResult> CreateShootingBlitzRtpConfig([FromBody] CreateShootingBlitzRtpSiteRequest request)

// ❌ Wrong: Do not use /api prefix in Site Controller
[Route("/api/shooting_blitz_rtp")]
```

### 1.2 Use MediatR
- Controller must use `mediator.Send()` to call Handler
- Do not call Service or API directly

```csharp
// ✅ Correct: Use mediator.Send
public async Task<IActionResult> CreateShootingBlitzRtpConfig([FromBody] CreateShootingBlitzRtpSiteRequest request)
{
    var result = await mediator.Send(request);
    return Json(result);
}

// ❌ Wrong: Call Service directly
public async Task<IActionResult> CreateShootingBlitzRtpConfig([FromBody] CreateShootingBlitzRtpSiteRequest request)
{
    var result = await gameService.CreateShootingBlitzRtpConfig(request);
    return Json(result);
}
```

## 2. Handler Guidelines

### 2.1 Handler Only Calls Service
- Handler receives `*SiteRequest`
- Handler calls `gameService.Method(request)`
- Handler **does NOT call** Refit API directly

```csharp
// ✅ Correct: Handler calls Service
public Task<ShootingBlitzRtpConfig> Handle(CreateShootingBlitzRtpSiteRequest request, CancellationToken cancellationToken)
    => gameService.CreateShootingBlitzRtpConfig(request);

// ❌ Wrong: Handler calls Refit API directly
public Task<ShootingBlitzRtpConfig> Handle(CreateShootingBlitzRtpSiteRequest request, CancellationToken cancellationToken)
    => shootingBlitzRtpApi.Create(apiRequest);
```

### 2.2 Handler Method Parameters
- Only receive one Request parameter
- Do not decompose Request properties

```csharp
// ✅ Correct: Pass entire Request
=> gameService.CreateShootingBlitzRtpConfig(request);

// ❌ Wrong: Decompose Request properties
=> gameService.CreateShootingBlitzRtpConfig(request.Rtp, request.MultiplierSteps);
```

### 2.3 Set All Configs in EditRequest Handler
- In `Handle(EditRequest...)` method, must set all game Configs

```csharp
// ✅ Correct: Set all Configs
viewModel.CascadingSlotsConfig = game.CascadingSlotsConfig;
viewModel.SlotRNGConfig = game.SlotRNGConfig;
viewModel.CrashConfig = game.CrashConfig;
viewModel.PlinkoConfig = game.PlinkoConfig;
viewModel.ShootingBlitzConfig = game.ShootingBlitzConfig;  // Don't miss!

// ❌ Wrong: Missing some Configs
viewModel.PlinkoConfig = game.PlinkoConfig;
// Missing ShootingBlitzConfig
```

## 3. Service Guidelines

### 3.1 Service Interface Uses Site Request
- Service interface receives `*SiteRequest` type
- **Do NOT** decompose into multiple parameters

```csharp
// ✅ Correct: Use Site Request
public interface IGameSettingService
{
    Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request);
}

// ❌ Wrong: Decompose into multiple parameters
public interface IGameSettingService
{
    Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(double rtp, List<MultiplierStepsConfig> multiplierSteps);
}
```

### 3.2 Service Implementation Passes Request Directly
- Service passes `*SiteRequest` directly to Refit API
- **No need** to create additional `*ApiRequest` type

```csharp
// ✅ Correct: Pass Request directly
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
    => shootingBlitzRtpApi.Create(request);

public Task<bool> UpdateShootingBlitzRtpConfig(UpdateShootingBlitzRtpSiteRequest request)
    => shootingBlitzRtpApi.Update(request.Id, request);

// ❌ Wrong: Create additional ApiRequest type
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
{
    var apiRequest = new CreateShootingBlitzRtpApiRequest { ... };  // Not needed!
    return shootingBlitzRtpApi.Create(apiRequest);
}
```

## 4. Refit API Guidelines

### 4.1 Use Unified IGameSettingApi
- All Game Settings related APIs should be defined in `IGameSettingApi`
- **Do NOT** create separate API Interface for each feature

### 4.2 Single Endpoint Per Game
- Each game should have only **one** main configuration endpoint
- Use `ActionType` parameter to distinguish different operations (CRUD)
- This is easier to manage, especially in team development

```csharp
// ✅ Correct: Single endpoint + ActionType
public interface IGameSettingApi : IBaseAuthorizationApi
{
    [Post("/game_settings/update_shooting_blitz_config")]
    Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig([Body] UpdateShootingBlitzConfigRequest request);
}

// Request includes ActionType
public class UpdateShootingBlitzConfigRequest
{
    public ShootingBlitzActionType ActionType { get; set; }  // Query, Create, Update, Delete, Select
    public string Id { get; set; }                           // For Update, Delete, Select
    public ShootingBlitzConfig Config { get; set; }          // Game config
    public ShootingBlitzRtpConfigData RtpConfig { get; set; } // RTP config data
}

// ❌ Wrong: Create separate endpoint for each operation
[Get("/shooting_blitz_rtp")]
[Post("/shooting_blitz_rtp")]
[Put("/shooting_blitz_rtp/{id}")]
[Delete("/shooting_blitz_rtp/{id}")]
[Post("/shooting_blitz_rtp/select")]
```

### 4.3 ActionType Enum Design
```csharp
public enum ShootingBlitzActionType
{
    UpdateConfig = 0,      // Update game config
    QueryRtpConfigs = 1,   // Query all RTP configs
    CreateRtpConfig = 2,   // Create RTP config
    UpdateRtpConfig = 3,   // Update RTP config
    DeleteRtpConfig = 4,   // Delete RTP config
    SelectRtpConfig = 5    // Select/Apply RTP config
}
```

### 4.4 Response Design
```csharp
public class ShootingBlitzConfigResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public ShootingBlitzRtpConfig RtpConfig { get; set; }           // Single result (Create/Update)
    public IList<ShootingBlitzRtpConfig> RtpConfigs { get; set; }   // List result (Query)
}
```

## 5. Request Type Naming Convention

| Layer | Naming Convention | Example | Description |
|-------|-------------------|---------|-------------|
| Site Request | `*SiteRequest` | `CreateShootingBlitzRtpSiteRequest` | Used in Site layer, also passed to Refit API |
| Backend Request | `*Request` | `CreateShootingBlitzRtpConfigRequest` | Used in Backend API |

> **Note**: No need to define `*ApiRequest` type, use `*SiteRequest` directly

## 6. File Location Guidelines

```
Features/GameSettings/
├── GameSettingsController.cs      # Controller
├── GamesHandler.cs                # MediatR Handler
├── GameSettingService.cs          # Service Interface & Implementation
├── IGameSettingApi.cs             # Refit API Interface (unified location)
├── Requests/
│   ├── ShootingBlitzRtpRequests.cs  # Site Request definitions
│   └── ...
└── ViewModels/
    └── ...
```

> **Note**: Do not create separate `IXxxApi.cs` for each feature, add to unified `IGameSettingApi.cs`

## 7. New Game Feature Checklist

When adding a new game configuration feature, check the following in order:

### 7.1 Use Single Endpoint + ActionType Pattern

- [ ] **ActionType Enum** - Define action type enum for the game
- [ ] **Request Type** - Define unified Request type with ActionType
- [ ] **Result Type** - Define unified Response type
- [ ] **Refit API** - Add **one** method to `IGameSettingApi`
- [ ] **Service** - Add **one** method to `IGameSettingService` / `GameSettingService`
- [ ] **Handler** - Add **one** `IRequestHandler` to `GamesHandler`
- [ ] **Controller** - Add **one** route method
- [ ] **Backend API** - Implement ActionType dispatch logic in API
- [ ] **EditRequest Handler** - If new Config, set in `Handle(EditRequest...)`

### 7.2 Example: Adding NewGame Configuration

```csharp
// 1. ActionType Enum
public enum NewGameActionType { UpdateConfig, QueryConfigs, CreateConfig, ... }

// 2. Unified Request
public class UpdateNewGameConfigRequest
{
    public NewGameActionType ActionType { get; set; }
    public string Id { get; set; }
    public NewGameConfigData ConfigData { get; set; }
}

// 3. Unified Result
public class NewGameConfigResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public NewGameConfig Config { get; set; }
    public IList<NewGameConfig> Configs { get; set; }
}

// 4. Refit API (only one method)
[Post("/game_settings/update_newgame_config")]
Task<NewGameConfigResult> UpdateNewGameConfig([Body] UpdateNewGameConfigRequest request);
```

## 8. Common Mistakes

### 8.1 Handler Calls API Directly
```csharp
// ❌ Wrong
return await shootingBlitzRtpApi.Create(request);

// ✅ Correct
return gameService.CreateShootingBlitzRtpConfig(request);
```

### 8.2 Service Creates Additional API Request
```csharp
// ❌ Wrong: No need to create additional ApiRequest
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
{
    var apiRequest = new CreateShootingBlitzRtpApiRequest { ... };
    return shootingBlitzRtpApi.Create(apiRequest);
}

// ✅ Correct: Pass SiteRequest directly
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
    => shootingBlitzRtpApi.Create(request);
```

### 8.3 Create Separate Endpoint for Each Operation
```csharp
// ❌ Wrong: Separate endpoint for each operation, hard to manage
[Get("/shooting_blitz_rtp")]
Task<IList<ShootingBlitzRtpConfig>> GetAllShootingBlitzRtpConfigs();

[Post("/shooting_blitz_rtp")]
Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(...);

[Put("/shooting_blitz_rtp/{id}")]
Task<bool> UpdateShootingBlitzRtpConfig(...);

[Delete("/shooting_blitz_rtp/{id}")]
Task<bool> DeleteShootingBlitzRtpConfig(...);

// ✅ Correct: Single endpoint + ActionType
[Post("/game_settings/update_shooting_blitz_config")]
Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig([Body] UpdateShootingBlitzConfigRequest request);
```

### 8.4 Create Separate API Interface
```csharp
// ❌ Wrong: No need to create separate API Interface for each game
public interface IShootingBlitzRtpApi { ... }
public interface IPlinkoApi { ... }

// ✅ Correct: Unified in IGameSettingApi, one method per game
public interface IGameSettingApi
{
    Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig([Body] UpdateShootingBlitzConfigRequest request);
    Task<PlinkoConfigResult> UpdatePlinkoConfig([Body] UpdatePlinkoConfigRequest request);
}
```

### 8.5 Frontend JavaScript Calls Wrong URL
```javascript
// ❌ Wrong: Frontend should not call /api path
$.ajax({ url: '/api/shooting_blitz_rtp', ... });

// ✅ Correct: Frontend calls Site Controller path
$.ajax({ url: '/shooting_blitz_rtp', ... });
```

### 8.6 Missing Config Setting
```csharp
// ❌ Wrong: Missing newly added Config in Handle(EditRequest...)
viewModel.PlinkoConfig = game.PlinkoConfig;
// Forgot ShootingBlitzConfig

// ✅ Correct: Set all Configs
viewModel.PlinkoConfig = game.PlinkoConfig;
viewModel.ShootingBlitzConfig = game.ShootingBlitzConfig;
```

### 8.7 Service Method Decomposes Request Parameters
```csharp
// ❌ Wrong: Decompose into multiple parameters
Task<bool> UpdateConfig(string id, double rtp, List<MultiplierStepsConfig> multiplierSteps);

// ✅ Correct: Receive single Request parameter
Task<bool> UpdateConfig(UpdateShootingBlitzRtpSiteRequest request);
```

### 8.8 JSON Serialization Case Mismatch (Frontend/Backend)

ASP.NET Core MVC Controller 預設使用 `System.Text.Json` 回傳 JSON，屬性名稱為 **PascalCase**，但前端 JavaScript 期望 **camelCase**。

```csharp
// ❌ Wrong: Missing JsonPropertyName - Frontend receives { "Success": true }
public class ShootingBlitzConfigResult
{
    public bool Success { get; set; }      // JavaScript: result.success → undefined!
    public string Message { get; set; }
}

// ✅ Correct: Add JsonPropertyName - Frontend receives { "success": true }
using System.Text.Json.Serialization;

public class ShootingBlitzConfigResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }      // JavaScript: result.success → true ✓

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("rtpConfig")]
    public ShootingBlitzRtpConfig RtpConfig { get; set; }

    [JsonPropertyName("rtpConfigs")]
    public IList<ShootingBlitzRtpConfig> RtpConfigs { get; set; }
}
```

```javascript
// Frontend expects camelCase
$.ajax({
    url: '/shooting_blitz_config',
    success: function (result) {
        if (result.success) {  // ← Needs camelCase from backend
            showSuccess();
        } else {
            showError(result.message);
        }
    }
});
```

> **Rule**: 當建立新的 Response/Result 類別供 Controller 回傳給前端時，**必須**在所有屬性上加上 `[JsonPropertyName("camelCase")]` 屬性。

---

## 9. Game-Specific UI Structure

### 9.1 ShootingBlitz UI Architecture

ShootingBlitz uses a **consolidated UI structure** where all settings are in the General Setting Modal:

| Entry Point | Location | Features |
|-------------|----------|----------|
| **General Setting** | Game Management → General Setting column | General Tab + Teams Tab + **RTP Tab** |
| **Odds** | Game Management → Odds column | **Empty (Not used)** |

> **Important**: ShootingBlitz does NOT use the Odds column. RTP settings are integrated into the General Setting Modal's RTP Tab.

### 9.2 General Setting Modal Tab Structure (ShootingBlitz)

```
General Setting Modal: ShootingBlitz
├── [General Tab]     → Game status, Enable Free Bet, Use Live Score, Delay Time
├── [Teams Tab]       → Team CRUD management
└── [RTP Tab]         → RTP Config selection + CRUD (previously Odds Modal)
```

### 9.3 JavaScript File Organization

| File | Purpose |
|------|---------|
| `shooting-blitz-general.js` | General Tab + Teams Tab functionality |
| `shooting-blitz-rtp.js` | RTP Tab functionality (RTP Config CRUD) |

> Both JS files are loaded in `GameDetails_ShootingBlitz.cshtml`

### 9.4 Files to Update When Modifying ShootingBlitz

| File | Description |
|------|-------------|
| `GameDetails_ShootingBlitz.cshtml` | General Setting Modal (General + Teams + RTP Tabs) |
| `shooting-blitz-general.js` | JS for General/Teams Tabs |
| `shooting-blitz-rtp.js` | JS for RTP Tab |
| `OddSettings.cshtml` | Ensure ShootingBlitz branch returns empty |

### 9.5 Deprecated Files (Do Not Use)

| File | Reason |
|------|--------|
| `OddsSettings_ShootingBlitz.cshtml` | **Deleted** - Integrated into GameDetails_ShootingBlitz.cshtml |
| `shooting-blitz-odds.js` | **Renamed** to `shooting-blitz-rtp.js` |

---

**Last Updated**: 2025-12-05

