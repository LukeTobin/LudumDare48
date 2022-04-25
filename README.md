# Ludum Dare 48

This Ludum Dare's theme was "deeper and deeper". We worked in a team of two to create our game called Cavebop. Below, you will find more information about our creative process, our game's goals and assets we used.

## Theme and concept

With the theme being "deeper and deeper", our first thought was to make a nautical themed game. We've been playing with the idea of a mechanic that lets the player use a boss' abilities after they defeated them. Initially, we wanted to make a nautical roguelike in which the player would battle various sea creatures, taking on their powers as they went. However, we quickly realized that a nautical theme would be too predictable in some sense, so we kept on exploring. 
During our brainstorming session, various themes kept popping up, ideas of making a dating sim (deeper and deeper in love with somebody), a game about falling deeper and deeper into madness, and finally, a cave exploration puzzle game. 
Because we were only two people, it was crucial to keep the scope as small as possible, so that we'd have a functioning prototype in the end. Since we're only aspiring game designers, we opted to use an asset pack that we had purchased from itch. 
The idea of the game is that the player has to make their way through a dark mine within a certain turn limit. Some tiles are destroyed by hitting them once, while others require more hits or are indestructable. Because the player can only see the tiles that are one in front and to the sides of them, they need to evaluate their choices carefully, so that they can reach the exit without running out of turns. 

### Platform

The game is made to be played on Desktop, but we're currently toying with the idea to release it for mobile as well.

### Internal Goals

* The player should feel challenged by the level design
* The player should feel an urgency to make the right choices in their pathfinding

## Level

![Alt Text](https://cdn.discordapp.com/attachments/275293865441755139/836350298191560764/cover.JPG)
![Alt Text](https://cdn.discordapp.com/attachments/275293865441755139/836350278743752724/screenshot_1.JPG)
![Alt Text](https://cdn.discordapp.com/attachments/275293865441755139/836350288457105448/screenshot_1.1.JPG)


## Technical Design

### Controls

The player moves with ASD and key buttons. 

### Mechanics

* Apart from moving, the player is able to destroy certain elements by walking into them. Some elements require more than one hit. 
* The player runs on a resource called stamina. It's a turn based system. Every move will cost the player one point of stamina. If the player runs out of stamina before reaching a door, the level is reset. 
* It's possible that the player runs into a bomb while digging. The bomb has a turn counter on it that goes down with every move the player makes. The player needs to make their way outside of the bomb's radius before it explodes. If the player doesn't make it, the level resets. Upon exploding, the bomb deals a certain amount of damage to its surrounding, leaving some tiles destroyed or damaged. 
* Some exits will need a key in order for the player to progress. The key is usually hidden in the level somewhere. If the player fails to find the key before their stamina runs out, the game is reset. 
* Since the player is unable to backtrack onto the row above, finding keys can be especially hard. Finding ladders will allow the player to go back up a row. 

## Art

Since we're such a small team, we opted to use S4M_UR4I's asset pack from itch. (you can find the link to it [here](https://s4m-ur4i.itch.io/huge-pixelart-asset-pack)) Thank you so much for making these assets, it really made our lives easier! <3

## Sound

All of our sound is by [Sidearm Studio.](https://assetstore.unity.com/packages/audio/sound-fx/ultimate-sound-fx-bundle-151756)


# Download

You can find a free download of our game [here](https://luketobin.itch.io/cavebop)
