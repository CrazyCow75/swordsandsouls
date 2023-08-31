using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Scenes;

namespace Downpour
{
    public class MainMenu : MonoBehaviour
    {
        public void Play() {
            if(!DataManager.Instance.Load()) {
                DataManager.Instance.FreshSave();
            }

            Debug.Log(DataManager.Instance.GameData.SpawnAreaName);

            SceneLoader.Instance.LoadScene(DataManager.Instance.GameData.SpawnAreaName + "" + DataManager.Instance.GameData.SpawnRoomId, SceneLoader.FromSpawnPoint("RespawnPoint"));
        }

        public void Settings() {
            // TODO: later
        }

        public void Quit() {
            Application.Quit();
        }
    }
}
