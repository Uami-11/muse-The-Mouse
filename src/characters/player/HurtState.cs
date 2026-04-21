using System;
using Godot;

public partial class HurtState : State
{
    [Export]
    private Muse _player;

    [Export]
    private Timer hurtTimer;

    public override void _Ready()
    {
        hurtTimer.Timeout += OnTimerTimeout;
    }

    public override void Enter()
    {
        hurtTimer.Start();
        _player.sprite.Play("hurt");

        _player.Velocity = new Vector2(_player.FacingRight ? -80f : 80f, 100f);
    }

    public override void PhysicsUpdate(double delta)
    {
        _player.Velocity = _player.Velocity with
        {
            Y = _player.Velocity.Y + _player.Gravity * (float)delta,
        };

        _player.MoveAndSlide();
    }

    public void OnTimerTimeout()
    {
        Machine.TransitionTo(_player.IsOnFloor() ? "IdleState" : "FallState");
    }
}
