using System.Collections;
using LD52.Data.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LD52.Data.Characters.Opponents {
	[RequireComponent(typeof(TargetSelectionUi))]
	public class OpponentUi : MonoBehaviour {
		[SerializeField] protected TargetSelectionUi _targetSelection;
		[SerializeField] protected Opponent          _opponent;
		[SerializeField] protected CharacterBarUi    _barUi;
		[SerializeField] protected Image             _portraitImage;
		[SerializeField] protected TMP_Text          _nameLabel;
		[SerializeField] protected SimpleCardUi[]    _cards;
		[SerializeField] protected RectTransform[]   _cardArrowAnchors;
		[SerializeField] protected RectTransform     _arrow;
		[SerializeField] protected float             _arrowSpeed = 200;

		private Opponent          opponent        => _opponent;
		public  TargetSelectionUi targetSelection => _targetSelection ? _targetSelection : _targetSelection = GetComponent<TargetSelectionUi>();

		public void Set(Opponent opponent) {
			_barUi.Unset();
			_opponent = opponent;
			if (!_opponent) return;
			_portraitImage.sprite = opponent.character.portrait;
			_nameLabel.text = opponent.displayName;
			for (var i = 0; i < _cards.Length; ++i) {
				_cards[i].gameObject.SetActive(opponent.actions.Length > i);
				if (opponent.actions.Length > i) _cards[i].SetCard(opponent.actions[i]);
			}
			_barUi.Set(_opponent.character);

			_arrow.SetParent(_cardArrowAnchors[opponent.nextActionIndex]);
			_arrow.anchorMin = Vector2.zero;
			_arrow.anchorMax = Vector2.one;
			_arrow.offsetMin = Vector2.zero;
			_arrow.offsetMax = Vector2.one;
			targetSelection.character = opponent.character;
		}

		public IEnumerator MoveArrow(int overCardIndex) {
			_arrow.SetParent(_cardArrowAnchors[overCardIndex]);
			_arrow.anchorMin = Vector2.zero;
			_arrow.anchorMax = Vector2.one;
			yield return null;

			while (_arrow.offsetMin != Vector2.zero) {
				_arrow.offsetMin = Vector2.MoveTowards(_arrow.offsetMin, Vector2.zero, Time.deltaTime * _arrowSpeed);
				_arrow.offsetMax = Vector2.MoveTowards(_arrow.offsetMax, Vector2.zero, Time.deltaTime * _arrowSpeed);

				yield return null;
			}
		}

		private void Update() {
			if (!opponent) return;
			_portraitImage.sprite = opponent.character.portrait;
		}
	}
}