using LD52.Data.Games;
using UnityEngine;
using Utils.GameStates;

namespace LD52.Scenes.GameScene {
	public class GameController : MonoBehaviour {
		[SerializeField] protected GameData _gameData;
		[SerializeField] protected Game     _game;
		[SerializeField] protected GameUi   _gameUi;

		private void Start() {
			AbstractGameState.ui = _gameUi;
			_game = new Game(_gameData);
			AbstractGameState.game = _game;
			GameState.ChangeState(IntroGameState.state);
		}
	}
}