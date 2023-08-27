using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour;
using Downpour.Input;

namespace Downpour.Entity.Player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerInteractableController : PlayerComponent
    {
        private InteractablePrompt _interactablePrompt;

        IInteractable _interactable;
        
        protected override void Awake() {
            base.Awake();
        }
        protected void Start() {
            _interactablePrompt = InteractablePrompt.Instance;
            InputReader.Instance.PickupEvent += _handlePickupInput;
        }
        private void OnTriggerEnter2D(Collider2D other) {
            if(other.CompareTag("Interactable")) {
                if(other.transform.TryGetComponent(out IInteractable interactable)) {
                    _interactablePrompt.DisplayInteractablePrompt(interactable.InteractText(this.Player), 
                        new Vector2(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 2f));

                
                    _interactable = interactable;
                    
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if(other.CompareTag("Interactable")) {
                if(other.transform.TryGetComponent(out IInteractable interactable)) {
                    _interactablePrompt.DisableInteractivePrompt();
                    
                    _interactable = null;
                }
            }
        }

        private void _handlePickupInput(bool pickup) {
            if(!pickup || _interactable == null) {
                return;
            }

            if(_interactable.CanInteract(this.Player)) {
                _interactable.OnInteract(this.Player);
                _interactablePrompt.DisableInteractivePrompt();
            }
            
        }
    }
}
