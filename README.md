**Overview**

This project is an accessible endless runner game specifically designed for visually impaired players (VIPs). It integrates non-visual feedback mechanisms such as audio cues and haptic feedback to provide players with critical information about the game environment, helping them navigate the virtual world without relying on visuals.

The game was developed using the Unity game engine, with a primary focus on making the game accessible and engaging for VIPs. The player controls a character that automatically runs through a series of lanes, dodging obstacles through swipe gestures. Audio and haptic feedback mechanisms ensure that players can receive feedback about their movements and obstacles, making it possible to play without sight.


**Features**

Lane-Based Audio Feedback: The game uses lane-specific audio to let players know which lane they are in. For example, when the player is in the left lane, they hear sound in the left ear, in the right lane, the sound plays in the right ear, and in the center lane, sound is played in both ears.

Obstacle Detection and Warnings: When the player is approaching an obstacle in their lane, the game provides audio warnings and haptic feedback. The sounds and vibrations trigger when the player gets within proximity of the obstacle, ensuring timely feedback to change lanes or jump to avoid it.

Haptic Feedback: Haptic feedback is triggered when players approach obstacles, providing tactile alerts to complement the audio warnings. This helps VIPs react to the game environment without relying on visuals.

Dynamic Speed Increase: The game increases in difficulty as the player progresses by dynamically increasing the speed of the character’s movement.

Swipe Controls: Players can swipe left or right to change lanes or swipe up to jump over obstacles. These swipe gestures are designed to be simple and intuitive for players with visual impairments.

Score System: The game includes a score based on the time players survive, offering a rewarding experience by keeping track of how far they have progressed.


**Gameplay**

The player starts on a three-lane track with randomly generated obstacles. They must navigate between lanes or jump to avoid obstacles. As the game progresses, the character moves faster, making it more challenging to react in time. Audio and haptic cues help the player stay aware of their surroundings, ensuring they can make informed decisions despite the absence of visual feedback.


**Installation**

1. Clone this repository:
   git clone https://github.com/yashmauzo/endlessrunner.git
2. Open the project in Unity.
3. Run the game in the editor or build it for Android to test it on a mobile device.

**Credits**

This project drew significant inspiration and learning from Chaker Gamra's Endless Runner Game (https://github.com/Chaker-Gamra/Endless-Runner-Game.git) and the corresponding YouTube series by Chaker Gamra. His tutorials were invaluable in setting up the basic game mechanics and implementing key features like swipe-based controls and the fundamental runner structure. You can find his YouTube tutorial series here - (https://www.youtube.com/watch?v=DxKIuyOjwPw&list=PL0WgRP7BtOez8O7UAQiW0qAp-XfKZXA9W).

While this project builds upon the structure and concepts provided by Chaker Gamra's tutorials, additional features, such as lane-specific audio feedback and haptic feedback for VIPs, were implemented to ensure accessibility. The process of integrating these accessibility features required significant effort and adjustments to the original codebase, making this project a unique contribution to the field of accessible gaming.

**License**

This project is licensed under the MIT License. See the LICENSE file for details.
