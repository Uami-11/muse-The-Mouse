using Godot;

public partial class Walker : CharacterBody2D
{
    [ExportCategory("Walker Vars")]
    [Export]
    public float PatrolSpeed = 40f;

    [Export]
    public float ChaseSpeed = 70f;

    [Export]
    public float Gravity = 540f;

    [Export]
    public float MaxFallSpeed = 840f;

    [ExportCategory("References")]
    [Export]
    public AnimatedSprite2D Sprite;

    [Export]
    public HealthComponent Health;

    [Export]
    public HurtboxComponent Hurtbox;

    [Export]
    public Area2D detection;

    [Export]
    public StateMachineComponent StateMachine;

    [Export]
    public RayCast2D WallDetector;

    [Export]
    public RayCast2D EdgeDetector;

    public bool FacingRight { get; set; } = false;
    public Node2D Player { get; private set; }

    public override void _Ready()
    {
        Health.Died += () => StateMachine.TransitionTo("DeadState");

        Hurtbox.Hurt += () => StateMachine.TransitionTo("HurtState");

        detection.BodyEntered += OnBodyEntered;
        detection.BodyExited += OnBodyExited;

        SetFacing(false); // init raycasts pointing right
    }

    public void SetFacing(bool right)
    {
        FacingRight = right;
        Sprite.FlipH = right;

        float sign = right ? 1f : -1f;
        WallDetector.TargetPosition = new Vector2(
            Mathf.Abs(WallDetector.TargetPosition.X) * sign,
            0
        );
        EdgeDetector.TargetPosition = new Vector2(
            Mathf.Abs(EdgeDetector.TargetPosition.X) * sign,
            10
        );
    }

    public bool HitsWall() => WallDetector.IsColliding();

    public bool AtEdge() => IsOnFloor() && !EdgeDetector.IsColliding();

    private void OnBodyEntered(Node2D body)
    {
        if (!body.IsInGroup("player"))
            return;
        Player = body;
        StateMachine.TransitionTo("ChaseState");
    }

    private void OnBodyExited(Node2D body)
    {
        if (!body.IsInGroup("player"))
            return;
        Player = null;
        StateMachine.TransitionTo("PatrolState");
    }
}
