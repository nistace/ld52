using System.Collections;
using LD52.Data.Games;
using UnityEngine;
using Utils.Coroutines;

namespace LD52.Scenes.GameScene {
	public class EquipHeroesGameState : AbstractGameState {
		public static EquipHeroesGameState state { get; } = new EquipHeroesGameState();
		private EquipHeroesGameState() { }

		protected override void Enable() {
			ui.Show(GameUi.Panel.EquipHeroes);
			Debug.Log("EquipHeroesGameState");
			CoroutineRunner.Run(AndContinue());
		}

		private static IEnumerator AndContinue() {
			yield return new WaitForSeconds(.5f);
			ChangeState(BattleGameState.state);
		}

		protected override void Disable() { }
	}
}