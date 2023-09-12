using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class RegenCard : Card
    {
        public RegenCard(CardData _cardData) : base(_cardData) { }
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
                    playerStats.regen += 1;
                    break;
                case 2:
                    playerStats.regen += 1;
                    break;
                case 3:
                    playerStats.regen += 2;
                    break;
                case 4:
                    playerStats.regen += 2;
                    break;
                case 5:
                    playerStats.regen += 3;
                    break;
            }
            
            return playerStats;
        }
    }
}
