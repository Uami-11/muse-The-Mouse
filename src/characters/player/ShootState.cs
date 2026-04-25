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

        _player.SpawnBullet(dir);
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 dir = _player.FacingRight ? Vector2.Right : Vector2.Left;
        if (_player.IsOnFloor())
            _player.Velocity = _player.Velocity with
            {
                X = Mathf.MoveToward(_player.Velocity.X, 0, (float)350.0 * (float)delta),
            };
        else
            _player.Velocity = _player.Velocity with
            {
                Y = _player.Velocity.Y + 600 * (float)delta,
            };

        _player.MoveAndSlide();

        if (!_player.sprite.IsPlaying())
        {
            if (Input.IsActionPressed("shoot") && _player.CanShoot())
            {
                _player.sprite.Play("shoot");
                _player.StartShootCooldown();
                _player.SpawnBullet(dir);
            }
            else
            {
                Machine.TransitionTo(_wasOnFloor ? "IdleState" : "FallState");
            }
        }
    }
}
