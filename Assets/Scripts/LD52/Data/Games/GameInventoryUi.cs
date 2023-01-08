using System;
using System.Collections.Generic;
using System.Linq;
using LD52.Data.Cards;
using LD52.Data.Characters.Heroes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD52.Data.Games {
	public class GameInventoryUi : MonoBehaviour {
		[SerializeField] protected HeroEquipmentUi[] _heroes;
		[SerializeField] protected CardReserveUi     _reserve;
		[SerializeField] protected SimpleCardUi      _draggedCard;
		[SerializeField] protected Button            _confirmButton;

		public UnityEvent onConfirmClicked => _confirmButton.onClick;

		private void Start() => HideDraggedCard();

		public void Set(IReadOnlyList<Hero> heroes, CardReserve reserve) {
			_heroes.ForEach((t, i) => t.Set(heroes.GetSafe(i)));
			_reserve.Set(reserve);
		}

		public void ShowReserveCardAsBeingDragged(int index) => _reserve.ShowCardAsBeingDragged(index);
		public void ShowCardAsBeingDragged(Hero hero, int index) => _heroes.First(t => t.hero == hero).ShowCardAsBeingDragged(index);

		public void ShowDraggedCard(Card card) {
			_draggedCard.Set(card, null);
			_draggedCard.gameObject.SetActive(true);
		}

		public void CancelShowingCardsAsBeingDragged() {
			_heroes.ForEach(t => t.CancelAllCardsBeingDragged());
			_reserve.CancelAllCardsBeingDragged();
		}

		public void HideDraggedCard() => _draggedCard.gameObject.SetActive(false);

		private void Update() => _draggedCard.transform.position = Input.mousePosition;
	}
}