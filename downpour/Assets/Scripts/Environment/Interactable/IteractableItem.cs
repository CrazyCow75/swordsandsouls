using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Player;

namespace Downpour
{
    public class IteractableItem : MonoBehaviour, IInteractable
    {
        [SerializeField] private int _itemId;

        public GameObject delete;

        public CardData cardData;
        public void OnInteract(Player player) {
            player.PlayerStatsController.unlockCard(cardData);
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
