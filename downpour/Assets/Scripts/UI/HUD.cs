using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Downpour.Entity.Player;

namespace Downpour
{
    public class HUD : MonoBehaviour
    {
        public Slider healthBar;
        void Update()
        {
            if(Player.Instance != null) {
                healthBar.value = Player.Instance.PlayerStatsController.getHealth() / Player.Instance.PlayerStatsController.CurrentPlayerStats.MaxHealth;
            }
        }
    }
}
