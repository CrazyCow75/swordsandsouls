using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class MovementSpeedCard : Card
    {
        public MovementSpeedCard(CardData _cardData) : base(_cardData) { }
        public override void whileActive(Player player) {
            return;
        }

        //on reveal
        public override void onUse(Player player) {
            return;
        }

        //dont think we need a reference to player here?

        //other note; im not using ur overriden method thing bc i think its handled differently for movement
        public override PlayerData.PlayerStats getPlayerStatBuffs(PlayerData.PlayerStats playerStats) {
            if(Player.Instance.PlayerMovementController.Grounded)
                playerStats.MoveSpeed *= 1.25f;
            return playerStats;
        }
    }
}
