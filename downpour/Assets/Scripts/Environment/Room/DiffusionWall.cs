using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Combat;
using Downpour.Entity.Player;

namespace Downpour
{
    public class DiffusionWall : MonoBehaviour
    {
        private HealthSystem _healthSystem;

        private void Awake() {
            _healthSystem = GetComponent<HealthSystem>();
        }

        public void Update() {
            if(Player.Instance.PlayerStatsController.weapon.m_CardData.id == 5) {
                _healthSystem.Invincible = false;
            } else {
                _healthSystem.Invincible = true;
            }
        }
    }
}
