using System.Collections;
using LD52.Data.Characters;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Utils.Ui;

public class TargetArrowUi : MonoBehaviourUi {
	private const float speed  = .2f;
	private const float offset = 40;
	private const float min    = 20;

	[SerializeField] protected Image  _renderer;
	[SerializeField] protected float  _defaultOpacity = .5f;
	[SerializeField] protected float  _hoveredOpacity = 1f;
	[SerializeField] protected bool   _hovered;
	[SerializeField] protected Canvas _parentCanvas;

	private Canvas parentCanvas => _parentCanvas ? _parentCanvas : _parentCanvas = GetComponentInParent<Canvas>();

	public IEnumerator Reveal(GenericCharacter target) {
		transform.offsetMin = transform.offsetMin.With(y: 0);
		transform.offsetMax = transform.offsetMax.With(y: Mathf.Max(min, Vector2.Distance(transform.position, BattleUiData.uiPerCharacter[target].position) - offset) / parentCanvas.scaleFactor);
		transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, BattleUiData.uiPerCharacter[target].position - (Vector2)transform.position));
		_renderer.color = _renderer.color.With(a: 0);
		SetHovered(false);
		yield return null;
		_renderer.enabled = true;
		while (!Mathf.Approximately(_renderer.color.a, _defaultOpacity)) {
			if (!_hovered) _renderer.color = _renderer.color.With(a: Mathf.MoveTowards(_renderer.color.a, _defaultOpacity, Time.deltaTime * speed));
			yield return null;
		}
	}

	public void Hide() {
		SetHovered(false);
		_renderer.enabled = false;
	}

	public void SetHovered(bool hovered) {
		_hovered = hovered;
		_renderer.color = _renderer.color.With(a: _hovered ? _hoveredOpacity : _defaultOpacity);
	}
}