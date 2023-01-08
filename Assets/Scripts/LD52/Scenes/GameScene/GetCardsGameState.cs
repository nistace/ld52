using LD52.Data.Cards;
using LD52.Data.Games;
using Utils.Extensions;

namespace LD52.Scenes.GameScene {
	public class GetCardsGameState : AbstractGameState {
		public static GetCardsGameState state { get; } = new GetCardsGameState();
		private GetCardsGameState() { }

		protected override void Enable() {
			ui.loot.Set(game.GenerateBoosters());
			ui.Show(GameUi.Panel.GetCards);

			CardZoomUi.SetEnabled(true);
			CardZoomUi.equippedInfoVisible = true;

			BoosterUi.onBoosterClicked.AddListenerOnce(GetCards);
		}

		private static void GetCards((Card first, Card second) booster) {
			(var first, var second) = booster;
			game.cardReserve.AddCard(first);
			game.cardReserve.AddCard(second);
			ChangeStateToNextOutroStep(ScenarioStepReward.Card);
		}

		protected override void Disable() {
			BoosterUi.onBoosterClicked.RemoveListener(GetCards);
			CardZoomUi.SetEnabled(false);
		}
	}
}