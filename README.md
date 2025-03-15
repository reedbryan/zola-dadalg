# zola-dadalg
This project is an implementation of a niche board game called Zola. It Includes two AI opponents with algorithms proposed to me by my Dad. 

I’d like to make it clear that I don’t take any credit for the game of Zola itself, only this program. The original board game Zola was created by Mark Steere and can be played online on his [website](https://boardgamegeek.com/boardgame/331666/zola). My motivation for the project started with my Dad who had been playing Zola consistently online for a week or two back in 2022. He is a math professor and would analyze these types of games, coming up with algorithms to try and solve them. Thus my inspiration for this project. I was already into Unity at the time so I decided to try building my own version of the game he and I could play and together and implement his algorithms. The two AIs playable in this program are both based on algorithms he proposed, edited to fit the project's semantics. The files for the "easy" and "hard" AIs are still named “DadAlgNum1” and “DadAlgNum2” in the final build of the game which I think is pretty funny.

## Table of Contents
- [Game Rules](#game-rules)
- [Project Features](#project-features)
    - [Interface](#interface)
    - [Movement Restrictions](#movement-restrictions)
    - [AI algorithms](#ai-opponents)
- [About Me](#about-me)

## Game Rules
Below are the Zola's game rules, created by [Mark Steere](https://boardgamegeek.com/boardgamedesigner/2321/mark-steere). I found this PDF on his old website (which no longer exists), where he generously gave permission to use it for gameplay explanation. Mark, if you're reading this and have changed your mind about permissions or have any concerns regarding the contents of this project, please reach out. P.S. I think you do great work!
![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Graphics/ZolaRules.png)

## Project Features

### Interface
#### Board
The board is made up of seperate game obejcts representing the each tile. Each tile object has an [Tile_ID.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Tile/Tile_ID.cs) script to keep track of state as well as it's posistion on the board and a collider to allow for interactibility. This way when a tile is clicked, the correct data is sent to Game Controller to be evaluted and acted upon. See [GameController.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/GameControl.cs) and [Movement.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Movement.cs) for the code.

Zola is playable with different board sizes (standard being 6x6), this project supports 4x4, 6x6, & 8x8 tile boards with a notatble decrease in AI response time as the boardsize is increased.

| 4x4 | 6x6 | 8x8 |
|-----|-----|------|
| ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/4x4sc.png) | ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/6x6sc.png) | ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/8x8sc.png) |

At the begining of the game, the board is drawn based on the perameters provided by the user in the game menu, using indexing to place tiles in their correct location for the selected configuration (see [DrawBoard.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Board/DrawBoard.cs)). 
```c#
Vector2 spawnPos = new Vector2(x * tileIndent, y * tileIndent) + zeroZero + new Vector2(tileIndent / 2, tileIndent / 2);
```

#### Game Log & Move Counter
The game log is a quality of life feature added to increase user conprehension. Logging the order in which moves where taken and their nature, similar to online chess interfaces.

The "possible moves" counter (found in the top left), uses logic from the AI algorithms to calculate the number of legal moves playable at in given state (see [Finding Possible Moves](#finding-possible-moves)). Zola is not commonly played and it can sometimes be difficult to find a good move. By showing the player the number of possible moves makes it easier for them to identify possible courses of action without straight up telling them what to do. Note that it is possible to have zero moves. In this case the opposing player simply gets to move again (see [Game Rules](#game-rules) for futher explination).

### Movement Restrictions

ADD


### AI algorithms
To be clear, the "AI" algorithms in this project do not actually implement any **learning** intelgence (ML, DL, etc) and are purely logic based. That being said they are able to evaluate the game state and make semi-intellegent desisions, so I will continue to use the term AI throughout my explinations.

#### Finding Possible Moves
Both AI's, first step when making a move is to retrieve the list of possible moves. Moves are found by iterating through tiles, evaluating legality by checking for any broken restrictions such as:
- Jumping a piece
- Moving non-linearly
- Capturing ilegally
Due to Zola's movement restrictions these checks became complex, taking many different variables into account. An example of this is  the `CapturingMove` function of [Restrictions.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Restrictions.cs) which eveluates the legality of a capture. This code snip is one of the checks done in `CapturingMove` to make sure the pieces aren't being skipped over.

```c#
// Blocking piece check
BoardState BS = GameObject.Find("Board").GetComponent<BoardState>();

Vector2 rawDiff = targetTileID.position - currentTileID.position;
int indent;
if (rawDiff.x == 0)
    indent = (int)rawDiff.y;
else
    indent = (int)rawDiff.x;
indent = Mathf.Abs(indent);

for (int i = 1; i <= indent; i++)
{
    GameObject midTile = BS.GetTileFromPosition(currentTileID.position + ((rawDiff / indent)) * i);
    if (debug)
        Debug.Log("Mid tile#" + (i + 1) + ": " + midTile.GetComponent<Tile_ID>().position);
    if (midTile.GetComponent<Tile_ID>().occupent != null && midTile != targetTile)
    {
        if (debug)
            Debug.Log("Is occupied");
        return false;
    }
}
```

This is done in the [Restrictions.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Restrictions.cs) script, see the `FindAllMovesV4` (easy AI) & `FindAllMovesV5` (hard AI) functions for the code.

#### Evaluation
My Dad's core theory behind is algorithms was to value tiles based on that tile's distance from the center of the board (noted as DFC), as that determines their capturing power. Because Zola is only played on boards with even dimentions I needed to calculate the [euclidean distance](https://en.wikipedia.org/wiki/Euclidean_distance) to the center to get an accurate value. Meaning the distance from the center of the tile to the point where the 4 center squares meet (see [BoardState.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Board/BoardState.cs) for calculations).

The **Easy AI**, iterates through the list of possible moves, comparing the difference in total among it's DFC values (see `GetDFCScore` in [DadAlgNum1.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/AI/DadAlgNum1.cs)) for each possible move. Selecting the the move that nets the largest gain in its DFC score.

```c#
Vector4 FindBestMove()
{
    List<Vector4> allMoves = Restrictions.FindAllMovesV4(color); // all legal moves for <color> pieces
    Vector4 bestMove = new Vector4(0, 0, 0, 0);
    float highestDFCScore = float.NegativeInfinity;

    foreach (var move in allMoves)
    {
        float futureDFCScore = GetDFCScore(color, move);
        if (futureDFCScore > highestDFCScore)
        {
            highestDFCScore = futureDFCScore;
            bestMove = move;
        }
    }

    return bestMove;
}
```

This method was is the most basic implementation of my dads algoritm. It plays decently well and will likely beat someone new to the game.

The **Hard AI** takes the euclidean DFC theology and takes it step further by looking one move further into the future. Taking into account all every responce to all of it's possible moves. 

## **About Me**
My name is Reed Bryan and I am currently a software engineering undergrad at the University of Victoria. I started this project in 2022, decided to finally finish and release it around 2 years later in December of 2023. This is one of the projects I started but never managed to finish during that time. Recently I’ve started applying for jobs so I’ve been working to put more of them online. Back in 2020 I got really into game development after taking a computer programming course in the 10th grade. I started with a great web-based block coder called scratch which helped me to build an understanding of programming logic before I moved on to Unity. Unity is a free commercial game engine used to create professional games played by millions of people, and to create smaller projects like this one. I’ve been using Unity for over 5 years now and have a lot of appreciation for it as it's been a great source of learning and creativity for me.