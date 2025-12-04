# Admin Site 開發規範 (Cursor Rules)

本文檔描述 `GamesAdmin.Site` 專案的開發規範，確保代碼一致性和架構正確性。

## 架構流程

```
Browser (JavaScript)
    ↓ AJAX Call
Controller (GameSettingsController)
    ↓ mediator.Send(SiteRequest)
Handler (GamesHandler)
    ↓ gameService.Method(SiteRequest)
Service (GameSettingService)
    ↓ 創建 ApiRequest，調用 Refit API
Refit API Interface (IXxxApi)
    ↓ HTTP Request
Backend API (GamesAdmin.Api)
```

## 1. Controller 規範

### 1.1 路由定義
- Site Controller 的路由**不需要** `/api` 前綴
- 前端 JavaScript 直接調用 Site Controller

```csharp
// ✅ 正確：Site Controller 路由
[HttpPost, Audit]
[Route("/shooting_blitz_rtp")]
public async Task<IActionResult> CreateShootingBlitzRtpConfig([FromBody] CreateShootingBlitzRtpSiteRequest request)

// ❌ 錯誤：不要在 Site Controller 使用 /api 前綴
[Route("/api/shooting_blitz_rtp")]
```

### 1.2 使用 MediatR
- Controller 必須使用 `mediator.Send()` 調用 Handler
- 不要直接調用 Service 或 API

```csharp
// ✅ 正確：使用 mediator.Send
public async Task<IActionResult> CreateShootingBlitzRtpConfig([FromBody] CreateShootingBlitzRtpSiteRequest request)
{
    var result = await mediator.Send(request);
    return Json(result);
}

// ❌ 錯誤：直接調用 Service
public async Task<IActionResult> CreateShootingBlitzRtpConfig([FromBody] CreateShootingBlitzRtpSiteRequest request)
{
    var result = await gameService.CreateShootingBlitzRtpConfig(request);
    return Json(result);
}
```

## 2. Handler 規範

### 2.1 Handler 只調用 Service
- Handler 接收 `*SiteRequest`
- Handler 調用 `gameService.Method(request)`
- Handler **不直接調用** Refit API

```csharp
// ✅ 正確：Handler 調用 Service
public Task<ShootingBlitzRtpConfig> Handle(CreateShootingBlitzRtpSiteRequest request, CancellationToken cancellationToken)
    => gameService.CreateShootingBlitzRtpConfig(request);

// ❌ 錯誤：Handler 直接調用 Refit API
public Task<ShootingBlitzRtpConfig> Handle(CreateShootingBlitzRtpSiteRequest request, CancellationToken cancellationToken)
    => shootingBlitzRtpApi.Create(apiRequest);
```

### 2.2 Handler 方法參數
- 只接收一個 Request 參數
- 不要拆解 Request 的屬性

```csharp
// ✅ 正確：傳遞整個 Request
=> gameService.CreateShootingBlitzRtpConfig(request);

// ❌ 錯誤：拆解 Request 屬性
=> gameService.CreateShootingBlitzRtpConfig(request.Rtp, request.MultiplierSteps);
```

### 2.3 EditRequest Handler 中設置所有 Config
- 在 `Handle(EditRequest...)` 方法中，必須設置所有遊戲的 Config

```csharp
// ✅ 正確：設置所有 Config
viewModel.CascadingSlotsConfig = game.CascadingSlotsConfig;
viewModel.SlotRNGConfig = game.SlotRNGConfig;
viewModel.CrashConfig = game.CrashConfig;
viewModel.PlinkoConfig = game.PlinkoConfig;
viewModel.ShootingBlitzConfig = game.ShootingBlitzConfig;  // 不要遺漏！

// ❌ 錯誤：遺漏某些 Config
viewModel.PlinkoConfig = game.PlinkoConfig;
// 缺少 ShootingBlitzConfig
```

## 3. Service 規範

### 3.1 Service Interface 使用 Site Request
- Service 介面接收 `*SiteRequest` 類型
- **不要**拆解成多個參數

```csharp
// ✅ 正確：使用 Site Request
public interface IGameSettingService
{
    Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request);
}

// ❌ 錯誤：拆解成多個參數
public interface IGameSettingService
{
    Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(double rtp, List<MultiplierStepsConfig> multiplierSteps);
}
```

### 3.2 Service Implementation 直接傳遞 Request
- Service 直接將 `*SiteRequest` 傳給 Refit API
- **不需要**創建額外的 `*ApiRequest` 類型

```csharp
// ✅ 正確：直接傳遞 Request
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
    => shootingBlitzRtpApi.Create(request);

public Task<bool> UpdateShootingBlitzRtpConfig(UpdateShootingBlitzRtpSiteRequest request)
    => shootingBlitzRtpApi.Update(request.Id, request);

// ❌ 錯誤：創建額外的 ApiRequest 類型
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
{
    var apiRequest = new CreateShootingBlitzRtpApiRequest { ... };  // 不需要！
    return shootingBlitzRtpApi.Create(apiRequest);
}
```

## 4. Refit API 規範

### 4.1 統一使用 IGameSettingApi
- 所有 Game Settings 相關的 API 都應該定義在 `IGameSettingApi` 中
- **不要**為每個功能創建獨立的 API Interface

### 4.2 每個遊戲使用單一配置端點
- 每個遊戲應該只有**一個**主要的配置端點
- 使用 `ActionType` 參數區分不同操作（CRUD）
- 這樣便於管理，特別是多人開發時

```csharp
// ✅ 正確：單一端點 + ActionType
public interface IGameSettingApi : IBaseAuthorizationApi
{
    [Post("/game_settings/update_shooting_blitz_config")]
    Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig([Body] UpdateShootingBlitzConfigRequest request);
}

// Request 中包含 ActionType
public class UpdateShootingBlitzConfigRequest
{
    public ShootingBlitzActionType ActionType { get; set; }  // Query, Create, Update, Delete, Select
    public string Id { get; set; }                           // 用於 Update, Delete, Select
    public ShootingBlitzConfig Config { get; set; }          // 遊戲配置
    public ShootingBlitzRtpConfigData RtpConfig { get; set; } // RTP 配置數據
}

// ❌ 錯誤：為每個操作創建獨立端點
[Get("/shooting_blitz_rtp")]
[Post("/shooting_blitz_rtp")]
[Put("/shooting_blitz_rtp/{id}")]
[Delete("/shooting_blitz_rtp/{id}")]
[Post("/shooting_blitz_rtp/select")]
```

### 4.3 ActionType 枚舉設計
```csharp
public enum ShootingBlitzActionType
{
    UpdateConfig = 0,      // 更新遊戲配置
    QueryRtpConfigs = 1,   // 查詢所有 RTP 配置
    CreateRtpConfig = 2,   // 創建 RTP 配置
    UpdateRtpConfig = 3,   // 更新 RTP 配置
    DeleteRtpConfig = 4,   // 刪除 RTP 配置
    SelectRtpConfig = 5    // 選擇/應用 RTP 配置
}
```

### 4.4 Response 設計
```csharp
public class ShootingBlitzConfigResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public ShootingBlitzRtpConfig RtpConfig { get; set; }           // 單一結果（Create/Update）
    public IList<ShootingBlitzRtpConfig> RtpConfigs { get; set; }   // 列表結果（Query）
}
```

## 5. Request 類型命名規範

| 層級 | 命名規範 | 範例 | 說明 |
|------|----------|------|------|
| Site Request | `*SiteRequest` | `CreateShootingBlitzRtpSiteRequest` | Site 層使用，也傳給 Refit API |
| Backend Request | `*Request` | `CreateShootingBlitzRtpConfigRequest` | Backend API 使用 |

> **注意**：不需要定義 `*ApiRequest` 類型，直接使用 `*SiteRequest`

## 6. 檔案位置規範

```
Features/GameSettings/
├── GameSettingsController.cs      # Controller
├── GamesHandler.cs                # MediatR Handler
├── GameSettingService.cs          # Service Interface & Implementation
├── IGameSettingApi.cs             # Refit API Interface（統一放這裡）
├── Requests/
│   ├── ShootingBlitzRtpRequests.cs  # Site Request 定義
│   └── ...
└── ViewModels/
    └── ...
```

> **注意**：不要為每個功能創建獨立的 `IXxxApi.cs`，統一在 `IGameSettingApi.cs` 中添加

## 7. 新增遊戲功能 Checklist

新增一個遊戲的配置功能時，請依序檢查：

### 7.1 使用單一端點 + ActionType 模式

- [ ] **ActionType 枚舉** - 定義該遊戲的操作類型枚舉
- [ ] **Request 類型** - 定義包含 ActionType 的統一 Request 類型
- [ ] **Result 類型** - 定義統一的 Response 類型
- [ ] **Refit API** - 在 `IGameSettingApi` 添加**一個**方法
- [ ] **Service** - 在 `IGameSettingService` / `GameSettingService` 添加**一個**方法
- [ ] **Handler** - 在 `GamesHandler` 添加**一個** `IRequestHandler`
- [ ] **Controller** - 添加**一個**路由方法
- [ ] **Backend API** - 在 API 端實現根據 ActionType 分派邏輯
- [ ] **EditRequest Handler** - 如有新 Config，在 `Handle(EditRequest...)` 中設置

### 7.2 範例：新增 NewGame 配置

```csharp
// 1. ActionType 枚舉
public enum NewGameActionType { UpdateConfig, QueryConfigs, CreateConfig, ... }

// 2. 統一 Request
public class UpdateNewGameConfigRequest
{
    public NewGameActionType ActionType { get; set; }
    public string Id { get; set; }
    public NewGameConfigData ConfigData { get; set; }
}

// 3. 統一 Result
public class NewGameConfigResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public NewGameConfig Config { get; set; }
    public IList<NewGameConfig> Configs { get; set; }
}

// 4. Refit API（只有一個方法）
[Post("/game_settings/update_newgame_config")]
Task<NewGameConfigResult> UpdateNewGameConfig([Body] UpdateNewGameConfigRequest request);
```

## 8. 常見錯誤

### 8.1 Handler 直接調用 API
```csharp
// ❌ 錯誤
return await shootingBlitzRtpApi.Create(request);

// ✅ 正確
return gameService.CreateShootingBlitzRtpConfig(request);
```

### 8.2 Service 創建額外的 API Request
```csharp
// ❌ 錯誤：不需要創建額外的 ApiRequest
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
{
    var apiRequest = new CreateShootingBlitzRtpApiRequest { ... };
    return shootingBlitzRtpApi.Create(apiRequest);
}

// ✅ 正確：直接傳遞 SiteRequest
public Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(CreateShootingBlitzRtpSiteRequest request)
    => shootingBlitzRtpApi.Create(request);
```

### 8.3 為每個操作創建獨立端點
```csharp
// ❌ 錯誤：每個操作都有獨立端點，難以管理
[Get("/shooting_blitz_rtp")]
Task<IList<ShootingBlitzRtpConfig>> GetAllShootingBlitzRtpConfigs();

[Post("/shooting_blitz_rtp")]
Task<ShootingBlitzRtpConfig> CreateShootingBlitzRtpConfig(...);

[Put("/shooting_blitz_rtp/{id}")]
Task<bool> UpdateShootingBlitzRtpConfig(...);

[Delete("/shooting_blitz_rtp/{id}")]
Task<bool> DeleteShootingBlitzRtpConfig(...);

// ✅ 正確：單一端點 + ActionType
[Post("/game_settings/update_shooting_blitz_config")]
Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig([Body] UpdateShootingBlitzConfigRequest request);
```

### 8.4 創建獨立的 API Interface
```csharp
// ❌ 錯誤：不需要為每個遊戲創建獨立的 API Interface
public interface IShootingBlitzRtpApi { ... }
public interface IPlinkoApi { ... }

// ✅ 正確：統一在 IGameSettingApi 中，每個遊戲一個方法
public interface IGameSettingApi
{
    Task<ShootingBlitzConfigResult> UpdateShootingBlitzConfig([Body] UpdateShootingBlitzConfigRequest request);
    Task<PlinkoConfigResult> UpdatePlinkoConfig([Body] UpdatePlinkoConfigRequest request);
}
```

### 8.4 前端 JavaScript 調用錯誤的 URL
```javascript
// ❌ 錯誤：前端不應該調用 /api 路徑
$.ajax({ url: '/api/shooting_blitz_rtp', ... });

// ✅ 正確：前端調用 Site Controller 路徑
$.ajax({ url: '/shooting_blitz_rtp', ... });
```

### 8.5 遺漏 Config 設置
```csharp
// ❌ 錯誤：在 Handle(EditRequest...) 中遺漏新增的 Config
viewModel.PlinkoConfig = game.PlinkoConfig;
// 忘記 ShootingBlitzConfig

// ✅ 正確：設置所有 Config
viewModel.PlinkoConfig = game.PlinkoConfig;
viewModel.ShootingBlitzConfig = game.ShootingBlitzConfig;
```

### 8.6 Service 方法拆解 Request 參數
```csharp
// ❌ 錯誤：拆解成多個參數
Task<bool> UpdateConfig(string id, double rtp, List<MultiplierStepsConfig> multiplierSteps);

// ✅ 正確：接收單一 Request 參數
Task<bool> UpdateConfig(UpdateShootingBlitzRtpSiteRequest request);
```

---

**最後更新**：2025-12-04

