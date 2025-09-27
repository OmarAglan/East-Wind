# **EAST WIND - Game Design Document**
### **Version 1.0 - Development Build**
### **Modern Military Real-Time Strategy Game**

---

## **📋 TABLE OF CONTENTS**

1. [Executive Summary](#1-executive-summary)
2. [Game Overview](#2-game-overview)
3. [Core Gameplay](#3-core-gameplay)
4. [Factions & Units](#4-factions--units)
5. [Technical Architecture](#5-technical-architecture)
6. [Current Implementation Status](#6-current-implementation-status)
7. [Art & Audio Direction](#7-art--audio-direction)
8. [Multiplayer Design](#8-multiplayer-design)
9. [Roadmap & Milestones](#9-roadmap--milestones)

---

## **1. EXECUTIVE SUMMARY**

### **Game Title:** East Wind
### **Genre:** Real-Time Strategy (RTS)
### **Platform:** PC (Windows, Mac, Linux)
### **Engine:** Unity 6 (URP)
### **Target Audience:** RTS enthusiasts, military strategy fans
### **Development Status:** Pre-Alpha (Core Systems Development)

### **Core Concept:**
A modern military RTS featuring realistic combat between global superpowers. Players command authentic military units in tactical battles emphasizing combined arms warfare, strategic positioning, and resource control.

### **Key Features:**
- Realistic modern military units and tactics
- Asymmetric faction design (USA vs Russia initially)
- Streamlined resource system
- Large-scale battles on expansive maps
- Planned multiplayer support
- Data-driven architecture for modding potential

---

## **2. GAME OVERVIEW**

### **2.1 Core Pillars**

#### **Pillar 1: Authentic Military Combat**
- Real-world units with accurate capabilities
- Realistic engagement ranges and damage models
- Combined arms tactics essential for victory

#### **Pillar 2: Strategic Depth, Tactical Execution**
- High-level resource management
- Micro-intensive unit control
- Meaningful faction differences

#### **Pillar 3: Scalability & Polish**
- Start with 2 factions, expandable to 8+
- Mod-friendly architecture
- Performance-optimized for large battles

### **2.2 Victory Conditions**

| Mode | Description | Status |
|------|-------------|--------|
| **Annihilation** | Destroy all enemy structures | ✅ Implemented |
| **Conquest** | Control key points for timer | 🔄 Planned |
| **Assassination** | Destroy enemy command center | 🔄 Planned |
| **Economic Victory** | Accumulate resource target | 🔄 Planned |

### **2.3 Game Flow**

```mermaid
Start Match → Build Base → Gather Resources → Train Forces → 
Scout Enemy → Engage Combat → Expand/Defend → Victory/Defeat
```

---

## **3. CORE GAMEPLAY**

### **3.1 Resource System**

#### **Primary Resources:**

| Resource | Purpose | Generation | Implementation |
|----------|---------|------------|----------------|
| **Supplies** | Universal currency | Supply Depots, Oil Derricks | 🔄 In Progress |
| **Power** | Advanced unit cap | Power Plants | 🔄 In Progress |
| **Command Points** | Population limit | Command Centers | ✅ Designed |

#### **Resource Flow:**
- Initial supplies: 10,000
- Supply Depot income: 200/minute
- Oil Derrick income: 400/minute
- Power Plant generation: 100 power units

### **3.2 Base Building**

#### **Construction Rules:**
- Grid-based placement (currently free placement)
- Proximity requirements for some buildings
- Power radius system for advanced structures
- Build radius extends from Command Centers

### **3.3 Combat System**

#### **Damage Calculation:**
```
Final Damage = Base Damage × Armor Modifier × Range Modifier × Accuracy
```

#### **Damage Type vs Armor Matrix:**

| Damage Type | None | Light | Medium | Heavy | Reactive |
|-------------|------|-------|--------|-------|----------|
| **Kinetic** | 100% | 100% | 75% | 50% | 60% |
| **High Explosive** | 100% | 150% | 100% | 75% | 100% |
| **HEAT** | 100% | 200% | 150% | 100% | 30% |
| **Fragmentation** | 150% | 100% | 50% | 25% | 40% |

### **3.4 Unit Control**

#### **Implemented Commands:**
- ✅ Move
- ✅ Attack
- ✅ Stop
- ✅ Attack-Move
- ✅ Control Groups (0-9)
- ✅ Box Selection

#### **Planned Commands:**
- 🔄 Patrol
- 🔄 Guard
- 🔄 Formation Move
- 🔄 Stance Changes (Aggressive/Defensive/Passive)

---

## **4. FACTIONS & UNITS**

### **4.1 UNITED STATES - "Technology & Precision"**

#### **Faction Traits:**
- High-tech units with superior individual performance
- Expensive but powerful
- Strong air force
- Precision strikes
- Advanced information warfare

#### **Unique Mechanics:**
- **Drone Surveillance:** Extended vision on certain units
- **GPS Targeting:** Artillery gets accuracy bonus
- **Air Superiority:** Faster aircraft reload at airfields

#### **Unit Roster (Current Design):**

| Unit | Role | Health | Cost | Weapon | Status |
|------|------|--------|------|--------|--------|
| **Infantry Squad** | Basic Infantry | 100/soldier | 200 | M4A1 Carbine | ✅ Defined |
| **Javelin Team** | Anti-Tank | 150 | 400 | FGM-148 Javelin | ✅ Defined |
| **M2A3 Bradley** | IFV | 500 | 600 | 25mm Bushmaster | 🔄 Planned |
| **M1A2 Abrams** | MBT | 1000 | 1200 | 120mm M256 | ✅ Defined |
| **AH-64 Apache** | Attack Heli | 400 | 1500 | 30mm + Hellfires | 🔄 Planned |
| **F-35 Lightning** | Fighter | 300 | 2000 | Missiles + Bombs | 🔄 Planned |

#### **Building Roster:**

| Building | Purpose | Cost | Power | Status |
|----------|---------|------|-------|--------|
| **Command Center** | HQ, Workers | 2000 | 0 | ✅ Defined |
| **Power Plant** | Power Generation | 600 | -100 | ✅ Defined |
| **Barracks** | Infantry Production | 800 | 20 | 🔄 In Progress |
| **War Factory** | Vehicle Production | 2000 | 50 | 🔄 Planned |
| **Airfield** | Aircraft Production | 3000 | 75 | 🔄 Planned |

### **4.2 RUSSIA - "Mass & Firepower"**

#### **Faction Traits:**
- Cost-effective units
- Superior numbers
- Heavy armor focus
- Area saturation weapons
- Strong defensive capabilities

#### **Unique Mechanics:**
- **Conscript Surge:** Rapidly train cheap infantry
- **Reactive Armor:** Tanks resist HEAT rounds
- **Artillery Doctrine:** Reduced artillery cost

#### **Unit Roster (Current Design):**

| Unit | Role | Health | Cost | Weapon | Status |
|------|------|--------|------|--------|--------|
| **Conscript Squad** | Basic Infantry | 80/soldier | 150 | AK-12 | ✅ Defined |
| **RPG Team** | Anti-Tank | 120 | 300 | RPG-29 | ✅ Defined |
| **BMP-3** | IFV | 450 | 500 | 100mm + 30mm | 🔄 Planned |
| **T-90M** | MBT | 1100 | 1000 | 125mm 2A46M | ✅ Defined |
| **Mi-28 Havoc** | Attack Heli | 450 | 1400 | 30mm + Ataka | 🔄 Planned |
| **Su-25 Frogfoot** | CAS Aircraft | 350 | 1800 | Rockets + Bombs | 🔄 Planned |

---

## **5. TECHNICAL ARCHITECTURE**

### **5.1 Core Systems Status**

| System | Description | Status | Implementation |
|--------|-------------|--------|----------------|
| **Entity-Component** | Modular unit architecture | ✅ Complete | BaseEntity, UnitEntity |
| **Data Architecture** | ScriptableObject-based | ✅ Complete | UnitData, WeaponData |
| **Input System** | RTS controls | ✅ Complete | RTSPlayerController |
| **Camera System** | RTS camera | ✅ Complete | RTSCameraController |
| **Movement System** | NavMesh pathfinding | ✅ Complete | MoverComponent |
| **Combat System** | Basic combat | ✅ Complete | AttackerComponent |
| **Health System** | Damage & armor | ✅ Complete | HealthComponent |
| **Selection System** | Unit selection | ✅ Complete | SelectableComponent |
| **Resource System** | Economy | 🔄 In Progress | - |
| **Building System** | Base construction | 🔄 Next | - |
| **Production System** | Unit training | 📋 Planned | - |
| **Fog of War** | Vision system | 📋 Planned | - |
| **AI System** | Computer opponents | 📋 Planned | - |
| **Multiplayer** | Network play | 📋 Future | - |

### **5.2 Component Architecture**

```
GameObject (Unit)
├── UnitEntity (Core Logic)
├── HealthComponent (Survivability)
├── MoverComponent (Movement)
├── AttackerComponent (Combat)
├── SelectableComponent (Player Interaction)
├── FogRevealerComponent (Vision)
└── NavMeshAgent (Pathfinding)
```

### **5.3 Data-Driven Design**

All unit and weapon stats are stored in ScriptableObjects:
- **Pros:** Easy balancing, no recompilation, moddable
- **Cons:** Initial setup time, more assets to manage

---

## **6. CURRENT IMPLEMENTATION STATUS**

### **6.1 Completed Features** ✅

#### **Core Framework:**
- Unity 6 project with URP
- Folder structure organized
- Layer system configured
- ScriptableObject data system

#### **Gameplay Systems:**
- Camera controls (pan, zoom, rotate)
- Unit selection (single, box, multi)
- Control groups
- Basic movement commands
- Attack commands
- Health and damage
- Component-based entities

### **6.2 In Progress** 🔄

#### **Current Sprint:**
- Resource Manager implementation
- Resource gathering mechanics
- Basic UI elements
- Building placement system

### **6.3 Upcoming Features** 📋

#### **Next Milestone:**
- Production buildings
- Unit training queues
- Building construction
- Power system
- Fog of War

---

## **7. ART & AUDIO DIRECTION**

### **7.1 Visual Style**

#### **Art Direction:** Semi-Realistic Military
- Authentic unit proportions
- Realistic military colors (olive, tan, gray)
- Readable silhouettes for gameplay
- Clear faction differentiation

#### **Technical Specs:**
- **Poly Budget:** 5,000-15,000 per unit
- **Texture Resolution:** 2048x2048 for vehicles
- **LOD Levels:** 3 per unit
- **Draw Distance:** 200 units

### **7.2 Audio Design**

#### **Categories:**
- **Weapon Sounds:** Authentic recordings
- **Unit Responses:** Professional military chatter
- **Ambient:** Battlefield atmosphere
- **Music:** Orchestral military themes

---

## **8. MULTIPLAYER DESIGN**

### **8.1 Architecture**

#### **Network Model:** Deterministic Lockstep
- Only commands sent over network
- All clients simulate identically
- Advantages: Low bandwidth, supports many units
- Challenges: Must maintain determinism

### **8.2 Planned Modes**

| Mode | Players | Description | Priority |
|------|---------|-------------|----------|
| **1v1 Ranked** | 2 | Competitive ladder | High |
| **Team Battle** | 2v2, 3v3, 4v4 | Team-based combat | Medium |
| **Free-for-All** | 2-8 | Every player for themselves | Low |
| **Comp Stomp** | 1-4 vs AI | Cooperative vs AI | Medium |

### **8.3 Determinism Requirements**

Current codebase follows deterministic principles:
- ✅ Fixed timestep for simulation
- ✅ No random without seed
- ✅ Integer-based IDs
- ⚠️ Need to implement fixed-point math

---

## **9. ROADMAP & MILESTONES**

### **Milestone 1: Core Systems** ✅ COMPLETE
- Basic entity system
- Movement and combat
- Camera and controls
- **Status:** Done

### **Milestone 2: Economy & Production** 🔄 CURRENT
- Resource gathering
- Building placement
- Unit production
- Basic UI
- **Target:** 1-2 weeks

### **Milestone 3: First Playable** 📋
- 5 units per faction
- 5 buildings per faction
- One complete map
- Win/lose conditions
- **Target:** 3-4 weeks

### **Milestone 4: Vertical Slice** 📋
- All USA units (10-12)
- All Russia units (10-12)
- 3 maps
- Basic AI opponent
- **Target:** 2 months

### **Milestone 5: Alpha** 📋
- Fog of War
- Advanced AI
- Sound effects
- Visual effects
- Balance pass
- **Target:** 3 months

### **Milestone 6: Beta** 📋
- Multiplayer implementation
- Matchmaking
- Additional factions
- Polish and optimization
- **Target:** 6 months

---

## **📊 METRICS FOR SUCCESS**

### **Performance Targets:**
- 60 FPS with 200 units on screen
- < 100ms input latency
- < 2 second load times
- Multiplayer sync within 50ms

### **Gameplay Targets:**
- Average match: 15-25 minutes
- Unit response time: < 200ms
- Time to first combat: 5-7 minutes
- Actions per minute: 50-150 (skill-based)

---

## **🎮 CONTROLS REFERENCE**

| Input | Action |
|-------|--------|
| **Left Click** | Select |
| **Right Click** | Command |
| **Drag Left** | Box Select |
| **WASD/Arrows** | Pan Camera |
| **Mouse Wheel** | Zoom |
| **Q/E** | Rotate Camera |
| **Middle Mouse** | Drag Pan |
| **Ctrl+#** | Create Group |
| **#** | Select Group |
| **Shift+Click** | Add to Selection |
| **Double Click** | Select All Type |
| **S** | Stop |
| **A** | Attack Move |

---

## **📝 APPENDICES**

### **A. File Structure**
```
Assets/_Game/
├── Scripts/Core/
├── Scripts/Systems/
├── Scripts/Components/
├── Data/Units/
├── Data/Buildings/
├── Data/Weapons/
├── Prefabs/
└── Scenes/
```

### **B. Dependencies**
- Unity 6.0+
- Universal Render Pipeline
- AI Navigation Package
- TextMeshPro

### **C. Development Tools**
- Unity Editor
- Visual Studio/VS Code
- Git Version Control
- Notion/Trello (Task tracking)

---

**Document Version:** 1.0  
**Last Updated:** Current Development Build  
**Next Review:** After Milestone 2 Completion