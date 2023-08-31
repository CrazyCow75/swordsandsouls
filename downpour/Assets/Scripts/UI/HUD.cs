using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Downpour.Entity.Player;
using TMPro;

namespace Downpour
{
    public class HUD : MonoBehaviour
    {
        public Slider healthBar;
        public TextMeshProUGUI money;
        void Update()
        {
            if(Player.Instance != null) {
                //Debug.Log( Player.Instance.PlayerStatsController.getHealth() / (float)Player.Instance.PlayerStatsController.CurrentPlayerStats.MaxHealth) ;
                healthBar.value = (float)(Player.Instance.PlayerStatsController.getHealth()) / Player.Instance.PlayerStatsController.CurrentPlayerStats.MaxHealth;
                money.text = "$:" + Player.Instance.PlayerStatsController.money;
            }
        }
    }
}
