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
      "opponentCount": 4,
      "name": "star player rate 2% - 1 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "away": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "pays": [1, 1.06, 1.12, 1.19, 1.27, 1.36, 1.46, 1.58, 1.73, 1.9, 2.11, 2.37, 2.71, 3.17, 3.8, 4.75, 6.33, 9.5, 19]
    },
    {
      "opponentCount": 3,
      "name": "star player rate 2% - 2 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5] },
      "away": { "starPlayerRate": 0.03, "starPositions": [3, 4, 5] },
      "pays": [1.06, 1.18, 1.33, 1.52, 1.78, 2.14, 2.67, 3.56, 5.33]
    }
  ]
}
```

> **Note**: 
> - `opponentCount` represents opponent count (equivalent to mine count), selecting opponent loses
> - `home` and `away` set star player settings for home and away teams respectively, can have different rates and positions
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
| Use Live Score | Checkbox | Whether to use live score |
| Delay Start Time | Number | Delay start time |

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
| RTP | Display RTP value (e.g., 95%) |
| Multiplier Steps | Display number of modes |
| Actions | Edit / Delete buttons |
| Create New | Button to create new RTP Config |

##### Create/Edit RTP Config (Sub-Modal)

###### RTP Field
| Field | Type | Validation |
|-------|------|------------|
| RTP | Number (0-1) | **No validation** (temporarily) |

###### Multiplier Steps Configuration (Mode Tabs)
| Field | Type | Description |
|-------|------|-------------|
| opponentCount | Number | Opponent count (equivalent to mine count, selecting opponent loses) |
| name | String | Mode name (e.g., "star player rate 2% - 1 mines") |
| home.starPlayerRate | Number | Home team star player appearance rate |
| home.starPositions | Array<Number> | Home team star player available pick positions |
| away.starPlayerRate | Number | Away team star player appearance rate |
| away.starPositions | Array<Number> | Away team star player available pick positions |
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
      "opponentCount": 4,
      "name": "star player rate 2% - 1 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "away": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "pays": [1, 1.06, 1.12, 1.19, 1.27]
    },
    {
      "opponentCount": 3,
      "name": "star player rate 2% - 2 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5] },
      "away": { "starPlayerRate": 0.03, "starPositions": [3, 4, 5] },
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
| `Features/GameSettings/Views/GameDetails_ShootingBlitz.cshtml` | Modify | General Setting Modal (General + Teams + **RTP** Tabs) |
| `Features/GameSettings/Views/OddSettings.cshtml` | Modify | Remove ShootingBlitz branch (Odds column displays empty) |
| `Features/GameSettings/Views/OddsSettings_ShootingBlitz.cshtml` | **Delete** | Integrated into GameDetails_ShootingBlitz.cshtml |
| `Features/GameSettings/IGameSettingApi.cs` | Modify | Add API interface |
| `Features/GameSettings/IShootingBlitzRtpApi.cs` | **New** | RTP API interface |
| `Features/GameSettings/IShootingBlitzTeamApi.cs` | **New** | Team API interface |
| `Features/GameSettings/Requests/UpdateShootingBlitzConfigRequest.cs` | **New** | Site Request |
| `wwwroot/js/shooting-blitz-general.js` | **New** | JavaScript for General Setting Modal (General + Teams) |
| `wwwroot/js/shooting-blitz-rtp.js` | **New** | JavaScript for RTP Tab (RTP Config CRUD) |

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
6. **Unified UI Entry Points**:
   - **General Setting** (Game Management → General Setting column): General config + Teams management + **RTP Config management**
   - **Odds** (Game Management → Odds column): **Not used for ShootingBlitz, displays empty**
7. **Pays Array Format**:
   - `pays` array index corresponds to picks value: `pays[0]` = picks 1, `pays[1]` = picks 2, ...
   - Picks field auto-displays in UI (read-only), users only need to input Pays values
   - Different opponentCount can have different pays array lengths
8. **Home/Away Star Player Settings**:
   - `home` and `away` can have different star player rates (`starPlayerRate`) and positions (`starPositions`)
   - This allows home and away teams to have different gameplay characteristics
9. **Opponent Count**:
   - `opponentCount` represents opponent count, equivalent to "mine count" concept
   - Selecting opponent loses (similar to minesweeper game)

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
      "opponentCount": 4,
      "name": "star player rate 2% - 1 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "away": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "pays": [1, 1.06, 1.12, 1.19, 1.27, 1.36, 1.46, 1.58, 1.73, 1.9, 2.11, 2.37, 2.71, 3.17, 3.8, 4.75, 6.33, 9.5, 19]
    },
    {
      "opponentCount": 3,
      "name": "star player rate 2% - 2 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5] },
      "away": { "starPlayerRate": 0.03, "starPositions": [3, 4, 5] },
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
      "opponentCount": 4,
      "name": "star player rate 2% - 1 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "away": { "starPlayerRate": 0.025, "starPositions": [3, 4, 5, 6] },
      "pays": [1, 1.08, 1.16, 1.25, 1.35, 1.47, 1.62, 1.80, 2.03, 2.32]
    },
    {
      "opponentCount": 3,
      "name": "star player rate 2% - 2 mines",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5] },
      "away": { "starPlayerRate": 0.03, "starPositions": [3, 4, 5] },
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
├── [RTP Tab] ← Previously Odds Modal functionality integrated here
│   ├── Current Selection Section
│   │   ├── Label: "Current RTP Config"
│   │   ├── Dropdown: [Select RTP Config ▼]
│   │   │   ├── -- Select RTP Config --
│   │   │   ├── RTP 95% (id: xxx)
│   │   │   ├── RTP 96% (id: yyy)
│   │   │   └── RTP 97% (id: zzz)
│   │   └── [Apply Selection] Button
│   │
│   ├── RTP Config List Section
│   │   ├── Display RTP Config List (Table)
│   │   │   ├── RTP (%)
│   │   │   ├── Multiplier Steps Count
│   │   │   └── Actions (Edit / Delete)
│   │   │
│   │   ├── [Create New RTP Config] Button
│   │   │   └── Open Sub-Modal
│   │   │       ├── RTP (Number Input)
│   │   │       ├── Multiplier Steps Section (Tabs)
│   │   │       │   ├── [Add Mode] Button
│   │   │       │   └── Mode Tabs
│   │   │       │       ├── Opponent Count (Number)
│   │   │       │       ├── Name (Text)
│   │   │       │       ├── Home Star Player Settings
│   │   │       │       │   ├── Star Player Rate
│   │   │       │       │   └── Star Positions (comma separated)
│   │   │       │       ├── Away Star Player Settings
│   │   │       │       │   ├── Star Player Rate
│   │   │       │       │   └── Star Positions (comma separated)
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
                                            │ │   - Use Live Score  │ │
                                            │ │   - Delay Time      │ │
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
                                            │ │   RTP Tab (NEW)     │ │
                                            │ │   - Current RTP     │ │
                                            │ │     Selection       │ │
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
              └─────────────────────────┘                              └─────────────────────────┘
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
  "rtp": 0.95,
  "multiplierSteps": [
    {
      "opponentCount": 4,
      "name": "...",
      "home": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "away": { "starPlayerRate": 0.02, "starPositions": [3, 4, 5, 6] },
      "pays": [1, 1.06, 1.12, 1.19]
    }
  ]
}
```

### Changes Summary (v1 → Current)
| Item | v1 | Current |
|------|-----|-----|
| bombCount | ✅ Required | ❌ **Removed** |
| multiplierSteps.multiplier | `[{ picks, pays }]` | ❌ **Removed** |
| multiplierSteps.pays | ❌ N/A | ✅ **New**: `[number]` array |

### Changes Summary (v2 → Current)
| Item | v2 | Current |
|------|-----|-----|
| mode | ✅ `mode` | ✅ **Renamed**: `opponentCount` |
| starPlayerRate | ✅ Single field | ✅ **Split**: `home.starPlayerRate`, `away.starPlayerRate` |
| starPosition | ✅ Single array | ✅ **Split & Renamed**: `home.starPositions`, `away.starPositions` |

> **Note**: 
> - `pays[index]` corresponds to `picks = index + 1`
> - `opponentCount` represents opponent count (equivalent to mine count), selecting opponent loses
> - `home` and `away` can have different star player rates and positions
