using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    using Downpour.Entity.Enemy;
    public class Surfin : MonoBehaviour
    {   
        public AudioClip titleScreen;
        public AudioClip junkyard;
        public AudioClip boss1;
        public AudioClip boss2;
        public AudioClip boss3;

        public AudioSource audioSource;

        public void playSurfin() {
            audioSource.Play();
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void OnSliderChange(float newValue)
        {
            GetComponent<AudioSource>().volume = newValue;
        }


        // Update is called once per frame
        void Update()
        {
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay) {
                if(RoomManager.Instance.AreaName == "NeonCity" && RoomManager.Instance.RoomNumber == 15) {
                        if(FindObjectOfType<MegaGoblin>().detectedPlayer) {
                            if(audioSource.clip != boss3) {
                            audioSource.clip = boss3;
                            audioSource.Play();
                            return;
                            }
                        return;
                    }
                }
                if(RoomManager.Instance.AreaName == "NeonCity" && RoomManager.Instance.RoomNumber == 13) {
                        if(FindObjectOfType<MegaChomper>().detectedPlayer) {
                            if(audioSource.clip != boss1) {
                            audioSource.clip = boss1;
                            audioSource.Play();
                            return;
                        }
                        return;
                    }
                    
                }
                if(RoomManager.Instance.AreaName == "Junkyard" && RoomManager.Instance.RoomNumber == 6) {
                        if(FindObjectOfType<BouncingBones>().detectedPlayer) {
                            if(audioSource.clip != boss2) {
                            audioSource.clip = boss2;
                            audioSource.Play();
                            return;
                        }
                        return;
                    }
                    
                }
                
                if(RoomManager.Instance.AreaName == "NeonCity" || RoomManager.Instance.AreaName == "OuterCity") {
                    if(audioSource.clip != titleScreen) {
                        audioSource.clip = titleScreen;
                        audioSource.Play();
                    }
                    
                }
                if(RoomManager.Instance.AreaName == "Junkyard") {
                    if(audioSource.clip != junkyard) {
                        audioSource.clip = junkyard;
                        audioSource.Play();
                    }
                    
                }
            }
        }
    }
}
