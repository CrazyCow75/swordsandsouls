using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Player;
using System;

namespace Downpour
{
    public class Collectable : MonoBehaviour, IInteractable
    {
        public GameObject delete;

        public event Action<Collectable> CollectEvent;

        public void OnInteract(Player player) {
            player.PlayerStatsController.currentCells++;
            CollectEvent?.Invoke(this);
            Destroy(delete);
        }

        public bool CanInteract(Player player) {
            return true;
        }

        public string InteractText(Player player) {
            return "TAKE";
        }
    }
}
