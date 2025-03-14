# zola-dadalg
This project is an implementation of a niche board game called Zola. It Includes two AI opponents with algorithms proposed to me by my Dad. 

I’d like to make it clear that I don’t take any credit for the game of Zola itself, only this program. The original board game Zola was created by Mark Steere and can be played online on his [website](https://boardgamegeek.com/boardgame/331666/zola). My motivation for the project started with my Dad who had been playing Zola consistently online for a week or two back in 2022. He is a math professor and would analyze these types of games, coming up with algorithms to try and solve them. Thus my inspiration for this project. I was already into Unity at the time so I decided to try building my own version of the game he and I could play and together and implement his algorithms. The two AIs playable in this program are both based on algorithms he proposed, edited to fit the project's semantics. The files for the "easy" and "hard" AIs are still named “DadAlgNum1” and “DadAlgNum2” in the final build of the game which I think is pretty funny.

## Table of Contents
- [Game Rules](#game-rules)
- [Project Features](#project-features)
    - [Interface](#interface)
    - [AI algorithms](#ai-opponents)
- [About Me](#about-me)

## Game Rules
Below are the Zola's game rules, outlining gameplay created by Mark Steere.
![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Graphics/ZolaRules.png)

## Project Features

### Interface
The game's interface is created from scratch, using only sample sprites provided in the editor upon creating the project.

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

The "possible moves" counter (found in the top left), uses logic from the AI algorithms to calculate the number of legal moves playable at in given state (see [AI algorithms](#ai-opponents)). Zola is not commonly played and it can sometimes be difficult to find a good move. By showing the player the number of possible moves makes it easier for them to identify possible courses of action without straight up telling them what to do. Note that it is possible to have zero moves. In this case the opposing player simply gets to move again (see [Game Rules](#game-rules) for futher explination).

### AI algorithms


## **About Me**
My name is Reed Bryan and I am currently a software engineering undergrad at the University of Victoria. I started this project in 2022, decided to finally finish and release it around 2 years later in December of 2023. This is one of the projects I started but never managed to finish during that time. Recently I’ve started applying for jobs so I’ve been working to put more of them online. Back in 2020 I got really into game development after taking a computer programming course in the 10th grade. I started with a great web-based block coder called scratch which helped me to build an understanding of programming logic before I moved on to Unity. Unity is a free commercial game engine used to create professional games played by millions of people, and to create smaller projects like this one. I’ve been using Unity for over 5 years now and have a lot of appreciation for it as it's been a great source of learning and creativity for me.