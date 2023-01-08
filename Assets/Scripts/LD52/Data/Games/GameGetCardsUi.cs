using LD52.Data.Cards;
using UnityEngine;

namespace LD52.Data.Games {
	public class GameGetCardsUi : MonoBehaviour {
		[SerializeField] protected BoosterUi[]  _boosters;
		[SerializeField] protected GameObject[] _orGameObjects;

		public void Set((Card, Card)[] pairs) {
			for (var i = 0; i < pairs.Length; ++i) {
				_boosters[i].gameObject.SetActive(true);
				_boosters[i].Set(pairs[i]);
				if (i > 0) _orGameObjects[i - 1].gameObject.SetActive(true);
			}
			for (var i = pairs.Length; i < _boosters.Length; ++i) {
				_boosters[i].gameObject.SetActive(false);
				if (i > 0) _orGameObjects[i - 1].gameObject.SetActive(false);
			}
		}
	}
}