REFORM REPORT – by Bruno Leote
==============================

Summary
-------
This document outlines the improvements made to the original Unity Snake game project, highlighting my professional fit for the **Senior Unity Developer** role at GameHouse.

The main goal was to improve architecture, maintainability, memory usage, testability, and prepare the game for future scalability and LiveOps support — all within the given time box.

Tasks Overview
--------------

1. MEMORY MANAGEMENT
---------------------
- Replaced sound spawning per SFX call with a centralized SoundManager that reuses preloaded sounds.
- Replaced all `transform.Find` calls with serialized references to reduce lookup costs.
- Reduced scene loader complexity by merging the previous 2-class system into a single persistent loader.
- Removed unnecessary GameObjects and Cameras from the scene hierarchy.
NOTE: pooling was considered for spawning apples and making the snake body growth, but due to the low frequency of these events, polling would just add an unnecessary complexity to the game logic.

2. TESTABILITY & UNIT TESTS
----------------------------
- Refactored `Snake` class into two: 
  - `SnakeModel` (pure logic, testable)
  - `SnakeController` (MonoBehaviour, input/visuals)
- Created and passed unit tests for:
  - Snake movement, growth, self-collision, direction handling
  - Grid wrapping logic (LevelGrid)
  - Score progress and Highscore record 
- Project was structured with proper `Tests.asmdef` and Edit Mode test environment.
- Avoided testing MonoBehaviours directly, focusing on isolated, logic-heavy units.

3. LIVEOPS-READY ARCHITECTURE
-----------------------------
- Introduced a `RemoteConfigManager` singleton that simulates remote JSON config parsing.
- The configurable parameters (e.g., snake sprites, move speed, start direction) are now read from config. The list of configurable parameters can be extended as needed.
- Comments and structure left in place for integration with Firebase Remote Config or Unity Remote Config in production.

4. MAINTAINABILITY & EXTENDABILITY
----------------------------------
- Modularized the codebase into multiple assemblies:
  - `GameAssembly` for core logic
  - `UtilsAssembly` for helper components
  - `TestsAssembly` for unit tests
- Improved class and functions naming, organization, and separation of responsibilities.
- Improved scene organization, with clearer breakdowns that help readability.
- Reusable interfaces (e.g., `IGameInput`) introduced to allow for future platform-specific inputs.

5. REDUCING COMPILE TIMES
--------------------------
- Separated code into `.asmdef` files:
  - Prevented unnecessary recompilation of unrelated code (e.g., menu logic when editing gameplay).
- Kept external code (CodeMonkey) isolated to its own assembly.

6. MULTIPLATFORM INPUT SUPPORT
-----------------------------------------
- Legacy `Input.GetKeyDown` logic abstracted into `IGameInput` interface.
- Introduced `PCGameInput` class using current Input system.
- Architecture is now ready for plug-and-play controller, mobile, or new Unity Input System without touching game logic.

Why These Tasks Were Prioritized
--------------------------------
- **Memory and compile-time optimizations** were low-effort/high-impact wins.
- **Testability** and **architecture** were clear priorities to make future LiveOps, platform support, and feature expansion easier and safer.
- **LiveOps scaffolding** was mocked in a realistic and extensible way, allowing for future growth.

Remaining (Optional) Work
-------------------------
- Implement actual remote config (Firebase/Unity RC).
- Add controller/touchscreen input support using `IGameInput`.
- Consider ScriptableObject-based asset loading for deeper LiveOps.

All improvements were scoped and prioritized to fit the challenge window without overengineering.

Thanks for reading!
Bruno Leote
