# **RTS Game Setup Manual - Complete Guide**

## **📘 Table of Contents**
1. [Initial Project Setup](#1-initial-project-setup)
2. [Scene Setup Guide](#2-scene-setup-guide)
3. [Camera & Controls Setup](#3-camera--controls-setup)
4. [Creating Units - Step by Step](#4-creating-units---step-by-step)
5. [Creating Buildings](#5-creating-buildings)
6. [Creating Data Assets](#6-creating-data-assets)
7. [Layer & Tag Configuration](#7-layer--tag-configuration)
8. [Testing Checklist](#8-testing-checklist)
9. [Common Issues & Solutions](#9-common-issues--solutions)

---

## **1. INITIAL PROJECT SETUP**

### **1.1 Project Configuration**
```
1. Create New Unity 6 Project
2. Choose "3D (URP)" template
3. Name: "East-Wind" (or your choice)
4. Enable Version Control (Git recommended)
```

### **1.2 Project Settings**
```
Edit > Project Settings:

Player:
- Company Name: [Your Studio]
- Product Name: East Wind RTS

Graphics:
- Render Pipeline: URP
- Anti-aliasing: MSAA 2x

Quality:
- Create RTS-specific quality level
- Shadow Distance: 150
- Shadow Cascades: 4

Physics:
- Fixed Timestep: 0.02 (50Hz for RTS)
```

### **1.3 Required Packages**
```
Window > Package Manager:

Essential:
✅ AI Navigation (com.unity.ai.navigation)
✅ Universal RP
✅ TextMeshPro
✅ Cinemachine (optional, for cutscenes)

Recommended:
- ProBuilder (for prototyping)
- Post Processing
```

---

## **2. SCENE SETUP GUIDE**

### **2.1 Create Base Scene**

**Step-by-Step Scene Creation:**

```hierarchy
TestBattle (Scene)
├── Environment/
│   ├── Terrain (100x100) [Layer: Ground]
│   ├── Directional Light
│   └── NavMeshSurface
├── Managers/
│   ├── GameManager
│   ├── UnitManager
│   └── ResourceManager (when created)
├── Camera/
│   └── RTSCamera
├── UI/
│   └── Canvas
└── Units/
    └── [Spawned units go here]
```

### **2.2 Terrain Setup**
```
1. GameObject > 3D Object > Terrain
   OR
   Create large Plane (Scale: 100, 1, 100)

2. Add NavMeshSurface component:
   - Window > AI > Navigation
   - Add Component > NavMeshSurface
   - Agent Type: Humanoid
   - Click "Bake"

3. Set Layer to "Ground"
```

### **2.3 Lighting Setup**
```
1. Directional Light:
   - Rotation: (50, -30, 0)
   - Intensity: 1.2
   - Shadows: Soft Shadows

2. Environment Lighting:
   - Window > Rendering > Lighting
   - Environment Lighting: Gradient
   - Ambient Color: Slightly blue for outdoor
```

---

## **3. CAMERA & CONTROLS SETUP**

### **3.1 RTS Camera GameObject**

**Create GameObject Structure:**
```
RTSCamera (Empty GameObject)
├── Camera (Unity Camera Component)
└── UI Camera (Optional, for UI rendering)
```

**Components on RTSCamera GameObject:**

| Component | Settings |
|-----------|----------|
| **Transform** | Position: (50, 50, 50), Rotation: (45, 0, 0) |
| **Camera** | FOV: 60, Clipping: 0.3-1000 |
| **RTSCameraController** | See settings below |
| **RTSPlayerController** | See settings below |
| **Audio Listener** | (Only one in scene) |

**RTSCameraController Settings:**
```csharp
Pan Speed: 20
Pan Border Thickness: 10
Pan Limit: (100, 100)
Zoom Speed: 20
Min Zoom: 20
Max Zoom: 120
Zoom Smoothness: 5
Rotation Speed: 100
```

**RTSPlayerController Settings:**
```csharp
Player ID: 0
Team Color: Blue
Selectable Layer: "Selectable"
Ground Layer: "Ground"
```

---

## **4. CREATING UNITS - STEP BY STEP**

### **4.1 Basic Unit Prefab Structure**

**GameObject Hierarchy:**
```
TankUnit (Root GameObject) [Layer: Selectable]
├── Model (Visual representation)
│   ├── Hull (3D Model/Primitive)
│   ├── Turret (For rotating turret)
│   └── Barrel
├── UI/
│   ├── HealthBar Canvas
│   └── Selection Indicator
└── Effects/
    ├── Muzzle Flash Point
    └── Death Effect Point
```

### **4.2 Required Components List**

**On Root GameObject (TankUnit):**

| Component | Required | Purpose | Configuration |
|-----------|----------|---------|---------------|
| **Transform** | ✅ | Position/Rotation | Position: (0,0,0) |
| **UnitEntity** | ✅ | Core unit logic | Link UnitData SO |
| **HealthComponent** | ✅ | Health management | Set max health |
| **SelectableComponent** | ✅ | Selection system | Auto-configured |
| **MoverComponent** | ✅* | Movement (*if mobile) | Set speeds |
| **AttackerComponent** | ✅* | Combat (*if armed) | Link WeaponData |
| **FogRevealerComponent** | ✅ | Vision system | Set vision range |
| **NavMeshAgent** | ✅* | Pathfinding (*if mobile) | Auto-configured |
| **Collider** | ✅ | Selection/Hit detection | Box or Capsule |
| **Rigidbody** | ⚠️ | Physics (optional) | Kinematic = true |
| **AudioSource** | ⚠️ | Unit sounds | 3D Spatial |

### **4.3 Step-by-Step Unit Creation**

#### **STEP 1: Create Base GameObject**
```
1. Create Empty GameObject
2. Rename to unit type (e.g., "USA_Abrams_Tank")
3. Set Layer to "Selectable"
4. Position at (0, 0, 0)
```

#### **STEP 2: Add Visual Model**
```
1. Add Child GameObject "Model"
2. Add 3D model or primitives:
   - For testing: Use Cube for hull, Cylinder for turret
   - Scale appropriately (Tank: ~3x1.5x4)
3. Set materials/colors
```

#### **STEP 3: Add Core Components**
```
Add in this order:
1. NavMeshAgent (if mobile unit)
   - Speed: 5
   - Angular Speed: 120
   - Stopping Distance: 0.5
   - Auto Braking: true

2. UnitEntity
   - Will auto-add other components

3. Individual Components:
   - HealthComponent
   - SelectableComponent
   - MoverComponent (if mobile)
   - AttackerComponent (if armed)
   - FogRevealerComponent
```

#### **STEP 4: Configure Components**

**HealthComponent:**
```csharp
Max Health: 1000
Armor: 100
Armor Type: Heavy
```

**MoverComponent:**
```csharp
Move Speed: 5
Turn Speed: 120
Acceleration: 8
Stopping Distance: 0.5
```

**AttackerComponent:**
```csharp
Primary Weapon: [Link WeaponData SO]
Turret Transform: [Link turret GameObject]
Weapon Mount Point: [Link barrel end point]
```

**FogRevealerComponent:**
```csharp
Vision Range: 15
Detection Range: 12
```

#### **STEP 5: Create Selection Indicator**
```
1. Add Cylinder child "SelectionIndicator"
2. Scale: (2, 0.01, 2)
3. Position: (0, 0.1, 0)
4. Remove Collider component
5. Material: Green transparent
6. Link to SelectableComponent
```

#### **STEP 6: Save as Prefab**
```
1. Drag to: Assets/_Game/Prefabs/Units/USA/
2. Name: "USA_Abrams_Tank"
3. Create Prefab Variant for different levels
```

---

## **5. CREATING BUILDINGS**

### **5.1 Building Structure**

```
Barracks (Root) [Layer: Selectable]
├── Model/
│   ├── Foundation
│   ├── Walls
│   └── Roof
├── Construction/
│   ├── Scaffolding (Hidden when complete)
│   └── Construction Site
├── UI/
│   ├── HealthBar
│   ├── Production Progress
│   └── Rally Point Flag
└── Spawn Points/
    └── Unit Spawn Point
```

### **5.2 Building Components**

| Component | Purpose |
|-----------|---------|
| **BuildingEntity** | Core building logic |
| **HealthComponent** | Building health |
| **SelectableComponent** | Selection |
| **ProductionComponent** | Unit production |
| **PowerConsumer** | Power requirements |

---

## **6. CREATING DATA ASSETS**

### **6.1 Create Weapon Data**

```
1. Right-click in: Assets/_Game/Data/Weapons/
2. Create > RTS > Data > Weapon
3. Name: "120mm_Tank_Cannon"
4. Configure:
```

**Example Tank Cannon:**
```yaml
Weapon Name: M256 120mm
Damage: 500
Rate of Fire: 10 (rounds/min)
Range: 50 (meters)
Damage Type: Kinetic
Armor Penetration: 800
Blast Radius: 2
Accuracy: 0.9
Moving Accuracy: 0.6
Projectile Speed: 1000
```

### **6.2 Create Unit Data**

```
1. Right-click in: Assets/_Game/Data/Units/USA/
2. Create > RTS > Data > Unit
3. Name: "USA_Abrams_Tank"
4. Configure:
```

**Example Tank Data:**
```yaml
Unit Name: M1A2 Abrams
Designation: M1A2
Faction: USA
Unit Type: HeavyVehicle

Stats:
  Max Health: 1000
  Armor: 300
  Armor Type: Composite

Movement:
  Move Speed: 5
  Turn Speed: 60
  Acceleration: 3
  Can Move: true

Combat:
  Primary Weapon: [Link 120mm_Tank_Cannon]
  Vision Range: 15
  Detection Range: 12

Production:
  Cost Supplies: 800
  Cost Power: 20
  Build Time: 45
  Command Points: 3
  
Requirements:
  Required Building: War Factory
  Tech Level: Tier2
```

---

## **7. LAYER & TAG CONFIGURATION**

### **7.1 Required Layers**

**Edit > Project Settings > Tags and Layers:**

| Layer # | Name | Used For |
|---------|------|----------|
| 6 | Ground | Terrain, movement clicks |
| 7 | Selectable | Units, buildings |
| 8 | UI | UI elements |
| 9 | FogOfWar | Fog rendering |
| 10 | Minimap | Minimap camera |
| 11 | Effects | Particles, explosions |

### **7.2 Physics Layer Matrix**

```
Disable collisions between:
- UI ↔ Everything except UI
- Effects ↔ Everything
- Minimap ↔ Everything except Minimap
```

---

## **8. TESTING CHECKLIST**

### **8.1 Unit Testing**
- [ ] Unit spawns correctly
- [ ] Can be selected (green circle appears)
- [ ] Health bar visible
- [ ] Can receive move commands
- [ ] Pathfinding works
- [ ] Can attack targets
- [ ] Takes damage properly
- [ ] Dies and is removed

### **8.2 Camera Testing**
- [ ] WASD panning works
- [ ] Edge panning works
- [ ] Mouse wheel zoom
- [ ] Q/E rotation
- [ ] Middle mouse drag

### **8.3 Selection Testing**
- [ ] Single click selection
- [ ] Box selection
- [ ] Shift+click add to selection
- [ ] Control groups (Ctrl+1, etc.)
- [ ] Double-click select all of type

---

## **9. COMMON ISSUES & SOLUTIONS**

### **Issue: Units won't move**
```
Solutions:
✓ Check NavMesh is baked
✓ Ensure NavMeshAgent component present
✓ Check unit is on NavMesh (Y position)
✓ Verify Ground layer is set
```

### **Issue: Can't select units**
```
Solutions:
✓ Set unit layer to "Selectable"
✓ Add Collider component
✓ Check SelectableComponent is present
✓ Verify camera has correct layer mask
```

### **Issue: Components missing references**
```
Solutions:
✓ Ensure all SOs are created first
✓ Link Data assets in Inspector
✓ Check component initialization order
✓ Verify entity OwnerID is set
```

### **Issue: Errors on play**
```
Solutions:
✓ Create UnitManager GameObject
✓ Ensure only one AudioListener
✓ Check all required components
✓ Verify namespace references
```

---

## **📝 Quick Reference Card**

### **Unit Creation Order:**
1. GameObject + Layer
2. NavMeshAgent
3. UnitEntity
4. Other Components
5. Link Data Assets
6. Configure Values
7. Test
8. Save as Prefab

### **Minimum Viable Unit:**
- GameObject (Layer: Selectable)
- UnitEntity
- HealthComponent  
- SelectableComponent
- Collider
- Visual Model

### **Minimum Viable Building:**
- GameObject (Layer: Selectable)
- BuildingEntity
- HealthComponent
- SelectableComponent
- Collider
- Static Model