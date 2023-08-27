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
        public Image[] equipped;
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

            int i = 0;

            foreach(Card c in player.PlayerStatsController.cards) {
                if(c == null) {
                    equipped[i].gameObject.SetActive(false);
                } else {
                    equipped[i].gameObject.SetActive(true);
                    equipped[i].sprite = c.m_CardData.image;
                }
                i++;
            }
        }
        private void Awake() {
            foreach(CardUI c in cardUIs) {
                c.OnClick += _onCardSelect;
            }

            registerCards();
            
        }

        private void Start() {
            player = Player.Instance;

            selectedCard = cardUIs[0].c;
            
            int i = 0;

            foreach(Card c in player.PlayerStatsController.cards) {
                if(c == null) {
                    equipped[i].gameObject.SetActive(false);
                } else {
                    equipped[i].gameObject.SetActive(true);
                    equipped[i].sprite = c.m_CardData.image;
                }
                i++;
            }
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
            cardUIs[1].c = new MovementSpeedCard(cardDatas[1]);
            cardUIs[2].c = new SwingSpeedCard(cardDatas[2]);
        }
    }
}
