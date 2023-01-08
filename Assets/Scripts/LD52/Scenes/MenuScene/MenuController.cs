using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Extensions;

namespace LD52.Scenes {
	public class MenuController : MonoBehaviour {
		[SerializeField] protected MenuUi _menuUi;

		private void Start() {
			_menuUi.onClick.AddListenerOnce(() => SceneManager.LoadScene("Roguelike"));
		}
	}
}