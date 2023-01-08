using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

namespace LD52.Scenes {
	public class GameOverController : MonoBehaviour {
		public static bool won { get; set; } = true;

		[SerializeField] private GameOverUi _gameOverUi;

		private void Start() {
			_gameOverUi.Initialize(won);
			_gameOverUi.onClick.AddListenerOnce(() => SceneManager.LoadScene("Menu"));
		}
	}
}