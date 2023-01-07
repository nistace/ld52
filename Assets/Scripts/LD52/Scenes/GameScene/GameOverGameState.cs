using System.Collections;
using LD52.Data.Games;
using UnityEngine;
using Utils.Coroutines;

namespace LD52.Scenes.GameScene {
	public class GameOverGameState : AbstractGameState {
		public static GameOverGameState state { get; } = new GameOverGameState();
		private GameOverGameState() { }

		protected override void Enable() {
			ui.Show(GameUi.Panel.Outro);
			Debug.Log("GameOverGameState");
		}

		protected override void Disable() { }
	}
}