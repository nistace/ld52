using LD52.Data.Cards;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utils.Extensions;

namespace LD52.Data.Games {
	public class CardReserveUi : MonoBehaviour {
		public class DragAndDropEvent : UnityEvent<int, GameObject> { }

		[SerializeField] protected SimpleCardUi[] _cards;

		private CardReserve reserve { get; set; }

		public static DragAndDropEvent onDrag { get; } = new DragAndDropEvent();
		public static DragAndDropEvent onDrop { get; } = new DragAndDropEvent();

		private void Start() {
			_cards.ForEach(t => {
				t.onPointerDown.AddListenerOnce(HandlePointerDownOnCard);
				t.onPointerUp.AddListenerOnce(HandlePointerUpOnCard);
			});
		}

		private void HandlePointerUpOnCard(SimpleCardUi card, RaycastResult result) => onDrop.Invoke(_cards.IndexOf(card), result.gameObject);
		private void HandlePointerDownOnCard(SimpleCardUi card, RaycastResult result) => onDrag.Invoke(_cards.IndexOf(card), result.gameObject);

		public void Set(CardReserve reserve) {
			this.reserve?.onChanged.RemoveListener(Refresh);
			this.reserve = reserve;
			Refresh();
			this.reserve.onChanged.AddListenerOnce(Refresh);
		}

		private void Refresh() {
			for (var i = 0; i < _cards.Length; ++i) {
				_cards[i].gameObject.SetActive(reserve.count > i);
				if (reserve.count > i) {
					_cards[i].Set(reserve[i], null);
				}
			}
		}

		public void CancelAllCardsBeingDragged() => _cards.ForEach(t => t.color = Color.white);
		public void ShowCardAsBeingDragged(int index) => _cards[index].color = new Color(1, 1, 1, .5f);
	}
}