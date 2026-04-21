using System;
using Godot;

public partial class ShootState : State
{
    [Export]
    private Muse _player;

    private bool _wasOnFloor;

    public override void Enter()
    {
        _player.sprite.Play("shoot");
        _wasOnFloor = _player.IsOnFloor();

        Vector2 dir = _player.FacingRight ? Vector2.Right : Vector2.Left;
        string origin = _player.FacingRight ? "ShootOriginRight" : "ShootOriginLeft";

        // SPawn bullter
    }

    public override void Exit()
    {
        _player.StartShootCooldown();
    }

    public override void PhysicsUpdate(double delta)
    {
        if (!_player.IsOnFloor())
        {
            _player.Velocity = _player.Velocity with
            {
                Y = _player.Velocity.Y + _player.Gravity * (float)delta,
            };
        }

        _player.MoveAndSlide();

        if (!_player.sprite.IsPlaying())
            Machine.TransitionTo(_wasOnFloor ? "IdleState" : "FallState");
    }
}
