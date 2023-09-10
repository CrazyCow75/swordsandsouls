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
                    playerStats.critChance += 25;
                    break;
                case 3:
                    playerStats.critChance += 35;
                    break;
                case 4:
                    playerStats.critChance += 45;
                    break;
                case 5:
                    playerStats.critChance += 60;
                    break;
            }
            
            return playerStats;
        }
    }
}
