using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class SwingSpeedCard : Card
    {
        public SwingSpeedCard(CardData _cardData) : base(_cardData) { }
        public override void whileActive(Player player) {
            return;
        }

        //on reveal
        public override void onUse(Player player) {
            return;
        }
        public override PlayerData.PlayerStats getPlayerStatBuffs(PlayerData.PlayerStats playerStats, int level) {
            switch(level) {
                case 1:
                    playerStats.SlashSpeed *= 0.9f;
                    playerStats.SlashCooldown *= 0.9f;
                    break;
                case 2:
                    playerStats.SlashSpeed *= 0.75f;
                    playerStats.SlashCooldown *= 0.75f;
                    break;
                case 3:
                    playerStats.SlashSpeed *= 0.6f;
                    playerStats.SlashCooldown *= 0.6f;
                    break;
                case 4:
                    playerStats.SlashSpeed *= 0.5f;
                    playerStats.SlashCooldown *= 0.5f;
                    break;
                case 5:
                    playerStats.SlashSpeed *= 0.4f;
                    playerStats.SlashCooldown *= 0.4f;
                    break;
            }
            
            return playerStats;
        }
    }
}
