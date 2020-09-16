<h1 class="text-center">Functionality</h1>

## Scenes

Ash has a few major facets to cover before you can just dive in on a new project. Simply put
Ash has a central theme of `Scene` classes that house all the game logic. They have no
assumptions made about them so you are free to create your game under any paradigm that can
be reduced to a single `Scene` class.

By default this is [nothing but an interface](https://github.com/JonSnowbd/Ash/blob/py-2.0/Ash.Portable/IScene.cs)
that exposes some necessary methods like `Update` and `Render`. By default there is nothing
that implements this, so there are only 2 options; rolling your own, or importing [Ash.DefaultEC](/overview/nez-defaultec).

----

## Core

The next important concept is the `Core` class. This main class is your new `Game` if you
are coming from Monogame/FNA/XNA. Your main client should inherit from this, and from there
you use its life cycle to create Scenes and other good stuff your game needs.

Overridable lifecycle methods of interest are as follows;

```csharp
// The biggest thing you will need to override in Ash. Unlike in XNA-derivatives
// you will do all your loading here. Core.Content is global and will not be unloaded
// until the game closes, so is useful for global assets such as UI.
protected virtual void Initialize();

// Run before the first `Core.Update` but after `Core.Initialize`
protected virtual void BeginRun();

// Run before `Core.Draw` to determine whether drawing is necessary.
// This can be useful to use Ash to create efficient GUI Applications.
protected virtual bool BeginDraw();

// Self explanatory. You shouldn't need to override update on core since `Scene` classes
// manage all the game logic. But still useful for say, toggling debug rendering globally.
protected override void Update(GameTime gameTime);

// Run after the game loop has been terminated, before the application closes.
// Useful to flush game settings and data unrelated to game assets to disk.
protected virtual void EndRun();
```

----

<h1 class="text-center">Assets</h1>

## Loading

Asset loading in Ash is different to XNA-derivatives. Instead of using a content builder
we load the assets directly through `NezContentManager` classes that have special methods
to load each file type(including stuff like Ogmo/Tiled/Json files).

There will always be a `GlobalNezContentManager` available on the `Core` class, this
will let you load global assets to use everywhere that will never get unloaded.

## Unloading

As long as you used a NezContentManager, you don't have to care about it. Assets
in each Scene implementation will unload as needed automatically when they are removed.

<h1 class="text-center">Useful Bits</h1>