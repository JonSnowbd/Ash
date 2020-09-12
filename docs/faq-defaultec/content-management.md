Content Management
==========
Nez includes it's own content management system that builds on the MonoGame ContentManager class. All content management goes through the `NezContentManager` which is a subclass of `ContentManager`. The debug console has a 'assets' command that will log all scene or global assets so you will always know what is still in memory.

Nez provides containers for global and per-scene content. You can also create your own `NezContentManager` at any time if you need to manage some short-lived assets. You can also unload assets at any time via the `UnloadAsset<T>` method. Note that Effects should be unloaded with `UnloadEffect` since they are a special case.


## Global Content
There is a global NezContentManager available via `Core.Content`. You can use this to load up assets that will survive the life of your application. Things like your fonts, shared atlases, shared sound effects, etc.


## Scene Content
Each scene has it's own `NezContentManager` (Scene.Content) that you can use to load per-scene assets. When a new scene is set all of the assets from the previous scene will automatically be unloaded for you.



## Loading Effects
There are several ways to load Effects with Nez that are not present in MonoGame. These were added to make Effect management easier, especially when dealing with Effects that are subclasses of Effect (such as AlphaTestEffect and BasicEffect). All of the built-in Nez Effects can also be loaded easily. The available methods are:

- **LoadMonoGameEffect<T>:** loads and manages any Effect that is built-in to MonoGame such as BasicEffect, AlphaTestEffect, etc
- **LoadEffect/LoadEffect<T>:** loads an ogl/fxb effect directly from file and handles disposing of it when the ContentManager is disposed
- **LoadEffect<T>( string name, byte[] effectCode ):** loads an ogl/fxb effect directly from its bytes and handles disposing of it when the ContentManager is disposed
- **LoadNezEffect:** loads a built-in Nez effect. These are any of the `Effect` subclasses in the Nez/Graphics/Effects folder.
- **LoadOgmoProject:** loads an [Ogmo](https://ogmo-editor-3.github.io) `project.ogmo` file, but none of its levels(loaded through the ogmo project that is returned.)
- **LoadJson<T>:** loads a .json file and coerces it into the given generic type using Newtonsoft's Json.Net. Takes an optional context object to be used in custom converters.

## Async Loading
`NezContentManager` provides asynchronous loading of assets as well. You can load a single asset or an array of assets via the `LoadAsync<T>` method. It takes a callback Action that will be called once the assets are loaded.