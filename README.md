# zola-dadalg
This project is an implementation of a niche board game called Zola. It Includes two AI opponents with algorithms proposed to me by my Dad. 

I’d like to make it clear that I don’t take any credit for the game of Zola itself, only this program. The original board game Zola was created by Mark Steere and can be played online on his [website](https://www.marksteeregames.com/). My Dad was playing Zola consistently on said website for a week or two back in 2022. He is a math professor and would analyze these types of games, coming up with algorithms to try and solve them. Thus my inspiration for this project. I was already into Unity at the time so I decided to try building my own version of the game he and I could play and together and implement his algorithms. The two AIs playable in this program are both based on algorithms he proposed, edited to fit the projects semantics. The files for the "easy" and "hard" AIs are still named “DadAlgNum1” and “DadAlgNum2” in the final build of the game which I think is pretty funny.

## Table of Contents
- [Project Features](#project-features)
    - [Interface](#interface)
    - [AI algorithms](#ai-opponents)
- [About Me](#about-me)

## **Project Features**

### Interface
The game's interface is created from scratch, using only sample sprites provided in the editor upon creating the project.

#### Board
The board is made up of seperate game obejcts representing the each tile. Each tile object has an [Tile_ID.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Tile/Tile_ID.cs) script to keep track of state as well as it's posistion on the board and a collider to allow for interactibility. This way when a tile is clicked, the correct data is sent to Game Controller to be evaluted and acted upon.

See [GameController.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/GameControl.cs) and [Movement.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Management/Movement.cs) for the code.

At the begining of the game, the board is drawn based on the perameters provided by the user in the game menu (see [DrawBoard.cs](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/Board/DrawBoard.cs)). Zola is playable with different board sizes (standard being 6x6), this project supports 4x4, 6x6, & 8x8 tile boards with a notatble decrease in AI response time as the boardsize is increased.

| 4x4 | 6x6 | 8x8 |
|-----|-----|------|
| ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/4x4sc.png) | ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/6x6sc.png) | ![Alt text](https://github.com/reedbryan/zola-dadalg/blob/main/Assets/README-SC/8x8sc.png) |

### AI algorithms


## **About Me**
My name is Reed Bryan and I am currently a software engineering undergrad at the University of Victoria. I started this project in 2022, decided to finally finish and release it around 2 years later in December of 2023. This is one of the projects I started but never managed to finish during that time. Recently I’ve started applying for jobs so I’ve been working to put more of them online. Back in 2020 I got really into game development after taking a computer programming course in the 10th grade. I started with a great web-based block coder called scratch which helped me to build an understanding of programming logic before I moved on to Unity. Unity is a free commercial game engine used to create professional games played by millions of people, and to create smaller projects like this one. I’ve been using Unity for over 5 years now and have a lot of appreciation for it as it's been a great source of learning and creativity for me.