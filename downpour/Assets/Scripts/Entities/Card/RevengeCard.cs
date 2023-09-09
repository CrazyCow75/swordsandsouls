using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class RevengeCard : Card
    {
        public RevengeCard(CardData _cardData) : base(_cardData) { }
        public override void whileActive(Player player) {
            return;
        }

        //on reveal
        public override void onUse(Player player) {
            return;
        }

        //dont think we need a reference to player here?

        //other note; im not using ur overriden method thing bc i think its handled differently for movement
        public override PlayerData.PlayerStats getPlayerStatBuffs(PlayerData.PlayerStats playerStats, int level) {
            switch(level) {
                case 1:
                    playerStats.revengeDamage += 4;
                    break;
                case 2:
                    playerStats.revengeDamage += 5;
                    break;
                case 3:
                    playerStats.revengeDamage += 6;
                    break;
                case 4:
                    playerStats.revengeDamage += 8;
                    break;
                case 5:
                    playerStats.revengeDamage += 10;
                    break;
            }
            
                
            return playerStats;
        }
    }
}
