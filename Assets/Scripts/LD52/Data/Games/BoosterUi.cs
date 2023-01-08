using System;
using LD52.Data.Cards;
using LD52.Data.Characters.Heroes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

public class BoosterUi : MonoBehaviour {
	public class Event : UnityEvent<(Card, Card)> { }

	[SerializeField] protected SimpleCardUi _first;
	[SerializeField] protected SimpleCardUi _second;
	[SerializeField] protected Button       _getButton;

	private (Card, Card) pair { get; set; }

	public static Event onBoosterClicked { get; } = new Event();

	private void Start() {
		_getButton.onClick.AddListenerOnce(() => onBoosterClicked.Invoke(pair));
	}

	public void Set((Card, Card) pair) {
		this.pair = pair;
		_first.Set(pair.Item1, null);
		_second.Set(pair.Item2, null);
	}
}