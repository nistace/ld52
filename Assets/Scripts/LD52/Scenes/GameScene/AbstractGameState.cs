using LD52.Data.Games;
using Utils.Extensions;
using Utils.GameStates;

namespace LD52.Scenes.GameScene {
	public abstract class AbstractGameState : GameState {
		public static Game   game { get; set; }
		public static GameUi ui   { get; set; }

		private static ScenarioStepReward[] rewardSequence { get; } = { ScenarioStepReward.Character, ScenarioStepReward.Card };

		protected static void ChangeStateToNextOutroStep(ScenarioStepReward? previousStep) {
			ScenarioStepReward? nextStep = null;
			var previousStepIndex = previousStep.HasValue ? rewardSequence.IndexOf(previousStep.Value) : -1;
			for (var i = previousStepIndex + 1; nextStep == null && i < rewardSequence.Length; ++i) {
				if (game.GetCurrentScenarioStep().rewards.HasFlag(rewardSequence[i])) {
					nextStep = rewardSequence[i];
				}
			}

			if (nextStep == ScenarioStepReward.Character) ChangeState(RecruitHeroGameState.state);
			else if (nextStep == ScenarioStepReward.Card) ChangeState(GetCardsGameState.state);
			else {
				game.EndCurrentScenarioStep();
				if (game.IsScenarioEnded()) ChangeState(GameOverGameState.state);
				else if (game.GetCurrentScenarioStep().equipmentStep) ChangeState(EquipHeroesGameState.state);
				else ChangeState(BattleGameState.state);
			}
		}
	}
}