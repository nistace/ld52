using LD52.Data.Characters.Heroes;
using UnityEngine;

namespace LD52.Data.Games {
	public class GameRecruitUi : MonoBehaviour {
		[SerializeField] protected RecruitHeroUi _leftRecruitHero;
		[SerializeField] protected RecruitHeroUi _rightRecruitHero;
		[SerializeField] protected GameObject    _orGameObject;

		public void Set(Hero left, Hero right) {
			_leftRecruitHero.gameObject.SetActive(left);
			_leftRecruitHero.Set(left);
			_orGameObject.SetActive(left && right);
			_rightRecruitHero.gameObject.SetActive(right);
			_rightRecruitHero.Set(right);
		}
	}
}