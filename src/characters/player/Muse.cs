using System;
using Godot;

public partial class Muse : CharacterBody2D
{
    [ExportCategory("player resources")]
    [Export]
    public AnimatedSprite2D sprite;

    [Export]
    public HealthComponent health;

    [Export]
    public HurtboxComponent hurtbox;

    [Export]
    public StateMachineComponent StateMachine;

    [ExportCategory("player vars")]
    [Export]
    public float MoveSpeed = 80f;

    [Export]
    public float Acceleration = 500f;

    [Export]
    public float Friction = 800f;

    [Export]
    public float AirAcceleration = 300f;

    [Export]
    public float JumpForce = -200f;

    [Export]
    public float Gravity = 600f;

    [Export]
    public Timer coyoteTimer;

    [Export]
    public Timer jumpBufferTimer;

    [Export]
    public Timer shootCooldown;

    public bool FacingRight { get; set; } = false;

    public override void _Ready()
    {
        AddToGroup("player");
        hurtbox.Hurt += OnHurt;
    }

    public void SetFacing(bool right)
    {
        FacingRight = right;
        sprite.FlipH = right;
    }

    public void StartCoyoteTime()
    {
        coyoteTimer.Start();
    }

    public bool CoyoteValid()
    {
        return !coyoteTimer.IsStopped();
    }

    public void StartJumpBuffer()
    {
        jumpBufferTimer.Start();
    }

    public bool JumpBufferValid()
    {
        return !jumpBufferTimer.IsStopped();
    }

    public void StartShootCooldown()
    {
        shootCooldown.Start();
    }

    public bool CanShoot()
    {
        return shootCooldown.IsStopped();
    }

    public void OnHurt()
    {
        StateMachine.TransitionTo("HurtState");
    }
}
