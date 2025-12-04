# ShootingBlitz Odds Update - 修改文件清單

## 問題描述

在 Game Management 選擇 Shooting Blitz 遊戲，點擊 Odds，建立 RTP Config 後點擊 "Save" 沒有任何反應。

### 問題 1：404 Not Found
**根本原因**：前端 JavaScript 調用 `/api/shooting_blitz_rtp`，但 Site Controller 沒有這個路由。

**錯誤訊息**：
```
POST /api/shooting_blitz_rtp
Status: 404 Not Found
```

### 問題 2：400 Bad Request
**根本原因**：Refit API 使用 `ShootingBlitzRtpConfig` 作為參數，但後端 API 期望 `CreateShootingBlitzRtpConfigRequest`。

**錯誤訊息**：
```
Refit.ApiException
Response status code does not indicate success: 400 (Bad Request).
```

### 問題 3：不符合 CQRS 架構規範
**根本原因**：Site Controller 直接調用 `IShootingBlitzRtpApi`，應該使用 `mediator.Send()` 模式。

## 架構說明

### 正確架構（使用 MediatR）

```
Browser (JavaScript)
    ↓ POST /shooting_blitz_rtp (Site 路由)
Site Controller (GameSettingsController)
    ↓ mediator.Send(CreateShootingBlitzRtpRequest)
Site Handler (GamesHandler)
    ↓ shootingBlitzRtpApi.Create() (Refit)
    ↓ 發送到: http://localhost:7778/api/shooting_blitz_rtp
API Controller (ShootingBlitzRtpConfigController)
    ↓ Route: api/shooting_blitz_rtp
MongoDB
```

### 錯誤架構（直接調用 Refit）- 已棄用

```
Site Controller → shootingBlitzRtpApi.Create() (直接調用，不符合規範)
```

## 修改文件清單

### 1. 前端 JavaScript - 移除 `/api` 前綴 ✅ 已完成

**文件**: `GamesAdmin/src/GamesAdmin.Site/wwwroot/js/shooting-blitz-odds.js`

**修改內容**：
- `'/api/shooting_blitz_rtp/select'` → `'/shooting_blitz_rtp/select'`
- `'/api/shooting_blitz_rtp/' + id` → `'/shooting_blitz_rtp/' + id`
- `'/api/shooting_blitz_rtp'` → `'/shooting_blitz_rtp'`

### 2. Site Requests - 新增 Request 類 ✅ 已完成

**文件**: `GamesAdmin/src/GamesAdmin.Site/Features/GameSettings/Requests/ShootingBlitzRtpRequests.cs` (新增)

**新增內容**：
```csharp
public class CreateShootingBlitzRtpSiteRequest : IRequest<ShootingBlitzRtpConfig>
{
    public double Rtp { get; set; }
    public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
}

public class UpdateShootingBlitzRtpSiteRequest : IRequest<bool>
{
    public string Id { get; set; }
    public double Rtp { get; set; }
    public List<MultiplierStepsConfig> MultiplierSteps { get; set; }
}

public class DeleteShootingBlitzRtpSiteRequest : IRequest<bool>
{
    public string Id { get; set; }
}

public class SelectShootingBlitzRtpSiteRequest : IRequest<bool>
{
    public string Name { get; set; }
    public string RtpConfigId { get; set; }
}
```

### 3. Site Handler - 添加 Handler 方法 ✅ 已完成

**文件**: `GamesAdmin/src/GamesAdmin.Site/Features/GameSettings/GamesHandler.cs`

**修改內容**：
1. 實現 `IRequestHandler<CreateShootingBlitzRtpSiteRequest, ShootingBlitzRtpConfig>`
2. 實現 `IRequestHandler<UpdateShootingBlitzRtpSiteRequest, bool>`
3. 實現 `IRequestHandler<DeleteShootingBlitzRtpSiteRequest, bool>`
4. 實現 `IRequestHandler<SelectShootingBlitzRtpSiteRequest, bool>`

### 4. Site Controller - 修改為使用 mediator.Send ✅ 已完成

**文件**: `GamesAdmin/src/GamesAdmin.Site/Features/GameSettings/GameSettingsController.cs`

**修改內容**：
1. 移除 `IShootingBlitzRtpApi` 依賴注入
2. 修改路由方法使用 `mediator.Send()`：
   - `CreateShootingBlitzRtpConfig` → `mediator.Send(CreateShootingBlitzRtpSiteRequest)`
   - `UpdateShootingBlitzRtpConfig` → `mediator.Send(UpdateShootingBlitzRtpSiteRequest)`
   - `DeleteShootingBlitzRtpConfig` → `mediator.Send(DeleteShootingBlitzRtpSiteRequest)`
   - `SelectShootingBlitzRtpConfig` → `mediator.Send(SelectShootingBlitzRtpSiteRequest)`

### 5. Refit API - 參數類型 ✅ 已完成

**文件**: `GamesAdmin/src/GamesAdmin.Site/Features/GameSettings/IShootingBlitzRtpApi.cs`

**修改內容**：
1. 添加 `CreateShootingBlitzRtpApiRequest` 類
2. 添加 `UpdateShootingBlitzRtpApiRequest` 類
3. 修改 Refit 方法參數類型

### 6. 後端 Service - 修復 AutoMapper 問題 ✅ 已完成

**文件**: `GamesAdmin/src/GamesAdmin.Api/GameSettings/ShootingBlitzRtpConfigService.cs`

**問題**：AutoMapper 的 `ReverseMap()` 在創建新記錄時，會嘗試將 null 的 `Id` (string) 映射到 `ObjectId`，導致錯誤。

**修改內容**：手動創建 Entity，避免 AutoMapper 映射 Id 欄位。

### 7. Handler 錯誤日誌 ✅ 已完成

**文件**: `GamesAdmin/src/GamesAdmin.Site/Features/GameSettings/GamesHandler.cs`

**修改內容**：添加 Refit.ApiException 的詳細錯誤日誌，記錄 URL、狀態碼和錯誤內容。

## 不需要修改的文件

以下文件**不需要修改**，因為它們已經正確：

| 文件 | 說明 |
|------|------|
| `ShootingBlitzRtpConfigController.cs` | 後端 API 路由正確 |
| `ShootingBlitzRtpConfigHandler.cs` | 後端 Handler 邏輯正確 |
| `ServiceMiddleware.cs` | Refit 已註冊 |

## 參考範例

參考 `GamesHandler.cs` 中其他功能的實現方式：

```csharp
// Controller 調用方式
var result = await mediator.Send(new UpdateOddsRequest(...));

// Handler 實現方式
public async Task<bool> Handle(UpdateOddsRequest request, CancellationToken cancellationToken)
{
    return await gameSettingApi.UpdateOdds(new UpdateOddsApiRequest
    {
        Name = request.Name,
        // ... mapping
    });
}
```

## 驗證步驟

1. 重新編譯 Site 項目
2. 重新啟動 Site 項目
3. 訪問 Game Management → Shooting Blitz → Odds
4. 點擊 "Create New RTP Config"
5. 填寫 RTP 值和至少一個 Mode（點擊 "Add Mode"）
6. 點擊 "Save"
7. 確認 RTP Config 成功創建

