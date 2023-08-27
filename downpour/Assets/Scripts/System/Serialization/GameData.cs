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
        
    }
}
