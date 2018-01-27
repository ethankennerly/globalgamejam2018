# Features

# To do

- [ ] It is based on tiles, like an old 2D Legend of Zelda game.
- [ ] People are walking in a predictable pattern, turning right when they bump into something.
- [ ] I'm programming, that a player interacts with the mobile phone by tapping any tile to place the virus.
- [ ] When a person bumps into a virus tile, they pickup the virus and continue straight if they can.
- [ ] After a second or two, the virus replicates in the person.
- [ ] When a sick person bumps into another person, a copy of the virus transmits.
- [ ] If a person gets four copies of the virus, the person immediately leaves.
- [ ] The player earns a score by how many seconds it took them to get everyone in the restaurant sick.
- [ ] Cindy is illustrating a cute restaurant with people walking around.
- [ ] The whole restaurant fits on the screen of a mobile phone.
- [ ] To make it plausible why everyone is walking, and why there are arbitrarily placed obstacles, we were thinking some obstacles look a buffet, counter, or bar.

# Technical design

- [ ] Right turner component
    - [ ] Velocity 2D
- [ ] Right turner system
    - [ ] Move in current direction.
    - [ ] Bump.
- [ ] Grid of tiles

Entity moves.
Collides.
Tile.
Snap.
Step on virus sick index 1.
Detect next tile.
Turn.
Sick timer.
Sick index.
Sick 4 leaves.
Contact spreads.
Total time.
Best time.
Time penalty for living.
Level 2.



Mobile Tile velocity x y.  Arrival time.  Speed.
Right turner
OnEnable adds to system.

System:
Dontdestroyinload
On add:  Snap to tile center.
update
Move by time.
Overflow time to next.
Sync position between tiles.
At arrival time, snap position, look at what’s at next tile.
If obstacle, rotate velocity right.

Tap:  place virus at tile.
World to tile.

If tile is unreachable do not place.  Corners are unreachable.
Unreachable has no movable tile adjacent.


Infection count, time
OnEnable:  add to system
System
Add On Tile collide event
If count 2 or more and not equal to other, transfer.
If count 4 stop walking.  Add leaving.
After leaving,
Disable leaver.
dispatch entity leaving.

Dispatch change in count of infections.
No more infections.  Game over.

