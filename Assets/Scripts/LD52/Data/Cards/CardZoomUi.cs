﻿using System.Collections;
using LD52.Assets;
using UnityEngine;
using Utils.Extensions;

namespace LD52.Data.Cards {
	public class CardZoomUi : MonoBehaviour {
		private static CardZoomUi instance { get; set; }

		public static bool instanceEnabled {
			get => instance.enabled;
			set => instance.enabled = value;
		}

		[SerializeField] protected Card       _hoveredCard;
		[SerializeField] protected FullCardUi _cardUi;

		private void Start() {
			instance = this;
			SimpleCardUi.onCardHover.AddListenerOnce(HandleCardOver);
			SimpleCardUi.onCardStopHover.AddListenerOnce(HandleCardStopOver);
			instanceEnabled = false;
		}

		private void HandleCardOver(Card card) {
			_hoveredCard = card;
			StopAllCoroutines();
			StartCoroutine(ShowCard());
		}

		private void HandleCardStopOver(Card card) {
			if (card != _hoveredCard) return;
			_hoveredCard = null;
		}

		private void OnEnable() {
			StopAllCoroutines();
			if (_hoveredCard) StartCoroutine(ShowCard());
		}

		private IEnumerator ShowCard() {
			if (!_hoveredCard) yield break;
			_cardUi.Set(_hoveredCard);
			_cardUi.gameObject.SetActive(true);

			while (_hoveredCard) {
				_cardUi.transform.position = Input.mousePosition;
				yield return null;
			}
			_cardUi.gameObject.SetActive(false);
		}

		private void OnDisable() {
			_cardUi.gameObject.SetActive(false);
		}
	}
}