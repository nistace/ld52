using System.Collections;
using LD52.Data.Games;
using UnityEngine;
using Utils.Coroutines;

namespace LD52.Scenes.GameScene {
	public class PickNewCharacterGameState : AbstractGameState {
		public static PickNewCharacterGameState state { get; } = new PickNewCharacterGameState();
		private PickNewCharacterGameState() { }

		protected override void Enable() {
			ui.Show(GameUi.Panel.RecruitHero);
			// TODO RECRUIT HERO
			Debug.Log("PickNewCharacterGameState");
			CoroutineRunner.Run(AndContinue());
		}

		private static IEnumerator AndContinue() {
			yield return new WaitForSeconds(.5f);
			ChangeStateToNextOutroStep(ScenarioStepReward.Character);
		}

		protected override void Disable() { }
	}
}