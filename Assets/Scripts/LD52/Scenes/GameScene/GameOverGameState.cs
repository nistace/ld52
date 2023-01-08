using LD52.Data.Games;
using UnityEngine.SceneManagement;

namespace LD52.Scenes.GameScene {
	public class GameOverGameState : AbstractGameState {
		public static GameOverGameState state { get; } = new GameOverGameState();
		private GameOverGameState() { }

		protected override void Enable() {
			ui.Show(GameUi.Panel.Outro);
			GameOverController.won = game.IsScenarioEnded();
			SceneManager.LoadScene("GameOver");
		}

		protected override void Disable() { }
	}
}