using Godot;
using System;

public partial class HelloWorld : Node2D
{
    private float time = 0.0f;

    // Declare label var
    private Label timeLabel;

    // Setup function
    public override void _Ready()
    {
        base._Ready();
        
        // Initialize label var to the label in scene titled "TimeLabel", which is parented under "Node2D"
        timeLabel = GetNode<Label>("Node2D/TimeLabel");
    }

    // Update function
    public override void _Process(double delta)
    {
        base._Process(delta);

        // Increment time by deltaTime, and update TimeLabel text
        time += (float)delta;
        timeLabel.Text = $"Time Elapsed: {time:F1}s";
    }
}
