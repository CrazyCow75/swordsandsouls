using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Downpour.Combat;
using Downpour;

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

        public Card[] cards;

        public bool Invincible;
        public float iframeTime;
        public float iframeCounter;

        protected override void Awake() {
            base.Awake();

            cards = new Card[3];
            
            _updatePermanentBuffs();
            _updatePlayerStats();

            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.SetMaxHealth(CurrentPlayerStats.MaxHealth);
            _healthSystem.ResetHealth();

            _healthSystem.DeathEvent += _invokeDeathEvent;
            _healthSystem.DamageEvent += _invokeDamageEvent;
        }

        private void Update() {
            if(iframeCounter > 0f) {
                iframeCounter -= Time.deltaTime;
                Invincible = true;
            } else {
                Invincible = false;
            }
        }

        private void _invokeDeathEvent() {
            PlayerDeathEvent?.Invoke();
        }

        private void _invokeDamageEvent(int damage) {
            PlayerDamagedEvent?.Invoke(damage);
        }

        public void TakeDamage(int damage) {
            if(Invincible) {
                return;
            }

            iframeCounter = iframeTime;

            _healthSystem.TakeDamage(damage);
        }

        private PlayerData.PlayerStats _updatePlayerStats() {
            m_currentPlayerStats = _playerData.BasePlayerStats;

            for(int i = 0; i < cards.Length; i++) { // Update based on cards
                Card c = cards[i];
                if(c != null) {
                    m_currentPlayerStats = c.getPlayerStatBuffs(m_currentPlayerStats);
                }
            }
            // TODO: Update based on beads, buffs/debuffs
            return m_currentPlayerStats;
        }

        private void _updatePermanentBuffs() {
            // TODO: check for movement abilities, health upgrades, lighter upgrades, ranged, melee upgrades, mana upgrade
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
                    Debug.Log(i);
                    _updatePlayerStats();
                    return;
                }
            }

            
        }

        public void unlockCard(CardData c) {
            UnlockedCards.Add(c.id);
        }

    }
}
