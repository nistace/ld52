using UnityEngine;

namespace LD52.Data.Games {
	public class GameUi : MonoBehaviour {
		[SerializeField] protected GameBattleUi    _battle;
		[SerializeField] protected GameRecruitUi   _recruit;
		[SerializeField] protected GameGetCardsUi  _loot;
		[SerializeField] protected GameInventoryUi _inventory;

		public GameBattleUi    battle    => _battle;
		public GameRecruitUi   recruit   => _recruit;
		public GameGetCardsUi  loot      => _loot;
		public GameInventoryUi inventory => _inventory;

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
			_loot.gameObject.SetActive(panel == Panel.GetCards);
			_inventory.gameObject.SetActive(panel == Panel.EquipHeroes);
		}
	}
}