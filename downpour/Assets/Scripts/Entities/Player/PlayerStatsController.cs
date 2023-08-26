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
        public PlayerData.PlayerStats CurrentPlayerStats { get { return _updatePlayerStats(); } }
        private PlayerData.PlayerStats m_currentPlayerStats;

        private HealthSystem _healthSystem;

        public event Action<int> PlayerDamagedEvent;
        public event Action PlayerDeathEvent;

        public CardType[] ownedCards;

        public Card[] cards;

        protected override void Awake() {
            base.Awake();
            
            _updatePermanentBuffs();

            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.SetMaxHealth(CurrentPlayerStats.MaxHealth);
            _healthSystem.ResetHealth();

            _healthSystem.DeathEvent += _invokeDeathEvent;
            _healthSystem.DamageEvent += _invokeDamageEvent;
        }

        private void _invokeDeathEvent() {
            PlayerDeathEvent?.Invoke();
        }

        private void _invokeDamageEvent(int damage) {
            PlayerDamagedEvent?.Invoke(damage);
        }

        private PlayerData.PlayerStats _updatePlayerStats() {
            m_currentPlayerStats = _playerData.BasePlayerStats;

            foreach(Card c in cards) { // Update based on cards
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
    }
}
