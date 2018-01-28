# Features

- [x] It is based on tiles, like an old 2D Legend of Zelda game.
- [x] People are walking in a predictable pattern.
- [x] If bump in middle of walking between tiles, reverse direction.
- [x] A player interacts with the mobile phone by tapping any tile to place the virus.
- [x] The whole restaurant fits on the screen of a mobile phone.
- [x] A cute restaurant with people walking around.
- [x] When a person begins to overlap a virus tile, they pickup the virus and continue straight if they can.
- [x] After a second or two, the virus replicates in the person.
- [x] When a sick person bumps into another person, a copy of the virus transmits.
- [x] Some people go up and down.
- [x] To make it plausible why everyone is walking, and why there are arbitrarily placed obstacles, we were thinking some obstacles look a buffet, counter, or bar.
- [x] Animates how many copies of a virus.
- [x] Animates time progressing to next virus replication.
- [x] If a person gets four copies of the virus, the person immediately leaves.

# To do

- [ ] Read tap anywhere to place a virus.
- [ ] Start screen:  Germs!
- [ ] End screen.
- [ ] Score number of people sick.
- [ ] The player earns a score by how many seconds it took them to get everyone in the restaurant sick.
- [ ] Animates person with copies of virus.
- [ ] People do not get stuck in a corner.
- [ ] When bump into something, person turns right.
- [ ] Level 2.
- [ ] Read how many infected.
- [ ] Read best time.

# Technical design

- [ ] Right turner component
    - [ ] Velocity 2D
- [ ] Right turner system
    - [ ] Move in current direction.
    - [ ] Bump.
- [ ] Grid of tiles
- [ ] Ensures system accessed before its component is enabled.
    - [ ] Load systems in previous scene.  DontDestroyOnLoad.

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

