using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour;
using System;
using Downpour.Scenes;
using Downpour.Input;
using UnityEngine.SceneManagement;

namespace Downpour {
    public class GameManager : SingletonPersistent<GameManager>
    {   
        public enum GameState {
            Menu,
            Gameplay
        }

        public GameState CurrentGameState { get; private set; }

        public event Action<Scene, LoadSceneMode> sceneLoaded;

        protected override void Awake() { // temp
            base.Awake();
            CurrentGameState = GameState.Menu;

            
        }

        // Initialization requiring other scripts.
        private void Start() {
            InputReader.Instance.EnableGameplayInput();

            // Physics2D.IgnoreLayerCollision(8, 9);
            SceneManager.sceneLoaded += onInit;
        }

        // public void ChangeToGameState() {
        //     CurrentGameState = GameState.Gameplay;
        // }

        public void onInit(Scene scene, LoadSceneMode mode) {
            if(SceneLoader.Instance.currentSceneTransitionData.spawnPoint != "MainMenu") {
                CurrentGameState = GameState.Gameplay;
            } else {
                CurrentGameState = GameState.Menu;
            }

            Debug.Log("Event Invoked");

            sceneLoaded?.Invoke(scene, mode);
        }
    }
}
