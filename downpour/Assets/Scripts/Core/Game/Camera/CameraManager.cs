using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Downpour.Entity.Player;
using Downpour.Scenes;
using UnityEngine.SceneManagement;

namespace Downpour
{
    public class CameraManager : SingletonPersistent<CameraManager>
    {
        public Camera MainCamera;
        public CameraShaker CameraShaker;
        public CinemachineVirtualCamera VCamera { get; private set; }
        public CinemachineConfiner2D VCameraConfines { get; private set; }

        protected override void Awake() {
            //Debug.Log("Awake");
            base.Awake();
            MainCamera = Camera.main;
            
            VCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            CameraShaker = GetComponentInChildren<CameraShaker>();
            VCameraConfines = GetComponentInChildren<CinemachineConfiner2D>();
        }

        protected void Start() {
            
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay) {
                VCamera.m_Follow = Player.Instance.transform;

                VCamera.PreviousStateIsValid = false;
                
                VCamera.ForceCameraPosition(Player.Instance.transform.position + new Vector3(0f, 2.75f, 0f), Quaternion.identity);

                VCamera.gameObject.SetActive(false);
                VCamera.gameObject.SetActive(true);

                
            }

            GameManager.Instance.sceneLoaded += onInit;
        }

        public void SetVCameraConfines(PolygonCollider2D collider2D) {
            VCameraConfines.m_BoundingShape2D = collider2D;
        }

        public void onInit(Scene scene, LoadSceneMode mode) {
            Debug.Log("INIT");
            VCamera.gameObject.SetActive(false);
            // Debug.Log(Player.Instance);
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay) {
                

                VCamera.m_Follow = Player.Instance.transform;

                VCamera.PreviousStateIsValid = false;
                
                //VCamera.transform.position = Player.Instance.transform.position + new Vector3(0f, 2.75f, 0f);
                
                

                //Debug.Log(VCamera.transform.position);

                //VCamera.m_Follow = Player.Instance.transform;
            }
            VCamera.gameObject.SetActive(true);
        }
    }
}
