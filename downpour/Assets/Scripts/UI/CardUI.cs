using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Downpour
{
    public class CardUI : MonoBehaviour
    {
        private Vector3 _originalScale;

        public event Action<Card> OnClick;

        public Card c;

        public Image image;

        private void Awake() {
            _originalScale = transform.localScale;
        }

        private void Update() {
            if(c!=null) {
                image.sprite = c.m_CardData.image;
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
