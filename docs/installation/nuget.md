### You will need

- [Dotnet Core SDK](https://dotnet.microsoft.com/download)
- Any text editor of your choice.

This tutorial will show you how to make your own .Net Core 3.1 Monogame game with Ash.

?> For this document, anywhere you see `GAME` you should substitute with your games name.

----

### <small>1.</small> Solution File & Project

First things first navigate to your workspace folder(this doesnt refer to a specific folder,
just wherever you want to keep your work!) in a command line that can use `dotnet`.

First things first, we need a solution file to open in Rider/VSC/VS.

```bash
$ mkdir GAME
$ cd GAME
$ dotnet new solution
```

Now we create the actual dotnet project. Note: It's okay to have GAME/GAME.

```bash
$ mkdir GAME 
$ cd GAME
$ dotnet new console
```

Then we just add the project to the solution. 

```bash
$ cd ..
$ dotnet sln add ./GAME/GAME.csproj
```

### <small>2.</small> NuGet References

Now we need to edit the .csproj a bit. Open up `GAME/GAME.csproj` in your text editor and
get ready to make a few small changes.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>

    <!-- Changed to WinExe to hide default console. -->
    <OutputType>WinExe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>

  </PropertyGroup>

  <!-- Adding an item group to keep it organized, and it contains the package -->
  <!-- references for Ash and the default Ash content. -->

  <!-- Note! This documentation may not be up to date, always check nuget for the latest -->
  <!-- versions of ash. -->
  <ItemGroup>
    <PackageReference Include="Ash" Version="1.1.6" />
    <PackageReference Include="Ash.Content" Version="1.2.2" />
  </ItemGroup>

</Project>
```

### <small>3.</small> Game Content

Now you're basically done. The only thing you need to do now is modify `Program.cs` to start
a game. Open up your solution and get coding!

For simple testing, lets just instantiate a default `Core` and run it. Note you don't want
to do this for actual games since you likely want to subclass `Core` to load your own
game.

This is just to have a window that opens.

```csharp
using Ash;

namespace GAME
{
    class Program
    {
        static void Main(string[] args)
        {
            using var game = new Core();
            game.Run();
        }
    }
}
```

And now you have a fully functional Ash game that boots! 

### Extra

There is more interesting things you can add to your .csproj to make it much easier to handle.
One of which is to enable all assets being immediately copied over to the build, no more managing
VS file references!

```xml
<ItemGroup>
    <Content Include="Content\**">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </Content>  
</ItemGroup>
```