using System.Collections;
using LD52.Data.Games;
using UnityEngine;
using Utils.Coroutines;

namespace LD52.Scenes.GameScene {
	public class GetCardsGameState : AbstractGameState {
		public static GetCardsGameState state { get; } = new GetCardsGameState();
		private GetCardsGameState() { }

		protected override void Enable() {
			ui.Show(GameUi.Panel.GetCards);
			// TODO GET CARDS
			Debug.Log("GetCardsGameState");
			CoroutineRunner.Run(AndContinue());
		}

		private static IEnumerator AndContinue() {
			yield return new WaitForSeconds(.5f);
			ChangeStateToNextOutroStep(ScenarioStepReward.Card);
		}

		protected override void Disable() { }
	}
}