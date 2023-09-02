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
        public override PlayerData.PlayerStats getPlayerStatBuffs(PlayerData.PlayerStats playerStats, int level) {
            Debug.Log(level);
            switch(level) {
                case 1:
                    playerStats.damageMultiplier *= 1.5f;
                    break;
                case 2:
                    playerStats.damageMultiplier *= 1.65f;
                    break;
                case 3:
                    playerStats.damageMultiplier *= 1.8f;
                    break;
                case 4:
                    playerStats.damageMultiplier *= 2f;
                    break;
                case 5:
                    playerStats.damageMultiplier *= 2.5f;
                    break;
            }
            return playerStats;
        }
    }
}
