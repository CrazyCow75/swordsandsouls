using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Enemy;
using System;
using Downpour.UI;
using Downpour.Entity.Player;
using Downpour.Scenes;

namespace Downpour
{
    public class RoomManager : Singleton<RoomManager>
    {
        [field: SerializeField] public int RoomNumber { get; private set; }
        [field: SerializeField] public string AreaName { get; private set; }

        public bool[] FirstTimeKill;
        public bool[] Killed;
        public Enemy[] Enemies;

        public bool[] Broken;
        public BreakableWall[] BreakableWalls;


        public bool[] Looted;
        public Coins[] CoinPiles;

        public Transform[] SpawnPoints;

        public SceneReference thisScene;

        public bool[] Collected;
        public Collectable[] Collectables;

        protected override void Awake() {
            base.Awake();
        }

        private void Start() {
            SceneLoader.Instance.BeforeSceneLoadEvent += _disableInput;
            Player.Instance.PlayerStatsController.RestEvent += OnRest;

            Enemies = FindObjectsOfType<Enemy>() as Enemy[];
            System.Array.Sort(Enemies, (a, b) => a.name.CompareTo(b.name));

            FirstTimeKill = new bool[Enemies.Length];
            Killed = new bool[Enemies.Length];

            BreakableWalls = FindObjectsOfType<BreakableWall>() as BreakableWall[];
            System.Array.Sort(BreakableWalls, (a, b) => a.name.CompareTo(b.name));

            Broken = new bool[BreakableWalls.Length];

            CoinPiles = FindObjectsOfType<Coins>() as Coins[];
            System.Array.Sort(CoinPiles, (a, b) => a.name.CompareTo(b.name));

            Looted = new bool[CoinPiles.Length];

            Collectables = FindObjectsOfType<Collectable>() as Collectable[];
            System.Array.Sort(Collectables, (a, b) => a.name.CompareTo(b.name));

            Collected = new bool[Collectables.Length];

            foreach(Enemy e in Enemies) {
                e.EnemyDeathEvent += _handleEnemyDeath;
            }

            foreach(BreakableWall e in BreakableWalls) {
                e.BreakableWallBreakEvent += _handleWallBreak;
            }

            foreach(Coins e in CoinPiles) {
                e.CoinDeathEvent += _handleCoinDeath;
            }

            foreach(Collectable e in Collectables) {
                e.CollectEvent += _handleCollectDeath;
            }

            DataManager.Instance.LoadEvent += _handleLoadEvent; // TODO move this to another script that is made for handeling loads
            DataManager.Instance.Load();


            //_handleLoadEvent();

            // TODO load firstime kill from save
        }

        public void OnRest() {
            DataManager.Instance.AutoSave();

            for(int i = 0; i < DataManager.Instance.GameData.RoomDatas.Count; i++) {
                DataManager.RoomData roomData = DataManager.Instance.GameData.RoomDatas[i];

                for(int j = 0; j < roomData.killed.Length; j++) {
                    roomData.killed[j] = false;
                }
            }

            Player.Instance.PlayerStatsController.ResetHealth();

            DataManager.Instance.GameData.SpawnAreaName = AreaName;
            DataManager.Instance.GameData.SpawnRoomId = RoomNumber;

            DataManager.Instance.Save();
        }

        private void _disableInput() {
            DataManager.Instance.LoadEvent -= _handleLoadEvent;
        }

        private void _handleLoadEvent() {
            Debug.Log(gameObject.name);
            GameData g = DataManager.Instance.GameData;

            for(int i = 0; i < g.RoomDatas.Count; i++) {
                DataManager.RoomData roomData = g.RoomDatas[i];

                if(roomData.areaName == AreaName && roomData.roomID == RoomNumber) { // Found Existing Room
                    FirstTimeKill = roomData.firstTimeKill;
                    Killed = roomData.killed;
                    Broken = roomData.broken;
                    Looted = roomData.looted;
                    

                    break;
                }
            }

            PlayerStatsController p = Player.Instance.PlayerStatsController;
            
            foreach(int i in g.UnlockedCards) {
                p.UnlockedCards.Add(i);
            }
            
            foreach(CardUI c in UIManager.Instance.inventory.GetComponent<CardUIManager>().cardUIs) {
                if(c.c.m_CardData.id == g.EquippedCard1) {
                    p.cards[0] = c.c;
                }
                if(c.c.m_CardData.id == g.EquippedCard2) {
                    p.cards[1] = c.c;
                }
                if(c.c.m_CardData.id == g.EquippedCard3) {
                    p.cards[2] = c.c;
                }
            }

            p.money = g.money;

            p.CardLevels = g.CardLevels;

            foreach(Transform t in SpawnPoints) {
                if(t == null) {
                    Debug.Log(t);
                    continue;
                }
                if(t.gameObject.name == SceneLoader.Instance.currentSceneTransitionData.spawnPoint) {
                   
                    Player.Instance.transform.position = t.position;
                }
            }

            Player.Instance.PlayerStatsController.setHealth(g.PlayerHealth);

            Player.Instance.PlayerStatsController.collectedCells = g.collectedCells;
            Player.Instance.PlayerStatsController.currentCells = g.currentCells;

            for(int i = 0; i < BreakableWalls.Length; i++) {
                if(Broken[i]) {
                    Destroy(BreakableWalls[i].gameObject);
                }
            }

            for(int i = 0; i < CoinPiles.Length; i++) {
                if(Looted[i]) {
                    Destroy(CoinPiles[i].gameObject);
                }
            }

            for(int i = 0; i < Killed.Length; i++) {
                if(Killed[i]) {
                    Destroy(Enemies[i].gameObject);
                }
            }

            for(int i = 0; i < Collectables.Length; i++) {
                if(Collected[i]) {
                    Destroy(Collectables[i].gameObject);
                }
            }

            UIManager.Instance.inventory.refresh();

            Player.Instance.PlayerStatsController._updatePlayerStats();

             if(SceneLoader.Instance.currentSceneTransitionData.spawnPoint == "RespawnPoint") {
                        OnRest();
                    }
        }

        private void _handleEnemyDeath(Enemy e) {
            int i = 0;
            foreach(Enemy _e in Enemies) {
                if(_e == null) {
                    i++;
                    continue;
                }
                if(_e.gameObject.GetInstanceID() == e.gameObject.GetInstanceID()) {
                    Debug.Log("Found Enemy");

                    if(!FirstTimeKill[i]) {
                        if(e.soul != null)
                            Instantiate(e.soul, e.soulSpawnPoint.position, Quaternion.identity);
                    }

                    FirstTimeKill[i] = true;

                    Killed[i] = true;

                    Player.Instance.PlayerStatsController.money += _e.moneyDrop;

                    return;
                }
                i++;
            }
            
        }

        private void _handleWallBreak(BreakableWall e) {
            int i = 0;
            foreach(BreakableWall _e in BreakableWalls) {
                if(_e == null) {
                    i++;
                    continue;
                }
                if(_e.gameObject.GetInstanceID() == e.gameObject.GetInstanceID()) {
                    // Debug.Log("Found Enemy");

                    Broken[i] = true;

                    return;
                }
                i++;
            }
            
        }

        private void _handleCoinDeath(Coins e) {
            int i = 0;
            foreach(Coins _e in CoinPiles) {
                if(_e == null) {
                    i++;
                    continue;
                }
                if(_e.gameObject.GetInstanceID() == e.gameObject.GetInstanceID()) {
                    // Debug.Log("Found Enemy");

                    Looted[i] = true;
                    Player.Instance.PlayerStatsController.money += 300;

                    return;
                }
                i++;
            }
            
        }

        private void _handleCollectDeath(Collectable e) {
            int i = 0;
            foreach(Collectable _e in Collectables) {
                if(_e == null) {
                    i++;
                    continue;
                }
                if(_e.gameObject.GetInstanceID() == e.gameObject.GetInstanceID()) {
                    // Debug.Log("Found Enemy");

                    Collected[i] = true;

                    return;
                }
                i++;
            }
            
        }
        
        // TODO: add room serialization
    }
}
