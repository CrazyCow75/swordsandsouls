using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    public class Surfin : MonoBehaviour
    {   

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
            
        }
    }
}
