# GameJam-Defend
For this jam I'm trying to recreate a certain flavor of tower defence games, my inspiration being the WC3 "Maul TD"-style custom games.
<br>In these games there is no separation between minion pathing and tower placement.
<br>Minions try to take the shortest path between the start, any intermediate path points, and the end.
<br>Towers can be placed anywhere in this area, even disrupting the minions pathing. 
<br>This means players can strategially place their towers in such a way to maximize the time the minions are in range of their towers.
<br>If the player places their towers in such a way that the minions path is blocked, the minions attack and destroy the towers until a valid path exists again.
<br>Buildings are typically 2x2 units wide. Minions only need 1 unit to move through.

## Features so far
### Grid visualization
Made with a shader graph shader using world position to draw grid lines and inputs for highlighted square and size. 
This will appear when placing a building to help the user place buildings accurately.


## TODO
### Gameplay
- ~~Grid visualization~~
- Tower placement 
    - Mark cells as blocked for the pathing generator.
- Path generator
    - Take a start point, end point and blocked cells list. Use A* to find a path between the points.
    - Refresh whenever a tower is placed
    - Generate a spline for more organic minion movement
### Tech
- Switch to UI Toolkit from uGUI