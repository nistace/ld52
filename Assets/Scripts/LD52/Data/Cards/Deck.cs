using System;
using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

namespace LD52.Data.Cards {
	[Serializable]
	public class Deck {
		[SerializeField] protected List<Card> _cards          = new List<Card>();
		[SerializeField] protected List<Card> _stack          = new List<Card>();
		[SerializeField] protected List<Card> _drawn          = new List<Card>();
		[SerializeField] protected List<Card> _discardedCards = new List<Card>();

		public IReadOnlyList<Card> allCards    => _cards;
		public int                 stackSize   => _stack.Count;
		public int                 discardSize => _discardedCards.Count;
		public int                 drawnSize   => _drawn.Count;
		public IReadOnlyList<Card> drawnCards  => _drawn;

		public Card DrawNext() {
			if (_stack.Count == 0) Shuffle();
			var card = _stack[0];
			_stack.RemoveAt(0);
			_drawn.Add(card);
			return card;
		}

		public void DiscardDrawnCards() {
			_discardedCards.AddRange(_drawn);
			_drawn.Clear();
		}

		public void Shuffle() {
			_drawn.Clear();
			_discardedCards.Clear();
			_stack.Clear();
			_stack.AddRange(_cards);
			_stack.Shuffle();
		}

		public void Initialize(IEnumerable<Card> firstCards) {
			_cards.Clear();
			_cards.AddRange(firstCards);
		}

		public void ReplaceCard(int index, Card newCard) => _cards[index] = newCard;

		public bool TryPeekTopOfDiscard(out Card card) {
			card = _discardedCards.Count > 0 ? _discardedCards[^1] : default;
			return _discardedCards.Count > 0;
		}
	}
}