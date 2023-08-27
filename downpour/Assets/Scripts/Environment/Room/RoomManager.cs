using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Downpour.Entity.Enemy;
using System;

namespace Downpour
{
    public class RoomManager : Singleton<RoomManager>
    {
        [field: SerializeField] public int RoomNumber { get; private set; }
        [field: SerializeField] public string AreaName { get; private set; }

        public bool[] FirstTimeKill;
        public Enemy[] Enemies;

        

        private void Start() {
            Enemies = FindObjectsOfType<Enemy>() as Enemy[];
            System.Array.Sort(Enemies, (a, b) => a.name.CompareTo(b.name));

            FirstTimeKill = new bool[Enemies.Length];

            foreach(Enemy e in Enemies) {
                e.EnemyDeathEvent += _handleEnemyDeath;
            }

            DataManager.Instance.LoadEvent += _handleLoadEvent;
            DataManager.Instance.Load();
            

            // TODO load firstime kill from save
        }

        private void _handleLoadEvent() {
            GameData g = DataManager.Instance.GameData;

            for(int i = 0; i < g.RoomDatas.Count; i++) {
                DataManager.RoomData roomData = g.RoomDatas[i];

                if(roomData.areaName == AreaName && roomData.roomID == RoomNumber) { // Found Existing Room
                    FirstTimeKill = roomData.firstTimeKill;

                    return;
                }
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

                    FirstTimeKill[i] = true;
                }
                i++;
            }
            
        }
        
        // TODO: add room serialization
    }
}
