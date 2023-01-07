using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.Ui;

namespace LD52.Data.Cards {
	public class SimpleCardUi : MonoBehaviourUi, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
		public class Event : UnityEvent<SimpleCardUi> { }

		[SerializeField] protected Card  _card;
		[SerializeField] protected Image _cardImage;

		public Event onClick { get; } = new Event();

		public static Card.Event onCardHover     { get; } = new Card.Event();
		public static Card.Event onCardStopHover { get; } = new Card.Event();
		public static Card.Event onCardClicked   { get; } = new Card.Event();

		public Card card => _card;

		public void SetCard(Card card) {
			_card = card;
			_cardImage.enabled = _card;
			if (!_card) return;
			_cardImage.sprite = _card.icon;
		}

		public void OnPointerEnter(PointerEventData eventData) => onCardHover.Invoke(_card);
		public void OnPointerExit(PointerEventData eventData) => onCardStopHover.Invoke(_card);

		public void OnPointerClick(PointerEventData eventData) {
			onClick.Invoke(this);
			onCardClicked.Invoke(_card);
		}
	}
}