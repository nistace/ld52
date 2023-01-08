using UnityEngine;

namespace LD52.Data.Games {
	public class GameUi : MonoBehaviour {
		[SerializeField] protected GameBattleUi  _battle;
		[SerializeField] protected GameRecruitUi _recruit;

		public GameBattleUi  battle  => _battle;
		public GameRecruitUi recruit => _recruit;

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
			_recruit.gameObject.SetActive(panel == Panel.RecruitHero);
		}
	}
}