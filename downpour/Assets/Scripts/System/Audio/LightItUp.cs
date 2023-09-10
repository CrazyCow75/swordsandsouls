using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class LightItUp : MonoBehaviour
    {

        public AudioSource a;

        public void PlayLightItUp() {
            a.volume = GetComponent<AudioSource>().volume;
            a.Play();
        }

        public void OnSliderChange(float newValue)
        {
            a.volume = newValue;
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
