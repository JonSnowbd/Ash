<h1 class="text-center">Components</h1>

```csharp
// Easily create reusable pieces of logic that can apply to every
// game object!
public class Oscillate : ECComponent, IUpdatable
{
    public Vector2 Direction = new Vector2(1f, 0f);
    public float Speed = 2f;
    public float Strength = 20f;
    void IUpdatable.Update()
    {
        var amount = Mathf.Sin(Time.TimeSinceSceneLoad * Speed) * 0.15915482422f;
        Entity.Position += Direction * amount * Time.DeltaTime * (Strength * Speed);
    }
}
```

<h1 class="text-center">Content Management</h1>

```csharp
// Be done with the MGCB pipeline. Load your assets directly, in project
// or anywhere with an absolute path for external assets!
PlayerAtlas = Scene.Content.LoadTexture("Content/Atlas/Player.png");
PlayerJump  = Scene.Content.LoadSoundEffect("Content/Sound/PlayerJump.ogg");

// Worry not, disposing is handled for you when the scene is gone
```

<h1 class="text-center">Sleek Input</h1>

```csharp
// Virtual inputs make creating powerful control schemes easy, quick
// and most importantly strongly typed.
JumpButton = new VirtualButton();
JumpButton.AddGamePadButton(GamepadIndex, Buttons.A);
JumpButton.AddKeyboardKey(Keys.Space);
JumpButton.AddKeyboardKey(Keys.W);

if (JumpButton.IsPressed)
{
    // Easy to use too!
}
```

<h1 class="text-center">Easy Debugging</h1>

```csharp
// Static methods to add debug elements to the scene
void IUpdatable.Update()
{
    if (JumpButton.IsPressed)
    {
        // Draw a debug text message for 2 seconds:
        ECDebug.DrawText("Debug Text!", 2f);
        // Draw a jump trajectory for 2 seconds:
        ECDebug.DrawLine(Entity.Position, GetJumpTarget(), Color.White, 2f);
    }
}

// Or an override in `Component` to draw constant debug overlays
public override void DebugRender(Batcher batcher)
{
    base.DebugRender(batcher);
    // Make a green circle follow the entity:
    batcher.DrawCircle(Entity.Position, 5f, Color.Green);
}
```