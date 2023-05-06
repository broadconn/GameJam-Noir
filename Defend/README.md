# GameJam-Defend
For this jam I'm trying to recreate the mechanics from a certain flavor of the tower defence genre, my inspiration being the WC3 custom game "Wintermaul".
<br>In this game there is no separation between enemy pathing and tower placement.
<br>Enemies try to take the shortest path between the start, any intermediate path points, and the end.
<br>Towers can be placed anywhere in the area, even disrupting enemy pathing. 
<br>This means players can strategially place their towers in such a way to maximize the time the enemies are in range of their towers.
<br>If the player places their towers in such a way that the enemy path is blocked, the enemies attack and destroy the towers until a valid path exists again.
<br>Buildings are typically 2x2 units wide. Enemies only need 1 unit to move through.

## Features so far
### Grid visualization
Made with a shader graph shader using world position to draw grid lines and inputs for highlighted square and size. 
This will appear when placing a building to help the user place buildings accurately without drawing the grid everywhere.
### Tower Placement
Towers can be placed on the grid. The cells they occupy are maked as such, so towers can not be placed on top of each other.


## Roadmap
### Gameplay
- ✔ ~~Grid visualization~~
- ✔ ~~Tower placement~~
    - ✔ ~~Mark cells as blocked for the pathing generator.~~
- Path generator
    - Take a start point, end point and blocked cells list. Use A* to find a path between the points.
    - Enemies follow this path. Use a spline for more natural movement.
    - Refresh the path whenever a tower is placed
- Minion spawning
    - Come up with a way to define waves of enemies
    - Enemies follow the pathing spline
- Towers attack and enemies die
- Camera movement
    - Click and drag the ground to pan the camera
    - Restrict the camera to the game area
- Tower selling and upgrades
    - Sell for 75% of their original cost
    - Upgrade system (trees?)
### Tech
- Switch to UI Toolkit from uGUI