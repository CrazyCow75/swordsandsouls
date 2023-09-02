using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class HealthCard : Card
    {
        public HealthCard(CardData _cardData) : base(_cardData) { }
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
                    playerStats.MaxHealth += 10;
                    break;
                case 2:
                    playerStats.MaxHealth += 25;
                    break;
                case 3:
                    playerStats.MaxHealth += 50;
                    break;
                case 4:
                    playerStats.MaxHealth += 75;
                    break;
                case 5:
                    playerStats.MaxHealth += 100;
                    break;
            }
            
            return playerStats;
        }
    }
}
