using Godot;
using System;

public partial class ArmoredAxeman : CharacterBody2D
{
	private CharacterBody2D _target;
	private int _speed = 100;
	private float _attackRange = 20; // Distância de ataque
	private AnimationPlayer _animationPlayer;
	private bool _isAttacking = false;
	
	private Sprite2D _myAnimatedSprite2D;

	public override void _Ready()
	{
		_target = GetParent().GetNode<CharacterBody2D>("PlayerCharacter"); // Nome do nó do jogador
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer"); // Assumindo que o AnimationPlayer está como filho do Axeman
		_myAnimatedSprite2D = GetNode<Sprite2D>("Sprite2D");
	}

	public override void _Process(double delta)
	{
		if (_target != null)
		{
			var scr = _target as KnightTemplar;
			if(scr.IsDie)
				return;
				
			Vector2 direction = (_target.Position - Position).Normalized();
			Position += new Vector2(direction.X * _speed * (float)delta, 0);
			
			// Verifica se o Axeman está dentro do alcance de ataque
			if (GlobalPosition.DistanceTo(_target.GlobalPosition) <= _attackRange && !_isAttacking)
			{
				_isAttacking = true;
				_animationPlayer.Play("attack_ia", -1, 1.0f, false); // Inicia a animação de ataque
			}
			else if (direction != Vector2.Zero && !_isAttacking)
			{			
				_animationPlayer.Play("run_ia");
			}
			else if(!_isAttacking)
			{
				_animationPlayer.Play("idle_ia");
			}
			
			bool isLeft = direction.X < 0;
			_myAnimatedSprite2D.FlipH = isLeft;
		}
	}

	private void _OnAttackAnimationFinished() // Sinal do AnimationPlayer quando a animação "Attack" termina
	{
		_isAttacking = false;
	}

	private void _OnAttackAnimationFrame(int frame)
	{
		if (frame == 0)
		{
			var scr = _target as KnightTemplar;
			scr.TakeDamage(10); // Chama a função TakeDamage do Knight Templar
		}
	}
}
