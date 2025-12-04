# ShootingBlitz Odds Update - 修改文件清單

## 問題描述

在 Game Management 選擇 Shooting Blitz 遊戲，點擊 Odds，建立 RTP Config 後點擊 "Save" 沒有任何反應。

**根本原因**：前端 JavaScript 調用 `/api/shooting_blitz_rtp`，但 Site Controller 沒有這個路由。

**錯誤訊息**：
```
POST /api/shooting_blitz_rtp
Status: 404 Not Found
```

## 架構說明

```
Browser (JavaScript)
    ↓ POST /shooting_blitz_rtp (Site 路由)
Site Controller (GameSettingsController)
    ↓ shootingBlitzRtpApi.Create() (Refit)
    ↓ 發送到: http://localhost:7778/api/shooting_blitz_rtp
API Controller (ShootingBlitzRtpConfigController)
    ↓ Route: api/shooting_blitz_rtp
MongoDB
```

## 修改文件清單

### 1. 前端 JavaScript - 移除 `/api` 前綴

**文件**: `GamesAdmin/src/GamesAdmin.Site/wwwroot/js/shooting-blitz-odds.js`

**修改內容**：
- Line 40: `'/api/shooting_blitz_rtp/select'` → `'/shooting_blitz_rtp/select'`
- Line 228: `'/api/shooting_blitz_rtp/' + id` → `'/shooting_blitz_rtp/' + id`
- Line 228: `'/api/shooting_blitz_rtp'` → `'/shooting_blitz_rtp'`
- Line 251: `'/api/shooting_blitz_rtp/' + id` → `'/shooting_blitz_rtp/' + id`

### 2. Site Controller - 添加路由

**文件**: `GamesAdmin/src/GamesAdmin.Site/Features/GameSettings/GameSettingsController.cs`

**修改內容**：
1. 添加 `IShootingBlitzRtpApi` 依賴注入
2. 添加以下路由方法：
   - `POST /shooting_blitz_rtp` - CreateShootingBlitzRtpConfig
   - `PUT /shooting_blitz_rtp/{id}` - UpdateShootingBlitzRtpConfig
   - `DELETE /shooting_blitz_rtp/{id}` - DeleteShootingBlitzRtpConfig
   - `POST /shooting_blitz_rtp/select` - SelectShootingBlitzRtpConfig

## 不需要修改的文件

以下文件**不需要修改**，因為它們已經正確：

| 文件 | 說明 |
|------|------|
| `IShootingBlitzRtpApi.cs` | Refit API 定義正確 |
| `ShootingBlitzRtpConfigController.cs` | 後端 API 路由正確 |
| `ShootingBlitzRtpConfigHandler.cs` | Handler 邏輯正確 |
| `ShootingBlitzRtpConfigService.cs` | Service 邏輯正確 |
| `ServiceMiddleware.cs` | Refit 已註冊 |

## 驗證步驟

1. 重新編譯 Site 項目
2. 重新啟動 Site 項目
3. 訪問 Game Management → Shooting Blitz → Odds
4. 點擊 "Create New RTP Config"
5. 填寫 RTP 值和至少一個 Mode
6. 點擊 "Save"
7. 確認 RTP Config 成功創建

