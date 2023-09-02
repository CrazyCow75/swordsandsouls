using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Player;

namespace Downpour
{
    public class Collector : MonoBehaviour, IInteractable
    {
        public int[] rewards = new int[15];

        public GameObject bingoBananza;

        public void OnInteract(Player player) {
            int collected = player.PlayerStatsController.currentCells  - 1;
            int floor = player.PlayerStatsController.collectedCells;

            if(collected == -1) {
                return;
            } else {
                int index = floor;

                for(int i = index; i < floor + collected; i++) {
                    Debug.Log(i + " Reward Collected");
                    player.PlayerStatsController.money += rewards[i];
                }
            }

            player.PlayerStatsController.currentCells = 0;

            player.PlayerStatsController.collectedCells += collected;
        }

        public bool CanInteract(Player player) {
            return true;
        }

        public string InteractText(Player player) {
            return "GIVE";
        }
    }
}
