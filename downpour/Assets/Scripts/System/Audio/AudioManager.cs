using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class AudioManager : MonoBehaviour
    {
        //i have no idea why i need the <> - something to do with audiosource like not being a type or something??? idk but it compiles with no errors :skull:

        private AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();


        void Awake() {
            allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        }
        
        
        public void StopAllAudio() {
            foreach(AudioSource audioS in allAudioSources) {
                audioS.Stop();
            }
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
