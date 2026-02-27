# Lindenmayer System In Unity

This project implements a D0L L-System with bracketing and a stochastic L-System extension to generate 3D plants in Unity. The pipeline is:

1. L-System rewrite engine mutates an initial symbol string (axiom) for N iterations using a ruleset. 


2. A 3D turtle interpreter reads the final symbol list and builds plant geometry (cylinders for branches, filled polygons for leaves). 


3. A UI allows switching between plant presets and adjusting morphology parameters like iterations, angle, step length, and radius. 


## Requirements

Unity Editor(Version 6000.3.xxx)



## How to Run

- Open in Unity: Add the project folder to Unity Hub and open.

- Load the Scene: Navigate to Assets/Scenes/ and open MainSimulation.

- Run: Press the Play button in the Unity Editor.



## How to Control the Plants

Runtime controls can be adjusted through the inspector tab of LSystemManager.

These include:


### Preset selection


- D0L_Plant

- Stochastic_Birch

- Stochastic_Bush

- Stochastic_Oak


### Iterations
- How many rewrite passes

### Step Length
- Branch segment length

### Radius
- Branch cylinder radius

### Angle Deg
- Rotation angle (degrees)

### Regenerate Button

- Apply changes to preset and controls, and regenerate the structure.




## L-System Implementation (D0L + Stochastic Extension)
### D0L

The D0L-system is defined by the triple G = ( Σ , P, α ), where:

 - Σ = { F , [ , ] , X , L , + , - , & , ^ , \ , / , }
    - F: Move forward by step and draw a branch cylinder from start to end.

    - \+ / - :Rotate around a local axis by ±angleDeg (used for branching direction changes).

    - & / ^: Pitch down/up by ±angleDeg.

    - \ / /: Roll by angleDeg.

    - [: Push current state onto stack; create a new branch node for grouping.

    - ]: Pop state from stack; resume growth from the saved state.

    - L: Create a leaf using a small filled polygon near the turtle’s position.

    - X: Ignored by the turtle, used as control symbols that guide rewriting but do not directly draw.


This allows the system to create branches easily because the turtle can return to an earlier position/orientation. 


### Stochastic extension

A stochastic L-system allows multiple rules per symbol, chosen randomly using weights, instead of requiring exactly one deterministic mapping. 

In this project, RewriteRuleSet stores multiple RewriteOption entries per predecessor symbol and selects one via weighted random choice.


## Plant Presets and Their Visual Characteristics

This project includes four presets in SystemManager.Preset:

### 1) D0L_Plant (Deterministic)

Type: 
- Deterministic

Behavior:

- Starts from axiom F and rewrites F into a pattern containing bracketed branches and roll/pitch changes.

Look:

- Consistent branching structure every run.

#### Suggested parameters:

- iterations: 3–5

- angle: 20–35

- step: ~1.0

- radius: ~0.05

### 2) Stochastic_Birch

Type: 
- Stochastic

Behavior: 
- Mostly upward growth with lighter branching and leaves near tips.

Look:

- Taller, thinner main structure, less dense branching than oak.

#### Suggested parameters

- iterations: 5–6

- angle: 12–22

- step: 0.8–1.3

- radius: 0.02–0.05

### 3) Stochastic_Bush

Type: 
- Stochastic

Behavior: 
- Short segments, frequent branching, lots of leaves.

Look:

- Compact, rounded mass, dense leaf, chaotic branching.
#### Suggested parameters

- iterations: 4–6

- angle: 20–40

- step: 0.5–0.9

- radius: 0.03–0.06

### 4) Stochastic_Oak

Type: 
- Stochastic

Behavior: 
- Wider crown, more asymmetry, more 3D spread (uses yaw/pitch/roll combinations).

Look:

- Strong branching, wider canopy, more variation between runs.

#### Suggested parameters

- iterations: 5–6

- angle: 18–30

- step: 0.8–1.2

- radius: 0.05–0.10
