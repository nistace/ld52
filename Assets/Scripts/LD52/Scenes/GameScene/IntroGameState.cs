using System.Collections;
using LD52.Data.Games;
using UnityEngine;
using Utils.Coroutines;

namespace LD52.Scenes.GameScene {
	public class IntroGameState : AbstractGameState {
		public static IntroGameState state { get; } = new IntroGameState();
		private IntroGameState() { }

		protected override void Enable() {
			ui.Show(GameUi.Panel.Intro);
			// TODO INTRO
			CoroutineRunner.Run(WaitAndStartBattle());
		}

		private static IEnumerator WaitAndStartBattle() {
			yield return new WaitForSeconds(.5f);
			ChangeState(game.GetCurrentScenarioStep().equipmentStep ? EquipHeroesGameState.state : BattleGameState.state);
		}

		protected override void Disable() { }
	}
}