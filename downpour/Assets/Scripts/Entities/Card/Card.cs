using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downpour
{

    using Downpour.Entity.Player;

    public abstract class Card
    {
        public CardData m_CardData;

        //on going
        public abstract void whileActive(Player player);

        //on reveal
        public abstract void onUse(Player player);

        public abstract PlayerData.PlayerStats getPlayerStatBuffs(PlayerData.PlayerStats playerStats);
    }
}
