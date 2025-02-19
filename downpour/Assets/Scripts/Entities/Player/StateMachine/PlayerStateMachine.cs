using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Downpour.Entity.Player;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerStateMachine : StateMachine
    {
        public PlayerIdleState IdleState { get; private set; }
        public PlayerRunState RunState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerFallState FallState { get; private set; }
        public PlayerSlashState SlashState { get; private set; }
        public PlayerDashState DashState { get; private set; }

        public Player Player { get; private set; }

        public event Action<PlayerState> StateChangeEvent;

        private void Awake() {
            IdleState = new PlayerIdleState(this);
            RunState = new PlayerRunState(this);
            JumpState = new PlayerJumpState(this);
            FallState = new PlayerFallState(this);
            SlashState = new PlayerSlashState(this);
            DashState = new PlayerDashState(this);

            Player = GetComponent<Player>();
        }

        private void Start() {
            EnterDefaultState();
        }

        public void PlayStateAnimation() {
            (CurrentState as PlayerState).PlayStateAnimation();
        }

        public override void ChangeState(State state) {
            base.ChangeState(state);
            StateChangeEvent?.Invoke(state as PlayerState);
        }
        
        public bool EnterDefaultState() {
            // Check if current state can't enter default, like falling, wallclimbing, dashing, ect.
            ChangeState(Player.PlayerMovementController.MovingDirection == 0 ? IdleState : RunState);
            return true;
        }

        public bool EnterJumpState() {
            if(!Player.PlayerMovementController.DesiredJump) {
                return false;
            }
            if(!JumpState.JumpStateJumpLogicHandler.CanJump(Player.PlayerMovementController, Player.PlayerStatsController)) {
                return false;
            }

            ChangeState(JumpState);
            return true;
        }

        public bool EnterFallState() {
            if(Player.PlayerMovementController.Grounded || Player.PlayerMovementController.rbVelocityY >= 0) {
                return false;
            }

            ChangeState(FallState);
            return true;
        }

        public bool EnterSlashState() {
            if(!Player.PlayerCombatController.DesiredSlash || !Player.PlayerCombatController.CanSlash) {
                return false;
            }

            ChangeState(SlashState);
            return true;
        }

        public bool EnterDashState() {
            //Debug.Log("ENTER DASH STATE");

            if(!Player.PlayerMovementController.DesiredDash || !Player.PlayerMovementController.CanDash) {
                return false;
            }

            ChangeState(DashState);
            return true;
        }
    }
}
