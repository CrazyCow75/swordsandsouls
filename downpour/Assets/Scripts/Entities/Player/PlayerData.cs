using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Downpour.Entity.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptables/Entity/Player Data")]
    public class PlayerData : ScriptableObject
    {
        #if UNITY_EDITOR
        public bool DrawGizmos;
        #endif
        [Serializable]
        public class ColliderBounds {
            public Rect bounds;
            public Rect feetRect;
            public Rect slashRightRect;
            public Rect slashLeftRect;

            public Rect diffusionRightRect;
            public Rect diffusionLeftRect;

            public Rect handRightRect;
            public Rect handLeftRect;

            public Rect revengeRect;
        }

        [field: SerializeField] public ColliderBounds StandColliderBounds { get; private set; }

        [field: SerializeField] public PlayerStats BasePlayerStats { get; private set; }

        [Serializable]
        public struct PlayerStats {
            [field: SerializeField] public int MaxHealth;
            [field: SerializeField] public int MaxSpirit;

            [field: SerializeField, Range(0f, 100f)] public float MoveSpeed;

            [field: SerializeField, Range(0f, 10f)] public float JumpHeight;

            public float MinJumpTime;
            [field: SerializeField, Range(0f, 100f)] public float MaxFallSpeed;
            [field: SerializeField, Range(0f, 1f)] public float CoyoteTime;
            [field: SerializeField, Range(0f, 0.5f)] public float JumpBufferTime;
            [field: SerializeField] public bool HasDoubleJump;


            // what's the 4 there for?? where are these variables getting initialized??
            public int SlashDamage => (int)(4 * damageMultiplier);
            public int DiffusionDamage => (int)(6 * damageMultiplier);
            // [field: SerializeField, Range(0, 4)] public int SlashLevel;
            // [field: SerializeField] public int[] BaseSlashDamageValues { get; private set; }
            [field: SerializeField] public float SlashSpeed;
            [field: SerializeField] public float SlashCooldown;
            [field: SerializeField] public float SlashRange;
            [field: SerializeField] public float ComboTime;
            [field: SerializeField] public float SlashBufferTime;

            [field: SerializeField] public float SlashKnockbackMultiplier;
            [field: SerializeField] public float SlashKnockbackTime;

            [field: SerializeField] public float DiffusionSpeed;
            [field: SerializeField] public float DiffusionCooldown;
            [field: SerializeField] public float BulletSpeed;
            [field: SerializeField] public float BulletCooldown;
            [field: SerializeField] public float BombSpeed;
            [field: SerializeField] public float BombCooldown;
            [field: SerializeField] public int BulletDamage  => (int)(2 * damageMultiplier);
            [field: SerializeField] public int BombDamageMin => (int)(1 * damageMultiplier);
            [field: SerializeField] public int BombDamageMax => (int)(20 * damageMultiplier);

            [field: SerializeField] public float DiffusionKnockbackMultiplier;
            [field: SerializeField] public float DiffusionKnockbackTime;

            [field: SerializeField] public float DashCooldown;
            [field: SerializeField] public float DashSpeed;
            [field: SerializeField] public float DashLength;
            [field: SerializeField] public float DashBufferTime;
            [field: SerializeField] public bool HasAirDash;

            public float damageMultiplier; 
            public int damageReduction;

            public int revengeDamage;
            public int critChance;
            public int dodgeChance;
            public int regen;
        }
    }
}
