using System.Collections;
using System.Collections.Generic;
using LD52.Data.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD52.Data.Characters.Heroes {
	[RequireComponent(typeof(TargetSelectionUi))]
	public class HeroUi : MonoBehaviour {
		[SerializeField] protected TargetSelectionUi _targetSelection;
		[SerializeField] protected CharacterBarUi    _barUi;
		[SerializeField] protected Image             _heroImage;
		[SerializeField] protected TMP_Text          _heroName;
		[SerializeField] protected RectTransform     _discardCardParent;
		[SerializeField] protected SimpleCardUi      _discardTopCard;
		[SerializeField] protected RectTransform     _stackCardParent;
		[SerializeField] protected RectTransform[]   _optionParents;
		[SerializeField] protected SimpleCardUi[]    _optionCards;
		[SerializeField] protected Hero              _hero;
		[SerializeField] protected float             _cardMovementSpeed = 2;

		public TargetSelectionUi targetSelection => _targetSelection ? _targetSelection : _targetSelection = GetComponent<TargetSelectionUi>();

		public class Event : UnityEvent<Hero, int> { }

		public static Event onDrawnCardClicked { get; } = new Event();

		private Hero hero => _hero;

		private void Start() => _optionCards.ForEach(t => t.onClick.AddListenerOnce(HandleOptionCardClicked));
		private void HandleOptionCardClicked(SimpleCardUi card) => onDrawnCardClicked.Invoke(hero, _optionCards.IndexOf(card));

		public void SetHero(Hero hero) {
			_barUi.Unset();
			_hero = hero;
			if (!_hero) return;
			_heroImage.sprite = hero.character.portrait;
			_heroName.text = _hero.displayName;
			for (var i = 0; i < _optionParents.Length; ++i) {
				_optionCards[i].transform.SetParent(_stackCardParent);
				_optionCards[i].transform.MoveAnchorsKeepPosition(Vector2.zero, Vector2.one);
				_optionCards[i].transform.offsetMin = Vector2.zero;
				_optionCards[i].transform.offsetMax = Vector2.zero;
			}
			_discardTopCard.gameObject.SetActive(_hero.TryPeekTopOfDiscard(out var topOfDiscard));
			_discardTopCard.SetCard(topOfDiscard);
			_barUi.Set(hero.character);
			targetSelection.character = hero.character;
		}

		public IEnumerator MoveDiscardToStack() {
			_discardTopCard.gameObject.SetActive(false);
			foreach (var card in _optionCards) {
				card.transform.SetParent(_stackCardParent);
				card.transform.anchorMin = Vector2.zero;
				card.transform.anchorMax = Vector2.one;
			}

			yield return null;

			while (_optionCards[^1].transform.offsetMin != Vector2.zero) {
				foreach (var card in _optionCards) {
					card.transform.offsetMin = Vector2.MoveTowards(card.transform.offsetMin, Vector2.zero, Time.deltaTime * _cardMovementSpeed);
					card.transform.offsetMax = Vector2.MoveTowards(card.transform.offsetMax, Vector2.zero, Time.deltaTime * _cardMovementSpeed);
				}
				yield return null;
			}
		}

		public IEnumerator DrawCards(IReadOnlyList<Card> cards) {
			for (var i = 0; i < cards.Count; ++i) {
				_optionCards[i].SetCard(cards[i]);
				_optionCards[i].transform.SetParent(_stackCardParent);
				_optionCards[i].transform.anchorMin = Vector2.zero;
				_optionCards[i].transform.anchorMax = Vector2.one;
				_optionCards[i].transform.offsetMin = Vector2.zero;
				_optionCards[i].transform.offsetMax = Vector2.zero;
			}

			yield return null;

			for (var i = 0; i < cards.Count; ++i) {
				_optionCards[i].transform.SetParent(_optionParents[i]);
				_optionCards[i].transform.anchorMin = Vector2.zero;
				_optionCards[i].transform.anchorMax = Vector2.one;
			}

			yield return null;

			while (_optionCards[cards.Count - 1].transform.offsetMin != Vector2.zero) {
				for (var i = 0; i < cards.Count; ++i) {
					_optionCards[i].transform.offsetMin = Vector2.MoveTowards(_optionCards[i].transform.offsetMin, Vector2.zero, Time.deltaTime * _cardMovementSpeed);
					_optionCards[i].transform.offsetMax = Vector2.MoveTowards(_optionCards[i].transform.offsetMax, Vector2.zero, Time.deltaTime * _cardMovementSpeed);
				}
				yield return null;
			}
		}

		public IEnumerator MoveDrawnToDiscard() {
			foreach (var card in _optionCards) {
				card.transform.SetParent(_discardCardParent);
				card.transform.anchorMin = Vector2.zero;
				card.transform.anchorMax = Vector2.one;
			}

			yield return null;
			while (_optionCards[^1].transform.offsetMin != Vector2.zero) {
				foreach (var card in _optionCards) {
					card.transform.offsetMin = Vector2.MoveTowards(card.transform.offsetMin, Vector2.zero, Time.deltaTime * _cardMovementSpeed);
					card.transform.offsetMax = Vector2.MoveTowards(card.transform.offsetMax, Vector2.zero, Time.deltaTime * _cardMovementSpeed);
				}
				yield return null;
			}

			_discardTopCard.gameObject.SetActive(true);
			_discardTopCard.SetCard(_optionCards[^1].card);
		}

		private void Update() {
			if (!hero) return;
			_heroImage.sprite = hero.character.portrait;
		}
	}
}