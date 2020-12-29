<h1 align="center">Ash</h1>

<p align="center">
    <a href="https://www.nuget.org/packages/Ash/"><img src="https://img.shields.io/nuget/v/Ash?label=Ash%20Nuget" /></a>
    <a href="https://www.nuget.org/packages/Ash.Content/"><img src="https://img.shields.io/nuget/v/Ash.Content?label=Ash%20Content%20Nuget" /></a>
    <a href="https://www.nuget.org/packages/Ash.Gia/"><img src="https://img.shields.io/nuget/v/Ash.Gia?label=Ash%20Gia%20Nuget" /></a>
    <a href="https://www.nuget.org/packages/Ash.DefaultEC/"><img src="https://img.shields.io/nuget/v/Ash.DefaultEC?label=Ash%DefaultEC%20Nuget" /></a>
    <a href="https://github.com/JonSnowbd/Ash/blob/production/LICENSE"><img src="https://img.shields.io/github/license/JonSnowbd/Ash" /></a>
</p>

Ash is a fast and simple Nez+Monogame based 2D Game Framework that gets you
right into developing your game, skipping the usual setup of things such as
the MGCB Content Builder and templates.

## Warning

For now, Ash is still refactoring and general features. I do not recommend using Ash yet for production projects until I've covered all the ground
in samples.

## Installation


There are 2 guides to install Ash:

- [Installation Via Submoduling](https://jonsnowbd.github.io/Ash/#/installation/gitmodule)
- [Installation Via NuGet](https://jonsnowbd.github.io/Ash/#/installation/nuget)

Note: You want to reference [`Ash.Content`](https://www.nuget.org/packages/Ash.Content/) if
you want to use the default assets that Ash needs to run. Alternatively, in your `Core` subclass,
override LoadDefaultContent to supply your own assets, or alternatively set them to null.
