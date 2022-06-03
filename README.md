# Rara Coding Challenge

Proposed solution for Rara coding challenge, for Unity 2021.3.3f1.

## High level description of solution

### Player Movement
Player movement is controlled using Unity's new `Input System`, and a `Character Controller` component.

### Entity properties
Entity properties, such as speed, damage, firing rate, etc. are exposed to the inspector using `[SerializeField] private` variables, and are part of the script that handles the entity.

### Game states

There are four possible game states: `Build, Play, Win and GameOver`. These states are handled in the `GameStateManager` script, which also handles game resetting and player score.

### Winning and losing
The player loses the game when the avatar's health reaches zero. Enemies and projectiles damage the player by different amounts, according to the set damage value for each entity. This damage is inflicted to the player by getting the enemy's damage value property via `OnTriggerEnter`. 

The player wins the game by reaching the Flag object with the avatar.

After winning or losing, players can retry the level to play again.

`Collider` components are attached to the mesh `GameObjects`, and `Kinematic RigidBodies` attached to the parent `GameObject` of each relevant entity.

### Communication between components

To mitigate component coupling, communication between different components is achieved mainly via C# `events`. In a few cases, it was necessary to get components via the `GameObject.Find` method, or `GameObject.GetComponent` outside `Start()` or `Awake()`, however, this was attempted to be kept to a minimum.

### Object pooling
A simple object pool was implemented for the Turret NPC's projectiles.

### Object spawning in build mode
Object spawning in build mode was achieved by casting a ray from the camera to the mouse click position, using Unity's new `Input System` and filtering for the "Floor" tag.

### Navigation and NavMesh updating
Chaser and Wanderer NPCs are NavMesh Agents that navigate the Navigation Mesh present in the level floor plane. This Navigation Mesh is updated each time a Turret, Low Block, High Block or Mine is placed on the level, since they have the `NavMesh Obstacle` component attached, with the `Carve` option set to true.

## If I had more time...

If I had more time, I would have given a bit more thought to implementing `ScriptableObjects` to govern entity properties and accomplish a more data-driven solution. 

The way the player gets damage can also be improved, it would be nice to rework this a bit. Currently, it gets the NPC component from the transform that triggers `OnTriggerEnter`. Maybe there's a better way using `ScriptableObjects`.

General architecture and state handling could be improved upon after this prototype, establishing more clearly when to send events, or when is it okay to just get GameObjects or Components via serialized fields, `GameObject.Find`, etc.