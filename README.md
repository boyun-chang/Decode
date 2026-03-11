# Decode
## Download
You need to download the model and textures and place them in the Assets folder.

You can download the model and textures here. >> <a href="https://drive.google.com/file/d/1kJyKEKlBRcx3x2RDZi8H-5Gc6m4r-zTF/view?usp=sharing"><img src="https://img.shields.io/badge/Google Drive-4285F4?style=flat-square&logo=googledrive&logoColor=white"/></a>

## Game Introduction
A puzzle game where you travel back in time to prevent a tragedy. 
We aimed to create an immersive storytelling puzzle that offers players the experience of changing the future through their abilities.

## 🎬 Gameplay Video
Demo video >> <a href="https://www.youtube.com/watch?app=desktop&v=BkP6e70cGzE"><img src="https://img.shields.io/badge/Youtube-FF0000?style=flat-square&logo=youtube&logoColor=white"/></a>

## 🎮 Game Overview
This is a puzzle game that progresses by solving various environmental gimmicks.
* **Key Mechanics**: Wire connection, clue gathering, decoding Morse code
* **Stages**:
  * Stage 1 : Dining Room
  * Stage 2 : Data Reading Room
  * Stage 3 : Underground / Machine Room

## 🛠 Development & Team
* **Role**: Programmer
* **Development Environment**: Unity
* **Development Period**: 5 Months
* **Team Structure**: 4 Members (2 Designers, 2 Programmers)

## 💻 Technical Features

### 1. Enhanced Visual Feedback
Focused on visual feedback using shaders and particle effects so players can intuitively grasp information.
* **Electricity Charging Effect**: Created grayscale textures and developed a shader that fills up according to the object's UVs to visualize the flow of electricity.
* **Object State Visualization**: Developed an outline extraction shader to clearly represent the current interactive state of objects.
* **Interactive UI**: Implemented animations and UI effects synchronized with the exact timing of the player pressing the Morse code key.

### 2. Puzzle Mechanics Implementation
Focused on developing robust logic to build complex puzzle mechanics.
* **Wire Signal Communication System**: Placed invisible trigger objects at wire intersections to detect incoming electrical currents from previous wires, dynamically dictating the behavior and activation of subsequent wires.
