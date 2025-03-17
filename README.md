# Zola Dadalg
This project is an implementation of a niche board game called Zola. It includes two AI opponents with algorithms proposed to me by my Dad. 

[Play Online](https://simmer.io/@reedoover/zola-with-bots)

[Download](https://reedoover.itch.io/zola)

I’d like to make it clear that I don’t take any credit for the game of Zola itself, only this program. The original board game Zola was created by Mark Steere and can be played online on his [website](https://boardgamegeek.com/boardgame/331666/zola). My motivation for the project started with my Dad who had been playing Zola consistently online for a week or two back in 2022. He is a math professor and would analyze these types of games, coming up with algorithms to try and solve them. Thus my inspiration for this project. I was already into Unity at the time so I decided to try building my own version of the game he and I could play and together and implement his algorithms. The two AIs playable in this program are both based on algorithms he proposed, edited to fit the project's semantics. The files for the "easy" and "hard" AIs are still named “DadAlgNum1” and “DadAlgNum2” in the final build of the game which I think is pretty funny.

## Table of Contents
- [Game Rules](#game-rules)
- [Project Features](#project-features)
    - [Interface](#interface)
    - [Movement Restrictions](#movement-restrictions)
    - [AI Algorithms](#ai-opponents)
    - [AI Improvement](#ai-improvement)
- [About Me](#about-me)

## Game Rules
Below are Zola's rules, (pdf created by [Mark Steere](https://boardgamegeek.com/boardgamedesigner/2321/mark-steere)). I found this PDF on his old website (which no longer exists), where he generously gave permission to use it for gameplay explanation. Mark, if you're reading this and have changed your mind about permissions or have any concerns regarding the contents of this project, please reach out. P.S. I think you do great work!
![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Graphics/ZolaRules.png)

## Project Features

### **Interface**
#### **Board**
The board is made up of separate game objects representing the each tile. Each tile object has an [Tile_ID.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Tile/Tile_ID.cs) script to keep track of state as well as it's posistion on the board and a collider to allow for interactability. This way when a tile is clicked, the correct data is sent to Game Controller to be evaluated and acted upon. See [GameController.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/GameControl.cs) and [Movement.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Movement.cs) for the code.

Zola is playable with different board sizes (standard being 6x6), this project supports 4x4, 6x6, & 8x8 tile boards with a notable decrease in AI response time as the boardsize is increased.

| 4x4 | 6x6 | 8x8 |
|-----|-----|------|
| ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/4x4sc.png) | ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/6x6sc.png) | ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/8x8sc.png) |

At the begining of the game, the board is drawn based on the parameters provided by the user in the game menu, using indexing to place tiles in their correct location for the selected configuration (see [DrawBoard.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Board/DrawBoard.cs)). 

```c#
Vector2 spawnPos = new Vector2(x * tileIndent, y * tileIndent) + zeroZero + new Vector2(tileIndent / 2, tileIndent / 2);
```

#### **Game Log & Move Counter**
The game log is a quality of life feature added to increase user comprehension. Logging the order in which moves were taken and their nature, similar to online chess interfaces.

The "possible moves" counter (found in the top left), uses logic from the AI algorithms to calculate the number of legal moves playable at in given state (see [Finding Possible Moves](#finding-possible-moves)). Zola is not commonly played and it can sometimes be difficult to find a good move. By showing the player the number of possible moves makes it easier for them to identify possible courses of action without straight up telling them what to do. Note that it is possible to have zero moves. In this case the opposing player simply gets to move again (see [Game Rules](#game-rules) for further explanation).

### **Movement Restrictions**

Movement restrictions in Zola ensure that players adhere to specific rules when selecting and moving pieces, preserving the integrity and strategic depth of gameplay. Restrictions are divided into two primary types: capturing moves and non-capturing moves.

#### **Capturing Moves**
A capturing move occurs when a player attempts to take control of an opponent's occupied tile. To be valid, capturing moves must adhere to the following conditions:

- The selected piece cannot attempt to move onto its own tile (self-capture).
- Players cannot capture their own pieces.
- Moves must be linear (straight horizontal, vertical, or diagonal). Non-linear movements are disallowed.
- The piece must move towards or maintain an equal distance from the board's center (DFC). Moving closer to the center than the starting position is illegal.
- No piece can jump over another piece (blocking pieces make the move illegal).

An example of this is  the `CapturingMove` function of [Restrictions.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Restrictions.cs) which evaluates the legality of a capture. This code snip is one of the checks done in `CapturingMove` to make sure the pieces aren't being skipped over.

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

All the capturing move restrictions are validated using checks within the `CapturingMove` and `CapturingMove_CustomPieces` functions, found in [Restrictions.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Restrictions.cs).

#### **Non-Capturing Moves**
A non-capturing move involves moving a player's piece to an empty tile. Conditions for legality include:

- Moves must be exactly one tile in any direction.
- The selected tile must be empty (unoccupied).
- The target tile must have a strictly higher distance from the board’s center (DFC) than the starting tile.
- Moves must remain linear or diagonal, without any jumps.

Tthe non-Capturing move restrictions are validated using the `NonCapturingMove` and `NonCapturingMove_CustomPieces` functions, found in [Restrictions.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Restrictions.cs).

These restrictions were critical to implementing accurate gameplay logic and ensuring the AI algorithms could reliably evaluate valid moves in their algorithms.


### **AI Algorithms**
To be clear, the "AI" algorithms in this project do not actually implement any **learning** intelligence (ML, DL, etc) and are purely logic based. That being said they are able to evaluate the game state and make semi-intelligent desisions, so I will continue to use the term AI throughout my explanations.

#### **Finding Possible Moves**
Both AI's, first step when making a move is to retrieve the list of possible moves. Moves are found by iterating through a list of Vector4s representing all legal moves. That list is calculated by iterating through each tile on the board, retrieving its data from the ID component and using the checks outlined in [Movement Restrictions](#movement-restrictions) to evaluate legality.

```c#
/// <summary>
/// Returns all posible moves for the passed color. Returns a list of Vector4s where (x, y) is the piece's current position and
/// (z, w) is the target position.
/// </summary>
/// <param name="color"></param>
/// <returns></returns>
public static List<Vector4> FindAllMovesV4(string color)
```

See the `FindAllMovesV4` (easy AI) & `FindAllMovesV5` (hard AI) functions in [Restrictions.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Restrictions.cs) for the full code.

#### **Evaluation**
My Dad's core theory behind is algorithms was to value tiles based on that tile's distance from the center of the board (noted as DFC), as that determines their capturing power. Because Zola is only played on boards with even dimensions I needed to calculate the [euclidean distance](https://en.wikipedia.org/wiki/Euclidean_distance) to the center to get an accurate value. Meaning the distance from the center of the tile to the point where the 4 center squares meet (see [BoardState.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Board/BoardState.cs) for calculations).

The **Easy AI**, iterates through the list of possible moves, comparing the difference in total among it's DFC values (see `GetDFCScore` in [DadAlgNum1.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/AI/DadAlgNum1.cs)) for each possible move. Selecting the move that nets the largest gain in it's DFC score.

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

This method was is the most basic implementation of my dads algorithm. It plays decently well and will likely beat someone new to the game.

The **Hard AI** takes the euclidean DFC theology from the Easy AI and goes step further by looking one move further into the future. Taking into account all every response to all of it's possible moves. It does this by calculating the worst case scenario for every given move. Selecting the move with the least risk of DFC loss (see `FindBestMove` in [DadAlgNum2.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/AI/DadAlgNum2.cs)).

When playtesting the algorithm with my dad we observed that I, a less experienced Zola player, tended to perform better. We guessed that this defensive style of play made the Hard AI better against players thinking many moves into the future. This would make some sense as there are similar cases with chess engines being broken by using non-standard playstyles. Without any failsafes in place to prevent the AI from overestimating it's opponent I might consider the "Hard" AI to actually be weaker than the easy one. I do have some ideas for improving the performance of the hard AI if I were to revisit the project (see [AI Improvement](#ai-improvement)).


### **AI Improvement**
If I revisit this project, enhancing the AI would be my number one priority. The current algorithms, while functional, are simple and very inefficient, leading to a noticeable decrease in response time as the board size increases. Currently, every possible scenario is evaluated independently, causing unnecessary calculations for moves could be identified early as not worth considering. Introducing early-pruning techniques—such as [alpha-beta pruning](https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning) or [memoization](https://en.wikipedia.org/wiki/Memoization#:~:text=In%20computing%2C%20memoization%20or%20memoisation,the%20same%20inputs%20occur%20again.)—would dramatically decrease the number of unnecessary evaluations, speeding up AI decision-making would allow me to expand the depth of the algorithm. A depth-limited minimax algorithm combined with alpha-beta pruning would make for a streamlined and far more intelligent version of the current "Hard" AI. Another promising avenue is to explore iterative deepening, allowing the AI to progressively deepen its analysis within given time constraints to produce the best possible move under time pressure. Lastly, I could implement some heuristic optimizations like prioritizing evaluation based on board position patterns or historical game states. This option is less actractive to me as it would require some pretty intense analysis on Zola's game strategy and would require implementing some form of database.

Ultimately, any of these improvements would not only enhance AI efficiency but also significantly improve the quality and responsiveness of gameplay, resulting in a more engaging and challenging user experience.

## About Me
My name is Reed Bryan, I am currently a software engineering student at the University of Victoria. In my free time, I enjoy indie game development and have always found creativity to be a great motivator for building technical projects. I started this project in 2022, deciding to finally finish and release it around 2 years later in December of 2023. Recently, as I've started applying for jobs, I've been revisiting some of my older Unity projects to share online—this project being one of them. My passion for game development began in 2020 after taking a computer programming course in the 10th grade. They started us with this great web-based block coder called scratch which helped me to build an understanding of programming logic before my teacher recommended I moved on to Unity. Unity is a widely used commercial game engine for developing both professional titles enjoyed by millions and smaller personal projects like mine. Over the last five years, I've gained a sincere appritiation for the platform as it has been an encredible source of both learning and creativity.