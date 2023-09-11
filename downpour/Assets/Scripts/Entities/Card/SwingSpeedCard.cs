using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class SwingSpeedCard : Card
    {
        public SwingSpeedCard(CardData _cardData) : base(_cardData) { }
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
                    playerStats.SlashSpeed *= 0.9f;
                    playerStats.SlashCooldown *= 0.9f;

                    playerStats.DiffusionSpeed *= 0.9f;
                    playerStats.DiffusionCooldown *= 0.9f;

                    playerStats.BulletSpeed *= 0.9f;
                    playerStats.BulletSpeed *= 0.9f;

                    playerStats.BombSpeed *= 0.9f;
                    playerStats.BombCooldown *= 0.9f;
                    break;
                case 2:
                    playerStats.SlashSpeed *= 0.8f;
                    playerStats.SlashCooldown *= 0.8f;

                    playerStats.DiffusionSpeed *= 0.8f;
                    playerStats.DiffusionCooldown *= 0.8f;

                    playerStats.BulletSpeed *= 0.8f;
                    playerStats.BulletSpeed *= 0.8f;

                    playerStats.BombSpeed *= 0.8f;
                    playerStats.BombCooldown *= 0.8f;
                    break;
                case 3:
                    playerStats.SlashSpeed *= 0.7f;
                    playerStats.SlashCooldown *= 0.7f;

                    playerStats.DiffusionSpeed *= 0.7f;
                    playerStats.DiffusionCooldown *= 0.7f;

                    playerStats.BulletSpeed *= 0.7f;
                    playerStats.BulletSpeed *= 0.7f;

                    playerStats.BombSpeed *= 0.7f;
                    playerStats.BombCooldown *= 0.7f;
                    break;
                case 4:
                    playerStats.SlashSpeed *= 0.6f;
                    playerStats.SlashCooldown *= 0.6f;

                    playerStats.DiffusionSpeed *= 0.6f;
                    playerStats.DiffusionCooldown *= 0.6f;

                    playerStats.BulletSpeed *= 0.6f;
                    playerStats.BulletSpeed *= 0.6f;

                    playerStats.BombSpeed *= 0.6f;
                    playerStats.BombCooldown *= 0.6f;
                    break;
                case 5:
                    playerStats.SlashSpeed *= 0.5f;
                    playerStats.SlashCooldown *= 0.5f;

                    playerStats.DiffusionSpeed *= 0.5f;
                    playerStats.DiffusionCooldown *= 0.5f;

                    playerStats.BulletSpeed *= 0.5f;
                    playerStats.BulletSpeed *= 0.5f;

                    playerStats.BombSpeed *= 0.5f;
                    playerStats.BombCooldown *= 0.5f;
                    break;
            }
            
            return playerStats;
        }
    }
}
