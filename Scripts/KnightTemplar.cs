using Godot;
using System;

public partial class KnightTemplar : CharacterBody2D
{
	private int _health = 100;
	
	private const float SPEED = 300.0f;
	private const float JUMP_VELOCITY = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private float _gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	private Sprite2D _myAnimatedSprite2D;
	private AnimationPlayer _animationPlayer;
	
	public bool IsDie { get; private set; }

	public override void _Ready()
	{
		_myAnimatedSprite2D = GetNode<Sprite2D>("Sprite2D");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer"); 
	}

	public override void _PhysicsProcess(double delta)
	{
		if(IsDie)
			return;
			
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += _gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JUMP_VELOCITY;

		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * SPEED;
			
			if(direction.Y > 0)
				_animationPlayer.Play("jump");
			else
				_animationPlayer.Play("run");
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, SPEED);
			_animationPlayer.Play("idle");
		}
		
		Velocity = velocity;
		MoveAndSlide();
		bool isLeft = velocity.X < 0;
		_myAnimatedSprite2D.FlipH = isLeft;
	}
	
	public void TakeDamage(int amount)
	{
		if(IsDie)
			return;
			
		_health -= amount;
		if (_health <= 0)
			Die();
	}
	
	private void Die()
	{
		IsDie = true;
		_animationPlayer.Play("die", -1, 1.0f, false);
	}
}
