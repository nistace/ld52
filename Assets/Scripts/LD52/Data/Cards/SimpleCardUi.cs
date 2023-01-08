using LD52.Data.Characters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.Ui;

namespace LD52.Data.Cards {
	public class SimpleCardUi : MonoBehaviourUi, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
		public class Event : UnityEvent<SimpleCardUi> { }

		[SerializeField] protected Card             _card;
		[SerializeField] protected GenericCharacter _cardOwner;
		[SerializeField] protected Image            _backgroundImage;
		[SerializeField] protected Image            _cardImage;

		public Event onClick { get; } = new Event();

		public static Event onCardHover     { get; } = new Event();
		public static Event onCardStopHover { get; } = new Event();

		public Card             card      => _card;
		public GenericCharacter cardOwner => _cardOwner;

		public Color color {
			set {
				_backgroundImage.color = value;
				_cardImage.color = value;
			}
		}

		public void Set(Card card, GenericCharacter owner) {
			_card = card;
			_cardOwner = owner;
			_cardImage.enabled = _card;
			if (!_card) return;
			_cardImage.sprite = _card.icon;
		}

		public void OnPointerEnter(PointerEventData eventData) => onCardHover.Invoke(this);
		public void OnPointerExit(PointerEventData eventData) => onCardStopHover.Invoke(this);

		public void OnPointerClick(PointerEventData eventData) => onClick.Invoke(this);
	}
}