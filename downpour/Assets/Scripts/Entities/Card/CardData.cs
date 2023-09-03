using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{
    [CreateAssetMenu(menuName = "Card Data")]
    public class CardData : ScriptableObject
    {
        public string CardName;
        public int id;
        public Sprite image;

        public bool isWeapon = false;

        public string cardDesc;
    }
}
