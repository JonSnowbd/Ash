# Finite State Machines

## StateMachine (Advanced)

Code Overview

```csharp
// Where T is a State<M> where M is the state machine type.
public class StateMachine<T> 
{
    public event Action OnStateChanged;

    public State<T> CurrentState;
    public State<T> PreviousState;
    public float ElapsedTimeInState;

    // Constructor
    public StateMachine(T context, State<T> initialState)

    // adds the state to the machine
    public void AddState(State<T> state);

    // ticks the state machine with the provided delta time
    public virtual void Update(float deltaTime);

    // Gets a specific state from the machine without having to change to it.
    public virtual R GetState<R>() where R : State<T>;

    // changes the current state
    public R ChangeState<R>() where R : State<T>;
}

// Where T is StateMachine<S> where S is this state. Cyclical generic.
public abstract class State<T>
{
    // Used by `StateMachine<T>` to initialize the state when added.
    public void SetMachineAndContext(StateMachine<T> machine, T context);

    // called directly after the machine and context are set allowing the state to do any required setup
    public virtual void OnInitialized();

    // called when the state becomes the active state
    public virtual void Begin();

    // called before update allowing the state to have one last chance to change state
    public virtual void Reason();

    // called every frame this state is the active state
    public abstract void Update(float deltaTime);

    // called when this state is no longer the active state
    public virtual void End();
}
```

-----