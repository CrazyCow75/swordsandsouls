using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class CardUIManager : MonoBehaviour
    {
        public CardUI[] cardUIs;
        public CardUI[] equipped;
        public CardData[] cardDatas;

        public Image cardImage;
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI cardDesc;

        public TextMeshProUGUI equipText;
        private Card selectedCard;

        private Player player;

        public void onEquip() {
            player.PlayerStatsController.equipCard(selectedCard);
            _onCardSelect(selectedCard);
        }
        private void Awake() {
            foreach(CardUI c in cardUIs) {
                c.OnClick += _onCardSelect;
            }

            registerCards();
        }

        private void Start() {
            player = Player.Instance;
        }

        private void _onCardSelect(Card c) {
            cardImage.sprite = c.m_CardData.image;
            cardName.text = c.m_CardData.CardName;
            cardDesc.text = c.m_CardData.cardDesc;
            selectedCard = c;
            
            equipText.text = player.PlayerStatsController.hasCardEquipped(c) ? "UNEQUIP" : "EQUIP";
        }

        private void registerCards() {
            cardUIs[0].c = new StrengthCard(cardDatas[0]);
        }
    }
}
