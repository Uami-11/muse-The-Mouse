using System;
using Godot;

public partial class JumpState : State
{
    [Export]
    private Muse _player;

    public override void Enter()
    {
        _player.sprite.Play("jump");
        _player.Velocity = _player.Velocity with { Y = _player.JumpForce };
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

        if (Input.IsActionJustReleased("jump") && _player.Velocity.Y < 0)
        {
            _player.Velocity = _player.Velocity with { Y = _player.Velocity.Y * 0.82f };
        }

        if (_player.Velocity.Y > 0)
        {
            Machine.TransitionTo("FallState");
            return;
        }

        if (_player.IsOnFloor())
        {
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
