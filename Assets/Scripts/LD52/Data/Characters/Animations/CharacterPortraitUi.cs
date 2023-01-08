using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD52.Data.Cards;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD52.Data.Characters {
	public class CharacterPortraitUi : MonoBehaviour {
		private const float chargeSpeed       = 3000;
		private const float chargeSqrDistance = 20 * 20;

		[SerializeField] protected GenericCharacter _character;
		[SerializeField] protected Image            _portrait;
		[SerializeField] protected bool             _beingAnimated;
		[SerializeField] protected Image            _effectImage;

		private void Start() {
			if (_effectImage) {
				_effectImage.enabled = false;
				_effectImage.color = Color.clear;
			}
		}

		public void Set(GenericCharacter character) {
			_character = character;
			_portrait.sprite = _character.portrait;
		}

		private void Update() {
			if (_beingAnimated) return;
			if (!_character) return;
			_portrait.sprite = _character.portrait;
		}

		public IEnumerator Animate(CardEffectAnimationData animationData, Sprite effectSprite, IReadOnlyCollection<GenericCharacter> targets, UnityAction onChargedOrNoCharge = null) {
			if (animationData.casterCharge && targets.Any()) {
				_beingAnimated = true;
				_character.SetAnimation(CharacterAnimation.Charge);
				var chargedTargetPosition = BattleUiData.uiPerCharacter[targets.FirstOrDefault()].position;
				while (Vector2.SqrMagnitude((Vector2)_portrait.transform.position - chargedTargetPosition) > chargeSqrDistance) {
					_portrait.transform.position = Vector2.MoveTowards(_portrait.transform.position, chargedTargetPosition, Time.deltaTime * chargeSpeed);
					yield return null;
				}
				_beingAnimated = false;
				_character.DetermineAnimation();
			}
			_portrait.rectTransform.offsetMin = Vector2.zero;
			_portrait.rectTransform.offsetMax = Vector2.zero;
			onChargedOrNoCharge?.Invoke();
			var targetPortraits = targets.Select(t => BattleUiData.uiPerCharacter[t].portrait).ToArray();
			targetPortraits.ForEach(t => {
				t._effectImage.sprite = effectSprite;
				t._effectImage.enabled = true;
			});
			for (var time = 0f; !animationData.IsDone(time); time += Time.deltaTime) {
				var targetPosition = animationData.GetTargetOffset(time);
				var effectPosition = animationData.GetEffectOffset(time);
				var scale = animationData.GetSize(time);
				var effectColor = animationData.GetEffectColor(time);
				var targetColor = animationData.GetTargetColor(time);

				targetPortraits.ForEach(t => {
					t._portrait.rectTransform.localPosition = targetPosition;
					t._effectImage.rectTransform.localScale = scale;
					t._effectImage.rectTransform.localPosition = effectPosition;
					t._effectImage.color = effectColor;
					t._portrait.color = targetColor;
				});
				yield return null;
			}
			targetPortraits.ForEach(t => {
				t._portrait.rectTransform.localPosition = Vector3.zero;
				t._effectImage.rectTransform.localScale = Vector3.zero;
				t._effectImage.color = Color.clear;
				t._portrait.color = Color.white;
				t._effectImage.enabled = false;
			});
		}
	}
}