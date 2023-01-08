using System.Collections;
using LD52.Data.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

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
			if (_opponent) _opponent.onUpcomingActionChanged.RemoveListener(RefreshUpcomingAction);
			_barUi.Unset();
			_opponent = opponent;
			if (!_opponent) return;
			_portraitImage.sprite = opponent.character.portrait;
			_nameLabel.text = opponent.displayName;
			for (var i = 0; i < _cards.Length; ++i) {
				_cards[i].gameObject.SetActive(opponent.actions.Length > i);
				if (opponent.actions.Length > i) _cards[i].Set(opponent.actions[i], opponent.character);
			}
			_barUi.Set(_opponent.character);
			targetSelection.character = opponent.character;
			_opponent.onUpcomingActionChanged.AddListenerOnce(RefreshUpcomingAction);
			RefreshUpcomingAction();
		}

		private void RefreshUpcomingAction() {
			if (!_opponent) return;
			_arrow.gameObject.SetActive(opponent.character.alive && !opponent.actionDone);
			_arrow.SetParent(_cardArrowAnchors[opponent.nextActionIndex]);
			_arrow.anchorMin = Vector2.zero;
			_arrow.anchorMax = Vector2.one;
			_arrow.offsetMin = Vector2.zero;
			_arrow.offsetMax = Vector2.one;

			for (var i = 0; i < opponent.actions.Length; ++i) {
				_cards[i].color = opponent.character.alive && !opponent.actionDone && i == opponent.nextActionIndex ? Color.white : Color.gray;
			}

			// Display arrows to show targets
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