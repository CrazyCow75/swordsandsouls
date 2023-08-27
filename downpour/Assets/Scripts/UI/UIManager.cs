using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour.UI
{
    using Downpour.Input;
    public class UIManager : SingletonPersistent<UIManager>
    {
        public CardUIManager inventory;
        private void Start() {
            InputReader.Instance.OpenInventoryEvent += _onOpenInventory;
            
        }
        
        private void Update() {

        }

        private void _onOpenInventory(bool clicking) {
            if(clicking) {
                inventory.inventoryDisplay.SetActive(!inventory.inventoryDisplay.activeInHierarchy);
            }
        }
    }
}
