using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class DodgeCard : Card
    {
        public DodgeCard(CardData _cardData) : base(_cardData) { }
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
                    playerStats.dodgeChance += 10;
                    break;
                case 2:
                    playerStats.dodgeChance += 20;
                    break;
                case 3:
                    playerStats.dodgeChance += 30;
                    break;
                case 4:
                    playerStats.dodgeChance += 40;
                    break;
                case 5:
                    playerStats.dodgeChance += 50;
                    break;
            }
            
            return playerStats;
        }
    }
}
