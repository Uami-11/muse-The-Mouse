using System;
using Godot;

public partial class IdleState : State
{
    [Export]
    private Muse _player;

    public override void Enter()
    {
        _player.sprite.Play("idle");
    }

    public override void PhysicsUpdate(double delta)
    {
        _player.Velocity = _player.Velocity with
        {
            X = Mathf.MoveToward(_player.Velocity.X, 0, _player.Friction * (float)delta),
        };

        if (!_player.IsOnFloor())
        {
            Machine.TransitionTo("FallState");
            return;
        }

        if (Input.IsActionPressed("move_left") || Input.IsActionPressed("move_right"))
        {
            Machine.TransitionTo("WalkState");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            Machine.TransitionTo("JumpState");
            return;
        }

        if (Input.IsActionJustPressed("shoot") && _player.CanShoot())
        {
            Machine.TransitionTo("ShootState");
            return;
        }

        _player.MoveAndSlide();
    }
}
