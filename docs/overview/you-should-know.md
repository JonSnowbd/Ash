# You should know

Here is a nice list of all the fun bits of Ash you would otherwise have to delve around to
discover for yourself.

Learning the frameworks various features is best done by reading the source code linked
on each feature. There is no scary parts! It will be an easy read.

**NOTE** This isn't everything Ash has to offer! This is just what you get from using
`Ash.Portable/Ash.csproj`. Gia and DefaultEC offer many more features such as physics resolution
and general project layout.

### General

Overall these are useful in many situations.

- [Simple Scene](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/SimpleScene)
Don't want to use Gia/DefaultEC? need something dead simple? This handles the basics.
- [The Content Manager](https://github.com/JonSnowbd/Ash/blob/production/Ash.Portable/Assets/AshContentManager.cs)
Not really hidden, but overall a good read to see what you can load at runtime!
- [Binary Persistence](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/Persistence)
Read through `IPersistable` and then `FileDataStore` and you'll have a good idea of how you can get
extremely efficient runtime serialization.
- [Math Related Fun](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/Math)
This treasurebox includes `Rand` for easy random, `Bezier` for curves, and several helpers for things like
enum Flags.
- [Extensions](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/Utils/Extensions)
Many, many extension methods for a wide variety of Monogame classes.
- [Collections](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/Utils/Collections)
Ash comes with quite a few very convenient collection classes for things like Pools and convenient
raw array wrappers.
- [Virtual Inputs](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/Input/Virtual)
Arguably one of the most convenient parts of Ash. Create your game controls safely and effectively
with Virtual Inputs

### AI

Ash has a few tools to help make AI easier.

- [Behaviour Tree](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/AI/BehaviorTree)
A data structure for building a decision tree based on Actions and Conditions.
- [Scored based AI Decision Tree](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/AI/UtilityAI)
Similar to the previous, but decides based on calculated Scores.
- [Finite State Machines](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/AI/FSM)
Self explanatory, a huge time saver for player states and AI states for simple AI. Or anything really.
- [Easy Static Pathfinding](https://github.com/JonSnowbd/Ash/tree/production/Ash.Portable/AI/Pathfinding)
Just implement a single interface and you can get pathfinding for basically free in multiple algorithms.

### Graphics

A few things related to graphics in Ash.

- [Material](https://github.com/JonSnowbd/Ash/blob/production/Ash.Portable/Graphics/Material.cs)
A class for organizing your `Batcher.Begin(Material, TransformMatrix)` calls. Everything related to .Begin wrapped in here.
- [Simple Camera](https://github.com/JonSnowbd/Ash/blob/production/Ash.Portable/SimpleScene/SimpleCamera.cs)
Even if you don't end up using `SimpleScene`, the camera used inside is very intuitive to use anywhere.