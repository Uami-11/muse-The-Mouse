using Godot;

public partial class PatrolState : State
{
    [Export]
    private Walker _walker;

    public override void Enter() => _walker.Sprite.Play("walk");

    public override void PhysicsUpdate(double delta)
    {
        if (_walker.HitsWall() || _walker.AtEdge())
            _walker.SetFacing(!_walker.FacingRight);

        float dir = _walker.FacingRight ? 1f : -1f;
        _walker.Velocity = new Vector2(
            dir * _walker.PatrolSpeed,
            Mathf.Min(_walker.Velocity.Y + _walker.Gravity * (float)delta, _walker.MaxFallSpeed)
        );

        _walker.MoveAndSlide();
    }
}
