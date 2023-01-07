using LD52.Data.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LD52.Data.Games {
	public class BattlePlayingCardUi : MonoBehaviour {
		[SerializeField] protected FullCardUi _cardUi;
		[SerializeField] protected Button     _confirmButton;
		[SerializeField] protected TMP_Text   _messageLabel;
		[SerializeField] protected Button     _cancelButton;

		public enum ExpectedAction {
			CancelOnly,
			ConfirmOrCancel,
			SelectTargetOrCancel
		}

		public UnityEvent onConfirmClicked => _confirmButton.onClick;
		public UnityEvent onCancelClicked  => _cancelButton.onClick;

		public void Show(Card card, ExpectedAction expectedAction, string message = null) {
			_cardUi.Set(card);
			_cardUi.gameObject.SetActive(true);
			_confirmButton.gameObject.SetActive(expectedAction == ExpectedAction.ConfirmOrCancel);
			_messageLabel.text = message ?? string.Empty;
			_messageLabel.gameObject.SetActive(message != null);
			gameObject.SetActive(true);
		}

		public void Hide() => gameObject.SetActive(false);
	}
}