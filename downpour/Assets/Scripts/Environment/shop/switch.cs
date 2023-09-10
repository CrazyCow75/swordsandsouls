using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class Switch : MonoBehaviour, IInteractable
    {
        public CardData regen;
        public CardData bullet;
        public int price;
        public Transform spawnPos;

        public bool canInteract;

        public void OnInteract(Player player) {
            if(player.PlayerStatsController.money >= price) {
                player.PlayerStatsController.money -= price;

                if(player.PlayerStatsController.UnlockedCards.Contains(13)) {
                    player.PlayerStatsController.unlockCard(regen);
                } else {
                    player.PlayerStatsController.unlockCard(bullet);
                }
            }
        }


        public bool CanInteract(Player player) {
            return true;
        }

        public string InteractText(Player player) {
            return "PAY " + price;
        }
    }
}
