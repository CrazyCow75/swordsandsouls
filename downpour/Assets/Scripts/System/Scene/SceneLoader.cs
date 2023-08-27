using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Downpour.Scenes
{
    public class SceneLoader : SingletonPersistent<SceneLoader>
    {
        public struct SceneTransitionData {
            public string spawnPoint;
            public GameData gameData;
            public SceneReference currentScene;
        }

        public struct SceneUnloadData {
            public GameData gameData;
            public SceneReference currentScene;
            public SceneReference nextScene;
        }

        public event Action SceneLoadEvent;
        public event Action BeforeSceneLoadEvent;
        [SerializeField] private SceneReference _mainMenu;

        public SceneReference activeScene { get; private set; }

        public static SceneTransitionData FromSpawnPoint(string spawnPoint) {
            return new SceneTransitionData() { spawnPoint = spawnPoint };
        }

        public void LoadMainMenu() {
            LoadScene(_mainMenu, FromSpawnPoint("MainMenu"));
        }
         private IEnumerator _doSceneLoad(SceneReference sceneReference, SceneTransitionData transitionData) {
            

            OnUnloadScene(sceneReference);
            // TODO: add load transition
            yield return null;
            SceneLoadEvent?.Invoke();
            SceneManager.LoadScene(sceneReference);
        }

        public void LoadScene(SceneReference sceneReference, SceneTransitionData transitionData) {
            if(sceneReference != null) {
                StartCoroutine(_doSceneLoad(sceneReference, transitionData));
            }
        }

        private void OnUnloadScene(SceneReference nextScene) {
            SceneUnloadData unloadData = new SceneUnloadData() {
                gameData = DataManager.Instance.GameData,
                currentScene = activeScene,
                nextScene = nextScene,
            };
            
            // TODO: Fade to black

            BeforeSceneLoadEvent?.Invoke();
        }
    }
}
