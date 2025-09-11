# OperateSettings Feature Documentation

## Overview

The OperateSettings feature is a comprehensive gaming administration system that allows administrators to manage game configurations, currency settings, and currency group game operations. This feature follows Clean Architecture principles with CQRS pattern using MediatR.

## Architecture

### Project Structure

```
src/GamesAdmin.Site/Features/OperateSettings/
├── Views/
│   ├── Index.cshtml                    # Main tabbed interface
│   ├── GameOperations.cshtml          # Individual game management
│   ├── CurrencyOperations.cshtml      # Currency settings management
│   └── CurrencyGroupGameOperations.cshtml # Currency group game management
├── Controllers/
│   └── OperateSettingsController.cs   # MVC controller
├── Services/
│   └── OperateSettingsService.cs      # Business logic service
├── Requests/
│   └── ImportCurrencyGroupGameRequest.cs # Request models
├── ViewModels/
│   ├── GameSettingViewModel.cs        # View models
│   ├── CurrencySettingViewModel.cs
│   └── CurrencyGroupGameSettingViewModel.cs
└── agent.md                          # This documentation file
```

### External Assets Structure

```
src/GamesAdmin.Site/wwwroot/
├── scss/pages/
│   ├── operate-settings.scss         # Main tab interface styles
│   ├── game-operations.scss          # Game operations styles
│   ├── currency-operations.scss      # Currency operations styles
│   └── currency-group-game-operations.scss # Currency group styles
├── js/
│   ├── operate-settings.js           # Main tab functionality
│   ├── game-operations.js            # Game operations logic
│   ├── currency-operations.js        # Currency operations logic
│   └── currency-group-game-operations.js # Currency group logic
└── scss/pages/ (compiled CSS files)
    ├── operate-settings.css
    ├── game-operations.css
    ├── currency-operations.css
    └── currency-group-game-operations.css
```

## Feature Components

### 1. Index.cshtml - Main Tabbed Interface

**Purpose**: Provides a tabbed interface that loads different operation views in iframes.

**Key Features**:

-   Three main tabs: Game Operations, Currency Operations, Currency Group Game Operations
-   Lazy loading of iframe content
-   Responsive tab navigation

**External Files**:

-   `operate-settings.scss` - Tab styling and layout
-   `operate-settings.js` - Tab switching logic

**Key Functions**:

-   `switchTab(tabId)` - Handles tab switching and iframe loading

### 2. GameOperations.cshtml - Individual Game Management

**Purpose**: Allows administrators to manage individual game configurations with import/export capabilities.

**Key Features**:

-   Game list display with filtering and sorting
-   Collapse/expand functionality for game settings
-   Bulk import/export of game configurations
-   Real-time search and filtering
-   Game type highlighting (AutoRNG for type 0)
-   Enabled status with Unicode icons (✓/✗)

**External Files**:

-   `game-operations.scss` - Game list styling, collapse/expand animations
-   `game-operations.js` - Game management logic, import/export functionality

**Key Functions**:

-   `toggleAllGames()` - Toggle all game checkboxes
-   `exportSelectedGameConfigs()` - Export selected games
-   `filterGames()` - Filter games by search term
-   `importGameConfigs()` - Handle file import
-   `sortTable(columnIndex)` - Sort table by column

**UI Enhancements**:

-   Game settings display with `Info.DisplayName` (fallback to `Info.Name`)
-   Game type badges with "AutoRNG" for type 0
-   Enabled status with color-coded Unicode characters
-   Collapse/expand for detailed settings view
-   Table-specific scrolling with fixed header

### 3. CurrencyOperations.cshtml - Currency Settings Management

**Purpose**: Manages operator currency configurations with import/export capabilities.

**Key Features**:

-   Operator currency list display
-   JSON preview of currency configurations
-   Import/export functionality
-   Validation of import data structure

**External Files**:

-   `currency-operations.scss` - Currency table styling
-   `currency-operations.js` - Currency management logic

**Key Functions**:

-   `formatDate(date)` - Format date for file naming
-   `updateExportButton()` - Update export button state
-   `validateImportData(data)` - Validate import data structure
-   `showImportResult(message)` - Display import results

### 4. CurrencyGroupGameOperations.cshtml - Currency Group Game Management

**Purpose**: Manages currency group game settings with advanced collapse/expand functionality.

**Key Features**:

-   Currency group list with game settings
-   Collapse/expand for game settings details
-   Export modal with individual game selection
-   Import functionality with data validation
-   Expand All/Collapse All buttons in headers

**External Files**:

-   `currency-group-game-operations.scss` - Complex styling for modals and collapse/expand
-   `currency-group-game-operations.js` - Advanced UI interactions and data management

**Key Functions**:

-   Collapse/expand functionality for both main table and export modal
-   Dynamic modal content generation
-   Game item selection and filtering
-   Import data validation and processing

## Code Organization Patterns

### CSS/SCSS Organization

**SCSS Structure**:

```scss
@import "../base/variables";

// Feature-specific styles
// ======================

// Base layout
.container-fluid { ... }

// Component-specific styles
.nav-tabs { ... }
.game-setting-cell { ... }
.currency-json { ... }

// Responsive and interactive styles
@media queries { ... }
.animation-keyframes { ... }
```

**Key Principles**:

-   Use SCSS nesting for component organization
-   Import base variables for consistency
-   Separate concerns (layout, components, animations)
-   Use semantic class names
-   Compile to CSS for production

### JavaScript Organization

**JS Structure**:

```javascript
/**
 * Feature JavaScript
 * Brief description of functionality
 */

// Global variables
let globalState = {};

/**
 * Function documentation
 * @param {type} param - Parameter description
 * @returns {type} Return description
 */
function functionName(param) {
    // Implementation
}

// Main initialization
$(document).ready(function () {
    // Event handlers and initialization
});
```

**Key Principles**:

-   Use JSDoc for function documentation
-   Organize by functionality (utilities, event handlers, main logic)
-   Use meaningful variable and function names
-   Handle errors gracefully
-   Use modern JavaScript features (const, let, arrow functions)

### HTML/Razor Organization

**Razor Structure**:

```html
@model ViewModelType @{ ViewData["Title"] = "Page Title"; Layout = "../_Shared/_BodyLayout"; } @section Styles {
<link rel="stylesheet" href="~/scss/pages/feature-name.css" />
}

<!-- HTML content -->

@section Scripts {
<script src="~/js/feature-name.js"></script>
}
```

**Key Principles**:

-   Use external CSS and JS files
-   Keep HTML clean and semantic
-   Use Razor sections for assets
-   Follow consistent naming conventions

## Data Flow

### 1. Game Operations Flow

```
User Action → JavaScript Handler → API Call → Controller → Service → Database
     ↓
UI Update ← JavaScript Response Handler ← JSON Response ← Service Result
```

**Example: Game Import**

1. User selects file → `importGameConfigs()`
2. File validation → `handleFileImport()`
3. API call → `/api/OperateSettings/ImportGameConfigs`
4. Controller processes → `OperateSettingsController.ImportGameConfigs()`
5. Service handles business logic → `OperateSettingsService.ImportGame()`
6. Database update → MongoDB operations
7. Response returned → UI updates with success/error

### 2. Currency Operations Flow

```
User Action → JavaScript Handler → API Call → Controller → Service → Database
     ↓
UI Update ← JavaScript Response Handler ← JSON Response ← Service Result
```

**Example: Currency Export**

1. User selects operators → `exportSelected()`
2. Data collection → `selectedOperators.map()`
3. JSON generation → `JSON.stringify()`
4. File download → `Blob` and `URL.createObjectURL()`

### 3. Currency Group Game Operations Flow

```
User Action → JavaScript Handler → Complex UI Logic → API Call → Controller → Service
     ↓
UI Update ← JavaScript Response Handler ← JSON Response ← Service Result
```

**Example: Export with Game Selection**

1. User clicks export → `exportSelected()`
2. Modal generation → Dynamic HTML creation
3. Game selection → Individual checkbox handling
4. Final export → Selected games only

## UI/UX Patterns

### Collapse/Expand Functionality

**Implementation Pattern**:

```javascript
// Toggle individual items
$(document).on("click", ".item-summary", function () {
    var $details = $(this).siblings(".item-details");
    var $icon = $(this).find(".toggle-icon");

    if ($details.is(":visible")) {
        $details.slideUp(300);
        $icon.removeClass("icon-expand-less").addClass("icon-expand-more");
    } else {
        $details.slideDown(300);
        $icon.removeClass("icon-expand-more").addClass("icon-expand-less");
    }
});

// Toggle all items
$("#expandAll").click(function () {
    $(".item-details").each(function () {
        if (!$(this).is(":visible")) {
            $(this).slideDown(300);
        }
    });
});
```

**CSS Support**:

```scss
.item-summary {
    cursor: pointer;
    transition: background-color 0.2s;

    &:hover {
        background-color: #e9ecef;
    }

    .toggle-icon {
        transition: transform 0.2s;
    }

    &.expanded .toggle-icon {
        transform: rotate(180deg);
    }
}

.item-details {
    display: none;

    &.show {
        display: block;
    }
}
```

### Modal Management

**Pattern**:

```javascript
// Generate dynamic modal content
let modalContent = '<div class="table-responsive">...</div>';
$("#modalContainer").html(modalContent);
$("#modal").modal("show");

// Handle dynamic content events
$(document).on("click", ".dynamic-element", function () {
    // Handle click on dynamically generated content
});
```

### Data Validation

**Client-side Validation**:

```javascript
function validateImportData(data) {
    if (!Array.isArray(data)) {
        throw new Error("Import data must be an array");
    }

    for (const item of data) {
        if (!item.requiredField) {
            throw new Error("Missing required field");
        }
    }
}
```

**Error Handling**:

```javascript
try {
    const result = await apiCall();
    // Handle success
} catch (error) {
    showError(`Operation failed: ${error.message}`);
}
```

## API Integration

### Endpoints Used

1. **Game Operations**:

    - `GET /OperateSettings/GameOperations` - Load game list
    - `POST /api/OperateSettings/ImportGameConfigs` - Import games
    - `POST /api/OperateSettings/ExportGameConfigs` - Export games

2. **Currency Operations**:

    - `GET /OperateSettings/CurrencyOperations` - Load currency list
    - `POST /api/OperateSettings/ImportCurrencies` - Import currencies
    - `POST /api/OperateSettings/ExportCurrencies` - Export currencies

3. **Currency Group Game Operations**:
    - `GET /OperateSettings/CurrencyGroupGameOperations` - Load group list
    - `POST /api/OperateSettings/ImportCurrencyGroupGames` - Import groups
    - `POST /api/OperateSettings/ExportCurrencyGroupGames` - Export groups

### Request/Response Patterns

**Import Request**:

```json
{
    "data": [
        {
            "field1": "value1",
            "field2": "value2"
        }
    ]
}
```

**Success Response**:

```json
{
    "success": true,
    "message": "Operation completed successfully"
}
```

**Error Response**:

```json
{
    "success": false,
    "message": "Error description",
    "errors": ["Specific error details"]
}
```

## Development Guidelines

### Adding New Features

1. **Create View**: Add new `.cshtml` file in `Views/` folder
2. **Create Styles**: Add `.scss` file in `wwwroot/scss/pages/`
3. **Create Scripts**: Add `.js` file in `wwwroot/js/`
4. **Update Controller**: Add new action methods
5. **Update Service**: Add business logic methods
6. **Update Models**: Add request/response models if needed

### Code Standards

1. **Naming Conventions**:

    - Files: `kebab-case` for CSS/JS, `PascalCase` for C#
    - Classes: `PascalCase`
    - Functions: `camelCase`
    - Variables: `camelCase`

2. **Documentation**:

    - Use JSDoc for JavaScript functions
    - Use XML comments for C# methods
    - Include parameter and return type descriptions

3. **Error Handling**:

    - Always validate user input
    - Provide meaningful error messages
    - Handle both client and server-side errors

4. **Performance**:
    - Use lazy loading for iframes
    - Implement pagination for large datasets
    - Cache frequently accessed data

### Testing Considerations

1. **Unit Tests**: Test individual functions and methods
2. **Integration Tests**: Test API endpoints and data flow
3. **UI Tests**: Test user interactions and responsive design
4. **Error Scenarios**: Test error handling and edge cases

## Future Enhancements

### Potential Improvements

1. **Real-time Updates**: WebSocket integration for live data updates
2. **Advanced Filtering**: Multi-column filtering and search
3. **Bulk Operations**: Enhanced bulk import/export with progress indicators
4. **Audit Trail**: Track all changes with user and timestamp
5. **Role-based Access**: Different permissions for different user types
6. **Mobile Optimization**: Responsive design improvements
7. **Offline Support**: Local storage for offline operations
8. **Data Visualization**: Charts and graphs for data analysis

### Maintenance Notes

1. **Regular Updates**: Keep dependencies updated
2. **Performance Monitoring**: Monitor page load times and API response times
3. **Security Reviews**: Regular security audits of API endpoints
4. **User Feedback**: Collect and implement user feedback
5. **Code Reviews**: Regular code reviews for quality assurance

## Troubleshooting

### Common Issues

1. **Icons Not Rendering**: Check if Material Icons are loaded, use Unicode fallbacks
2. **Modal Not Opening**: Check if Bootstrap is loaded and modal IDs are correct
3. **API Calls Failing**: Check network tab for error details and CORS issues
4. **Styling Issues**: Check if SCSS is compiled to CSS and files are referenced correctly
5. **JavaScript Errors**: Check browser console for error details

### Debug Tools

1. **Browser DevTools**: Use for debugging JavaScript and CSS
2. **Network Tab**: Monitor API calls and responses
3. **Console**: Check for JavaScript errors and warnings
4. **Elements Tab**: Inspect HTML structure and CSS classes

---

**Last Updated**: 2025-01-10
**Version**: 1.0
**Maintainer**: AI Assistant
