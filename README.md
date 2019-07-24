## Introduction
The goal of this application is to get drones to self-driving to their target points, avoiding people, trees and walls.
The drone receives a negative reward when it comes into contact with an obstacle and is only rewarded when it reaches the target point.


### Object explaination
  1. 

### Reward List
  1. 

### Actions
- **`w`** - move forward.
- **`s`** - move backward.
- **`a`** - turn left.
- **`d`** - turn right.
- **`space`** - move up.
- **`shift`** - move down

## Installation
  1. Create Anaconda Environment
  ```python
  $conda create -n navigation python=3.6 (ml-agents requires only python 3.6)
  ```
