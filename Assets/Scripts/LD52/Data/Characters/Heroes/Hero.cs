using LD52.Data.Cards;
using UnityEngine;
using UnityEngine.Events;

namespace LD52.Data.Characters.Heroes {
	[RequireComponent(typeof(GenericCharacter))]
	public class Hero : MonoBehaviour {
		[SerializeField] protected GenericCharacter _character;
		[SerializeField] protected Deck             _deck = new Deck();

		public GenericCharacter character   => _character ? _character : GetComponent<GenericCharacter>();
		public string           displayName => character.displayName;
		public Deck             deck        => _deck;
		public int              armor       => character.armor;
		public int              health      => character.health;
		public int              mana        => character.mana;
		public int              maxHealth   => character.maxHealth;
		public int              maxMana     => character.maxMana;

		public UnityEvent onHealthChanged => character.onHealthChanged;
		public UnityEvent onArmorChanged  => character.onArmorChanged;
		public UnityEvent onManaChanged   => character.onManaChanged;

		public void PrepareForBattle() {
			character.PrepareForBattle();
			deck.Shuffle();
		}

		public bool TryPeekTopOfDiscard(out Card topOfDiscard) => _deck.TryPeekTopOfDiscard(out topOfDiscard);
	}
}