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
            playerStats.SlashSpeed *= 0.85f;
            playerStats.SlashCooldown *= 0.85f;
            return playerStats;
        }
    }
}
