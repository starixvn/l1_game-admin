# Shooting Blitz Game Setting - Development Requirements Document

> **Reference**: This document follows [AdminSite-CursorRules.md](./AdminSite-CursorRules.md) guidelines:
> - Single Endpoint + ActionType Pattern (Section 4.2)
> - Unified IGameSettingApi (Section 4.1)
> - No separate Controller/Handler/Service per feature (Section 7.1)

## 1. Game Basic Information

| Item | Value |
|------|-------|
| Name | `ShootingBlitz` |
| DisplayName | `Shooting Blitz` |
| GameId | `164` |
| GameType | `MinesRNGTurnBase = 15` (New Type) |

---

## 2. Database Architecture

### 2.1 Game_Configs Collection (General Settings)

- **Database**: `minigame`
- **Collection**: `game_configs`
- **New Field**: `shootingBlitzConfig`

```json
{
  "shootingBlitzConfig": {
    "rtpSetting": [
      {
        "key": "normal",
        "rtpConfigId": "ObjectId string"
      },
      {
        "key": "live",
        "rtpConfigId": "ObjectId string"
      }
    ],
    "scoreSetting": [
      {
        "homeScore": 1,
        "awayScore": 1,
        "probability": 0.5
      },
      {
        "homeScore": 2,
        "awayScore": 1,
        "probability": 0.3
      },
      {
        "homeScore": 1,
        "awayScore": 2,
        "probability": 0.2
      }
    ],
    "maxWinRound": 25
  }
}
```

> **Note**:
> - `rtpSetting`: Array of RTP mode configurations. Game Client sends `key`, Game Server returns corresponding RTP setting
>   - `key`: Mode identifier sent by Game Client (e.g., "normal", "live", "1", "2")
>   - `rtpConfigId`: ObjectId reference to `ShootingBlitz_rtpConfig` collection
> - `scoreSetting`: Random score generator for non-Live Score mode
>   - `homeScore` / `awayScore`: Predefined score combination
>   - `probability`: Selection probability for this score combination (all probabilities should sum to 1)
> - `maxWinRound`: Maximum win amount per round

### 2.2 RTP Config Collection (Standalone RTP Configuration)

- **Database**: `minigame`
- **Collection**: `ShootingBlitz_rtpConfig`

```json
{
  "_id": "ObjectId",
  "name": "RTP 95% Config",
  "multiplierSteps": [
    {
      "opponentCount": 4,
      "name": "star player rate 2% - 1 mines",
      "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
      "away": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
      "pays": [1, 1.06, 1.12, 1.19, 1.27, 1.36, 1.46, 1.58, 1.73, 1.9, 2.11, 2.37, 2.71, 3.17, 3.8, 4.75, 6.33, 9.5, 19]
    },
    {
      "opponentCount": 3,
      "name": "star player rate 2% - 2 mines",
      "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 5 },
      "away": { "starPlayerRate": 0.03, "startPosition": 3, "endPosition": 5 },
      "pays": [1.06, 1.18, 1.33, 1.52, 1.78, 2.14, 2.67, 3.56, 5.33]
    }
  ]
}
```

> **Note**: 
> - `name`: Display name for the RTP Config (e.g., "RTP 95% Config", "High RTP Config", "Live Mode Config")
> - `opponentCount` represents opponent count (equivalent to mine count), selecting opponent loses
> - `home` and `away` set star player settings for home and away teams respectively, can have different rates and position ranges
> - `startPosition` and `endPosition` define the range where star players can appear (e.g., startPosition: 3, endPosition: 6 means positions 3-6)
> - `pays` array index corresponds to picks value: `pays[0]` = picks 1 odds, `pays[1]` = picks 2 odds, and so on
> - Different `opponentCount` can have different `pays` array lengths

### 2.3 Teams Collection (Team Management)

- **Database**: `minigame`
- **Collection**: `ShootingBlitz_team`

```json
{
  "_id": "507f1f77bcf86cd799439011",
  "name": "FC Barcelona",
  "teamId": 12345,
  "isEnabled": true,
  "playerList": [
    {
      "name": "Lionel Messi",
      "playerNumber": 10
    },
    {
      "name": "Pedri",
      "playerNumber": 8
    }
  ]
}
```

> **Note**:
> - `isEnabled`: 控制此 Team 是否可被使用
>   - `true`: 啟用，Game Client 可取得此 Team
>   - `false`: 停用，Game Client 不會取得此 Team
>   - `null` 或欄位不存在: 視為 `true`（向後相容舊資料）
> - 新建立的 Team 預設 `isEnabled: true`

---

## 3. UI Requirements

### 3.1 UI Entry Points Overview

All ShootingBlitz settings are consolidated in the General Setting Modal:

| Entry Point | Location | Features |
|-------------|----------|----------|
| **General Setting** | Game Management → General Setting column (Edit button) | General Tab + Teams Tab + **RTP Tab** |
| **Odds** | Game Management → Odds column | **Empty (Not used)** |

> **Note**: 
> - This game has **NO Jackpot**
> - Odds column displays nothing for ShootingBlitz. RTP settings are integrated into General Setting Modal

### 3.2 General Setting Modal - Tab Structure

| Tab Name | Description |
|----------|-------------|
| General | General settings (game status, free bet, live score, delay time) |
| Teams | Team management, supports Create/Update/Delete teams and players |
| **RTP** | **RTP Config selection + CRUD management (previously in Odds Modal)** |

### 3.3 General Tab Fields

| Field | Type | Description |
|-------|------|-------------|
| Game Status | Display | Shows Enabled/Disabled |
| Game Type | Display | Shows MinesRNGTurnBase |
| Enable Free Bet | Checkbox | Whether to enable free betting |
| Delay Start Time | Number | Delay start time |
| Max Win Round | Number | Maximum win amount per round |

#### 3.3.1 RTP Setting Configuration

| Field | Type | Description |
|-------|------|-------------|
| Key | String | Mode identifier sent by Game Client (e.g., "normal", "live", "1", "2") |
| RTP Config | Dropdown | Select RTP Config (from `ShootingBlitz_rtpConfig`) |

- ✅ Can dynamically add/remove RTP Setting entries
- ✅ Each entry has a unique `key` for Game Client to reference
- ✅ Use different `key` values to distinguish Live and Non-Live modes (e.g., "live" vs "normal")

#### 3.3.2 Score Setting Configuration (Non-Live Score Mode)

| Field | Type | Description |
|-------|------|-------------|
| Home Score | Number | Home team score |
| Away Score | Number | Away team score |
| Probability | Number (0-1) | Selection probability for this score combination |

- ✅ Can dynamically add/remove score combinations
- ✅ Sum of all probabilities should equal 1
- ✅ Used for random score generation in non-Live Score mode

### 3.4 Teams Tab Features

#### 3.4.1 List Display

| Field | Description |
|-------|-------------|
| Team ID | Team ID |
| Team Name | Team name |
| **Enabled** | **是否啟用（✓ 或 ✗）** |
| Player Count | Number of players |
| Actions | Edit / Delete buttons |

> **Display Rules**:
> - 顯示資料庫中**所有** Team（不過濾）
> - 停用的 Team 建議以灰色背景或刪除線樣式區分
> - **不需要**過濾 Dropdown（Game API 會自行過濾給 Game Client）

#### 3.4.2 Create/Edit Team

##### Team Basic Information
| Field | Type | Description |
|-------|------|-------------|
| teamId | Number | Team ID (unique identifier) |
| name | String | Team name |
| **isEnabled** | **Checkbox** | **是否啟用此 Team（預設勾選）** |

##### Player List Configuration
| Field | Type | Description |
|-------|------|-------------|
| name | String | Player name |
| playerNumber | Number | Player jersey number |

- ✅ Can dynamically add/remove players
- ✅ Each Team is one record
- ✅ Supports Create/Update/Delete Team
- ✅ **新增的 Team 預設 `isEnabled: true`**

### 3.5 RTP Tab Features (in General Setting Modal)

#### 3.5.1 Game Management - Odds Column Display

| Item | Description |
|------|-------------|
| ShootingBlitz | **Empty (Odds feature not used)** |
| Other Games | Maintain existing Odds functionality |

> **Note**: ShootingBlitz RTP settings are integrated into the RTP Tab of General Setting Modal

#### 3.5.2 RTP Tab Layout

RTP Tab contains the following feature sections:

##### Current RTP Selection
| Field | Type | Description |
|-------|------|-------------|
| Selected RTP Config | Dropdown + Display | Shows current selected RTP Config, allows changing selection |
| Apply Selection | Button | Apply the selected RTP Config |

##### RTP Config List
| Field | Description |
|-------|-------------|
| Name | Display config name (e.g., "RTP 95% Config") |
| Multiplier Steps | Display number of modes |
| Actions | Edit / Delete buttons |
| Create New | Button to create new RTP Config |

##### Create/Edit RTP Config (Sub-Modal)

###### Name Field
| Field | Type | Validation |
|-------|------|------------|
| Name | String | Required, display name for the config |

###### Multiplier Steps Configuration (Mode Tabs)
| Field | Type | Description |
|-------|------|-------------|
| opponentCount | Number | Opponent count (equivalent to mine count, selecting opponent loses) |
| name | String | Mode name (e.g., "star player rate 2% - 1 mines") |
| home.starPlayerRate | Number | Home team star player appearance rate |
| home.startPosition | Number | Home team star player start position (range start) |
| home.endPosition | Number | Home team star player end position (range end) |
| away.starPlayerRate | Number | Away team star player appearance rate |
| away.startPosition | Number | Away team star player start position (range start) |
| away.endPosition | Number | Away team star player end position (range end) |
| pays | Array<Number> | Multiplier pays array (index+1 = picks) |

- ✅ Can dynamically add/remove Mode
- ✅ Can dynamically add/remove pays items
- ✅ Picks field auto-displays based on pays array index (index + 1)
- ✅ Different opponentCount can have different pays array lengths
- ✅ Home and Away can have different star player rates and positions

###### Pays Array UI Display
| Picks (Auto) | Pays (Input) | Actions |
|--------------|--------------|---------|
| 1 | [Input: 1.00] | [Remove] |
| 2 | [Input: 1.06] | [Remove] |
| 3 | [Input: 1.12] | [Remove] |
| ... | ... | ... |

> **Note**: Picks field is auto-displayed (read-only), users only need to input Pays values

---

## 4. API Design

> **Important**: Following AdminSite-CursorRules.md - Single Endpoint Per Game + ActionType Pattern

### 4.1 Unified ShootingBlitz Config API

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/game_settings/update_shooting_blitz_config` | **Single endpoint** for all ShootingBlitz operations |

### 4.2 ActionType Enum

```csharp
public enum ShootingBlitzActionType
{
    UpdateConfig = 0,       // Update game config (rtpSetting, scoreSetting, maxWinRound)
    QueryRtpConfigs = 1,    // Query all RTP configs
    CreateRtpConfig = 2,    // Create RTP config
    UpdateRtpConfig = 3,    // Update RTP config
    DeleteRtpConfig = 4,    // Delete RTP config
    QueryTeams = 5,         // Query all Teams
    CreateTeam = 6,         // Create Team
    UpdateTeam = 7,         // Update Team
    DeleteTeam = 8          // Delete Team
}
```

### 4.3 Unified Request

```csharp
public class UpdateShootingBlitzConfigRequest
{
    public ShootingBlitzActionType ActionType { get; set; }
    public string Id { get; set; }                              // For Update, Delete operations
    public ShootingBlitzConfig Config { get; set; }             // For UpdateConfig (rtpSetting, scoreSetting, maxWinRound)
    public ShootingBlitzRtpConfigData RtpConfig { get; set; }   // For RTP CRUD (name, multiplierSteps)
    public ShootingBlitzTeamData Team { get; set; }             // For Team CRUD
}
```

### 4.4 Unified Response

```csharp
public class ShootingBlitzConfigResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public ShootingBlitzRtpConfig RtpConfig { get; set; }           // Single RTP result
    public IList<ShootingBlitzRtpConfig> RtpConfigs { get; set; }   // RTP list result
    public ShootingBlitzTeam Team { get; set; }                     // Single Team result
    public IList<ShootingBlitzTeam> Teams { get; set; }             // Team list result
}
```

### 4.5 Request Examples

#### UpdateConfig (ActionType = 0)
```json
{
  "actionType": 0,
  "config": {
    "rtpSetting": [
      { "key": "normal", "rtpConfigId": "6123456789abcdef01234567" },
      { "key": "live", "rtpConfigId": "6123456789abcdef01234568" }
    ],
    "scoreSetting": [
      { "homeScore": 1, "awayScore": 1, "probability": 0.5 },
      { "homeScore": 2, "awayScore": 1, "probability": 0.3 },
      { "homeScore": 1, "awayScore": 2, "probability": 0.2 }
    ],
    "maxWinRound": 25
  }
}
```

#### QueryRtpConfigs (ActionType = 1)
```json
{
  "actionType": 1
}
```

#### CreateRtpConfig (ActionType = 2)
```json
{
  "actionType": 2,
  "rtpConfig": {
    "name": "RTP 95% Config",
    "multiplierSteps": [
      {
        "opponentCount": 4,
        "name": "star player rate 2% - 1 mines",
        "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
        "away": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
        "pays": [1, 1.06, 1.12, 1.19, 1.27]
      }
    ]
  }
}
```

#### UpdateRtpConfig (ActionType = 3)
```json
{
  "actionType": 3,
  "id": "6123456789abcdef01234567",
  "rtpConfig": {
    "name": "RTP 96% Config",
    "multiplierSteps": [...]
  }
}
```

#### DeleteRtpConfig (ActionType = 4)
```json
{
  "actionType": 4,
  "id": "6123456789abcdef01234567"
}
```

#### QueryTeams (ActionType = 5)
```json
{
  "actionType": 5
}
```

#### CreateTeam (ActionType = 6)
```json
{
  "actionType": 6,
  "team": {
    "teamId": 12345,
    "name": "FC Barcelona",
    "isEnabled": true,
    "playerList": [
      { "name": "Lionel Messi", "playerNumber": 10 },
      { "name": "Pedri", "playerNumber": 8 }
    ]
  }
}
```

> **Note**: `isEnabled` 預設為 `true`，可省略

#### UpdateTeam (ActionType = 7)
```json
{
  "actionType": 7,
  "id": "507f1f77bcf86cd799439011",
  "team": {
    "teamId": 12345,
    "name": "FC Barcelona",
    "isEnabled": false,
    "playerList": [...]
  }
}
```

#### DeleteTeam (ActionType = 8)
```json
{
  "actionType": 8,
  "id": "507f1f77bcf86cd799439011"
}
```

> **Note**: 
> - All operations use **single endpoint** with `ActionType` to distinguish
> - Both `rtpConfigId` and `rtpliveConfigId` reference the same `ShootingBlitz_rtpConfig` collection

---

## 5. Development File List

### 5.1 GamesAdmin.Core (Core Layer)

| File | Action | Description |
|------|--------|-------------|
| `Enumeration/GameType.cs` | Modify | Add `MinesRNGTurnBase = 15` |
| `Enumeration/GameId.cs` | Modify | Add `ShootingBlitz = 164` |
| `Models/GameConfigs/ShootingBlitzConfig.cs` | **New** | GameConfig internal settings Model (includes RtpSetting, ScoreSetting, MaxWinRound) |
| `Models/GameConfigs/ShootingBlitzRtpSetting.cs` | **New** | RTP Setting Model (key, rtpConfigId) |
| `Models/GameConfigs/ShootingBlitzScoreSetting.cs` | **New** | Score Setting Model (homeScore, awayScore, probability) |
| `Models/GameConfigs/ShootingBlitzRtpConfig.cs` | **New** | Standalone RTP Config Model (name, multiplierSteps) |
| `Models/GameConfigs/ShootingBlitzTeam.cs` | **New** | Team Model (including Player) |
| `Models/GameConfigs/GameConfig.cs` | Modify | Add ShootingBlitzConfig property |

### 5.2 GamesAdmin.Database (Data Layer)

| File | Action | Description |
|------|--------|-------------|
| `Entities/GameConfigs/GameConfigEntity.cs` | Modify | Add ShootingBlitzConfig property |
| `Entities/ShootingBlitz/ShootingBlitzRtpConfigEntity.cs` | **New** | RTP Config Entity (shared by normal and live mode) |
| `Entities/ShootingBlitz/ShootingBlitzTeamEntity.cs` | **New** | Team Entity |

### 5.3 GamesAdmin.Api (API Layer)

> **Note**: Following AdminSite-CursorRules.md - Single Endpoint + Unified Files

| File | Action | Description |
|------|--------|-------------|
| `GameSettings/Requests/UpdateShootingBlitzConfigRequest.cs` | **New** | Unified Request (ActionType + Config + RtpConfig + Team) |
| `GameSettings/Requests/ShootingBlitzConfigResult.cs` | **New** | Unified Response |
| `GameSettings/Requests/ShootingBlitzActionType.cs` | **New** | ActionType Enum |
| `GameSettings/GameSettingsController.cs` | Modify | Add single endpoint for ShootingBlitz |
| `GameSettings/GameSettingsHandler.cs` | Modify | Add Handler with ActionType dispatch |
| `GameSettings/GameSettingsService.cs` | Modify | Add methods for each ActionType |
| `_Shared/AutoMapping.cs` | Modify | Add mappings |
| `Startup.cs` | Modify | Register services |

> **Important**: Do NOT create separate Controller/Handler/Service for RTP or Team - use unified approach

### 5.4 GamesAdmin.Site (UI Layer)

> **Note**: Following AdminSite-CursorRules.md - Unified IGameSettingApi

| File | Action | Description |
|------|--------|-------------|
| `Features/GameSettings/ViewModels/EditViewModel.cs` | Modify | Add ShootingBlitzConfig, RtpConfigs, Teams properties |
| `Features/GameSettings/GamesHandler.cs` | Modify | Add Handler for ShootingBlitz operations |
| `Features/GameSettings/GameSettingService.cs` | Modify | Add methods calling unified API |
| `Features/GameSettings/IGameSettingApi.cs` | Modify | Add single `UpdateShootingBlitzConfig` method |
| `Features/GameSettings/Requests/UpdateShootingBlitzConfigSiteRequest.cs` | **New** | Site Request (ActionType + Config + RtpConfig + Team) |
| `Features/GameSettings/Views/GameDetails_ShootingBlitz.cshtml` | Modify | General Setting Modal (General + Teams + **RTP** Tabs) |
| `Features/GameSettings/Views/OddSettings.cshtml` | Modify | Remove ShootingBlitz branch (Odds column displays empty) |
| `Features/GameSettings/Views/OddsSettings_ShootingBlitz.cshtml` | **Delete** | Integrated into GameDetails_ShootingBlitz.cshtml |
| `wwwroot/js/shooting-blitz-general.js` | **New** | JavaScript for General Tab (RTP Setting + Score Setting + MaxWinRound) |
| `wwwroot/js/shooting-blitz-teams.js` | **New** | JavaScript for Teams Tab (Team CRUD) |
| `wwwroot/js/shooting-blitz-rtp.js` | **New** | JavaScript for RTP Tab (RTP Config CRUD) |

> **Important**: Do NOT create separate `IShootingBlitzRtpApi.cs` or `IShootingBlitzTeamApi.cs` - add to unified `IGameSettingApi.cs`

---

## 6. Validation Rules

| Item | Rule | Status |
|------|------|--------|
| rtpConfig.name | Required, non-empty string | ✅ Validation required |
| pays array | Must have at least 1 item | ✅ Validation required |
| teamId | Unique value | ✅ Validation required |
| team.isEnabled | Boolean (default: true) | ✅ Default value handling |
| playerNumber | Number | ✅ Validation required |
| rtpSetting.key | Unique within array | ✅ Validation required |
| rtpSetting.rtpConfigId | Required, valid ObjectId | ✅ Validation required |
| scoreSetting.probability | Sum of all probabilities should equal 1 | ✅ Validation required |
| scoreSetting.homeScore/awayScore | Non-negative integer | ✅ Validation required |
| maxWinRound | Positive number | ✅ Validation required |

---

## 7. Notes

1. **No Jackpot** - This game does not require Jackpot configuration
2. **Single Endpoint + ActionType Pattern** (Following AdminSite-CursorRules.md):
   - Use single API endpoint `/api/game_settings/update_shooting_blitz_config`
   - Use `ActionType` enum to distinguish operations (Config, RTP CRUD, Team CRUD)
   - Do NOT create separate endpoints for each CRUD operation
3. **Unified File Structure** (Following AdminSite-CursorRules.md):
   - Do NOT create separate Controller/Handler/Service for RTP or Team
   - Add to existing `GameSettingsController.cs`, `GamesHandler.cs`, `GameSettingService.cs`
   - Use unified `IGameSettingApi.cs`, do NOT create `IShootingBlitzRtpApi.cs`
4. **RTP Config Independent Management** - Can create multiple RTP Configs with different names (e.g., "RTP 95% Config", "Live Mode Config")
5. **RTP Config Structure**:
   - `name`: Display name for the config (replaces old `rtp` field)
   - `multiplierSteps`: Array of multiplier configurations
6. **RTP Setting Key Mechanism**:
   - Game Client sends `key` parameter to Game Server (e.g., "normal", "live")
   - Game Server looks up corresponding `rtpConfigId` from `rtpSetting` array
   - Use different `key` values to distinguish Live and Non-Live modes
7. **Score Setting (Non-Live Score Mode)**:
   - Configure multiple score combinations with probabilities
   - Game randomly selects score based on configured probabilities
   - All probabilities should sum to 1.0
8. **Max Win Round** - Maximum win amount per round
9. **Teams Independent Management** - Each Team is one record, supports independent CRUD
   - `isEnabled` field controls whether a Team is active
   - Default value is `true` for new Teams
   - `null` or missing field is treated as `true` (backward compatibility)
   - **Admin Site**: 顯示所有 Team（不過濾），列表包含 TeamId, TeamName, IsEnabled
   - **Game API**: 回傳給 Game Client 時只回傳 `isEnabled: true` 的 Team（Game API 負責過濾）
10. **MongoDB Collection Names**:
    - RTP Config: `ShootingBlitz_rtpConfig` (shared by normal and live mode)
    - Teams: `ShootingBlitz_team`
11. **Unified UI Entry Points**:
    - **General Setting** (Game Management → General Setting column): General config + Teams management + **RTP Config management**
    - **Odds** (Game Management → Odds column): **Not used for ShootingBlitz, displays empty**
12. **Pays Array Format**:
    - `pays` array index corresponds to picks value: `pays[0]` = picks 1, `pays[1]` = picks 2, ...
    - Picks field auto-displays in UI (read-only), users only need to input Pays values
    - Different opponentCount can have different pays array lengths
13. **Home/Away Star Player Settings**:
    - `home` and `away` can have different star player rates (`starPlayerRate`) and position ranges (`startPosition`, `endPosition`)
    - `startPosition` and `endPosition` define the range where star players can appear
    - This allows home and away teams to have different gameplay characteristics
14. **Opponent Count**:
    - `opponentCount` represents opponent count, equivalent to "mine count" concept
    - Selecting opponent loses (similar to minesweeper game)

---

## 8. Sample Data

### 8.1 Multiple RTP Config Examples

```json
// RTP Config 1: Normal Mode
{
  "_id": ObjectId("6123456789abcdef01234567"),
  "name": "RTP 95% - Normal Mode",
  "multiplierSteps": [
    {
      "opponentCount": 4,
      "name": "star player rate 2% - 1 mines",
      "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
      "away": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
      "pays": [1, 1.06, 1.12, 1.19, 1.27, 1.36, 1.46, 1.58, 1.73, 1.9, 2.11, 2.37, 2.71, 3.17, 3.8, 4.75, 6.33, 9.5, 19]
    },
    {
      "opponentCount": 3,
      "name": "star player rate 2% - 2 mines",
      "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 5 },
      "away": { "starPlayerRate": 0.03, "startPosition": 3, "endPosition": 5 },
      "pays": [1.06, 1.18, 1.33, 1.52, 1.78, 2.14, 2.67, 3.56, 5.33]
    }
  ]
}

// RTP Config 2: Live Mode
{
  "_id": ObjectId("6123456789abcdef01234568"),
  "name": "RTP 96% - Live Mode",
  "multiplierSteps": [
    {
      "opponentCount": 4,
      "name": "star player rate 2% - 1 mines",
      "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
      "away": { "starPlayerRate": 0.025, "startPosition": 3, "endPosition": 6 },
      "pays": [1, 1.08, 1.16, 1.25, 1.35, 1.47, 1.62, 1.80, 2.03, 2.32]
    },
    {
      "opponentCount": 3,
      "name": "star player rate 2% - 2 mines",
      "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 5 },
      "away": { "starPlayerRate": 0.03, "startPosition": 3, "endPosition": 5 },
      "pays": [1.08, 1.22, 1.40, 1.65, 2.00, 2.55, 3.50]
    }
  ]
}

// RTP Config 3: High RTP
{
  "_id": ObjectId("6123456789abcdef01234569"),
  "name": "High RTP Config",
  "multiplierSteps": [...]
}
```

> **Note**: All RTP Configs are stored in the same `ShootingBlitz_rtpConfig` collection. Use `name` field for display purposes.

### 8.2 Game Config Example

```json
{
  "name": "ShootingBlitz",
  "displayName": "Shooting Blitz",
  "gameId": 164,
  "type": 15,
  "shootingBlitzConfig": {
    "rtpSetting": [
      {
        "key": "normal",
        "rtpConfigId": "6123456789abcdef01234567"
      },
      {
        "key": "live",
        "rtpConfigId": "6123456789abcdef01234568"
      },
      {
        "key": "high",
        "rtpConfigId": "6123456789abcdef01234569"
      }
    ],
    "scoreSetting": [
      {
        "homeScore": 1,
        "awayScore": 1,
        "probability": 0.4
      },
      {
        "homeScore": 2,
        "awayScore": 1,
        "probability": 0.25
      },
      {
        "homeScore": 1,
        "awayScore": 2,
        "probability": 0.25
      },
      {
        "homeScore": 0,
        "awayScore": 0,
        "probability": 0.1
      }
    ],
    "maxWinRound": 25
  }
}
```

> **Note**:
> - `key: "normal"`: Normal mode - uses "RTP 95% - Normal Mode" config
> - `key: "live"`: Live Score mode - uses "RTP 96% - Live Mode" config
> - `key: "high"`: High RTP mode - uses "High RTP Config"
> - `scoreSetting`: Probabilities sum to 1.0 (0.4 + 0.25 + 0.25 + 0.1 = 1.0)
> - Game Client sends `key` to select which RTP config to use

### 8.3 Teams Examples

```json
// Team 1: FC Barcelona (啟用)
{
  "_id": "507f1f77bcf86cd799439011",
  "name": "FC Barcelona",
  "teamId": 12345,
  "isEnabled": true,
  "playerList": [
    {
      "name": "Lionel Messi",
      "playerNumber": 10
    },
    {
      "name": "Pedri",
      "playerNumber": 8
    },
    {
      "name": "Gavi",
      "playerNumber": 6
    }
  ]
}

// Team 2: Real Madrid (啟用)
{
  "_id": "507f1f77bcf86cd799439012",
  "name": "Real Madrid",
  "teamId": 12346,
  "isEnabled": true,
  "playerList": [
    {
      "name": "Karim Benzema",
      "playerNumber": 9
    },
    {
      "name": "Luka Modric",
      "playerNumber": 10
    },
    {
      "name": "Vinicius Jr",
      "playerNumber": 7
    }
  ]
}

// Team 3: AC Milan (停用 - 維護中)
{
  "_id": "507f1f77bcf86cd799439013",
  "name": "AC Milan",
  "teamId": 12347,
  "isEnabled": false,
  "playerList": [...]
}
```

> **Note**: 
> - `isEnabled: true` - Game Client 可取得此 Team
> - `isEnabled: false` - Game Client 不會取得此 Team（可用於暫時停用或維護）

---

## 9. UI Operation Flow

### 9.1 Game Management List Display

```
Game Management List
├── ...
├── ShootingBlitz Row
│   ├── Name: ShootingBlitz
│   ├── Enabled: ✓
│   ├── Operator: 1W
│   ├── Bet Limit: ...
│   ├── Setting: [Edit Button]
│   ├── Odds: (Empty, not used)
│   └── General Setting: [Edit Button] ← Click to open General Setting Modal (includes RTP settings)
└── ...
```

> **Note**: ShootingBlitz Odds column displays empty. All settings (including RTP) are consolidated in General Setting Modal

### 9.2 General Setting Modal - Complete Operation Flow

```
General Setting Modal: ShootingBlitz
├── [General Tab]
│   ├── Game Status: Enabled (Display)
│   ├── Game Type: MinesRNGTurnBase (Display)
│   ├── Enable Free Bet: [Checkbox]
│   ├── Delay Start Time: [Number Input]
│   ├── Max Win Round: [Number Input]
│   │
│   ├── RTP Setting Section
│   │   ├── [Add RTP Setting] Button
│   │   └── RTP Setting Rows
│   │       ├── Key: [Text Input] (e.g., "normal", "live", "high")
│   │       ├── RTP Config: [Dropdown - ShootingBlitz_rtpConfig]
│   │       └── [Remove] Button
│   │
│   └── Score Setting Section (Non-Live Score Mode)
│       ├── [Add Score Setting] Button
│       └── Score Setting Rows
│           ├── Home Score: [Number Input]
│           ├── Away Score: [Number Input]
│           ├── Probability: [Number Input 0-1]
│           └── [Remove] Button
│
├── [Teams Tab]
│   ├── Display Teams List (Table) - **顯示所有 Team，不過濾**
│   │   ├── Team ID
│   │   ├── Team Name
│   │   ├── **Enabled (✓/✗)**
│   │   ├── Player Count
│   │   └── Actions (Edit / Delete)
│   │   └── (Disabled Teams: gray background style)
│   │
│   ├── [Create New Team] Button
│   │   └── Open Sub-Modal
│   │       ├── Team ID (Number Input)
│   │       ├── Team Name (Text Input)
│   │       ├── **Enabled [Checkbox] (Default: checked)**
│   │       ├── Player List Section
│   │       │   ├── [Add Player] Button
│   │       │   └── Player Rows
│   │       │       ├── Player Name (Text Input)
│   │       │       ├── Player Number (Number Input)
│   │       │       └── [Remove] Button
│   │       └── [Save] / [Cancel] Buttons
│   │
│   └── Edit Team (Same as Create Modal, pre-filled data)
│
├── [RTP Tab] ← Previously Odds Modal functionality integrated here
│   │
│   ├── RTP Config Section
│   │   ├── Display RTP Config List (Table)
│   │   │   ├── Name (e.g., "RTP 95% - Normal Mode")
│   │   │   ├── Multiplier Steps Count
│   │   │   └── Actions (Edit / Delete)
│   │   │
│   │   ├── [Create New RTP Config] Button
│   │   │   └── Open Sub-Modal
│   │   │       ├── Name (Text Input, e.g., "RTP 95% - Normal Mode")
│   │   │       ├── Multiplier Steps Section (Tabs)
│   │   │       │   ├── [Add Mode] Button
│   │   │       │   └── Mode Tabs
│   │   │       │       ├── Opponent Count (Number)
│   │   │       │       ├── Name (Text)
│   │   │       │       ├── Home Star Player Settings
│   │   │       │       │   ├── Star Player Rate
│   │   │       │       │   ├── Start Position
│   │   │       │       │   └── End Position
│   │   │       │       ├── Away Star Player Settings
│   │   │       │       │   ├── Star Player Rate
│   │   │       │       │   ├── Start Position
│   │   │       │       │   └── End Position
│   │   │       │       ├── Pays Table
│   │   │       │       │   ├── Picks (Auto) | Pays (Input) | Actions
│   │   │       │       │   ├── 1 | [Input] | [Remove]
│   │   │       │       │   ├── 2 | [Input] | [Remove]
│   │   │       │       │   ├── ...
│   │   │       │       │   └── [Add Pays] Button
│   │   │       │       └── [Remove Mode] Button
│   │   │       └── [Save] / [Cancel] Buttons
│   │   │
│   │   └── Edit RTP Config (Same as Create, pre-filled data)
│   │
│   └── (RTP changes saved immediately via AJAX, no need to wait for Modal Save)
│
│   > Note: Use different `key` values (e.g., "normal", "live") in RTP Setting
│   > to distinguish modes. All configs are from the same collection.
│
└── [Close] / [Save] Buttons (Save only saves General Tab settings)
```

---

## 10. Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                        Game Management Page                          │
├─────────────────────────────────────────────────────────────────────┤
│  Name          │ Enabled │ ... │ Setting │  Odds   │ General Setting│
├─────────────────────────────────────────────────────────────────────┤
│  ShootingBlitz │    ✓    │ ... │   [✎]   │  (Empty) │      [✎]       │
└─────────────────────────────────────────────────────────────────────┘
                                                          │
                                                          ▼
                                            ┌─────────────────────────┐
                                            │   General Setting       │
                                            │        Modal            │
                                            │                         │
                                            │ ┌─────────────────────┐ │
                                            │ │   General Tab       │ │
                                            │ │   - Game Status     │ │
                                            │ │   - Enable Free Bet │ │
                                            │ │   - Delay Time      │ │
                                            │ │   - Max Win Round   │ │
                                            │ │   - RTP Setting[]   │ │
                                            │ │   - Score Setting[] │ │
                                            │ └─────────────────────┘ │
                                            │                         │
                                            │ ┌─────────────────────┐ │
                                            │ │   Teams Tab         │ │
                                            │ │   - Teams List      │ │
                                            │ │   - CRUD Actions    │ │
                                            │ └─────────────────────┘ │
                                            │           │             │
                                            │           ▼             │
                                            │ ┌─────────────────────┐ │
                                            │ │   RTP Tab           │ │
                                            │ │   - RTP Config List │ │
                                            │ │   - CRUD Actions    │ │
                                            │ └─────────────────────┘ │
                                            └─────────────────────────┘
                                                          │
                            ┌─────────────────────────────┴─────────────────────────────┐
                            │                                                           │
                            ▼                                                           ▼
              ┌─────────────────────────┐                              ┌─────────────────────────┐
              │   ShootingBlitz_team    │                              │  ShootingBlitz_rtpConfig│
              │       (MongoDB)         │                              │       (MongoDB)         │
              └─────────────────────────┘                              │  (shared by normal and  │
                                                                       │   live mode selection)  │
                                                                       └─────────────────────────┘
```

---

## 11. RTP Config Data Model Comparison

### Old Format v1 (Deprecated)
```json
{
  "rtp": 0.95,
  "bombCount": [
    { "bombCount": 4, "probability": 0.5 },
    { "bombCount": 3, "probability": 0.5 }
  ],
  "multiplierSteps": [
    {
      "mode": 4,
      "name": "...",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5, 6],
      "multiplier": [
        { "picks": 1, "pays": 1 },
        { "picks": 2, "pays": 1.06 }
      ]
    }
  ]
}
```

### Old Format v2 (Deprecated)
```json
{
  "rtp": 0.95,
  "multiplierSteps": [
    {
      "mode": 4,
      "name": "...",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5, 6],
      "pays": [1, 1.06, 1.12, 1.19]
    }
  ]
}
```

### New Format (Current)
```json
{
  "name": "RTP 95% - Normal Mode",
  "multiplierSteps": [
    {
      "opponentCount": 4,
      "name": "...",
      "home": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
      "away": { "starPlayerRate": 0.02, "startPosition": 3, "endPosition": 6 },
      "pays": [1, 1.06, 1.12, 1.19]
    }
  ]
}
```

### Changes Summary (v1 → Current)
| Item | v1 | Current |
|------|-----|-----|
| rtp | ✅ Number (0-1) | ❌ **Removed** - Replaced by `name` |
| name | ❌ N/A | ✅ **New**: Display name string |
| bombCount | ✅ Required | ❌ **Removed** |
| multiplierSteps.multiplier | `[{ picks, pays }]` | ❌ **Removed** |
| multiplierSteps.pays | ❌ N/A | ✅ **New**: `[number]` array |

### Changes Summary (v2 → Current)
| Item | v2 | Current |
|------|-----|-----|
| rtp | ✅ Number (0-1) | ❌ **Removed** - Replaced by `name` |
| name | ❌ N/A | ✅ **New**: Display name string |
| mode | ✅ `mode` | ✅ **Renamed**: `opponentCount` |
| starPlayerRate | ✅ Single field | ✅ **Split**: `home.starPlayerRate`, `away.starPlayerRate` |
| starPosition | ✅ Single array | ✅ **Split & Renamed**: `home.startPosition`/`home.endPosition`, `away.startPosition`/`away.endPosition` |

> **Note**: 
> - `name` is now a display name string (e.g., "RTP 95% - Normal Mode", "Live Mode Config")
> - `pays[index]` corresponds to `picks = index + 1`
> - `opponentCount` represents opponent count (equivalent to mine count), selecting opponent loses
> - `home` and `away` can have different star player rates and position ranges
> - `startPosition` and `endPosition` define the range where star players can appear (e.g., startPosition: 3, endPosition: 6 means positions 3-6)

---

## 12. ShootingBlitzConfig Data Model Comparison

### Old Format (Deprecated)
```json
{
  "shootingBlitzConfig": {
    "useLiveScore": true,
    "rtpConfigId": "ObjectId string"
  }
}
```

### New Format (Current)
```json
{
  "shootingBlitzConfig": {
    "rtpSetting": [
      {
        "key": "normal",
        "rtpConfigId": "ObjectId string"
      },
      {
        "key": "live",
        "rtpConfigId": "ObjectId string"
      }
    ],
    "scoreSetting": [
      {
        "homeScore": 1,
        "awayScore": 1,
        "probability": 0.5
      }
    ],
    "maxWinRound": 25
  }
}
```

### Changes Summary
| Item | Old | New |
|------|-----|-----|
| useLiveScore | ✅ Boolean | ❌ **Removed** - Replaced by `rtpSetting.key` mechanism |
| rtpConfigId | ✅ Single ObjectId | ❌ **Moved** - Now in `rtpSetting[].rtpConfigId` |
| rtpSetting | ❌ N/A | ✅ **New**: Array of RTP mode configurations |
| rtpSetting[].key | ❌ N/A | ✅ **New**: Mode identifier (e.g., "normal", "live") |
| rtpSetting[].rtpConfigId | ❌ N/A | ✅ **New**: RTP Config ObjectId |
| scoreSetting | ❌ N/A | ✅ **New**: Random score generator for non-Live mode |
| maxWinRound | ❌ N/A | ✅ **New**: Maximum win amount per round |

> **Note**: 
> - Game Client sends `key` parameter to select RTP mode (e.g., "normal", "live")
> - Live/Non-Live mode is distinguished by the `key` value, not by separate config fields
> - `scoreSetting` provides random score generation for non-Live Score mode
