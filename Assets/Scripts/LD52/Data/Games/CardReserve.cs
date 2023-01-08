using System;
using System.Collections.Generic;
using LD52.Data.Cards;
using UnityEngine;
using UnityEngine.Events;

namespace LD52.Data.Games {
	[Serializable]
	public class CardReserve {
		[SerializeField] protected List<Card> _reserve = new List<Card>();

		public Card this[int i] => _reserve[i];
		public int count => _reserve.Count;

		public UnityEvent onChanged { get; } = new UnityEvent();

		public void AddCard(Card card) {
			_reserve.Add(card);
			onChanged.Invoke();
		}

		public Card Take(int index) {
			var takenCard = _reserve[index];
			_reserve.RemoveAt(index);
			onChanged.Invoke();
			return takenCard;
		}
	}
}