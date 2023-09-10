using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class gacha : MonoBehaviour, IInteractable
    {
        public int[] ids;
        public GameObject[] prizes;
        public int price;
        public Transform spawnPos;

        public bool canInteract;

        public void OnInteract(Player player) {
            if(player.PlayerStatsController.money >= price) {
                player.PlayerStatsController.money -= price;

                bool found = false;

                while(!found) {
                    int index = Random.Range(0, ids.Length);

                    if(player.PlayerStatsController.UnlockedCards.Contains(ids[index])) {
                        Instantiate(prizes[index], spawnPos.position, Quaternion.identity);
                        
                        found = true;
                    }
                }
            }
        }

        public void startBuy() {
            canInteract = false;
        }

        public void endBuy() {
            canInteract = true;
        }

        public bool CanInteract(Player player) {
            return true;
        }

        public string InteractText(Player player) {
            return "PAY " + price;
        }
    }
}
