using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Scenes;

namespace Downpour
{
    public class GameInitialization : MonoBehaviour
    {
        private bool _init = false;
        private void Update() {
            if(!_init) {
                SceneLoader.Instance.LoadMainMenu();
                _init = true;
            }
        }
    }
}
