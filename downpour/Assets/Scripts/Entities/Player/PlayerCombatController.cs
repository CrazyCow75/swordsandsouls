using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Downpour.Input;
using Downpour.Combat;
using Downpour.Scenes;

namespace Downpour.Entity.Player
{
    public class PlayerCombatController : PlayerComponent
    {
        private InputReader _inputReader;
        public bool DesiredSlash { get; private set; }

        public event Action<int, float, float> SlashEvent;
        public event Action<IHittable, int, int> HitEvent;
        public event Action FinishSlashEvent;

        public int CurrentSlashComboAttack { get; private set; }
        public float SlashComboCounter { get; private set; }

        public float SlashCooldownCounter { get; private set; }
        public bool CanSlash { get; private set; }

        public float SlashBufferCounter { get; private set; }
        private bool _slashBuffered;

        [SerializeField] private GameObject _slashHitPrefab;
        [SerializeField] private ParticleSystem _slashHitParticle;

        [SerializeField] private float _slashHitEffectTime;
        [SerializeField] private float _slashHitEffectShakeDuration;
        [SerializeField] private float _slashHitEffectShakeMagnitude;

        private void Start() {
            
            CurrentSlashComboAttack = 0;

            _inputReader =  InputReader.Instance;
            HitEvent += _onHit;

            if(this.enabled) {
                _inputReader.SlashEvent += _handleSlashInput;
            }

            SceneLoader.Instance.BeforeSceneLoadEvent += _disableInput;
        }

        private void _disableInput() {
            _inputReader.SlashEvent -= _handleSlashInput;
        }
        
        private void Update() {
            if(SlashComboCounter > 0) {
                SlashComboCounter -= Time.deltaTime;
            }

            if(SlashBufferCounter > 0) {
                SlashBufferCounter -= Time.deltaTime;
            } else {
                _slashBuffered = false;
            }

            if(SlashCooldownCounter > 0) {
                CanSlash = false;
                SlashCooldownCounter -= Time.deltaTime;
            } else {
                CanSlash = true;
            }

            if(!((_playerStateMachine.CurrentState is PlayerSlashState) || (_playerStateMachine.CurrentState is PlayerDashState)) && _slashBuffered) {
                DesiredSlash = true;
            }
        }

        private void _handleSlashInput(bool startingSlash) {
            if(((_playerStateMachine.CurrentState is PlayerSlashState) || (_playerStateMachine.CurrentState is PlayerDashState)) &&  !_slashBuffered && startingSlash) {
                _slashBuffered = true;
                SlashBufferCounter = _playerStatsController.CurrentPlayerStats.SlashBufferTime;
            }
            DesiredSlash = startingSlash;
        }

        public IEnumerator Slash() {
            SlashEvent?.Invoke(_playerStatsController.CurrentPlayerStats.SlashDamage, _playerStatsController.CurrentPlayerStats.SlashSpeed, _playerStatsController.CurrentPlayerStats.SlashRange);
            
            _slashBuffered = false;

            _increaseSlashCombo();
            SlashComboCounter = _playerStatsController.CurrentPlayerStats.ComboTime;

            float cooldown = _playerStatsController.CurrentPlayerStats.SlashCooldown;
            if(_playerStatsController.weapon.m_CardData.id == 5) {
                cooldown = _playerStatsController.CurrentPlayerStats.DiffusionCooldown;
            }

            SlashCooldownCounter = cooldown;
            CanSlash = false;

            Collider2D[] hits = _checkSlashCollisions();
            if(hits.Length != 0) {
                foreach(Collider2D hit in hits) {
                    if(hit) {
                        if(hit.transform.TryGetComponent(out IHittable hittable)) {
                            var damage = _playerStatsController.CurrentPlayerStats.SlashDamage;
                            if(_playerStatsController.weapon.m_CardData.id == 5) {
                                damage = _playerStatsController.CurrentPlayerStats.DiffusionDamage;
                            }

                            bool crit = UnityEngine.Random.Range(1, 101) <= _playerStatsController.CurrentPlayerStats.critChance;

                            damage *= crit ? 3 : 1;

                            HitEvent?.Invoke(hittable, damage, this.transform.position.x > hit.transform.position.x ? 1 : -1);
                        }
                    }
                }
            }

            float speed = _playerStatsController.CurrentPlayerStats.SlashSpeed;
            if(_playerStatsController.weapon.m_CardData.id == 5) {
                speed = _playerStatsController.CurrentPlayerStats.DiffusionSpeed;
            }
            
            yield return new WaitForSeconds(speed);

            FinishSlashEvent?.Invoke();

            DesiredSlash = false;
        }

        public void ResetComboCounter() {
            SlashComboCounter = 0;
        }

        private Collider2D[] _checkSlashCollisions() {
            PlayerData.ColliderBounds _colliderBoundsSource = _playerMovementController.GetColliderBounds();
            Vector2 boundsPosition = (_playerMovementController.FacingDirection == 1 ? _colliderBoundsSource.slashRightRect.position : _colliderBoundsSource.slashLeftRect.position) * transform.localScale;
            
            if(_playerStatsController.weapon.m_CardData.id == 5) {
                boundsPosition = (_playerMovementController.FacingDirection == 1 ? _colliderBoundsSource.diffusionRightRect.position : _colliderBoundsSource.diffusionLeftRect.position) * transform.localScale;
            }

            var size = (_playerMovementController.FacingDirection == 1 ? _colliderBoundsSource.slashRightRect.size : _colliderBoundsSource.slashLeftRect.size);

            if(_playerStatsController.weapon.m_CardData.id == 5) {
                size = (_playerMovementController.FacingDirection == 1 ? _colliderBoundsSource.diffusionRightRect.size : _colliderBoundsSource.diffusionLeftRect.size);
            }
            
            Vector2 charPosition = transform.position;

            return Physics2D.OverlapBoxAll(charPosition + boundsPosition,
                size, 0, Layers.HittableLayer);
        }

        private void _increaseSlashCombo() {
            CurrentSlashComboAttack ++; // Increase Combo, Reset If Finished
            if(CurrentSlashComboAttack > 1) {
                CurrentSlashComboAttack = 0;
            }

            if(SlashComboCounter <= 0f) { // Reset Combo
                CurrentSlashComboAttack = 0;
            }
        }

        private void _onHit(IHittable hittable, int damage, int direction) {
            hittable.OnHit(this.Player, damage, -direction);
            
            // GameObject hitEffect = Instantiate(_slashHitPrefab, hittable.GetSlashEffectPosition(), Quaternion.identity);

            // if(CurrentSlashComboAttack == 1) {
            //         Vector3 scale = hitEffect.transform.localScale;
            //         scale.y *= -1;
            //         hitEffect.transform.localScale = scale;
            // }

            // Destroy(hitEffect, _slashHitEffectTime);

            _emitSlashParticle(hittable.GetSlashEffectPosition());

            CameraManager.Instance.CameraShaker.Shake(_slashHitEffectShakeDuration, _slashHitEffectShakeMagnitude);
        }

        private void _emitSlashParticle(Vector3 position) {

            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startSize = 2.5f;
            emitParams.startLifetime = 0.25f;
            emitParams.velocity = new Vector3(0f, 0f, 0f);
            //emitParams.position = position;
            
            emitParams.position = new Vector3(position.x, position.y, 0f);

            if(_playerMovementController.FacingDirection == -1) {
                emitParams.rotation = 245f;
            }

            // if(CurrentSlashComboAttack == 1) {
            //     emitParams.rotation += (_playerMovementController.FacingDirection == 1) ? 75f : -75f;
            // }

            // Debug.Log(emitParams.position.y);
            // Debug.Log(position.y);

            emitParams.applyShapeToPosition = true;

            _slashHitParticle.Emit(emitParams, 1);
        }
    }
}
