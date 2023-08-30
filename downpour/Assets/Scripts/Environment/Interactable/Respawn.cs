using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Scenes;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class Respawn : MonoBehaviour, IInteractable
    {
        [SerializeField] private Transform respawnPoint;

        private void Start() {
            if(SceneLoader.Instance.currentSceneTransitionData.spawnPoint == "RespawnPoint") {
                Player.Instance.PlayerStatsController.Rest(this);
            }
        }

        public void OnInteract(Player player) {
            player.PlayerStatsController.Rest(this);
            // Destroy(delete);
        }

        public bool CanInteract(Player player) {
            return true;
        }

        public string InteractText(Player player) {
            return "REST";
        }
    }
}
