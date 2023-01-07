using UnityEngine;

namespace LD52.Data.Games {
	public class GameUi : MonoBehaviour {
		[SerializeField] protected GameBattleUi _battle;

		public GameBattleUi battle => _battle;

		public enum Panel {
			Intro,
			Battle,
			RecruitHero,
			EquipHeroes,
			GetCards,
			Outro
		}

		public void Show(Panel panel) {
			_battle.gameObject.SetActive(panel == Panel.Battle);
		}
	}
}