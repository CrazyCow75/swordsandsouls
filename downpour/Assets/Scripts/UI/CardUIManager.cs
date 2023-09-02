using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace Downpour
{
    using Downpour.Entity.Player;
    public class CardUIManager : MonoBehaviour
    {
        public CardUI[] cardUIs;
        public Image[] equipped;
        public Image[] equippedBG;
        public TextMeshProUGUI[] equippedLevel;
        public CardData[] cardDatas;

        public Sprite[] cardBackgrounds;

        public Image cardImage;
        public Image cardBg;
        public TextMeshProUGUI cardLevel;
        public TextMeshProUGUI cardName;
        public TextMeshProUGUI cardDesc;

        public TextMeshProUGUI equipText;
        private Card selectedCard;

        private Player player;

        public GameObject inventoryDisplay;
         public int getCardsReq(int value) {
            if(value < 3) {
                return 3;
            }else if(value < 8) {
                return 8;
            } else if(value < 14) {
                return 14;
            } else if(value < 20) {
                return 20;
            } else {
                return 20;
            }
        }

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

                    equippedBG[i].sprite = cardBackgrounds
                    [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];

                    equippedLevel[i].text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
                
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
            GameManager.Instance.sceneLoaded += onInit;
        }

        private void onInit(Scene scene, LoadSceneMode s) {
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay) {

            
                Debug.Log("!!!!!");
                player = Player.Instance;

                // selectedCard = cardUIs[0].c;
                
                int i = 0;

                 foreach(Card c in player.PlayerStatsController.cards) {
                    if(c == null) {
                        equipped[i].gameObject.SetActive(false);
                    } else {
                        equipped[i].gameObject.SetActive(true);
                        equipped[i].sprite = c.m_CardData.image;

                        equippedBG[i].sprite = cardBackgrounds
                        [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];
                        
                        equippedLevel[i].text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
                    
                    }
                    i++;
                }

                _onCardSelect(cardUIs[0].c);
            }
        }

        public void refresh() {
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay) {
                player = Player.Instance;
                int i = 0;

                 foreach(Card c in player.PlayerStatsController.cards) {
                        if(c == null) {
                            equipped[i].gameObject.SetActive(false);
                        } else {
                            equipped[i].gameObject.SetActive(true);
                            equipped[i].sprite = c.m_CardData.image;

                            equippedBG[i].sprite = cardBackgrounds
                            [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];
                            
                            equippedLevel[i].text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
                        
                        }
                        i++;
                    }

                _onCardSelect(cardUIs[0].c);
            }
        }

        private void _onCardSelect(Card c) {
            cardImage.sprite = c.m_CardData.image;

            cardBg.sprite = cardBackgrounds
                        [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];
                        
            cardLevel.text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));

            cardName.text = c.m_CardData.CardName;
            cardDesc.text = c.m_CardData.cardDesc;
            selectedCard = c;
            
            equipText.text = player.PlayerStatsController.hasCardEquipped(c) ? "UNEQUIP" : "EQUIP";
        }

        private void registerCards() {
            cardUIs[0].c = new StrengthCard(cardDatas[0]);
            cardUIs[1].c = new MovementSpeedCard(cardDatas[1]);
            cardUIs[2].c = new SwingSpeedCard(cardDatas[2]);
            cardUIs[3].c = new HealthCard(cardDatas[3]);
        }
    }
}
