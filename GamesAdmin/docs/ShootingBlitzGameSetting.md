# Shooting Blitz Game Setting - Development Requirements Document

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
    "useLiveScore": true,
    "rtpConfigId": "ObjectId string"
  }
}
```

### 2.2 RTP Config Collection (Standalone RTP Configuration)

- **Database**: `minigame`
- **Collection**: `ShootingBlitz_rtpConfig`

```json
{
  "_id": "ObjectId",
  "rtp": 0.95,
  "multiplierSteps": [
    {
      "mode": 4,
      "name": "star player rate 2% - 1 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5, 6],
      "pays": [1, 1.06, 1.12, 1.19, 1.27, 1.36, 1.46, 1.58, 1.73, 1.9, 2.11, 2.37, 2.71, 3.17, 3.8, 4.75, 6.33, 9.5, 19]
    },
    {
      "mode": 3,
      "name": "star player rate 2% - 2 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5],
      "pays": [1.06, 1.18, 1.33, 1.52, 1.78, 2.14, 2.67, 3.56, 5.33]
    }
  ]
}
```

> **Note**: 
> - `pays` 陣列的 index 對應 picks 值：`pays[0]` = picks 1 的賠率, `pays[1]` = picks 2 的賠率, 以此類推
> - 不同 mode 的 `pays` 陣列長度可以不同

### 2.3 Teams Collection (Team Management)

- **Database**: `minigame`
- **Collection**: `ShootingBlitz_team`

```json
{
  "_id": "507f1f77bcf86cd799439011",
  "name": "FC Barcelona",
  "teamId": 12345,
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

---

## 3. UI Requirements

### 3.1 UI Entry Points Overview

ShootingBlitz 的設定分為兩個入口：

| Entry Point | Location | Features |
|-------------|----------|----------|
| **General Setting** | Game Management → General Setting 欄位 (編輯按鈕) | General Tab + Teams Tab |
| **Odds** | Game Management → Odds 欄位 (編輯按鈕) | RTP Config 選擇 + CRUD 管理 |

> **Note**: This game has **NO Jackpot**

### 3.2 General Setting Modal - Tab Structure

| Tab Name | Description |
|----------|-------------|
| General | General settings (game status, free bet, live score, delay time) |
| Teams | Team management, supports Create/Update/Delete teams and players |

### 3.3 General Tab Fields

| Field | Type | Description |
|-------|------|-------------|
| Game Status | Display | Shows Enabled/Disabled |
| Game Type | Display | Shows MinesRNGTurnBase |
| Enable Free Bet | Checkbox | Whether to enable free betting |
| Use Live Score | Checkbox | Whether to use live score |
| Delay Start Time | Number | Delay start time |

> **Note**: RTP Config selection has been moved to Game Management → Odds

### 3.4 Teams Tab Features

#### 3.4.1 List Display

| Field | Description |
|-------|-------------|
| Team ID | Team ID |
| Team Name | Team name |
| Player Count | Number of players |
| Actions | Edit / Delete buttons |

#### 3.4.2 Create/Edit Team

##### Team Basic Information
| Field | Type | Description |
|-------|------|-------------|
| teamId | Number | Team ID (unique identifier) |
| name | String | Team name |

##### Player List Configuration
| Field | Type | Description |
|-------|------|-------------|
| name | String | Player name |
| playerNumber | Number | Player jersey number |

- ✅ Can dynamically add/remove players
- ✅ Each Team is one record
- ✅ Supports Create/Update/Delete Team

### 3.5 Game Management - Odds Column

#### 3.5.1 Odds Column Display in Game List

| Item | Description |
|------|-------------|
| Display Content | Show current selected RTP value (e.g., "95%") |
| No Selection | Display "-- Not Set --" or empty |
| Click Action | Open RTP Config Modal |

#### 3.5.2 Odds Modal Features

Click the edit button in the Odds column to open a **standalone Modal** with the following features:

##### Current RTP Selection
| Field | Type | Description |
|-------|------|-------------|
| Selected RTP Config | Dropdown + Display | Shows current selected RTP Config, allows changing selection |

##### RTP Config List
| Field | Description |
|-------|-------------|
| RTP | Display RTP value (e.g., 95%) |
| Multiplier Steps | Display number of modes |
| Actions | Edit / Delete buttons |

##### Create/Edit RTP Config (Sub-Modal or Expandable Section)

###### RTP Field
| Field | Type | Validation |
|-------|------|------------|
| RTP | Number (0-1) | **No validation** (temporarily) |

###### Multiplier Steps Configuration (Mode Tabs)
| Field | Type | Description |
|-------|------|-------------|
| mode | Number | Mode number (represents mines count) |
| name | String | Mode name (e.g., "star player rate 2% - 1 mines") |
| starPlayerRate | Number | Star player appearance rate |
| starPosition | Array<Number> | Pick positions where star player can appear |
| pays | Array<Number> | Multiplier pays array (index+1 = picks) |

- ✅ Can dynamically add/remove Mode
- ✅ Can dynamically add/remove pays items
- ✅ Picks 欄位自動依照 pays 陣列 index 顯示（index + 1）
- ✅ 不同 mode 的 pays 陣列長度可以不同

###### Pays Array UI Display
| Picks (Auto) | Pays (Input) | Actions |
|--------------|--------------|---------|
| 1 | [Input: 1.00] | [Remove] |
| 2 | [Input: 1.06] | [Remove] |
| 3 | [Input: 1.12] | [Remove] |
| ... | ... | ... |

> **Note**: Picks 欄位為自動顯示（唯讀），使用者只需輸入 Pays 值

---

## 4. API Design

### 4.1 Game Config API (General Settings)

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/game_settings/update_shooting_blitz_config` | Update ShootingBlitz general settings |

**Request Body:**
```json
{
  "name": "ShootingBlitz",
  "shootingBlitzConfig": {
    "useLiveScore": true
  }
}
```

> **Note**: `rtpConfigId` is managed separately via Odds API (see 4.2)

### 4.2 RTP Config CRUD API (Odds Feature)

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/shooting_blitz_rtp` | Get all RTP Configs |
| GET | `/api/shooting_blitz_rtp/{id}` | Get single RTP Config |
| POST | `/api/shooting_blitz_rtp` | Create RTP Config |
| PUT | `/api/shooting_blitz_rtp/{id}` | Update RTP Config |
| DELETE | `/api/shooting_blitz_rtp/{id}` | Delete RTP Config |

**Create/Update Request Body:**
```json
{
  "rtp": 0.95,
  "multiplierSteps": [
    {
      "mode": 4,
      "name": "star player rate 2% - 1 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5, 6],
      "pays": [1, 1.06, 1.12, 1.19, 1.27]
    },
    {
      "mode": 3,
      "name": "star player rate 2% - 2 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5],
      "pays": [1.06, 1.18, 1.33]
    }
  ]
}
```

### 4.3 RTP Config Selection API (Odds Feature)

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/shooting_blitz_rtp/select` | Select RTP Config for the game |

**Request Body:**
```json
{
  "name": "ShootingBlitz",
  "rtpConfigId": "6123456789abcdef01234567"
}
```

> This API updates the `rtpConfigId` in `shootingBlitzConfig` within `game_configs` collection

### 4.4 Teams CRUD API

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/shooting_blitz_team` | Get all Teams |
| GET | `/api/shooting_blitz_team/{id}` | Get single Team |
| POST | `/api/shooting_blitz_team` | Create Team |
| PUT | `/api/shooting_blitz_team/{id}` | Update Team |
| DELETE | `/api/shooting_blitz_team/{id}` | Delete Team |

**Create/Update Request Body:**
```json
{
  "teamId": 12345,
  "name": "FC Barcelona",
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

---

## 5. Development File List

### 5.1 GamesAdmin.Core (Core Layer)

| File | Action | Description |
|------|--------|-------------|
| `Enumeration/GameType.cs` | Modify | Add `MinesRNGTurnBase = 15` |
| `Enumeration/GameId.cs` | Modify | Add `ShootingBlitz = 164` |
| `Models/GameConfigs/ShootingBlitzConfig.cs` | **New** | GameConfig internal settings Model |
| `Models/GameConfigs/ShootingBlitzRtpConfig.cs` | **New** | Standalone RTP Config Model |
| `Models/GameConfigs/ShootingBlitzTeam.cs` | **New** | Team Model (including Player) |
| `Models/GameConfigs/GameConfig.cs` | Modify | Add ShootingBlitzConfig property |

### 5.2 GamesAdmin.Database (Data Layer)

| File | Action | Description |
|------|--------|-------------|
| `Entities/GameConfigs/GameConfigEntity.cs` | Modify | Add ShootingBlitzConfig property |
| `Entities/ShootingBlitz/ShootingBlitzRtpConfigEntity.cs` | **New** | RTP Config Entity |
| `Entities/ShootingBlitz/ShootingBlitzTeamEntity.cs` | **New** | Team Entity |

### 5.3 GamesAdmin.Api (API Layer)

| File | Action | Description |
|------|--------|-------------|
| `GameSettings/Requests/UpdateShootingBlitzConfigRequest.cs` | **New** | Update Config Request (General Settings only) |
| `GameSettings/Requests/ShootingBlitzRtpConfigRequests.cs` | **New** | RTP CRUD + Selection Requests |
| `GameSettings/Requests/ShootingBlitzTeamRequests.cs` | **New** | Team CRUD Requests |
| `GameSettings/ShootingBlitzRtpConfigController.cs` | **New** | RTP Controller (for Odds feature) |
| `GameSettings/ShootingBlitzRtpConfigHandler.cs` | **New** | RTP Handler |
| `GameSettings/ShootingBlitzRtpConfigService.cs` | **New** | RTP Service |
| `GameSettings/ShootingBlitzTeamController.cs` | **New** | Team Controller |
| `GameSettings/ShootingBlitzTeamHandler.cs` | **New** | Team Handler |
| `GameSettings/ShootingBlitzTeamService.cs` | **New** | Team Service |
| `GameSettings/GameSettingsController.cs` | Modify | Add endpoints |
| `GameSettings/GameSettingsHandler.cs` | Modify | Add Handler |
| `GameSettings/GameSettingsService.cs` | Modify | Add methods |
| `_Shared/AutoMapping.cs` | Modify | Add mappings |
| `Startup.cs` | Modify | Register services |

### 5.4 GamesAdmin.Site (UI Layer)

| File | Action | Description |
|------|--------|-------------|
| `Features/GameSettings/ViewModels/EditViewModel.cs` | Modify | Add properties |
| `Features/GameSettings/GamesHandler.cs` | Modify | Add configuration method |
| `Features/GameSettings/Views/GameDetails_ShootingBlitz.cshtml` | **New** | General Setting Modal (General + Teams Tabs) |
| `Features/GameSettings/Views/OddSettings.cshtml` | Modify | Add ShootingBlitz branch for Odds Modal |
| `Features/GameSettings/Views/_ShootingBlitzOddsModal.cshtml` | **New** | Odds Modal (RTP Config List + CRUD + Selection) |
| `Features/GameSettings/IGameSettingApi.cs` | Modify | Add API interface |
| `Features/GameSettings/IShootingBlitzRtpApi.cs` | **New** | RTP API interface |
| `Features/GameSettings/IShootingBlitzTeamApi.cs` | **New** | Team API interface |
| `Features/GameSettings/Requests/UpdateShootingBlitzConfigRequest.cs` | **New** | Site Request |
| `wwwroot/js/shooting-blitz-general.js` | **New** | JavaScript for General Setting Modal (Teams) |
| `wwwroot/js/shooting-blitz-odds.js` | **New** | JavaScript for Odds Modal (RTP Config) |

---

## 6. Validation Rules

| Item | Rule | Status |
|------|------|--------|
| RTP Range | 0-1 | ❌ No validation temporarily |
| pays array | Must have at least 1 item | ✅ Validation required |
| teamId | Unique value | ✅ Validation required |
| playerNumber | Number | ✅ Validation required |

---

## 7. Notes

1. **No Jackpot** - This game does not require Jackpot configuration
2. **RTP Config Independent Management** - Can create multiple RTP Configs (e.g., RTP 95%, 96%, 97%)
3. **Game Config References RTP Config** - Linked via `rtpConfigId`
4. **Teams Independent Management** - Each Team is one record, supports independent CRUD
5. **MongoDB Collection Names**:
   - RTP Config: `ShootingBlitz_rtpConfig`
   - Teams: `ShootingBlitz_team`
6. **UI Entry Points Separation**:
   - **General Setting** (Game Management → General Setting column): General config + Teams management
   - **Odds** (Game Management → Odds column): RTP Config selection + CRUD management
7. **Pays Array Format**:
   - `pays` 陣列的 index 對應 picks 值：`pays[0]` = picks 1, `pays[1]` = picks 2, ...
   - UI 中 Picks 欄位自動顯示（唯讀），使用者只需輸入 Pays 值
   - 不同 mode 的 pays 陣列長度可以不同

---

## 8. Sample Data

### 8.1 Multiple RTP Config Examples

```json
// RTP Config 1: 95%
{
  "_id": ObjectId("..."),
  "rtp": 0.95,
  "multiplierSteps": [
    {
      "mode": 4,
      "name": "star player rate 2% - 1 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5, 6],
      "pays": [1, 1.06, 1.12, 1.19, 1.27, 1.36, 1.46, 1.58, 1.73, 1.9, 2.11, 2.37, 2.71, 3.17, 3.8, 4.75, 6.33, 9.5, 19]
    },
    {
      "mode": 3,
      "name": "star player rate 2% - 2 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5],
      "pays": [1.06, 1.18, 1.33, 1.52, 1.78, 2.14, 2.67, 3.56, 5.33]
    }
  ]
}

// RTP Config 2: 96%
{
  "_id": ObjectId("..."),
  "rtp": 0.96,
  "multiplierSteps": [
    {
      "mode": 4,
      "name": "star player rate 2% - 1 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5, 6],
      "pays": [1, 1.08, 1.16, 1.25, 1.35, 1.47, 1.62, 1.80, 2.03, 2.32]
    },
    {
      "mode": 3,
      "name": "star player rate 2% - 2 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5],
      "pays": [1.08, 1.22, 1.40, 1.65, 2.00, 2.55, 3.50]
    }
  ]
}
```

### 8.2 Game Config Example

```json
{
  "name": "ShootingBlitz",
  "displayName": "Shooting Blitz",
  "gameId": 164,
  "type": 15,
  "shootingBlitzConfig": {
    "useLiveScore": true,
    "rtpConfigId": "6123456789abcdef01234567"
  }
}
```

### 8.3 Teams Examples

```json
// Team 1: FC Barcelona
{
  "_id": "507f1f77bcf86cd799439011",
  "name": "FC Barcelona",
  "teamId": 12345,
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

// Team 2: Real Madrid
{
  "_id": "507f1f77bcf86cd799439012",
  "name": "Real Madrid",
  "teamId": 12346,
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
```

---

## 9. UI Operation Flow

### 9.1 Game Management List - Odds Column Display

```
Game Management List
├── ...
├── ShootingBlitz Row
│   ├── Name: ShootingBlitz
│   ├── Enabled: ✓
│   ├── Operator: 1W
│   ├── Bet Limit: ...
│   ├── Setting: [Edit Button]
│   ├── Odds: "95%" or "-- Not Set --" [Edit Button] ← Click to open Odds Modal
│   └── General Setting: [Edit Button] ← Click to open General Setting Modal
└── ...
```

### 9.2 General Setting Modal - Teams Tab Operation Flow

```
General Setting Modal: ShootingBlitz
├── [General Tab]
│   ├── Game Status: Enabled (Display)
│   ├── Game Type: MinesRNGTurnBase (Display)
│   ├── Enable Free Bet: [Checkbox]
│   ├── Use Live Score: [Checkbox]
│   └── Delay Start Time: [Number Input]
│
├── [Teams Tab]
│   ├── Display Teams List (Table)
│   │   ├── Team ID
│   │   ├── Team Name
│   │   ├── Player Count
│   │   └── Actions (Edit / Delete)
│   │
│   ├── [Create New Team] Button
│   │   └── Open Sub-Modal
│   │       ├── Team ID (Number Input)
│   │       ├── Team Name (Text Input)
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
└── [Close] / [Save] Buttons
```

### 9.3 Odds Modal - RTP Config Operation Flow

```
Odds Modal: ShootingBlitz RTP Configuration
├── Current Selection Section
│   ├── Label: "Current RTP Config"
│   ├── Dropdown: [Select RTP Config ▼]
│   │   ├── -- Select RTP Config --
│   │   ├── RTP 95% (id: xxx)
│   │   ├── RTP 96% (id: yyy)
│   │   └── RTP 97% (id: zzz)
│   └── [Apply Selection] Button
│
├── RTP Config List Section
│   ├── Display RTP Config List (Table)
│   │   ├── RTP (%)
│   │   ├── Multiplier Steps Count
│   │   └── Actions (Edit / Delete)
│   │
│   ├── [Create New RTP Config] Button
│   │   └── Open Sub-Modal / Expandable Section
│   │       ├── RTP (Number Input)
│   │       ├── Multiplier Steps Section (Tabs)
│   │       │   ├── [Add Mode] Button
│   │       │   └── Mode Tabs
│   │       │       ├── Mode Number
│   │       │       ├── Name
│   │       │       ├── Star Player Rate
│   │       │       ├── Star Position (comma separated)
│   │       │       ├── Pays Table
│   │       │       │   ├── Picks (Auto) | Pays (Input) | Actions
│   │       │       │   ├── 1 | [Input] | [Remove]
│   │       │       │   ├── 2 | [Input] | [Remove]
│   │       │       │   ├── ...
│   │       │       │   └── [Add Pays] Button
│   │       │       └── [Remove Mode] Button
│   │       └── [Save] / [Cancel] Buttons
│   │
│   └── Edit RTP Config (Same as Create, pre-filled data)
│
└── [Close] Button
```

---

## 10. Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                        Game Management Page                          │
├─────────────────────────────────────────────────────────────────────┤
│  Name          │ Enabled │ ... │ Setting │  Odds   │ General Setting│
├─────────────────────────────────────────────────────────────────────┤
│  ShootingBlitz │    ✓    │ ... │   [✎]   │  95%[✎] │      [✎]       │
└─────────────────────────────────────────────────────────────────────┘
                                      │                   │
                                      ▼                   ▼
                    ┌─────────────────────┐    ┌─────────────────────┐
                    │    Odds Modal       │    │ General Setting     │
                    │                     │    │      Modal          │
                    │ ┌─────────────────┐ │    │                     │
                    │ │ Current RTP     │ │    │ ┌─────────────────┐ │
                    │ │ Selection       │ │    │ │  General Tab    │ │
                    │ │ [Dropdown ▼]    │ │    │ │  - Game Status  │ │
                    │ └─────────────────┘ │    │ │  - Enable Free  │ │
                    │                     │    │ │  - Use Live     │ │
                    │ ┌─────────────────┐ │    │ │  - Delay Time   │ │
                    │ │ RTP Config List │ │    │ └─────────────────┘ │
                    │ │ + CRUD Actions  │ │    │                     │
                    │ └─────────────────┘ │    │ ┌─────────────────┐ │
                    └─────────────────────┘    │ │  Teams Tab      │ │
                              │                │ │  - Teams List   │ │
                              ▼                │ │  - CRUD Actions │ │
                    ┌─────────────────────┐    │ └─────────────────┘ │
                    │  ShootingBlitz_     │    └─────────────────────┘
                    │  rtpConfig          │              │
                    │  (MongoDB)          │              ▼
                    └─────────────────────┘    ┌─────────────────────┐
                                               │  ShootingBlitz_team │
                                               │  (MongoDB)          │
                                               └─────────────────────┘
```

---

## 11. RTP Config Data Model Comparison

### Old Format (Deprecated)
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

### New Format (Current)
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

### Changes Summary
| Item | Old | New |
|------|-----|-----|
| bombCount | ✅ Required | ❌ **Removed** |
| multiplierSteps.multiplier | `[{ picks, pays }]` | ❌ **Removed** |
| multiplierSteps.pays | ❌ N/A | ✅ **New**: `[number]` array |

> **Note**: `pays[index]` corresponds to `picks = index + 1`
