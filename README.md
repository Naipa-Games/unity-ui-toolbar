# Unity UI Toolbar

A powerful UI toolbar extension for Unity Editor, providing quick alignment, grouping, and component creation features for the Scene view.

<img width="552" height="130" alt="Preview" src="https://github.com/user-attachments/assets/05ccf36f-149a-4fe9-b1d6-b821a9c7f590" />

## ✨ Features

### 🎯 UI Alignment Toolbar

Quickly align and arrange UI elements in the Scene view without manually adjusting coordinates.

#### Grouping Features
- **Group** - Create a parent empty object for selected objects
- **Ungroup** - Dissolve the group while preserving child objects
- **Uniform Row Spacing** - Create uniform vertical spacing between selected objects
- **Uniform Column Spacing** - Create uniform horizontal spacing between selected objects
- **Grid Layout** - Arrange selected objects in a grid (supports custom row/column count and spacing)

#### Alignment Features
- **Align Left** - Align the left edges of selected UI elements
- **Align Center** - Horizontally center align selected UI elements
- **Align Right** - Align the right edges of selected UI elements
- **Align Top** - Align the top edges of selected UI elements
- **Align Middle** - Vertically center align selected UI elements
- **Align Bottom** - Align the bottom edges of selected UI elements

### 🎨 UI Component Toolbar

Quickly toggle creation of common UI components.

Supported Components:
- **Image** - Image component
- **Text** - Text component
- **Input Field** - Input field component
- **Button** - Button component
- **Toggle** - Toggle component
- **Slider** - Slider component
- **Scrollbar** - Scrollbar component

## 📦 Installation

### Method 1: Unity Package Manager (Recommended)

Requires Unity version with support for git package path query parameters (Unity >= 2019.3.4f1, Unity >= 2020.1a21).  
You can add `https://github.com/Naipa-Games/unity-ui-toolbar.git` to the Package Manager.

### Method 2: Manual Installation

- Download the latest .unitypackage file from the release page.
- Double-click the file to import it into your project.

## Getting Started

### Enable Toolbars

1. Open the Scene view
2. Click the `⋮` (more options) in the top-right corner of the Scene view
3. In the `Overlay Menu`, find and enable:
   - **UI Alignment Toolbar** - Alignment and grouping tools
   - **UI Component Toolbar** - Component creation tools

![OverlayMenu](https://github.com/user-attachments/assets/4099bf34-a53c-4a48-83a1-cfb26e9532d5)

### Using Grouping Features

**Create Group:**
1. Select multiple objects
2. Click the Group button
3. A parent object will be created containing all selected objects

**Ungroup:**
1. Select the group object
2. Click the Ungroup button
3. Child objects will be moved to the group's parent level, and the group object will be deleted

![Group](https://github.com/user-attachments/assets/fadb4b58-87bd-42a1-878a-ba7f0ee55dc2)

### Aligning UI Elements

**Single Object:**
- Select a RectTransform object
- Click an alignment button to align it relative to its parent

**Multiple Objects:**
- Select multiple RectTransform objects
- Click an alignment button to align all objects relative to the first selected object

![Alignment](https://github.com/user-attachments/assets/727031a0-7b24-4942-8efc-58f305ef1571)

### Using Spacing Features

**Uniform Row/Column Spacing:**
1. Select multiple objects (at least 2)
2. Click the spacing button to open the settings window
3. Set the spacing value (in pixels)
4. Click Apply to apply

![RowColumnSpacing](https://github.com/user-attachments/assets/f4a89ee6-1717-4793-b05b-0a868ee4d517)

### Using Grid Layout

1. Select the objects you want to arrange
2. Click the Grid Layout button
3. In the popup window, set:
   - Number of columns
   - Number of rows
   - Horizontal spacing
   - Vertical spacing
4. Click Apply to apply the layout

![GridLayout](https://github.com/user-attachments/assets/73e15bbe-861e-48b7-b1e3-b92b26d198e4)

### Creating UI Components

1. Select a Canvas or UI parent object in the Hierarchy. If nothing is selected, Canvas will be used as the parent.
2. Click the corresponding component button in the Component Toolbar, then click in the scene area to create.

![UIComponents](https://github.com/user-attachments/assets/7f3254ab-49ed-43cb-ad13-bd201f5eaf09)

## License

This project is licensed under the MIT License.
