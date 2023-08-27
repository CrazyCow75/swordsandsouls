using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Scenes;

namespace Downpour
{
    public class RoomExit : MonoBehaviour
    {
        [SerializeField] private SceneReference sceneReference;

        [field: SerializeField] private string _spawnPoint;

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Player")) {
                SceneLoader.Instance.LoadScene(sceneReference, SceneLoader.FromSpawnPoint(_spawnPoint));
            }
        }
    }
}
