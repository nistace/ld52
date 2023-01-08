using System.Collections.Generic;
using LD52.Data.Cards;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace LD52.Data.Characters.Heroes {
	[RequireComponent(typeof(GenericCharacter))]
	public class Hero : MonoBehaviour {
		public class Event : UnityEvent<Hero> { }

		[SerializeField] protected GenericCharacter _character;
		[SerializeField] protected Card[]           _defaultCards;
		[SerializeField] protected Card[]           _overridenCards;
		[SerializeField] protected Deck             _deck = new Deck();

		public GenericCharacter character   => _character ? _character : GetComponent<GenericCharacter>();
		public string           displayName => character.displayName;
		public Deck             deck        => _deck;
		public int              armor       => character.armor;
		public int              health      => character.health;
		public int              mana        => character.mana;
		public int              maxHealth   => character.maxHealth;
		public int              maxMana     => character.maxMana;

		public IReadOnlyList<Card> defaultCards => _defaultCards;

		public UnityEvent onHealthChanged => character.onHealthChanged;
		public UnityEvent onArmorChanged  => character.onArmorChanged;
		public UnityEvent onManaChanged   => character.onManaChanged;
		public UnityEvent onDeckChanged   { get; } = new UnityEvent();

		public void Initialize() {
			_overridenCards = _defaultCards.Length.CreateArray(_ => (Card)null);
			_deck.Initialize(_defaultCards);
			foreach (var card in _defaultCards) {
				character.attributeSet.Add(card.attributeBonus);
			}
		}

		public void PrepareForBattle() {
			character.PrepareForBattle();
			deck.Shuffle();
		}

		public bool TryPeekTopOfDiscard(out Card topOfDiscard) => _deck.TryPeekTopOfDiscard(out topOfDiscard);

		public bool IsInitialCard(int cardIndex) => !_overridenCards[cardIndex];

		public void OverrideCard(int index, Card newCard, out Card removedCard) {
			if (_overridenCards[index]) {
				removedCard = _overridenCards[index];
				character.attributeSet.Remove(_overridenCards[index].attributeBonus);
			}
			else {
				removedCard = null;
				character.attributeSet.Remove(_defaultCards[index].attributeBonus);
			}
			_overridenCards[index] = newCard;
			deck.ReplaceCard(index, newCard);
			character.attributeSet.Add(newCard.attributeBonus);
			onDeckChanged.Invoke();
		}

		public Card RemoveOverridingCard(int index) {
			if (!_overridenCards[index]) return null;
			var removedCard = _overridenCards[index];
			character.attributeSet.Remove(removedCard.attributeBonus);
			_overridenCards[index] = null;
			deck.ReplaceCard(index, defaultCards[index]);
			character.attributeSet.Add(defaultCards[index].attributeBonus);
			onDeckChanged.Invoke();
			return removedCard;
		}
	}
}