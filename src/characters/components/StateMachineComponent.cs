using System.Collections.Generic;
using Godot;

public partial class StateMachineComponent : Node
{
    [Export]
    public string InitialState = "";

    private State _currentState;
    private readonly Dictionary<string, State> _states = new();

    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                _states[child.Name] = state;
                state.Initialize(this);
            }
        }

        if (!string.IsNullOrEmpty(InitialState))
            TransitionTo(InitialState);
    }

    public void TransitionTo(string stateName)
    {
        if (!_states.ContainsKey(stateName))
        {
            GD.PrintErr($"State '{stateName}' not found in {Name}");
            return;
        }
        _currentState?.Exit();
        _currentState = _states[stateName];
        _currentState.Enter();
    }

    public override void _Process(double delta) => _currentState?.Update(delta);

    public override void _PhysicsProcess(double delta) => _currentState?.PhysicsUpdate(delta);
}
