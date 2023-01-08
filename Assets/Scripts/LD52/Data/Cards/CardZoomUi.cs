using System.Collections;
using UnityEngine;
using Utils.Extensions;

namespace LD52.Data.Cards {
	public class CardZoomUi : MonoBehaviour {
		private static CardZoomUi instance { get; set; }

		public static bool equippedInfoVisible {
			set => instance._cardUi.SetEquippedInfoVisible(value);
		}

		private static bool instanceEnabled { get; set; }

		[SerializeField] protected SimpleCardUi _hoveredCard;
		[SerializeField] protected FullCardUi   _cardUi;

		public static void SetEnabled(bool enabled) => instanceEnabled = enabled;

		private void Start() {
			instance = this;
			SimpleCardUi.onAnyCardHover.AddListenerOnce(HandleCardOver);
			SimpleCardUi.onAnyCardStopHover.AddListenerOnce(HandleCardStopOver);
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

		private IEnumerator ShowCard() {
			if (!instanceEnabled) yield break;
			if (!_hoveredCard) yield break;
			_cardUi.Set(_hoveredCard.card, _hoveredCard.cardOwner);
			_cardUi.gameObject.SetActive(true);
			Cursor.visible = false;

			while (_hoveredCard && instanceEnabled) {
				_cardUi.transform.position = Input.mousePosition;
				yield return null;
			}
			_cardUi.gameObject.SetActive(false);
			Cursor.visible = true;
		}
	}
}