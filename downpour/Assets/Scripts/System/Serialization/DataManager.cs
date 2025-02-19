using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Downpour.Scenes;
using System.IO;
using Downpour.Entity.Player;
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

        private void OnApplicationQuit() {
            AutoSave();
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
                    + "/NeonBlackoutSave.dat"); 

            bf.Serialize(file, GameData);
            file.Close();

            SaveEvent?.Invoke();
            Debug.Log("Saved Data");
        }

        public bool Load() {
            if (File.Exists(Application.persistentDataPath 
                    + "/NeonBlackoutSave.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = 
                        File.Open(Application.persistentDataPath 
                        + "/NeonBlackoutSave.dat", FileMode.Open);
                GameData data = (GameData)bf.Deserialize(file);

                file.Close();

                GameData = data;

                Debug.Log("Loaded Data");

                LoadEvent?.Invoke();

                return true;
            } else {
                Debug.Log("No Save Data Found");

                return false;
            }
        }

        public void AutoSave() {
            if(!(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay)) {
                // Save();
                return;
            }
            
            if(RoomManager.Instance != null) {
                RoomData r = new RoomData();
                r.firstTimeKill = RoomManager.Instance.FirstTimeKill;
                r.killed = RoomManager.Instance.Killed;
                r.broken = RoomManager.Instance.Broken;
                r.looted = RoomManager.Instance.Looted;
                r.collected = RoomManager.Instance.Collected;

                r.roomID = RoomManager.Instance.RoomNumber;
                r.areaName = RoomManager.Instance.AreaName;

                AddRoomData(r);
            }
            
            foreach(int i in Player.Instance.PlayerStatsController.UnlockedCards) {
                GameData.UnlockedCards.Add(i);
            }

            if(Player.Instance.PlayerStatsController.cards[0] != null) {
                GameData.EquippedCard1 = Player.Instance.PlayerStatsController.cards[0].m_CardData.id;
            } else {
                GameData.EquippedCard1 = -1;
            }
            if(Player.Instance.PlayerStatsController.cards[1] != null) {
                GameData.EquippedCard2 = Player.Instance.PlayerStatsController.cards[1].m_CardData.id;
            } else {
                GameData.EquippedCard2 = -1;
            }
            if(Player.Instance.PlayerStatsController.cards[2] != null) {
                GameData.EquippedCard3 = Player.Instance.PlayerStatsController.cards[2].m_CardData.id;
            } else {
                GameData.EquippedCard3 = -1;
            }

            if(Player.Instance.PlayerStatsController.weapon != null) {
                GameData.EquippedWeapon = Player.Instance.PlayerStatsController.weapon.m_CardData.id;
            } else {
                GameData.EquippedWeapon = -1;
            }

            GameData.CardLevels = Player.Instance.PlayerStatsController.CardLevels;

            GameData.PlayerHealth = Player.Instance.PlayerStatsController.getHealth();

            GameData.collectedCells = Player.Instance.PlayerStatsController.collectedCells;
            GameData.currentCells = Player.Instance.PlayerStatsController.currentCells;
            

            GameData.money = Player.Instance.PlayerStatsController.money;

            Save();
        }

        public void FreshSave() {
            GameData g = new GameData();
            g.SpawnAreaName = "OuterCity";
            g.SpawnRoomId = 0;
            g.PlayerHealth = 100;
            GameData = g;
            g.EquippedCard1 = -1;
            g.EquippedCard2 = -1;
            g.EquippedCard3 = -1;
            g.UnlockedCards.Add(0);
            g.UnlockedCards.Add(1);
            g.UnlockedCards.Add(2);
            g.UnlockedCards.Add(3);
            g.UnlockedCards.Add(4);

            //g.UnlockedCards.Add(5); // TEMP
            
            g.EquippedWeapon = 4;
            Debug.Log(GameData.SpawnAreaName);
            Save();
        }

        public void AddRoomData(RoomData r) {
            for(int i = 0; i < GameData.RoomDatas.Count; i++) {
                RoomData roomData = GameData.RoomDatas[i];
                if(roomData.areaName == r.areaName && roomData.roomID == r.roomID) { // Found Existing Room
                    roomData.firstTimeKill = r.firstTimeKill;
                    roomData.killed = r.killed;
                    roomData.broken = r.broken;
                    roomData.looted = r.looted;
                    roomData.collected = r.collected;
                    

                    return;
                }
            }

            GameData.RoomDatas.Add(r);
        }

        

        [Serializable]
        public struct RoomData {
            public bool[] firstTimeKill;
            public bool[] killed;
            public bool[] broken;
            public bool[] looted;
            public int roomID;

public bool[] collected;
            public string areaName;
        }
    }
}
