using Godot;

public abstract partial class State : Node
{
    protected StateMachineComponent Machine;

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update(double delta) { }

    public virtual void PhysicsUpdate(double delta) { }

    public void Initialize(StateMachineComponent machine) => Machine = machine;
}
