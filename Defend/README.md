# GameJam-Defend
For this jam I'm trying to recreate the mechanics from a certain flavor of the tower defence genre, my inspiration being the WC3 custom game "Wintermaul".
<br>In this game there is no separation between enemy pathing and tower placement.
<br>Enemies try to take the shortest path between the start, any intermediate path points, and the end.
<br>Towers can be placed anywhere in the area, even disrupting enemy pathing. 
<br>This means players can strategically place their towers in such a way to maximize the time the enemies are in range of their towers.
<br>If the player places their towers in such a way that the enemy path is blocked, the enemies attack and destroy the towers until a valid path exists again.
<br>Buildings are typically 2x2 units wide. Enemies only need 1 unit to move through.

## Features so far
### Grid visualization
Made with a shader graph shader using world position to draw grid lines and inputs for highlighted square and size. 
This will appear when placing a building to help the user place buildings accurately without drawing the grid everywhere.
### Tower Placement
Towers can be placed on the grid. The cells they occupy are marked as such, so towers can not be placed on top of each other.
### Camera Controls
Hold right-click and drag to pan the camera. This can be done while placing a tower.<br>
Mouse wheel to zoom. This can be done while panning.
### AI Pathing
I tried implementing my own pathfinding with the A* algorithm. This worked well for most cases, except where the goal is blocked off and the path leads directly away from the spawn location. In this case the pathing would head directly towards the goal until it hits the block, then slowly expand out in all directions until the search radius was big enough to get around the blockade.
<br>I could've spent more time coming up with better search heuristics (e.g. also path from the goal to the start, favor searching around placed towers etc). But then there were still a bunch of other problems to solve, such as changing the path when a tower is placed, how to handle placing the tower on enemies, and getting enemies to follow the path in a nice way. And I'm sure there would've been a plethora of other issues that arose along the way.
<br>And this is a gamejam, time is valuable! So I tried out Unity's AI Navigation package and it worked really well with very little setup. So I'm using that now. 

## Roadmap
### Gameplay
- ✔ ~~Grid visualization~~
- ✔ ~~Tower placement~~
    - ✔ ~~Mark cells as blocked for the pathing generator.~~
- ✔ ~~Camera movement~~
    - ✔ ~~Click and drag the ground to pan the camera~~
    - Restrict the camera to the game area
- 〰 ~~Path generator~~ 
    - ~~Take a start point, end point and blocked cells list. Use A* to find a path between the points.~~
    - ~~Enemies follow this path. Use a spline for more natural movement.~~
    - ~~Refresh the path whenever a tower is placed~~
- Minion spawning
    - Come up with a way to define waves of enemies
    - Enemies follow the pathing spline
- Towers attack and enemies die
    - Show tower range when placing it
    - Enemies have a coin reward
    - The original game had a way to force a tower to focus on a targetted enemy. Consider this.
- Tower selling and upgrades
    - Sell for 75% of their original cost
    - Upgrade system (trees?)
### Tech
- Switch to UI Toolkit from uGUI