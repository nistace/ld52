using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD52.Data.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Extensions;

namespace LD52.Data.Characters.Opponents {
	[RequireComponent(typeof(TargetSelectionUi))]
	public class OpponentUi : MonoBehaviour, ICharacterUi, IPointerEnterHandler, IPointerExitHandler {
		[SerializeField] protected TargetSelectionUi   _targetSelection;
		[SerializeField] protected CharacterPortraitUi _portrait;
		[SerializeField] protected Opponent            _opponent;
		[SerializeField] protected CharacterBarUi      _barUi;
		[SerializeField] protected TMP_Text            _nameLabel;
		[SerializeField] protected SimpleCardUi[]      _cards;
		[SerializeField] protected RectTransform[]     _cardArrowAnchors;
		[SerializeField] protected RectTransform       _arrow;
		[SerializeField] protected float               _arrowSpeed = 200;
		[SerializeField] protected RectTransform       _targetArrowAnchor;
		[SerializeField] protected TargetArrowUi       _targetArrowPrefab;

		private Opponent            opponent          => _opponent;
		private List<TargetArrowUi> targetArrows      { get; } = new List<TargetArrowUi>();
		public  TargetSelectionUi   targetSelection   => _targetSelection ? _targetSelection : _targetSelection = GetComponent<TargetSelectionUi>();
		public CharacterPortraitUi portrait          => _portrait ? _portrait : _portrait = GetComponent<CharacterPortraitUi>();
		public  Vector2             position => portrait.transform.position;

		public void Set(Opponent opponent) {
			if (_opponent) _opponent.onUpcomingActionChanged.RemoveListener(RefreshUpcomingAction);
			_barUi.Unset();
			_opponent = opponent;
			if (!_opponent) return;
			portrait.Set(_opponent.character);
			_nameLabel.text = opponent.displayName;
			for (var i = 0; i < _cards.Length; ++i) {
				_cards[i].gameObject.SetActive(opponent.actions.Length > i);
				if (opponent.actions.Length > i) _cards[i].Set(opponent.actions[i], opponent.character);
			}
			_barUi.Set(_opponent.character);
			targetSelection.character = opponent.character;
			_opponent.character.onHealthChanged.AddListenerOnce(HandleHealthChanged);
			_opponent.onUpcomingActionChanged.AddListenerOnce(RefreshUpcomingAction);
			RefreshUpcomingAction();
		}

		private void HandleHealthChanged() {
			if (!_opponent) return;
			if (opponent.character.dead || opponent.actionDone) {
				targetArrows.ForEach(t => t.Hide());
			}
		}

		private void RefreshUpcomingAction() {
			if (!_opponent) return;
			_arrow.gameObject.SetActive(opponent.character.alive && !opponent.actionDone);
			_arrow.SetParent(_cardArrowAnchors[opponent.nextActionIndex]);
			if (opponent.character.dead || opponent.actionDone) {
				targetArrows.ForEach(t => t.Hide());
			}
			_arrow.anchorMin = Vector2.zero;
			_arrow.anchorMax = Vector2.one;
			_arrow.offsetMin = Vector2.zero;
			_arrow.offsetMax = Vector2.one;

			for (var i = 0; i < opponent.actions.Length; ++i) {
				_cards[i].color = opponent.character.alive && !opponent.actionDone && i == opponent.nextActionIndex ? Color.white : Color.gray;
			}

			// Display arrows to show targets
			if (opponent.character.alive && !opponent.actionDone && (opponent.upcomingActionTargets?.Any() ?? false)) {
				StartCoroutine(RevealTargets());
			}
		}

		private IEnumerator RevealTargets() {
			for (var i = 0; i < opponent.upcomingActionTargets.Count; ++i) {
				if (targetArrows.Count <= i) {
					targetArrows.Add(Instantiate(_targetArrowPrefab, _targetArrowAnchor));
					targetArrows[i].Hide();
				}
				yield return null;
				StartCoroutine(targetArrows[i].Reveal(opponent.upcomingActionTargets[i]));
				yield return new WaitForSeconds(.2f);
			}
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

		public void OnPointerEnter(PointerEventData eventData) => targetArrows.ForEach(t => t.SetHovered(true));
		public void OnPointerExit(PointerEventData eventData) => targetArrows.ForEach(t => t.SetHovered(false));
	}
}