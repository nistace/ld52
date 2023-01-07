using LD52.Data.Cards;
using UnityEngine;

namespace LD52.Data.Characters.Opponents {
	[RequireComponent(typeof(GenericCharacter))]
	public class Opponent : MonoBehaviour {
		[SerializeField] protected GenericCharacter _character;
		[SerializeField] protected Card[]           _actions;
		[SerializeField] protected int              _nextActionIndex;

		public GenericCharacter character       => _character ? _character : GetComponent<GenericCharacter>();
		public string           displayName     => character.displayName;
		public int              armor           => character.armor;
		public int              health          => character.health;
		public int              mana            => character.mana;
		public int              maxHealth       => character.maxHealth;
		public int              maxMana         => character.maxMana;
		public Card[]           actions         => _actions;
		public int              nextActionIndex => _nextActionIndex;

		private void Reset() {
			_character = GetComponent<GenericCharacter>();
		}

		public void PrepareForBattle() {
			_nextActionIndex = 0;
			character.PrepareForBattle();
		}
	}
}