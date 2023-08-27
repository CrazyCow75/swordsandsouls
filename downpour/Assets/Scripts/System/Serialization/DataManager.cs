using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Downpour.Scenes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Downpour
{
    public class DataManager : SingletonPersistent<DataManager>
    {
        public List<DataManager.RoomData> RoomDatas = new List<DataManager.RoomData>();

        public GameData GameData { get; private set; }

        public event Action SaveEvent;
        public event Action LoadEvent;
        protected override void Awake() {
            base.Awake();
            if(GameData == null) {
                GameData = new GameData();
            }
        }
        private void Start() {
            SceneLoader.Instance.BeforeSceneLoadEvent += AutoSave;
        }

        private void Update() {
            if(UnityEngine.Input.GetKeyDown(KeyCode.J)) {
                AutoSave();
            }
            if(UnityEngine.Input.GetKeyDown(KeyCode.H)) {
                Load();
            }
        }

        public void Save() {
            BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
                    + "/SwordsAndSoulsSave.dat"); 

            

            bf.Serialize(file, GameData);
            file.Close();

            SaveEvent?.Invoke();
            Debug.Log("Saved Data");
        }

        public void Load() {
            if (File.Exists(Application.persistentDataPath 
                    + "/SwordsAndSoulsSave.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = 
                        File.Open(Application.persistentDataPath 
                        + "/SwordsAndSoulsSave.dat", FileMode.Open);
                GameData data = (GameData)bf.Deserialize(file);

                file.Close();

                GameData = data;

                Debug.Log("Loaded Data");

                LoadEvent?.Invoke();
            } else {
                Debug.Log("No Save Data Found");
            }
        }

        public void AutoSave() {
            if(RoomManager.Instance != null) {
                RoomData r = new RoomData();
                r.firstTimeKill = RoomManager.Instance.FirstTimeKill;
                r.roomID = RoomManager.Instance.RoomNumber;
                r.areaName = RoomManager.Instance.AreaName;

                AddRoomData(r);
            }

            Save();
        }

        public void AddRoomData(RoomData r) {
            for(int i = 0; i < GameData.RoomDatas.Count; i++) {
                RoomData roomData = GameData.RoomDatas[i];
                if(roomData.areaName == r.areaName && roomData.roomID == r.roomID) { // Found Existing Room
                    roomData.firstTimeKill = r.firstTimeKill;

                    return;
                }
            }

            GameData.RoomDatas.Add(r);
        }

        [Serializable]
        public struct RoomData {
            public bool[] firstTimeKill;
            public int roomID;
            public string areaName;

        }
    }
}
