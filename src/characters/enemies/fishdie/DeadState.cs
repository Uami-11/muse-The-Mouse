using Godot;

public partial class DeadState : State
{
    [Export]
    private Walker _walker;

    public override void Enter()
    {
        // Stop all movement and disable collision immediately
        _walker.Velocity = Vector2.Zero;
        _walker
            .GetNode<CollisionShape2D>("CollisionShape2D")
            .SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

        _walker.Sprite.Play("hurt");
        _walker.Sprite.AnimationFinished += _walker.QueueFree;
    }
}
