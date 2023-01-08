using LD52.Data.Cards;
using LD52.Data.Characters.Heroes;
using LD52.Data.Games;
using UnityEngine;
using Utils.Extensions;

namespace LD52.Scenes.GameScene {
	public class EquipHeroesGameState : AbstractGameState {
		public static EquipHeroesGameState state { get; } = new EquipHeroesGameState();
		private EquipHeroesGameState() { }

		protected override void Enable() {
			ui.inventory.Set(game.playerHeroes, game.cardReserve);
			ui.Show(GameUi.Panel.EquipHeroes);

			CardZoomUi.SetEnabled(true);
			CardZoomUi.equippedInfoVisible = true;

			HeroEquipmentUi.onDrag.AddListenerOnce(HandleHeroCardDragged);
			HeroEquipmentUi.onDrop.AddListenerOnce(HandleHeroCardDropped);
			CardReserveUi.onDrag.AddListenerOnce(HandleReserveCardDragged);
			CardReserveUi.onDrop.AddListenerOnce(HandleReserveCardDropped);
			ui.inventory.onConfirmClicked.AddListenerOnce(HandleInventoryClosed);
		}

		private static void HandleInventoryClosed() => ChangeState(BattleGameState.state);

		private static void HandleReserveCardDragged(int index, GameObject currentRaycastResult) {
			ui.inventory.ShowReserveCardAsBeingDragged(index);
			ui.inventory.ShowDraggedCard(game.cardReserve[index]);
			CardZoomUi.SetEnabled(false);
		}

		private static void HandleHeroCardDragged(Hero hero, int index, GameObject currentRaycastResult) {
			if (hero.IsInitialCard(index)) return;
			ui.inventory.ShowCardAsBeingDragged(hero, index);
			ui.inventory.ShowDraggedCard(hero.deck.allCards[index]);
			CardZoomUi.SetEnabled(false);
		}

		private void HandleReserveCardDropped(int index, GameObject currentRaycastResult) {
			if (currentRaycastResult.TryGetComponentInParent<CardReserveUi>(out _)) { }
			else if (currentRaycastResult.TryGetComponentInParent<SimpleCardUi>(out var droppedOnSlot) && currentRaycastResult.TryGetComponentInParent<HeroEquipmentUi>(out var heroBar)) {
				var card = game.cardReserve.Take(index);
				heroBar.hero.OverrideCard(heroBar.GetIndexOf(droppedOnSlot), card, out var removedCard);
				if (removedCard) game.cardReserve.AddCard(removedCard);
			}
			EndDragAndDrop();
		}

		private static void EndDragAndDrop() {
			ui.inventory.CancelShowingCardsAsBeingDragged();
			ui.inventory.HideDraggedCard();
			CardZoomUi.SetEnabled(true);
		}

		private void HandleHeroCardDropped(Hero hero, int index, GameObject currentRaycastResult) {
			if (hero.IsInitialCard(index)) { }
			else if (currentRaycastResult.TryGetComponentInParent<CardReserveUi>(out _)) {
				game.cardReserve.AddCard(hero.RemoveOverridingCard(index));
			}
			else if (currentRaycastResult.TryGetComponentInParent<SimpleCardUi>(out var droppedOnSlot) && currentRaycastResult.TryGetComponentInParent<HeroEquipmentUi>(out var heroBar)) {
				var hero1Card = hero.RemoveOverridingCard(index);
				heroBar.hero.OverrideCard(heroBar.GetIndexOf(droppedOnSlot), hero1Card, out var hero2Card);
				if (hero2Card) hero.OverrideCard(index, hero2Card, out _);
			}
			EndDragAndDrop();
		}

		protected override void Disable() {
			CardZoomUi.SetEnabled(false);

			HeroEquipmentUi.onDrag.RemoveListener(HandleHeroCardDragged);
			HeroEquipmentUi.onDrop.RemoveListener(HandleHeroCardDropped);
			CardReserveUi.onDrag.RemoveListener(HandleReserveCardDragged);
			CardReserveUi.onDrop.RemoveListener(HandleReserveCardDropped);
			ui.inventory.onConfirmClicked.RemoveListener(HandleInventoryClosed);
		}
	}
}