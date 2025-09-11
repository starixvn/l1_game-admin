# ChatBot Admin Notifications API Documentation

## Overview

The ChatBot Admin Notifications system provides automated notification functionality for administrative actions within the GamesAdmin system. This revolutionary feature sends real-time notifications to designated Slack channels when administrators make changes to game settings, configurations, and other critical system parameters, replacing the old Sentry error monitoring with proactive change alerts.

## Table of Contents

- [What Are Notifications?](#what-are-notifications)
- [Key Benefits](#key-benefits)
- [Getting Started](#getting-started)
- [How It Works](#how-it-works)
- [Message Examples](#message-examples)
- [Best Practices](#best-practices)
- [Common Issues](#common-issues)

## What Are Notifications?

The system can send instant messages to your team whenever important changes are made. This helps everyone stay informed about what's happening with the games and replaces the old reactive error monitoring with proactive change alerts.

### Key Benefits

- **Instant Alerts**: Know immediately when important changes happen
- **Team Coordination**: Keep everyone informed through Slack channels
- **Change Tracking**: See exactly what was changed and by whom
- **Proactive Monitoring**: Get notified before issues occur
- **Complete Audit Trail**: Full record of all administrative actions

## Getting Started

### For New Users

#### 1. Accessing the System

- Get your login credentials from your system administrator
- Open your web browser and go to the GamesAdmin website
- Log in using your username and password
- Notifications will automatically appear in your designated Slack channels

#### 2. Basic Tasks You'll Learn

- **Understanding Notifications**: Learn what types of changes trigger notifications
- **Team Coordination**: Use notifications to keep everyone informed
- **Change Tracking**: Monitor what was changed and by whom
- **Quick Response**: React quickly to any issues that arise

#### 3. Advanced Features

- **Notification Configuration**: Set up different channels for different types of changes
- **Message Templates**: Understand the structured notification formats
- **Integration**: Work with developers to configure the notification system

### For Current Users

#### New Features You Should Know About

- **Real-Time Slack Notifications**: Get instant alerts when changes are made (replaces Sentry error monitoring)
- **Proactive Monitoring**: Get notified before issues occur instead of after
- **Better Team Coordination**: Keep everyone informed through Slack channels

#### How to Use New Features

- **Stay Informed**: Use notifications to know when important changes happen
- **Team Coordination**: Keep everyone on the same page with instant alerts
- **Change Tracking**: See exactly what was changed and by whom

## How It Works

### System Components

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   Admin Action  │───▶│  ChatBotService  │───▶│   ChatBot API   │
│   (Game Config) │    │                  │    │                 │
└─────────────────┘    └──────────────────┘    └─────────────────┘
                                │
                                ▼
                       ┌──────────────────┐
                       │  Chat Channel    │
                       │  (Notifications) │
                       └──────────────────┘
```

### How Notifications Work

The notification system works in three simple steps:

1. **Admin Makes Changes**: When you update game settings, add new games, or modify configurations
2. **System Detects Changes**: The system automatically detects what was changed
3. **Slack Notification Sent**: A message is sent to your team's Slack channel with details about the changes

### Key Features

- **Instant Alerts**: Notifications are sent immediately when changes are made
- **Clear Messages**: Each notification shows exactly what was changed
- **Team Coordination**: Everyone on your team can see the changes in real-time
- **Change Tracking**: You can see who made what changes and when
- **Automatic Detection**: The system only sends notifications when actual changes occur

## Message Examples

### Game Configuration Changes

#### Sample 1: Basic Game Settings Update

```
CHANGE GAME CONFIG:
Lucky Wheel Premium has new Game Config!!!
Updated betting limits and enabled the game
Details: MinBet: 1.0 -> 5.0;
MaxBet: 1000.0 -> 2000.0;
Enabled: false -> true;
DelayStartTime: 0 -> 30
```

#### Sample 2: Bot Settings Modification

```
CHANGE BOT SETTINGS:
Dragon Quest has new Bot Settings!!!
Updated bot configuration for better performance
Details: BotEnabled: true -> false;
BotMaxBet: 500.0 -> 1000.0;
SmartBotEnabled: false -> true;
SmartBotGroupRatio: 0.5 -> 0.8
```

#### Sample 3: Odds Configuration Update

```
CHANGE ODDS CONFIG:
Fish Prawn Crab has new Odds Config!!!
Adjusted payout rates for better RTP
Details: high: 2.0 -> 1.95;
low: 1.8 -> 1.75;
red: 2.0 -> 1.95;
black: 2.0 -> 1.95
```

#### Sample 4: Currency Group Game Settings

```
CHANGE CURRENCY GROUP GAME:
Premium Games has new Currency Group Game!!!
Updated betting limits for USD currency group
Details: Enabled: true -> false;
MinBet: 0.1 -> 1.0;
MaxBet: 100.0 -> 500.0;
MaxBetPerChoices high: 50.0 -> 100.0;
CoinValues: 0.1, 0.5, 1.0, 5.0 -> 1.0, 5.0, 10.0, 50.0
```

### New Settings Addition

#### Sample 1: New Game Configuration

```
ADD GAME CONFIG
Details: {
  "GameId": 2001,
  "Name": "mysticalslots",
  "Info": {
    "Name": "Mystical Slots",
    "DisplayName": "Mystical Slots Adventure",
    "Type": 1
  },
  "EnableSetting": {
    "Enabled": true,
    "BotEnabled": true,
    "EnableFreeBet": false
  },
  "BetLimit": {
    "MinBet": 0.25,
    "MaxBet": 250.0
  }
}
```

#### Sample 2: New Currency Group Game

```
ADD CURRENCY GROUP GAME
Details: {
  "GameId": 1001,
  "GameName": "Lucky Wheel",
  "Enabled": true,
  "MinBet": 1.0,
  "MaxBet": 1000.0,
  "MaxWinPerRound": 5000.0,
  "GroupId": "premium-games-group-001",
  "MaxBetPerChoices": {
    "high": 100.0,
    "low": 50.0
  },
  "CoinValues": [1.0, 5.0, 10.0, 25.0, 50.0],
  "Chips": ["red", "blue", "green", "gold"],
  "BetLevels": [1.0, 2.0, 5.0, 10.0, 25.0, 50.0]
}
```

#### Sample 3: New Promotion Configuration

```
CREATE PROMOTION
Details: {
  "GameId": 1001,
  "Name": "Lucky Wheel",
  "Status": "Active",
  "IsDaily": true,
  "StartDateTime": "2025-01-11T10:00:00Z",
  "EndDateTime": "2025-01-12T10:00:00Z",
  "Odds": {
    "high": 2.5,
    "low": 2.2,
    "red": 2.0,
    "black": 2.0
  }
}
```

#### Sample 4: Tournament Configuration Addition

```
ADD TOURNAMENT CONFIG
Details: {
  "Name": "Weekly Championship",
  "StartDateTime": "2025-01-13T00:00:00Z",
  "EndDateTime": "2025-01-19T23:59:59Z",
  "Operators": ["OP001", "OP002", "OP003"],
  "WinningPrizes": [
    {
      "Rank": 1,
      "Prize": 1000.0,
      "Currency": "USD"
    },
    {
      "Rank": 2,
      "Prize": 500.0,
      "Currency": "USD"
    }
  ]
}
```

### Complex Configuration Changes

#### Sample 1: Multiple Settings Update

```
CHANGE GAME CONFIG:
Mega Jackpot Slots has new Game Config!!!
Comprehensive update including odds, bot settings, and tournament config
Details: MinBet: 0.5 -> 1.0;
MaxBet: 500.0 -> 1000.0;
BotEnabled: false -> true;
BotMaxBet: 100.0 -> 200.0;
SmartBotEnabled: false -> true;
OddsMultipliers: {"high": 1.0, "low": 1.0} -> {"high": 1.2, "low": 1.1};
TournamentConfig: null -> {"HaveTournament": true, "StartTime": "2025-01-11T20:00:00Z", "EndTime": "2025-01-11T22:00:00Z"}
```

#### Sample 2: Symbol Weight Configuration

```
CHANGE SYMBOL WEIGHT:
Fruit Slots has new Symbol Weight!!!
Updated symbol distribution for better game balance
Details: Weight Row 1: 1, 2, 3, 4, 5 -> 2, 3, 4, 5, 6;
Weight Row 2: 2, 3, 4, 5, 6 -> 3, 4, 5, 6, 7;
Weight Row 3: 3, 4, 5, 6, 7 -> 4, 5, 6, 7, 8
```

#### Sample 3: Margin Settings Update

```
CHANGE MARGIN SETTINGS:
Blackjack Pro has new Margin Settings!!!
Adjusted house edge for different bet levels
Details: Margin Level 1: {"basic": 0.02, "insurance": 0.05} -> {"basic": 0.015, "insurance": 0.04};
Margin Level 2: {"basic": 0.025, "insurance": 0.06} -> {"basic": 0.02, "insurance": 0.05}
```

### Import/Export Operations

#### Sample 1: Game Configuration Import

```
IMPORT GAME CONFIG
Details: {
  "ImportType": "GameConfigurations",
  "Source": "Backup_2025-01-10.json",
  "ItemsCount": 15,
  "SuccessCount": 14,
  "FailedCount": 1,
  "FailedItems": ["GameId: 1005 - Invalid odds configuration"],
  "Timestamp": "2025-01-11T14:30:00Z"
}
```

#### Sample 2: Currency Group Games Export

```
EXPORT CURRENCY GROUP GAMES
Details: {
  "ExportType": "CurrencyGroupGames",
  "OperatorId": "OP001",
  "GroupName": "Premium Games",
  "GamesCount": 8,
  "ExportTimestamp": "2025-01-11T15:45:00Z",
  "FileSize": "2.3KB",
  "IncludedGames": ["Lucky Wheel", "Dragon Quest", "Fish Prawn Crab", "Mega Slots"]
}
```

## Best Practices

### General Tips for Success

1. **Stay Informed**: Check your Slack notifications regularly to stay updated on changes
2. **Team Coordination**: Use notifications to keep everyone on the same page
3. **Change Tracking**: Review notification history to understand what changes were made
4. **Quick Response**: React quickly to any issues that arise from changes

### Working with Notifications

1. **Monitor Changes**: Pay attention to notifications about important game settings
2. **Coordinate with Team**: Use notifications to coordinate changes with other team members
3. **Document Changes**: Keep track of important changes mentioned in notifications
4. **Follow Up**: If a notification indicates a problem, follow up with the person who made the change

### Configuration Management

1. **Review Before Acting**: Always review what changes are being made before implementing them
2. **Test Changes**: Test changes in a safe environment before applying them to live games
3. **Communicate Changes**: Let your team know about important changes you're planning to make
4. **Monitor Results**: Watch how games perform after changes are made
