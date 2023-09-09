using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Downpour.Combat;
using Downpour;
using Downpour.UI;
using Downpour.Scenes;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(HealthSystem))]
    public class PlayerStatsController : PlayerComponent
    {
        public PlayerData.PlayerStats CurrentPlayerStats { get { return m_currentPlayerStats; } }
        private PlayerData.PlayerStats m_currentPlayerStats;

        private HealthSystem _healthSystem;

        public event Action<int> PlayerDamagedEvent;
        public event Action PlayerDeathEvent;

        public HashSet<int> UnlockedCards = new HashSet<int>();

        public Dictionary<int, int> CardLevels = new Dictionary<int, int>();

        public Card[] cards;

        public Card weapon;

        public bool Invincible;
        public float iframeTime;
        public float iframeCounter;

        public event Action RestEvent;

        public int collectedCells;
        public int currentCells;

        public int money;

        [SerializeField] private ParticleSystem _revengeParticle;

        protected override void Awake() {
            base.Awake();

            cards = new Card[3];

            _healthSystem = GetComponent<HealthSystem>();
            
            _updatePermanentBuffs();
            _updatePlayerStats();

            _healthSystem.SetMaxHealth(CurrentPlayerStats.MaxHealth);
            _healthSystem.ResetHealth();

            _healthSystem.DeathEvent += _invokeDeathEvent;
            _healthSystem.DamageEvent += _invokeDamageEvent;
        }

        private void Start() {
           // _healthSystem.SetHealth(DataManager.Instance.currentPlayerHealth);
        }

        private void Update() {
            if(iframeCounter > 0f) {
                iframeCounter -= Time.deltaTime;
                Invincible = true;
            } else {
                Invincible = false;
            }
        }

        public int getHealth() {
            return _healthSystem.CurrentHealthPoints;
        }

        private void _invokeDeathEvent() {
            PlayerDeathEvent?.Invoke();

            GameData g = DataManager.Instance.GameData;

            string aN = g.SpawnAreaName;
            int rN = g.SpawnRoomId;

            SceneLoader.Instance.LoadScene(aN + "" + rN, SceneLoader.FromSpawnPoint("RespawnPoint"));
        }

        private void _invokeDamageEvent(int damage) {
            PlayerDamagedEvent?.Invoke(damage);
        }

        public void TakeDamage(int damage) {
            if(Invincible) {
                return;
            }

            iframeCounter = iframeTime;

            bool dodge = UnityEngine.Random.Range(1, 101) <= _playerStatsController.CurrentPlayerStats.dodgeChance;

            // DataManager.Instance.currentPlayerHealth = getHealth();
            if(!dodge) {
                _healthSystem.TakeDamage(Mathf.Max(damage - m_currentPlayerStats.damageReduction, 1));

                
            }

            if(_playerStatsController.CurrentPlayerStats.revengeDamage != 0) {
                revenge();
            }
        }

        public void revenge() {
            PlayerData.ColliderBounds _colliderBoundsSource = _playerMovementController.GetColliderBounds();
            Vector2 boundsPosition = (_playerMovementController.FacingDirection == 1 ? _colliderBoundsSource.revengeRect.position : _colliderBoundsSource.revengeRect.position) * transform.localScale;
             var size = (_playerMovementController.FacingDirection == 1 ? _colliderBoundsSource.revengeRect.size : _colliderBoundsSource.revengeRect.size);

             Vector2 charPosition = transform.position;

            Collider2D[] hits = Physics2D.OverlapBoxAll(charPosition + boundsPosition,
                size, 0, Layers.HittableLayer);

                _emitRevengeParticle();

            if(hits.Length != 0) {
                foreach(Collider2D hit in hits) {
                    if(hit) {
                        if(hit.transform.TryGetComponent(out IHittable hittable)) {
                            var damage = _playerStatsController.CurrentPlayerStats.revengeDamage;

                            hittable.OnHit(this.Player, damage, this.transform.position.x > hit.transform.position.x ? 1 : -1);
                        }
                    }
                }
            }

        }

        private void _emitRevengeParticle() {

            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startSize = 0.33f;
            emitParams.startLifetime = 0.25f;
            emitParams.velocity = new Vector3(0f, 0f, 0f);
            //emitParams.position = position;
            
            emitParams.position = transform.position;

            // if(CurrentSlashComboAttack == 1) {
            //     emitParams.rotation += (_playerMovementController.FacingDirection == 1) ? 75f : -75f;
            // }

            // Debug.Log(emitParams.position.y);
            // Debug.Log(position.y);

            emitParams.applyShapeToPosition = true;

            _revengeParticle.Emit(emitParams, 1);
        }

        public PlayerData.PlayerStats _updatePlayerStats() {
            Debug.Log("UPDATE");
            m_currentPlayerStats = _playerData.BasePlayerStats;

            for(int i = 0; i < cards.Length; i++) { // Update based on cards
                Card c = cards[i];
                if(c != null) {
                    m_currentPlayerStats = c.getPlayerStatBuffs(m_currentPlayerStats, levelActual(getLevel(c.m_CardData.id)));
                }
            }
            // TODO: Update based on beads, buffs/debuffs
            _healthSystem.SetMaxHealth(m_currentPlayerStats.MaxHealth);

            return m_currentPlayerStats;
        }

        public void _updatePermanentBuffs() {
            // TODO: check for movement abilities, health upgrades, lighter upgrades, ranged, melee upgrades, mana upgrade
        }

        public int getLevel(int id) {
            int value = 0;
            CardLevels.TryGetValue(id, out value);
            return value;
        }

        public int levelActual(int value) {
            if(value < 3) {
                return 1;
            }else if(value < 8) {
                return 2;
            } else if(value < 14) {
                return 3;
            } else if(value < 20) {
                return 4;
            } else {
                return 5;
            }
        }

        public void setLevel(int id, int i) {
            try
            {
                CardLevels.Add(id, i);
            }
            catch (ArgumentException)
            {
                CardLevels[id] = i;
            }

            _updatePlayerStats();
        }

        public bool hasCardEquipped(Card c) {
            
            for(int i = 0; i < cards.Length; i++) {
                if(cards[i] == null) {
                    continue;
                }
                if(cards[i].m_CardData.id == c.m_CardData.id) {
                    return true;
                }
            }

            return false;
        }

        public void equipCard(Card c) {
            if(!c.m_CardData.isWeapon) {
                for(int i = 0; i < cards.Length; i++) {
                    if(cards[i] == null) {
                        continue;
                    }

                    if(cards[i].m_CardData.id == c.m_CardData.id) { // unequips
                        cards[i] = null;
                        _updatePlayerStats();
                        return;
                    }
                }

                if(!UnlockedCards.Contains(c.m_CardData.id)) {
                    return;
                }

                for(int i = 0; i < cards.Length; i++) {
                    if(cards[i] == null) {
                        
                        cards[i] = c;
                        //Debug.Log(i);
                        _updatePlayerStats();
                        return;
                    }
                }
            } else {
                if(!UnlockedCards.Contains(c.m_CardData.id)) {
                    return;
                }

                weapon = c;
                        //Debug.Log(i);
                        _updatePlayerStats();
                        return;
                
            }
        }
            
        

        public void unlockCard(CardData c) {
            
            UnlockedCards.Add(c.id);
            UIManager.Instance.inventory.refresh();

            if(!c.isWeapon) {

            if(getLevel(c.id) < 20) {
                setLevel(c.id, getLevel(c.id) + 1);
            }

            }
        }

        public void setHealth(int h) {
            _healthSystem.SetHealth(h);
        }

        public void Rest(Respawn rPoint) {
            RestEvent?.Invoke();
        }

        public void ResetHealth() {
            _healthSystem.ResetHealth();
        }
    }
}
