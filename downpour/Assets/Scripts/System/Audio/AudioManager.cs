using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class AudioManager : MonoBehaviour
    {

        //slider reference: AudioManager.Instance.s

        void Awake() {

        }
        
        AudioSource[] allAudioSources;

        // Start is called before the first frame update
        void Start()
        {
        
             //i have no idea why i need the <> - something to do with audiosource like not being a type or something??? idk but it compiles with no errors :skull:

            allAudioSources = FindObjectsOfType<AudioSource>();

            //allAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
            
        }

        public void StopAllAudio() {
            foreach(AudioSource audioS in allAudioSources) {
                audioS.Stop();
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
