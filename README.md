# Minesweeper

This is a console Minesweeper game made in C#. It’s split into a Core project (game logic) and a Console project (UI stuff). It also has seeded boards and a high score system that saves to a file.


## Board sizes

- 8x8 with 10 mines  
- 12x12 with 25 mines  
- 16x16 with 40 mines  

## How to play

At the start you enter a level number (seed). That controls how the board is generated.

- Same seed = same board every time  
- Different seed = different layout  
- If you press Enter it just picks a random one  

Goal is to reveal all the safe tiles without hitting a mine.

## Controls

r row col → reveal a tile  
f row col → flag/unflag a tile  
q → quit game  

Example:
r 2 3  
f 1 1  

## Symbols on board

# = hidden tile  
f = flagged tile  
b = bomb (only shows if you lose)  
. = empty tile  
1-8 = number of nearby mines  

## Seed 

The seed is the level number.

If you reuse it, you get the same board again which is helpful for testing or replaying.

## High scores

Scores get saved automatically when you win.

They go into:
data/scores.csv

Format is:
size,seconds,moves,seed,timestamp

Only the top 5 best scores per board size are kept.

## Unit tests

If included, tests are written with xUnit and can be run in Visual Studio Test Explorer.

They cover stuff like:
- board generation
- mine placement
- reveal logic
- cascade behavior
- win condition

## Structure

Core has all the game logic  
Console just handles input/output  
data folder stores scores