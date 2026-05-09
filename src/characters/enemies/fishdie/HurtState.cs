using Godot;

namespace enemies;

public partial class HurtState : State
{
    [Export]
    private Walker _walker;

    [Export]
    private Timer _hurtTimer;

    public override void _Ready() =>
        _hurtTimer.Timeout += () => Machine.TransitionTo("PatrolState");

    public override void Enter()
    {
        _hurtTimer.Start();
        _walker.Sprite.Play("hurt");

        // Knock back away from the direction the walker faces
        float knockDir = _walker.FacingRight ? -1f : 1f;
        _walker.Velocity = new Vector2(knockDir * 80f, -60f);
    }

    public override void PhysicsUpdate(double delta)
    {
        _walker.Velocity = _walker.Velocity with
        {
            Y = Mathf.Min(
                _walker.Velocity.Y + _walker.Gravity * (float)delta,
                _walker.MaxFallSpeed
            ),
        };
        _walker.MoveAndSlide();
    }
}
