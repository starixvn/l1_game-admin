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
  "bombCount": [
    {
      "bombCount": 4,
      "probability": 0.5
    },
    {
      "bombCount": 3,
      "probability": 0.5
    }
  ],
  "multiplierSteps": [
    {
      "mode": 4,
      "name": "star player rate 2% - 1 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5, 6],
      "multiplier": [
        { "picks": 1, "pays": 1 },
        { "picks": 2, "pays": 1.06 },
        { "picks": 3, "pays": 1.12 },
        { "picks": 4, "pays": 1.19 },
        { "picks": 5, "pays": 1.27 },
        { "picks": 6, "pays": 1.36 },
        { "picks": 7, "pays": 1.46 },
        { "picks": 8, "pays": 1.58 },
        { "picks": 9, "pays": 1.73 },
        { "picks": 10, "pays": 1.9 },
        { "picks": 11, "pays": 2.11 },
        { "picks": 12, "pays": 2.37 },
        { "picks": 13, "pays": 2.71 },
        { "picks": 14, "pays": 3.17 },
        { "picks": 15, "pays": 3.8 },
        { "picks": 16, "pays": 4.75 },
        { "picks": 17, "pays": 6.33 },
        { "picks": 18, "pays": 9.5 },
        { "picks": 19, "pays": 19 }
      ]
    },
    {
      "mode": 3,
      "name": "star player rate 2% - 2 mines",
      "starPlayerRate": 0.02,
      "starPosition": [3, 4, 5],
      "multiplier": [
        { "picks": 1, "pays": 1.06 },
        { "picks": 2, "pays": 1.18 },
        { "picks": 3, "pays": 1.33 }
        // ... more entries
      ]
    }
  ]
}
```

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

### 3.1 Tab Structure

| Tab Name | Description |
|----------|-------------|
| General | General settings (similar to other games) |
| RTP Config | Replaces Bot Config Tab, manages RTP configurations |
| Teams | Team management, supports Create/Update/Delete teams and players |

> **Note**: This game has **NO Jackpot**

### 3.2 General Tab Fields

| Field | Type | Description |
|-------|------|-------------|
| Game Status | Display | Shows Enabled/Disabled |
| Game Type | Display | Shows MinesRNGTurnBase |
| Enable Free Bet | Checkbox | Whether to enable free betting |
| Use Live Score | Checkbox | Whether to use live score |
| Delay Start Time | Number | Delay start time |
| RTP Config | Dropdown | Select RTP configuration (read from RTP Config Collection) |

### 3.3 RTP Config Tab Features

#### 3.3.1 List Display

| Field | Description |
|-------|-------------|
| RTP | Display RTP value (e.g., 95%) |
| Bomb Count Rules | Display bomb count rules summary |
| Multiplier Steps | Display number of modes |
| Actions | Edit / Delete buttons |

#### 3.3.2 Create/Edit RTP Config

##### RTP Field
| Field | Type | Validation |
|-------|------|------------|
| RTP | Number (0-1) | **No validation** (temporarily) |

##### Bomb Count Configuration
| Field | Type | Description |
|-------|------|-------------|
| bombCount | Number | Bomb count (e.g., 1-7) |
| probability | Number | Probability value |

- ✅ Can dynamically add/remove items
- ✅ Default values: 1, 2, 3, 4, 5, 6, 7
- ⚠️ **No validation for sum = 1**, but need to **display probability sum**

##### Multiplier Steps Configuration (Mode Tabs)
| Field | Type | Description |
|-------|------|-------------|
| mode | Number | Mode number (1-7 represents mines count) |
| name | String | Mode name (e.g., "star player rate 2% - 1 mines") |
| starPlayerRate | Number | Star player appearance rate |
| starPosition | Array<Number> | Pick positions where star player can appear |
| multiplier | Array<{picks, pays}> | Multiplier table |

- ✅ Can dynamically add/remove Mode
- ✅ Can dynamically add/remove multiplier items
- ✅ Picks must be in ascending order

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

---

## 4. API Design

### 4.1 Game Config API

| Method | Path | Description |
|--------|------|-------------|
| POST | `/api/game_settings/update_shooting_blitz_config` | Update ShootingBlitz general settings |

**Request Body:**
```json
{
  "name": "ShootingBlitz",
  "shootingBlitzConfig": {
    "useLiveScore": true,
    "rtpConfigId": "6123456789abcdef01234567"
  }
}
```

### 4.2 RTP Config CRUD API

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/shooting_blitz_rtp` | Get all RTP Configs |
| GET | `/api/shooting_blitz_rtp/{id}` | Get single RTP Config |
| POST | `/api/shooting_blitz_rtp` | Create RTP Config |
| PUT | `/api/shooting_blitz_rtp/{id}` | Update RTP Config |
| DELETE | `/api/shooting_blitz_rtp/{id}` | Delete RTP Config |

### 4.3 Teams CRUD API

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
| `GameSettings/Requests/UpdateShootingBlitzConfigRequest.cs` | **New** | Update Config Request |
| `GameSettings/Requests/ShootingBlitzRtpConfigRequests.cs` | **New** | RTP CRUD Requests |
| `GameSettings/Requests/ShootingBlitzTeamRequests.cs` | **New** | Team CRUD Requests |
| `GameSettings/ShootingBlitzRtpConfigController.cs` | **New** | RTP Controller |
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
| `Features/GameSettings/Views/GameDetails_ShootingBlitz.cshtml` | **New** | Detail settings page (with 3 Tabs) |
| `Features/GameSettings/Views/OddSettings.cshtml` | Modify | Add branch |
| `Features/GameSettings/IGameSettingApi.cs` | Modify | Add API interface |
| `Features/GameSettings/IShootingBlitzRtpApi.cs` | **New** | RTP API interface |
| `Features/GameSettings/IShootingBlitzTeamApi.cs` | **New** | Team API interface |
| `Features/GameSettings/Requests/UpdateShootingBlitzConfigRequest.cs` | **New** | Site Request |
| `wwwroot/js/shooting-blitz.js` | **New** | JavaScript interaction logic (RTP + Team) |

---

## 6. Validation Rules

| Item | Rule | Status |
|------|------|--------|
| RTP Range | 0-1 | ❌ No validation temporarily |
| bombCount probability sum | = 1 | ❌ No validation, but **need to display sum** |
| multiplier picks | Ascending order | ✅ Validation required |
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

---

## 8. Sample Data

### 8.1 Multiple RTP Config Examples

```json
// RTP Config 1: 95%
{
  "_id": ObjectId("..."),
  "rtp": 0.95,
  "bombCount": [
    { "bombCount": 3, "probability": 0.5 },
    { "bombCount": 4, "probability": 0.5 }
  ],
  "multiplierSteps": [/* ... */]
}

// RTP Config 2: 96%
{
  "_id": ObjectId("..."),
  "rtp": 0.96,
  "bombCount": [
    { "bombCount": 3, "probability": 0.4 },
    { "bombCount": 4, "probability": 0.6 }
  ],
  "multiplierSteps": [/* ... */]
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

### 9.1 Teams Tab Operation Flow

```
Teams Tab
├── Display Teams List (Table)
│   ├── Team ID
│   ├── Team Name
│   ├── Player Count
│   └── Actions (Edit / Delete)
│
├── [Create New Team] Button
│   └── Open Modal
│       ├── Team ID (Number Input)
│       ├── Team Name (Text Input)
│       ├── Player List Section
│       │   ├── [Add Player] Button
│       │   └── Player Rows
│       │       ├── Player Name (Text Input)
│       │       ├── Player Number (Number Input)
│       │       └── [Remove] Button
│       └── [Save] / [Cancel] Buttons
│
└── Edit Team (Same as Create Modal, pre-filled data)
```

### 9.2 RTP Config Tab Operation Flow

```
RTP Config Tab
├── Display RTP Config List (Table)
│   ├── RTP (%)
│   ├── Bomb Count Rules
│   ├── Multiplier Steps Count
│   └── Actions (Edit / Delete)
│
├── [Create New RTP Config] Button
│   └── Open Modal
│       ├── RTP (Number Input)
│       ├── Bomb Count Section
│       │   ├── [Add Bomb Count] Button
│       │   ├── Bomb Count Rows
│       │   │   ├── Bomb Count (Number)
│       │   │   ├── Probability (Number)
│       │   │   └── [Remove] Button
│       │   └── Display Probability Sum
│       ├── Multiplier Steps Section (Tabs)
│       │   ├── [Add Mode] Button
│       │   └── Mode Tabs
│       │       ├── Mode Number
│       │       ├── Name
│       │       ├── Star Player Rate
│       │       ├── Star Position
│       │       ├── Multiplier Table
│       │       │   ├── Picks | Pays | Actions
│       │       │   └── [Add Multiplier] Button
│       │       └── [Remove Mode] Button
│       └── [Save] / [Cancel] Buttons
│
└── Edit RTP Config (Same as Create Modal, pre-filled data)
```

