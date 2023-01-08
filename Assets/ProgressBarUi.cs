using System.Collections.Generic;
using LD52.Data.Games;
using UnityEngine;
using Utils.Extensions;

public class ProgressBarUi : MonoBehaviour {
	[SerializeField] protected Transform     _dotsContainer;
	[SerializeField] protected ProgressDotUi _progressDotPrefab;
	[SerializeField] protected ProgressDotUi _first;
	[SerializeField] protected ProgressDotUi _last;
	[SerializeField] protected RectTransform _arrow;

	private List<ProgressDotUi> dots { get; } = new List<ProgressDotUi>();

	public void Set(Game game) {
		game.onStepChanged.AddListenerOnce(RefreshArrow);
		dots.Add(_first);
		for (var i = 0; i < game.stepCount - 2; ++i) dots.Add(Instantiate(_progressDotPrefab, _dotsContainer));
		dots.Add(_last);
		RefreshArrow(0);
	}

	private void RefreshArrow(int step) {
		if (step < 0 || step >= dots.Count) return;
		StartCoroutine(dots[step].AttachArrow(_arrow));
	}
}