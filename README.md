# 🔫 Ricochet Shooter - Physics Puzzle

> A tactical shooter game based on vector reflection logic, trajectory prediction, and slow-motion effects.

![Image](https://github.com/user-attachments/assets/ced84de7-3864-417a-ba54-2d5ee777dbc6)

## 🎮 About The Project
Ricochet Shooter demonstrates proficiency in **Unity Physics**, **Vector Mathematics**, and **Raycasting**. The player must calculate angles to eliminate enemies hiding behind obstacles using bouncing bullets.

## 🛠 Tech Stack
*   **Engine:** Unity 6.3 (LTS)
*   **Language:** C#
*   **Physics:** Unity 3D Physics (Physic Material for bounciness)
*   **Rendering:** LineRenderer, URP (Universal Render Pipeline) suggested.

## 🚀 Key Technical Features

### 1. Trajectory Prediction (Raycasting)
*   Implemented a **Multi-step Raycast system** to predict the bullet's path before firing.
*   Used `Vector3.Reflect` to calculate accurate bounce angles off walls.
*   Visualized the path using `LineRenderer`, dynamically updating in real-time as the player aims.

### 2. Physics & Materials
*   Configured custom **Physic Materials** (Friction: 0, Bounciness: 1) to create a "perpetual bounce" effect essential for the gameplay loop.
*   Handled collision layers to distinguish between obstacles, enemies, and bounds.

### 3. Polish & Game Feel
*   **Slow Motion (TimeScale):** Implemented a `Time.timeScale` manipulation system when the final enemy is hit, creating a dramatic "Matrix-style" effect.
*   **FixedDeltaTime Adjustment:** Correctly adjusted `Time.fixedDeltaTime` during slow-mo to ensure smooth physics simulation.
*   **Shatter Effect:** Programmed a procedural mesh shattering effect (spawning debris with explosion force) to replace the enemy upon death.

### 4. Level System
*   Built a scalable `GameManager` to handle win/loss states and scene transitions.
*   Supports rapid level creation using modular walls and enemy prefabs.

## 🕹️ How to Play
*   **Drag Mouse:** Aim the laser sight.
*   **Release:** Fire the bullet.
*   **Goal:** Hit the red enemy (use walls to bounce bullets around obstacles).

---
**Developed by [Dang Hoang Nhu]**
[https://www.linkedin.com/in/%C4%91%E1%BA%B7ng-nhu-257b09317/]
