using System.Collections;
using UnityEngine;

public class ProgressDotUi : MonoBehaviour {
	[SerializeField] protected Transform _arrowAnchor;

	public IEnumerator AttachArrow(RectTransform arrow) {
		arrow.SetParent(_arrowAnchor);
		for (var lerp = 0f; lerp < 1; lerp += Time.deltaTime) {
			arrow.offsetMin = Vector2.Lerp(arrow.offsetMin, Vector2.zero, lerp);
			arrow.offsetMax = Vector2.Lerp(arrow.offsetMax, Vector2.zero, lerp);
			yield return null;
		}

		arrow.offsetMin = Vector2.zero;
		arrow.offsetMax = Vector2.zero;
	}
}