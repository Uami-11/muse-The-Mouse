using Godot;

public partial class ChaseState : State
{
    [Export]
    private Walker _walker;

    public override void Enter() => _walker.Sprite.Play("walk");

    public override void PhysicsUpdate(double delta)
    {
        if (_walker.Player == null)
        {
            Machine.TransitionTo("PatrolState");
            return;
        }

        bool playerIsRight = _walker.Player.GlobalPosition.X > _walker.GlobalPosition.X;
        _walker.SetFacing(playerIsRight);

        float xVel = _walker.HitsWall() ? 0f : (playerIsRight ? 1f : -1f) * _walker.ChaseSpeed;

        _walker.Velocity = new Vector2(
            xVel,
            Mathf.Min(_walker.Velocity.Y + _walker.Gravity * (float)delta, _walker.MaxFallSpeed)
        );

        _walker.MoveAndSlide();
    }
}
