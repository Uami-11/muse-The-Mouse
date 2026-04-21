using System;
using Godot;

public partial class FallState : State
{
    [Export]
    private Muse _player;

    public override void Enter()
    {
        _player.sprite.Play("fall");

        if (_player.Velocity.Y >= 0)
            _player.StartCoyoteTime();
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
                    _player.Velocity.Y,
                    _player.Velocity.Y * _player.MoveSpeed,
                    _player.AirAcceleration * (float)delta
                ),
            };
        }

        _player.Velocity = _player.Velocity with
        {
            Y = _player.Velocity.Y + _player.Gravity * (float)delta,
        };

        if (Input.IsActionJustPressed("jump") && _player.CoyoteValid())
        {
            Machine.TransitionTo("JumpState");
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            _player.StartJumpBuffer();
        }

        if (_player.IsOnFloor())
        {
            if (_player.JumpBufferValid())
            {
                Machine.TransitionTo("JumpState");
                return;
            }
            Machine.TransitionTo("IdleState");
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
