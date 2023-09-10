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

        public Image equippedWeapon;
        //public Image[] unlockedWeapons;
        public Image weaponBackground;

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

        public void Update() {
            if(GameManager.Instance.CurrentGameState == GameManager.GameState.Gameplay) {
                foreach(CardUI c in cardUIs) {
                    if(player.PlayerStatsController.UnlockedCards.Contains(c.c.m_CardData.id)) {
                        c.gameObject.SetActive(true);
                    } else {
                        c.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void onEquip() {
            player.PlayerStatsController.equipCard(selectedCard);
            _onCardSelect(selectedCard);

            Debug.Log("Equipped Weapon:" +  player.PlayerStatsController.weapon);

            int i = 0;

            foreach(Card c in player.PlayerStatsController.cards) {
                if(c == null) {
                    equipped[i].gameObject.SetActive(false);
                    equippedBG[i].gameObject.SetActive(false);
                    equippedLevel[i].gameObject.SetActive(false);
                } else {
                        equippedBG[i].gameObject.SetActive(true);
                        equippedLevel[i].gameObject.SetActive(true);
                        equipped[i].gameObject.SetActive(true);
                    equipped[i].sprite = c.m_CardData.image;

                    equippedBG[i].sprite = cardBackgrounds
                    [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];

                    equippedLevel[i].text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
                
                }
                i++;
            }
            if(!(player.PlayerStatsController.weapon == null))
                    equippedWeapon.sprite = player.PlayerStatsController.weapon.m_CardData.image;

            Debug.Log(player.PlayerStatsController.weapon.m_CardData.image);
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
                        equippedBG[i].gameObject.SetActive(false);
                        equippedLevel[i].gameObject.SetActive(false);
                    } else {
                        equippedBG[i].gameObject.SetActive(true);
                        equippedLevel[i].gameObject.SetActive(true);
                        equipped[i].gameObject.SetActive(true);
                        equipped[i].sprite = c.m_CardData.image;

                        equippedBG[i].sprite = cardBackgrounds
                        [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];
                        
                        equippedLevel[i].text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
                    
                    }
                    i++;
                }
                if(!(player.PlayerStatsController.weapon == null))
                    equippedWeapon.sprite = player.PlayerStatsController.weapon.m_CardData.image;

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
                            equippedBG[i].gameObject.SetActive(false);
                            equippedLevel[i].gameObject.SetActive(false);
                        } else {
                            equippedBG[i].gameObject.SetActive(true);
                            equippedLevel[i].gameObject.SetActive(true);
                            equipped[i].gameObject.SetActive(true);
                            equipped[i].sprite = c.m_CardData.image;

                            equippedBG[i].sprite = cardBackgrounds
                            [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];
                            
                            equippedLevel[i].text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
                        
                        }
                        i++;
                    }

                    if(!(player.PlayerStatsController.weapon == null))
                        equippedWeapon.sprite = player.PlayerStatsController.weapon.m_CardData.image;

                _onCardSelect(cardUIs[0].c);
            }
        }

        private void _onCardSelect(Card c) {
        
            cardImage.sprite = c.m_CardData.image;
            //Debug.Log(c);

            if(!c.m_CardData.isWeapon) {

            cardBg.sprite = cardBackgrounds
                        [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];
                        
            cardLevel.text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
            } else {
                cardBg.sprite = weaponBackground.sprite;
                cardLevel.text = "";
            }
            cardName.text = c.m_CardData.CardName;
            cardDesc.text = c.m_CardData.cardDesc;
            selectedCard = c;
            if(!c.m_CardData.isWeapon) {
                equipText.text = player.PlayerStatsController.hasCardEquipped(c) ? "UNEQUIP" : "EQUIP";
            } else {
                equipText.text = "EQUIP";
            }
        }

        private void registerCards() {
            cardUIs[0].c = new StrengthCard(cardDatas[0]);
            cardUIs[1].c = new MovementSpeedCard(cardDatas[1]);
            cardUIs[2].c = new SwingSpeedCard(cardDatas[2]);
            cardUIs[3].c = new HealthCard(cardDatas[3]);

            cardUIs[4].c = new HealthCard(cardDatas[4]);
            cardUIs[5].c = new HealthCard(cardDatas[5]);

            cardUIs[6].c = new CritCard(cardDatas[6]);
            cardUIs[7].c = new DodgeCard(cardDatas[7]);
            cardUIs[8].c = new RevengeCard(cardDatas[8]);
            cardUIs[9].c = new DefenseCard(cardDatas[9]);

            cardUIs[10].c = new DoubleJumpCard(cardDatas[10]);
            cardUIs[11].c = new RegenCard(cardDatas[11]);

            cardUIs[12].c = new HealthCard(cardDatas[12]);
            cardUIs[13].c = new HealthCard(cardDatas[13]);
        }
    }
}
