
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Downpour.Input;
using Downpour.Scenes;
using System;
using System.Diagnostics;

namespace Downpour.Entity.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : PlayerComponent
    {
        private InputReader _inputReader;

        // Enum to keep track of player direction
        public enum Direction { 
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        // Vars to keep track of direction.
        // Head direction is the way the blade will swing.
        // Facing direction is the direction the player is actually facing
        // Moving direction is the direction the player is moving. Can be zero if the player is not moving.
        public Direction HeadDirection { get; private set; }
        public int FacingDirection { get; private set; }
        public int MovingDirection { get; private set; }

        // Variable to keep track of true direction of the sprite. Used for flipping/
        private bool _spriteFacingRight = false;
        public bool SpriteFacingRight { get { return _spriteFacingRight; } }

        // Rigidbody with getters
        private Rigidbody2D _rb;
        public float rbVelocityX { get { return _rb.velocity.x; } }
        public float rbVelocityY { get { return _rb.velocity.y; } }
        public Vector2 rbVelocity { get { return _rb.velocity; } }
        public float rbPositionX { get { return _rb.position.x; } }
        public float rbPositionY { get { return _rb.position.y; } }

        // Jump Counters. Keeps track of coyote time and jump buffers.
        [field: SerializeField] public float CoyoteCounter { get; private set; }
        [field: SerializeField] public float JumpBufferCounter { get; private set; }

        // Bool to keep track of if the input is desiring a jump.
        public bool DesiredJump { get; private set; }

        // Bool to keep track of if the player has released the jump key.
        private bool _isJumpReset;
        public bool UsedDoubleJump { get; private set; }

        // Grounded bool with a public getter leading to _checkGroundedRequest
        private bool _grounded;
        public bool Grounded { get { return _checkGroundedRequest(); } }

        private PlayerData.ColliderBounds _colliderBoundsSource;

        [SerializeField] private Transform _playerSpriteTransform;

        public bool DesiredDash { get; private set; }
        public float DashCooldownCounter { get; private set; }
        [field: SerializeField] public bool CanDash { get; private set; }

        public float DashBufferCounter { get; private set; }
        private bool _dashBuffered;

        public event Action<float> DashEvent;
        public event Action FinishDashEvent;

        public ParticleSystem dust;
        public ParticleSystem dashParticle;

        // Initialization
        protected override void Awake() {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
        }

        // Initialization requiring other scripts.
        private void Start() {
            _inputReader =  InputReader.Instance;

            if(this.enabled) {
                _inputReader.MovementEvent += _handleMovementInput;
                _inputReader.JumpEvent += _handleJumpInput;
                _inputReader.DashEvent += _handleDashInput;
            }

            SceneLoader.Instance.BeforeSceneLoadEvent += _disableInput;

            _playerStateMachine.StateChangeEvent += _onStateChange;

            FacingDirection = 1;
        }

        private void _disableInput() {
            _inputReader.MovementEvent -= _handleMovementInput;
            _inputReader.JumpEvent -= _handleJumpInput;
            _inputReader.DashEvent -= _handleDashInput;

            SceneLoader.Instance.BeforeSceneLoadEvent -= _disableInput;
        }

        private void FixedUpdate() {
            _checkCollisions();

            _handleJumpData();
        }

        private void Update() {
            if (DashBufferCounter > 0) {
                DashBufferCounter -= Time.deltaTime;
            } else {
                _dashBuffered = false;
            }

            if (DashCooldownCounter > 0) {
                CanDash = false;
                DashCooldownCounter -= Time.deltaTime;
            } else {
                CanDash = true;
            }

            if (!((_playerStateMachine.CurrentState is PlayerSlashState) || (_playerStateMachine.CurrentState is PlayerDashState)) && _dashBuffered) {
                DesiredDash = true;
            }

            if ((_playerStateMachine.CurrentState is PlayerRunState))
            {
               var emitParams = new ParticleSystem.EmitParams();
                emitParams.startSize = 0.25f;
                emitParams.startLifetime = 0.15f;
                emitParams.velocity = new Vector3(_playerMovementController.FacingDirection == -1 ? 5f: -5f, 4f, 0f);
                //emitParams.position = position;

                emitParams.position = new Vector3(_playerMovementController.FacingDirection == -1 ? transform.position.x+0.6f : transform.position.x - 0.6f, transform.position.y-0.4f, 0f);

                //if (_playerMovementController.FacingDirection == -1)
                //{
                  //  emitParams.rotation = 245f;
                //}

                // if(CurrentSlashComboAttack == 1) {
                //     emitParams.rotation += (_playerMovementController.FacingDirection == 1) ? 75f : -75f;
                // }

                // Debug.Log(emitParams.position.y);
                // Debug.Log(position.y);
 
                emitParams.applyShapeToPosition = true;
                dust.Emit(emitParams, 1);

                var rotation = _playerSpriteTransform.rotation;

                Vector3 newRot = new Vector3(0f, 0f, 0f);
                if (FacingDirection == 1)
                    newRot = new Vector3(0f, 0f, -5f);
                else
                    newRot = new Vector3(0f, 0f, 5f);

                //_playerSpriteTransform.eulerAngles = newRot;

                _playerSpriteTransform.rotation = Quaternion.Lerp(rotation, Quaternion.Euler(newRot), 10.0f * Time.deltaTime);
            } else
            {
                Vector3 newRot = new Vector3(0, 0, 0);
                var rotation = _playerSpriteTransform.rotation;

                _playerSpriteTransform.eulerAngles = newRot;
                _playerSpriteTransform.rotation = Quaternion.Lerp(rotation, Quaternion.Euler(newRot), 10.0f * Time.deltaTime);
            }
        }

        public void emitJumpParticle() {
            var emitParams = new ParticleSystem.EmitParams();
                emitParams.startSize = 0.35f;
                emitParams.startLifetime = 0.2f;
                emitParams.velocity = new Vector3(0f, 2f, 0f);
                //emitParams.position = position;

                emitParams.position = new Vector3(transform.position.x, transform.position.y-0.5f, 0f);

                //if (_playerMovementController.FacingDirection == -1)
                //{
                  //  emitParams.rotation = 245f;
                //}

                // if(CurrentSlashComboAttack == 1) {
                //     emitParams.rotation += (_playerMovementController.FacingDirection == 1) ? 75f : -75f;
                // }

                // Debug.Log(emitParams.position.y);
                // Debug.Log(position.y);

                emitParams.applyShapeToPosition = true;
                dust.Emit(emitParams, 5);
        }

        // <summary>
        // Sets velocity of Player character.
        // </summary>
        public void setVelocity(float x, float y) {
            _rb.velocity = new Vector2(x, y);
        }

        public void setVelocity(Vector2 velocity) {
            _rb.velocity = velocity;
        }

        // Jump Movement

        private void _handleJumpInput(bool startingJump) {
            DesiredJump = startingJump;
        }

        public void emitDashParticles() {
            var emitParams = new ParticleSystem.EmitParams();
                emitParams.startSize = 0.3f;
                emitParams.startLifetime = 0.25f;
                emitParams.velocity = new Vector3(_playerMovementController.FacingDirection == -1 ? 6f: -6f, 3f, 0f);
                //emitParams.position = position;

                emitParams.position = new Vector3(_playerMovementController.FacingDirection == -1 ? transform.position.x+0.6f : transform.position.x - 0.6f, transform.position.y-0.4f, 0f);

                //if (_playerMovementController.FacingDirection == -1)
                //{
                  //  emitParams.rotation = 245f;
                //}

                // if(CurrentSlashComboAttack == 1) {
                //     emitParams.rotation += (_playerMovementController.FacingDirection == 1) ? 75f : -75f;
                // }

                // Debug.Log(emitParams.position.y);
                // Debug.Log(position.y);
 
                emitParams.applyShapeToPosition = true;
                dust.Emit(emitParams, 1);
        }

        private void _handleJumpData() {
            // Check if grounded to handle coyote time. If not, coyote time ticks down.
            if(Grounded) {
                CoyoteCounter = _playerStatsController.CurrentPlayerStats.CoyoteTime;
                UsedDoubleJump = false;
            } else {
                CoyoteCounter -= Time.deltaTime;
            }

            // Check for jumping while actively jumping to handle jump buffering.
            // Is Jump Reset checks for if the player has let go of the jump key and is ready to press it again to trigger a jump (in this case buffered)
            if(DesiredJump && _isJumpReset) {
                _isJumpReset = false;

                JumpBufferCounter = _playerStatsController.CurrentPlayerStats.JumpBufferTime;
            } else if(JumpBufferCounter > 0) {
                JumpBufferCounter -= Time.deltaTime;
            } else if (!DesiredJump) {
                _isJumpReset = true;
            }
        }

        public void UseDoubleJump() {
            UsedDoubleJump = true;
        }

        public void ResetJumpBuffer() {
            JumpBufferCounter = 0;
        }

        public void ResetCoyoteTime() {
            CoyoteCounter = 0;
        }

        // Horizontal Movement

        // Handles move event.
        private void _handleMovementInput(float x, float y, bool startingMovement) {
            _flipCheck();

            if(x != 0) {
                FacingDirection = x > 0 ? 1 : -1;

                if(y == 0) {
                    HeadDirection = x > 0 ? Direction.RIGHT : Direction.LEFT;
                }

                MovingDirection = startingMovement ? FacingDirection : 0;
            } else {
                MovingDirection = 0;
            }

            if(y != 0) {
                HeadDirection = y > 0 ? Direction.UP : Direction.DOWN;
            }
        }

        private void _onStateChange(PlayerState p) {
            _flipCheck();
        }

        // <summary>
        // Flips sprite if needed.
        // <summary/>
        private void _flipCheck() {
          //  Debug.Log(FacingDirection + " " + _spriteFacingRight);
            if(FacingDirection>0 && !_spriteFacingRight) {
                _flip();
            } else if(FacingDirection < 0 && _spriteFacingRight) {
                _flip();
            }
        }

        // <summary>
        // Flips sprite.
        // <summary/>
        private void _flip() {
            
            FacingDirection *= -1;
            //Debug.Log(_playerStateMachine.CurrentState + " " + (_playerStateMachine.CurrentState as PlayerState).CanFlip);

            if((_playerStateMachine.CurrentState as PlayerState).CanFlip) {
                _spriteFacingRight=!_spriteFacingRight;
                _playerStateMachine.PlayStateAnimation();
                _playerSpriteTransform.localScale = new Vector3(_playerSpriteTransform.localScale.x*-1f, 1f, 1f);
            }
        }

        // Colliders

        private bool _checkGroundedRequest() {
            _checkCollisions();
            return _grounded;
        }

        public void SetColliderBounds(PlayerData.ColliderBounds colliderBounds) {
            _colliderBoundsSource = colliderBounds;
            // _collider.offset = colliderBounds.bounds.min;
            // _collider.size = colliderBounds.bounds.size;
            _checkCollisions();
        }

        public PlayerData.ColliderBounds GetColliderBounds() {
            return _colliderBoundsSource;
        }

        private void _checkCollisions() {
            Vector2 charPosition = transform.position;
            Vector2 boundsPosition = _colliderBoundsSource.feetRect.position * transform.localScale;
            _grounded = Physics2D.OverlapBox(charPosition + boundsPosition, _colliderBoundsSource.feetRect.size, 0, Layers.GroundLayer);
        }

        private void _handleDashInput(bool startingDash) {
            if((_playerStateMachine.CurrentState is PlayerSlashState || _playerStateMachine.CurrentState is PlayerDashState) &&  !_dashBuffered && startingDash) {
                _dashBuffered = true;
                DashBufferCounter = _playerStatsController.CurrentPlayerStats.DashBufferTime;
            }
            DesiredDash = startingDash;
        }

        public IEnumerator Dash() {
            
            DashEvent?.Invoke(_playerStatsController.CurrentPlayerStats.DashSpeed);
            
            
            _dashBuffered = false;

            DashCooldownCounter = _playerStatsController.CurrentPlayerStats.DashCooldown;
            CanDash = false;

            _playerStatsController.iframeCounter = _playerStatsController.CurrentPlayerStats.DashLength;
            
            yield return new WaitForSeconds(_playerStatsController.CurrentPlayerStats.DashLength);

            FinishDashEvent?.Invoke();

            DesiredDash = false;
        }
    }
}