using Godot;

public partial class Bullet : Area2D
{
    [ExportCategory("Bullet vars")]
    [Export]
    private AnimatedSprite2D sprite;

    [Export]
    private VisibleOnScreenNotifier2D onScreen;

    [Export]
    public float Speed = 400.0f;

    [Export]
    public int Damage = 1;

    [Export]
    public string Team = "player";

    public Vector2 Direction { get; set; } = Vector2.Right;

    public override void _Ready()
    {
        Rotation = Direction.Angle();
        onScreen.ScreenExited += QueueFree;

        AreaEntered += OnAreaEntered;
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is not HurtboxComponent hurtbox)
            return;
        if (hurtbox.Team == Team)
            return;
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        QueueFree();
    }
}
