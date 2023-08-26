using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;

    public class StrengthCard : Card
    {
        public StrengthCard(CardData _cardData) : base(_cardData) { }
        public override void whileActive(Player player) {
            return;
        }

        //on reveal
        public override void onUse(Player player) {
            return;
        }
        public override PlayerData.PlayerStats getPlayerStatBuffs(PlayerData.PlayerStats playerStats) {
            playerStats.damageMultiplier *= 1.5f;
            return playerStats;
        }
    }
}
