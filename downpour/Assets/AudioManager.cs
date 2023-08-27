using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class AudioManager : SingletonPersistent<AudioManager>
    {

        private var allAudioSources : AudioSource[];


        // get all audiosources as an array
        void Awake() {
            allAudioSources = FindObjectsOfType(AudioSource) as AudioSource[];
        }
        
        
        public static void StopAllAudio() {
            for(var audioS : AudioSource in allAudioSources) {
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
