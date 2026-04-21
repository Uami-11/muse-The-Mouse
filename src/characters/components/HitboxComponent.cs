using System;
using Godot;

public partial class HitboxComponent : Area2D
{
    [ExportCategory("Hitbox vars")]
    [Export]
    public int Damage = 1;

    [Export]
    public string Team = "player";
}
