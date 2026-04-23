using Godot;

public partial class ShootState : State
{
    [Export]
    private Muse _player;

    private bool _wasOnFloor;

    public override void Enter()
    {
        _wasOnFloor = _player.IsOnFloor();
        _player.sprite.Play("shoot");
        _player.StartShootCooldown();

        Vector2 dir = _player.FacingRight ? Vector2.Right : Vector2.Left;
        string origin = _player.FacingRight ? "ShootOriginRight" : "ShootOriginLeft";

        // TODO: _player.SpawnBullet(origin, dir);
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!_player.IsOnFloor())
            _player.Velocity = _player.Velocity with
            {
                Y = Mathf.Min(
                    _player.Velocity.Y + _player.Gravity * (float)delta,
                    _player.MaxFallSpeed
                ),
            };

        _player.MoveAndSlide();

        if (!_player.sprite.IsPlaying())
        {
            if (Input.IsActionPressed("shoot") && _player.CanShoot())
            {
                _player.sprite.Play("shoot");
                _player.StartShootCooldown();
                // TODO: _player.SpawnBullet(...)
            }
            else
            {
                Machine.TransitionTo(_wasOnFloor ? "IdleState" : "FallState");
            }
        }
    }
}
