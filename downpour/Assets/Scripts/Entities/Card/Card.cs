using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{

    using Downpour.Entity.Player;

    public class Card : PlayerComponent
    {


        public CardType cardType;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        //on going
        public void whileActive(Player player, CardType cardType) {
            switch(cardType) {
                
            }
        }

        //on reveal
        public void onUse(Player player, CardType cardType) {
            switch(cardType) {

            }
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
