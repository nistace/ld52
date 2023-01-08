using System.Collections;
using UnityEngine;
using Utils.Extensions;

namespace LD52.Data.Cards {
	public class CardZoomUi : MonoBehaviour {
		private static CardZoomUi instance { get; set; }

		public static bool instanceEnabled {
			get => instance.enabled;
			set => instance.enabled = value;
		}

		[SerializeField] protected SimpleCardUi _hoveredCard;
		[SerializeField] protected FullCardUi   _cardUi;

		private void Start() {
			instance = this;
			SimpleCardUi.onCardHover.AddListenerOnce(HandleCardOver);
			SimpleCardUi.onCardStopHover.AddListenerOnce(HandleCardStopOver);
			instanceEnabled = false;
		}

		private void HandleCardOver(SimpleCardUi card) {
			_hoveredCard = card;
			StopAllCoroutines();
			StartCoroutine(ShowCard());
		}

		private void HandleCardStopOver(SimpleCardUi card) {
			if (_hoveredCard != card) return;
			_hoveredCard = null;
		}

		private void OnEnable() {
			StopAllCoroutines();
			if (_hoveredCard) StartCoroutine(ShowCard());
		}

		private IEnumerator ShowCard() {
			if (!_hoveredCard) yield break;
			_cardUi.Set(_hoveredCard.card, _hoveredCard.cardOwner);
			_cardUi.gameObject.SetActive(true);
			Cursor.visible = false;

			while (_hoveredCard) {
				_cardUi.transform.position = Input.mousePosition;
				yield return null;
			}
			_cardUi.gameObject.SetActive(false);
			Cursor.visible = true;
		}

		private void OnDisable() {
			_cardUi.gameObject.SetActive(false);
			Cursor.visible = true;
		}
	}
}