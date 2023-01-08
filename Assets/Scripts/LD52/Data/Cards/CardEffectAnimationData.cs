using System;
using UnityEngine;
using Utils.Extensions;

namespace LD52.Data.Cards {
	[Serializable]
	public class CardEffectAnimationData {
		[SerializeField] protected AnimationCurve _targetOffsetXCurve = AnimationCurve.Constant(0, 0, 0);
		[SerializeField] protected AnimationCurve _targetOffsetYCurve = AnimationCurve.Constant(0, 0, 0);
		[SerializeField] protected AnimationCurve _effectOffsetXCurve = AnimationCurve.Constant(0, 0, 0);
		[SerializeField] protected AnimationCurve _effectOffsetYCurve = AnimationCurve.Constant(0, 0, 0);
		[SerializeField] protected AnimationCurve _opacityCurve       = AnimationCurve.EaseInOut(0, 1, 1, 0);
		[SerializeField] protected AnimationCurve _sizeCurve          = AnimationCurve.EaseInOut(0, 0, 1, 1);
		[SerializeField] protected Color          _targetColoration   = Color.red;
		[SerializeField] protected AnimationCurve _colorationCurve    = AnimationCurve.EaseInOut(0, 1, 1, 0);
		[SerializeField] protected float          _duration           = 1;
		[SerializeField] protected bool           _casterCharge       = true;

		public bool casterCharge => _casterCharge;

		public bool IsDone(float duration) => duration > _duration;
		public Vector2 GetTargetOffset(float duration) => new Vector2(_targetOffsetXCurve.Evaluate(duration / _duration), _targetOffsetYCurve.Evaluate(duration / _duration));
		public Vector2 GetEffectOffset(float duration) => new Vector2(_effectOffsetXCurve.Evaluate(duration / _duration), _effectOffsetYCurve.Evaluate(duration / _duration));
		public Color GetEffectColor(float duration) => Color.white.With(_opacityCurve.Evaluate(duration / _duration));
		public Vector2 GetSize(float duration) => Vector2.one * _sizeCurve.Evaluate(duration / _duration);
		public Color GetTargetColor(float duration) => Color.Lerp(Color.white, _targetColoration, _colorationCurve.Evaluate(duration / _duration));
	}
}