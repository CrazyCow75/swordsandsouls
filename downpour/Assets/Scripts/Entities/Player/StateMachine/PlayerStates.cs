using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Combat;

namespace Downpour.Entity.Player
{
    public abstract class PlayerState : State {
        protected PlayerStateMachine _psm => (_sm as PlayerStateMachine);
        protected Player _player => _psm.Player;

        protected PlayerStatsController _playerStatsController => _player.PlayerStatsController;
        protected PlayerMovementController _playerMovementController => _player.PlayerMovementController;
        protected PlayerAnimationController _playerAnimationController => _player.PlayerAnimationController;
        
        public PlayerState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public bool CanFlip { get; protected set; }

        public override void Enter(State previousState) {
            CanFlip = true;
            PlayStateAnimation();
        }

        public virtual void PlayStateAnimation() {

        }
    }

    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void Enter(State previousState) {
            base.Enter(previousState);
            _playerMovementController.setVelocity(0, _playerMovementController.rbVelocityY);
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.IdleAnimationClip);
        }

        public override void Update() {
            if(_psm.EnterJumpState()
            || _psm.EnterFallState()
            || _psm.EnterSlashState()
            || _psm.EnterDashState()) {
                return;
            }

            if(_playerMovementController.MovingDirection != 0) {
                _psm.ChangeState(_psm.RunState);
            }
        }
    }

    public class PlayerRunState : PlayerState
    {
        public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { 
            RunStateRunLogicHandler = new RunLogicHandler(playerStateMachine);
        }

        public RunLogicHandler RunStateRunLogicHandler { get; private set; }

        public override void Enter(State previousState) {
            base.Enter(previousState);
            
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.RunAnimationClip);
        }

        public override void Update() {
            if(_psm.EnterJumpState()
            || _psm.EnterFallState()
            || _psm.EnterSlashState()
            || _psm.EnterDashState()) {
                return;
            }

            if(_playerMovementController.MovingDirection == 0) {
                _psm.ChangeState(_psm.IdleState);
            } else {
                RunStateRunLogicHandler.SetDesiredVelocity(_playerMovementController, _playerStatsController);
            }
        }

        public override void FixedUpdate() {
            _playerMovementController.setVelocity(RunStateRunLogicHandler.GetVelocityX(), _playerMovementController.rbVelocityY);
        }

        public class RunLogicHandler {
            private Vector2 _direction, _desiredVelocity;
            private PlayerStateMachine _playerStateMachine;

            public RunLogicHandler(PlayerStateMachine playerStateMachine) {
                _playerStateMachine = playerStateMachine;
            }

            public void SetDesiredVelocity(PlayerMovementController playerMovementController, PlayerStatsController playerStatsController) {
                _direction.x = playerMovementController.MovingDirection;
                _desiredVelocity = new Vector2((_direction.x * playerStatsController.CurrentPlayerStats.MoveSpeed), 0f);
            }

            public float GetVelocityX() {
                return _desiredVelocity.x;
            }
        }
    }

    public class PlayerJumpState : PlayerState
    {
        public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) {
            JumpStateJumpLogicHandler = new JumpLogicHandler(playerStateMachine);
        }

        public JumpLogicHandler JumpStateJumpLogicHandler { get; private set; }

        private Vector2 _velocity;
        

        public override void Enter(State previousState) {
            base.Enter(previousState);
            
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
            _playerMovementController.emitJumpParticle();
            _playerMovementController.jumpSFX.Play();
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.JumpAnimationClip);
        }

        public override void Update() {

            if(_psm.EnterSlashState()) {
                return;
            }

            if(_playerStatsController.CurrentPlayerStats.HasAirDash) {
                if(_psm.EnterDashState()) {
                    return;
                }
            }
            

            _psm.RunState.RunStateRunLogicHandler.SetDesiredVelocity(_playerMovementController, _playerStatsController);
        }

        public override void FixedUpdate() {
            _velocity = _playerMovementController.rbVelocity;
            _velocity.x = _psm.RunState.RunStateRunLogicHandler.GetVelocityX();

            _velocity.y = JumpStateJumpLogicHandler.HandleJump(_playerMovementController, _playerStatsController).y;

            if(!_playerMovementController.DesiredJump && !(JumpStateJumpLogicHandler.getJumpTime() < _playerStatsController.CurrentPlayerStats.MinJumpTime)) {
                _psm.EnterDefaultState();
            }

            _playerMovementController.setVelocity(_velocity);
            _playerMovementController.ResetCoyoteTime();

            if(_playerMovementController.rbVelocityY < 0) {
                _psm.ChangeState(_psm.FallState);
            }
            if(_psm.EnterFallState()) {
                return;
            }
        }

        public class JumpLogicHandler {
            private float _jumpSpeed;
            private bool _isJumping;
            private Vector2 _velocity;
            private PlayerStateMachine _playerStateMachine;
            private float _jumpTime;
            public JumpLogicHandler(PlayerStateMachine playerStateMachine) {
                _playerStateMachine = playerStateMachine;
                _jumpTime = 0f;
            }

            public bool CanJump(PlayerMovementController playerMovementController, PlayerStatsController playerStatsController) {
                 return (playerMovementController.CoyoteCounter > 0 
                || (!playerMovementController.UsedDoubleJump && playerStatsController.CurrentPlayerStats.HasDoubleJump && _isJumping)) && (playerMovementController.JumpBufferCounter > 0);
            }

            public Vector2 HandleJump(PlayerMovementController playerMovementController, PlayerStatsController playerStatsController) {
                _jumpTime += Time.deltaTime;

                _velocity = playerMovementController.rbVelocity;

                if(playerMovementController.Grounded) {
                    _isJumping = false;
                    _jumpTime = 0f;
                }
                //Debug.Log(_jumpTime);
                if(playerMovementController.DesiredJump || _jumpTime < playerStatsController.CurrentPlayerStats.MinJumpTime) {
                    _jumpAction(playerMovementController, playerStatsController);
                } else {
                    _velocity = new Vector2(_velocity.x, Mathf.Min(playerMovementController.rbVelocityY, 0.075f));
                }

                return _velocity;
            }

            public float getJumpTime() {
                return _jumpTime;
            }

            private void _jumpAction(PlayerMovementController playerMovementController, PlayerStatsController playerStatsController) {
                if(CanJump(playerMovementController, playerStatsController)) {
                    if(_isJumping && !(playerMovementController.CoyoteCounter > 0 ) ) {
                        playerMovementController.UseDoubleJump();
                        Debug.Log("Double Jump");
                    }

                    playerMovementController.ResetJumpBuffer();

                    _jumpSpeed = MathF.Sqrt(-2f * Physics2D.gravity.y * playerStatsController.CurrentPlayerStats.JumpHeight);
                    _isJumping = true;

                    if(playerMovementController.rbVelocityY > 0f) {
                        _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                    }
                    else if (_velocity.y < 0f)
                    {
                        _jumpSpeed += Mathf.Abs(playerMovementController.rbVelocityY);
                    }

                    _velocity.y += _jumpSpeed;
                }
            }
        }
    }

    public class PlayerFallState : PlayerState
    {
        public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        private float _fallStartPositionY;

        public override void Enter(State previousState) {
            base.Enter(previousState);

            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);
            _fallStartPositionY = _playerMovementController.rbPositionY;
        }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.FallAnimationClip);
        }

        public override void Update() {
            if(_psm.EnterJumpState()) {
                return;
            }

            if(_psm.EnterSlashState()) {
                return;
            }

            if(_playerStatsController.CurrentPlayerStats.HasAirDash) {
                if(_psm.EnterDashState()) {
                    return;
                }
            }
            // check sliding wall

            // Cap Max Fall Speed
            _playerMovementController.setVelocity(new Vector2(_playerMovementController.rbVelocityX, Mathf.Min(_playerMovementController.rbVelocityY, _playerStatsController.CurrentPlayerStats.MaxFallSpeed)));
            
            if(_playerMovementController.Grounded) {
                _playerMovementController.emitJumpParticle();
                _psm.EnterDefaultState();
            }

            _psm.RunState.RunStateRunLogicHandler.SetDesiredVelocity(_playerMovementController, _playerStatsController);
        }

        public override void FixedUpdate() {
            _handleHorizontalMovement();
        }

        private void _handleHorizontalMovement() {
            _playerMovementController.setVelocity(_psm.RunState.RunStateRunLogicHandler.GetVelocityX(), _playerMovementController.rbVelocityY);
        }
    }

    public class PlayerSlashState : PlayerState
    {
        public PlayerSlashState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        private PlayerCombatController _playerCombatController;

        private float _knockbackCounter;
        private bool _knockback;
        private int _knockbackDirection;
    
        public override void Enter(State previousState)
        {
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);

            _playerCombatController = _player.PlayerCombatController;

            _playerCombatController.FinishSlashEvent += _enterDefaultState;
            _playerCombatController.HitEvent += _triggerKnockback;
            
            _playerCombatController.StartCoroutine(_playerCombatController.Slash());
            base.Enter(previousState);

            CanFlip = false;
        }

        public override void PlayStateAnimation() {
            if(_playerStatsController.weapon.m_CardData.id == 4) {
                _playerAnimationController.PlayAnimation(_playerAnimationController.SlashAnimationClip, _playerStatsController.CurrentPlayerStats.SlashSpeed);
            } else if (_playerStatsController.weapon.m_CardData.id == 5) {
                _playerAnimationController.PlayAnimation(_playerAnimationController.DiffusionAnimationClip, _playerStatsController.CurrentPlayerStats.DiffusionSpeed);
            } else if(_playerStatsController.weapon.m_CardData.id == 12) {
                _playerAnimationController.PlayAnimation(_playerAnimationController.ShootAnimationClip, _playerStatsController.CurrentPlayerStats.BombSpeed);
            } else {
                _playerAnimationController.PlayAnimation(_playerAnimationController.ShootAnimationClip, _playerStatsController.CurrentPlayerStats.BulletSpeed);
            }
        }

        private void _enterDefaultState() {
            _playerCombatController.FinishSlashEvent -= _enterDefaultState;
            _playerCombatController.HitEvent -= _triggerKnockback;
            if(_playerMovementController.rbVelocityY > 0f && !(_psm.JumpState.JumpStateJumpLogicHandler.getJumpTime() < _playerStatsController.CurrentPlayerStats.MinJumpTime)) {
                _psm.ChangeState(_psm.JumpState);
                return;
            }
            _psm.EnterDefaultState();
        }

        private void _triggerKnockback(IHittable hittable, int damage, int direction) {
            _knockbackDirection = direction;
            _knockbackCounter = _playerStatsController.CurrentPlayerStats.SlashKnockbackTime;
        }

        public override void Update() {
            _psm.RunState.RunStateRunLogicHandler.SetDesiredVelocity(_playerMovementController, _playerStatsController);

            if(_knockbackCounter > 0) {
                _knockbackCounter -= Time.deltaTime;
                _knockback = true;
            } else {
                _knockback = false;
            }
        }

        public override void FixedUpdate() {
            _playerMovementController.setVelocity(_psm.RunState.RunStateRunLogicHandler.GetVelocityX(), _playerMovementController.rbVelocityY);

            _playerMovementController.setVelocity(_psm.JumpState.JumpStateJumpLogicHandler.HandleJump(_playerMovementController, _playerStatsController));
            _playerMovementController.ResetCoyoteTime();

            if(_knockback) {
                _playerMovementController.setVelocity(_knockbackDirection * _playerStatsController.CurrentPlayerStats.SlashKnockbackMultiplier, _playerMovementController.rbVelocityY);
            }
        }
    }

    public class PlayerDashState : PlayerState
    {
        public PlayerDashState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

        public override void PlayStateAnimation() {
            _playerAnimationController.PlayAnimation(_playerAnimationController.DashAnimationClip);
        }

        public override void Enter(State previousState)
        {
            _playerMovementController.SetColliderBounds(_player.PlayerData.StandColliderBounds);

            _playerMovementController.FinishDashEvent += _enterDefaultState;

            // _playerMovementController.setVelocity(new Vector2(0f, _playerMovementController.rbVelocityY > 0 ? 0.075f : 0f));

           
            
            _playerMovementController.StartCoroutine(_playerMovementController.Dash());
            base.Enter(previousState);

            CanFlip = false;
        }

        public override void FixedUpdate() {
            float direction = _playerMovementController.FacingDirection;
            Vector2 velocity = new Vector2(direction * _playerStatsController.CurrentPlayerStats.DashSpeed, 0f);

            _playerMovementController.setVelocity(velocity);
            _playerMovementController.emitDashParticles();
        }

        private void _enterDefaultState() {
            _psm.EnterDefaultState();
        }
    }
}
