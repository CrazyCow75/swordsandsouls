using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace Downpour
{
    [Serializable]
    public partial class GameData
    {
        
        public List<DataManager.RoomData> RoomDatas = new List<DataManager.RoomData>();
        
        public List<int> UnlockedCards = new List<int>();

        public int EquippedCard1;
        public int EquippedCard2;
        public int EquippedCard3;

        public int PlayerHealth;
    }
}
