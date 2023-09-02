using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Downpour.Entity.Player;
namespace Downpour
{
    public class CardUI : MonoBehaviour
    {
        private Vector3 _originalScale;

        public event Action<Card> OnClick;

        public Card c;

        public Sprite[] cardBackgrounds;
        public Image cardBackground;

        public TextMeshProUGUI cardLevel;
        public Image image;

        private void Awake() {
            _originalScale = transform.localScale;
        }

        private void Update() {
            if(c!=null) {


                cardBackground.sprite = cardBackgrounds
                    [Player.Instance.PlayerStatsController.levelActual(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id)) - 1];
                cardLevel.text = Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id) + "/" + getCardsReq(Player.Instance.PlayerStatsController.getLevel(c.m_CardData.id));
                
                image.sprite = c.m_CardData.image;
            }
        }

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

        public void onClick() {
            OnClick?.Invoke(c);
        }

        public void upSize() {
            transform.localScale *= 1.2f;
        }

        public void downSize() {
            transform.localScale = _originalScale;
        }
    }
}
