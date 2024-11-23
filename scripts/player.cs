using Godot;
using System;

public partial class player : Area2D
{
	// [Export] tag allows for value change in Godot editor
	[Export]
	public int playerSpeed { get; set; } = 400;

	public Vector2 screenSize;

    [Signal]
    public delegate void HitEventHandler();
    

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Position = new Vector2(100, 100);
        screenSize = GetViewportRect().Size;
        Hide();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero; // The player's movement vector.

        // 
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }
        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }
        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }

        // Get animated sprite 2d node
        var animatedSprite2D = GetNode<AnimatedSprite2D>("Sprite");

        // Normalize velocity vector to prevent diagonal movement from being faster than orthogonal movement
        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * playerSpeed;
            animatedSprite2D.Play();
        }
        else
        {
            animatedSprite2D.Stop();
        }

        // Update position, and stop player from leaving the window
        Position += velocity * (float)delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 75, screenSize.X - 75),
            y: Mathf.Clamp(Position.Y, 75, screenSize.Y - 75)
        );

        if (velocity.X != 0)
        {
            animatedSprite2D.Animation = "walk";
            animatedSprite2D.FlipV = false;
            animatedSprite2D.FlipH = velocity.X < 0;
        }
        else if (velocity.Y != 0)
        {
            animatedSprite2D.Animation = "up";
            animatedSprite2D.FlipV = velocity.Y > 0;
        }
    }

    // Signal for collision
    private void _on_body_entered(Node2D body)
    {
        // Hide player, defer physics property change
        // Defer is used to bypass an error for disabling collision shape during collision processing
        Hide();
        EmitSignal(SignalName.Hit);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }

}
