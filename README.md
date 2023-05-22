# 3DSeriousGames
This repository is part of my bachelor thesis project at TUM and contains a 3D game that combines two exciting mini-games: a Fruit Ninja-inspired game and an augmented reality (AR) game. The main platform provides a seamless experience for players to enjoy both games in a single application.

## Introduction

The game repository is designed to offer a captivating gaming experience by combining two engaging mini-games into a single platform. The first mini-game draws inspiration from the popular Fruit Ninja game, where players have to slice various food items while avoiding obstacles and less nutritious pieces. The second mini-game is an augmented reality (AR) game that brings virtual elements into the real world, providing an immersive gameplay experience.

## Features

1. **Main Platform**: The game consists of a main platform that seamlessly integrates both mini-games, allowing players to switch between them effortlessly. This main menu is shown directly at the start of the application and can be reached within each mini-games via the home button/menu.

2. **Fruit Ninja-inspired Game**: This mini-game is developed to educate the player with introductory knowledge in nutrition for heart disease patients, and tests the player's reflexes and precision. Food items will be thrown into the air, and the player must slice them with a virtual sword, earning points for each successful slice of the right pieces. However, they must avoid slicing any obstacles that appear, as well as slicing less nutritious food items will lead to negative score points.

3. **Augmented Reality Game**: This mini-game is designed with the purpose to increase the player's knowledge in nutrition guidelines for patients infected by covid-19. The AR game utilizes the device's camera and overlays virtual objects into the real world. Players will interact with these virtual elements, completing rounds of simulated food consumption, and collecting points for cleaning up the ground from spawned 'bad' food items. This game mode requires devices with AR capabilities.

4. **High Scores**: Both mini-games keep track of players' high scores, allowing them to challenge themselves to achieve new records.

## Installation

To install and run the game locally, follow these steps:

1. Clone the repository to your local machine using the following command:

   ```bash
   git clone git@github.com:xikito-ai/3DSeriousGames.git
   ```

   Note: Make sure you have the necessary permissions to access the repository.

2. Open the project in Unity.

3. Check that the player and build settings are correctly set. The AR game requires ARKit/ARCore depending on your mobile device. For example, to install this game successfully on an Android device navigate to Edit > Project Settings. In XR Plug-in Management, open the Android tab and enable ARCore.

4. Build and run the game. After a successful build the application should be installed on your connected mobile device.

5. Run the game: simply open the application on your mobile device and start playing!

## Usage

1. Launch the game on your device by following the installation instructions.

2. Upon starting the game, you will be presented with the main platform.

3. Use the provided controls or gestures to switch between the two mini-games (see respective UI elements in the game such as home buttons).

4. In the Fruit Ninja-inspired game, slice the nutritious food items while avoiding less nutritious ones and obstacles to earn points. Click at the info button to look at information about which foods to avoid and which to slice to get points. Achieve the highest score possible and aim to improve it in subsequent attempts.

5. In the augmented reality game, follow the on-screen instructions to interact with virtual elements overlaid on the real world. A placement indicator helps you to visualize placing the base ground opon which food items will be spawned in a jumping animation. After each round (indicated by a red timer bar at the top of the screen) the amount of 'friends' (foods to consume) and 'foes' (foods to avoid) are counted and the difference is added to the achieved scores. Higher number in friends will increase the score while a dominating foe count will descrease the current score. 

6. The game keeps track of your high scores in both mini-games. Challenge yourself to beat these scores.
