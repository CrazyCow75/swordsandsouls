using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class DoubleJumpCard : Card
    {
        public DoubleJumpCard(CardData _cardData) : base(_cardData) { }
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
                    playerStats.HasDoubleJump = true;
                    break;
                case 2:
                    playerStats.HasDoubleJump = true;
                    playerStats.CoyoteTime += 0.1f;
                    playerStats.JumpHeight += 2;
                    break;
                case 3:
                    playerStats.HasDoubleJump = true;
                    playerStats.HasAirDash = true;

                    playerStats.CoyoteTime += 0.1f;
                    playerStats.JumpHeight += 2;
                    break;
                case 4:
                    playerStats.HasDoubleJump = true;
                    playerStats.HasAirDash = true;

                    playerStats.CoyoteTime += 0.1f;
                    playerStats.DashCooldown -= 0.05f;
                    playerStats.JumpHeight += 1;
                    break;
                case 5:
                    playerStats.HasDoubleJump = true;
                    playerStats.HasAirDash = true;

                    playerStats.CoyoteTime += 0.1f;
                    playerStats.JumpHeight += 2;
                    playerStats.DashCooldown -= 0.1f;
                    break;
            }
            
            return playerStats;
        }
    }
}
