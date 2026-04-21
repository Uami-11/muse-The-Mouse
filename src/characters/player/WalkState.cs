using System;
using Godot;

public partial class WalkState : State
{
    [Export]
    private Muse _player;

    public override void Enter()
    {
        _player.sprite.Play("walk");
    }

    public override void PhysicsUpdate(double delta)
    {
        float dir = Input.GetAxis("move_left", "move_right");

        if (dir != 0)
        {
            _player.SetFacing(dir > 0);
            _player.Velocity = _player.Velocity with
            {
                X = Mathf.MoveToward(
                    _player.Velocity.X,
                    dir * _player.MoveSpeed,
                    _player.Acceleration * (float)delta
                ),
            };
        }
        else
        {
            _player.Velocity = _player.Velocity with
            {
                X = Mathf.MoveToward(_player.Velocity.X, 0, dir * _player.Friction * (float)delta),
            };
        }

        if (!_player.IsOnFloor())
        {
            Machine.TransitionTo("FallState");
            return;
        }

        if (dir == 0)
        {
            Machine.TransitionTo("IdleState");
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
