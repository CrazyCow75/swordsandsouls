using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class CritCard : Card
    {
        public CritCard(CardData _cardData) : base(_cardData) { }
        public override void whileActive(Player player) {
            return;
        }

        //on reveal
        public override void onUse(Player player) {
            return;
        }

        //dont think we need a reference to player here?

        public override PlayerData.PlayerStats getPlayerStatBuffs(PlayerData.PlayerStats playerStats, int level) {
            switch(level) {
                case 1:
                    playerStats.critChance += 15;
                    break;
                case 2:
                    playerStats.damageReduction += 25;
                    break;
                case 3:
                    playerStats.damageReduction += 35;
                    break;
                case 4:
                    playerStats.damageReduction += 45;
                    break;
                case 5:
                    playerStats.damageReduction += 60;
                    break;
            }
            
            return playerStats;
        }
    }
}
