using Godot;

public partial class JumpState : State
{
    [Export]
    private Muse _player;

    private bool _firstFrame;

    public override void Enter()
    {
        _firstFrame = true;
        _player.LeftGroundByJumping = true;
        _player.sprite.Play("jump");
        _player.Velocity = _player.Velocity with { Y = _player.JumpForce };
    }

    public override void PhysicsUpdate(double delta)
    {
        float dir = Input.GetAxis("move_left", "move_right");

        if (dir != 0)
        {
            _player.ApplyDirectionSnap(dir);
            _player.SetFacing(dir > 0);
            _player.Velocity = _player.Velocity with
            {
                X = Mathf.MoveToward(
                    _player.Velocity.X,
                    dir * _player.MoveSpeed,
                    _player.AirAcceleration * (float)delta
                ),
            };
        }

        if (!Input.IsActionPressed("jump") && _player.Velocity.Y < -120f)
            _player.Velocity = _player.Velocity with { Y = _player.Velocity.Y * 0.82f };

        _player.Velocity = _player.Velocity with
        {
            Y = Mathf.Min(
                _player.Velocity.Y + _player.Gravity * (float)delta,
                _player.MaxFallSpeed
            ),
        };

        _player.MoveAndSlide();

        if (_firstFrame)
        {
            _firstFrame = false;
            return;
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
    }
}
