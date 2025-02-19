using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Downpour.UI
{   
    using UnityEngine.UI;
    using Downpour.Input;
    public class UIManager : SingletonPersistent<UIManager>
    {   

        public Slider s;
        

        public CardUIManager inventory;
        public GameObject HUD;
        public GameObject MainMenu;

        public void Quit() {
            Application.Quit();
        }
        private void Start() {
            InputReader.Instance.OpenInventoryEvent += _onOpenInventory;
            GameManager.Instance.sceneLoaded += onInit;
        }
        
        private void Update() {

        }

        private void _onOpenInventory(bool clicking) {
            if(clicking && GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay) {
                inventory.inventoryDisplay.SetActive(!inventory.inventoryDisplay.activeInHierarchy);
                inventory.refresh();
            }
        }

        private void onInit(Scene scene, LoadSceneMode s) {
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Menu) {
                HUD.SetActive(false);
                MainMenu.SetActive(true);
            } else {
                HUD.SetActive(true);
                MainMenu.SetActive(false);
            }
        }
    }
}
