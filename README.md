
# Match3-Unity

**By:** Rodrigo Malhães Bukowitz < rodrigomalhaes@gmail.com >. 

## 1. Summary

- [1. Summary](#1-summary)
- [2. Gameplay](#2-gameplay)
     - [2.1. Game concept](#21-game-concept) 
     - [2.2. Additional rules](#22-additional-rules) 
- [3. Match3 Unity Project](#3-match3-unity-project)
     - [3.1. Properties](#31-properties) 
     - [3.2. Scenes](#32-scenes) 
     - [3.3. Prefabs](#33-prefabs) 
     - [3.4. Scripts](#34-scripts) 
     - [3.5. Editor](#35-editor) 
     - [3.6. Art](#36-art) 
     - [3.7. Audio](#37-audio) 
- [4. Design](#4-design)
     - [4.1. Menu UI](#41-menu-ui)
     - [4.2. Game UI](#42-game-ui)
     - [4.3. Scale for multiple resolutions](#43-scale-for-multiple-resolutions)
- [5. Tests](#5-tests)
    - [5.1. Unit](#51-unit)
    - [5.2. Real device](#52-real-device)
- [6. Music and SFX](#6-music-and-sfx)

## 2. Gameplay

The game can be played using a mouse or via touch.

### 2.1. Game concept

There are gems of different colors on a grid. Players have to move these gems up down left right to form a row or column of same colored 3 or more gems. When the player match gems of same kind, they gets destroyed and player get points for it. Everytime the player scores, the gems need to be moved down and freeing up place for new gems that will be automaticly generated on top. 

### 2.2. Additional rules

In addition to the traditional mechanics of a match3 game. In Match3-Unity the game is divided into rounds, which have a time of 2 minutes to be completed. The round is completed when the player reaches the round score. As the player progresses the score required to complete the round increases. If the time for the round ends, the player loses the game.



## 3. Match3 Unity Project

### 3.1. Properties

The game was made using Unity Version: 2020.3.0f1 (LTS).

### 3.2. Scenes

The game consists of 2 scenes, GameScene and MenuScene. GameScene is where the gameplay takes place and where you can change the game's properties and rules. 
MenuScene is the opening scene that features the game menu and high score.

### 3.3. Prefabs

There are 3 prefabs in the project:
- Gem : Gem that is used in gameplay. This is the gem that the board will instantiate on the GameScene.
- DummyGem: A dummy gem without gameplay logic to be displayed in the menu. This is the gem that the board will instantiate on the MenuScene.
- AudioManager: An empty game object that handles all music and SFX in the game.

### 3.4. Scripts

The scripts are organized in folders that relate to their functions in the game.
- Board : All scripts related with the game board and logic of the game.
- Gem: Gem related scripts. 
- Scaler: That dictate how the gem should work in the game
- UIScripts: Scripts related to interactions with UI elements.
- AudioManager: Scripts that manage and structs the music and SFX.

### 3.5. Editor

Folder containing the script for unit tests.

### 3.6. Art

Folder containing all UI and Sprite assets.

### 3.7. Audio 

Folder containing all Music and SFX of the game.

## 4. Design

All sprites and UI elements are from [PlayKids/Match3 repository](https://github.com/PlayKids/match3-test)

### 4.1. Menu UI

![alt text](https://github.com/rodrigomalhaesbuko/Match3-Unity/blob/main/ReadmeImages/MenuUI.png "Menu UI")

### 4.2. Game UI

The game UI contains information about the round and the player's current score at that time. I chose to place the UI at the top of the screen to act as a status bar for the game.

![alt text](https://github.com/rodrigomalhaesbuko/Match3-Unity/blob/main/ReadmeImages/5:5S.png "Game UI")

### 4.3. Scale for multiple resolutions

All UI elements and Sprites are suitable for all portrait resolutions.
Examples:

#### iPhone X/XS 2436x1125

![alt text](https://github.com/rodrigomalhaesbuko/Match3-Unity/blob/main/ReadmeImages/XS.png "Iphone X/XS 2436x1125 Test")

#### iPad Pro 2732x2048

![alt text](https://github.com/rodrigomalhaesbuko/Match3-Unity/blob/main/ReadmeImages/IpadPro.png "iPad Pro 2732x2048 Test")

#### iPhone 5/5S/5C/SE 640x1136

![alt text](https://github.com/rodrigomalhaesbuko/Match3-Unity/blob/main/ReadmeImages/5:5S.png "iPhone 5/5S/5C/SE 640x1136 Test")

## 5. Tests 

### 5.1. Unit

The unit tests in this project were applied onto the Board model (Board.cs). To ensure that all model logic were correct, several test scenarios were made in Editor/BoardTests.cs

### 5.2. Real device

I made an iOS build and played the game on an iPhone 7.

![alt text](https://github.com/rodrigomalhaesbuko/Match3-Unity/blob/main/ReadmeImages/iPhone7Gameplay.gif "Real device  iPhone 7 test")

## 6. Music and SFX   

Some musics and SFX are from [PlayKids/Match3 repository](https://github.com/PlayKids/match3-test).
All other music and SFX tracks added to the project are royalty-free. 




