using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Downpour.Entity.Player
{
    public class PlayerAnimationController : PlayerComponent
    {
        [field: SerializeField] public Animator PlayerAnimator { get; private set; }

        [field: SerializeField] public PlayerAnimationClip IdleAnimationClip { get; private set; }
        [field: SerializeField] public PlayerAnimationClip SlashAnimationClip { get; private set; }
        [field: SerializeField] public PlayerAnimationClip DiffusionAnimationClip { get; private set; }
        [field: SerializeField] public PlayerAnimationClip DashAnimationClip { get; private set; }
        [field: SerializeField] public PlayerAnimationClip ShootAnimationClip { get; private set; }

        public string CurrentAnimation { get; private set; }

        public void PlayAnimation(PlayerAnimationClip animationClip) {
            _resetAnimationSpeed(); // Reset Animation Speed
            animationClip.PlayAnimation(PlayerAnimator);
        }

        public void PlayAnimation(PlayerAnimationClip animationClip, float speed) {
            _setAnimationSpeed(speed); // Reset Animation Speed
            animationClip.PlayAnimation(PlayerAnimator);
        }

        private void _resetAnimationSpeed() {
            _setAnimationSpeed(1f);
        }

        private void _setAnimationSpeed(float speed) {
            PlayerAnimator.speed = 1/speed;
        }

        [Serializable]
        public struct PlayerAnimationClip {
            [field: SerializeField] public string Clip { get; private set; }

            public void PlayAnimation(Animator animator) {
                animator.Play(Clip);
            }
        }
    }
}
