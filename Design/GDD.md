# Game Design Document
## Lost Glider

## Table of Contents
1. Objective
2. Description
3. Technology
4. Overview

    4.1. Setting

    4.2. Plot

    4.3. Characters
    
    4.4. Levels

5. Gameplay
6. Graphics
7. Sound
8. Accessibility
---

## 1. Objective
Create an Android game with an educational background. Explain ideas  behind 
the gliding and cloud types.

## 2. Description
An Android game with a side-view (platformer-like) where a player is a  pilot
of a glider. The pilot have to control the glider altitude and  speed using
a speed brake and other controls. There is a need to use soaring lifts and
clouds in clouds to fly as long as possible and to avoid storms.

## 3. Technology
**Unity 2019.4.10** engine with a 2D template. No external assets. Excluding sound.

## 4. Overview

### 4.1 Setting
Beautiful Sky.

### 4.2 Plot
Random glider finds that is lost somewhere above the ground. Then, it tries to
fly as far as possible.

### 4.3 Characters
- Glider - a sailplane based on SZD-48 Jantar Standard 2/3.

### 4.4 Levels
#### Clouds
Nicely visible entities those change the Glider behaviour.
- Cumulonimbus - destroys the Glider.
- Cumulus congestus - increases altitude very quickly via cloud suck.
- Nimbostratus - hides the Glider completely.

#### Soaring Lifts
A little less visible phenomenons than clouds.
- Hot air - increases altitude.
- Cold air - decreases altitude.
- Wave lift - increases altitude slightly and speed.

### 5. Gameplay
Player attempts to fly as far as possible with the Glider using proper
Soaring Lifts and Clouds and avoiding Cumulonimbus.

### 6. Graphics
Pixel Art.

### 7. Sound
Downloaded from free sources. CC0 preferred.

### 8. Accessibility.
Configurable steering: left/right joystick or a device tilt.
