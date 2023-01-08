using System.Collections.Generic;
using System.Linq;
using LD52.Assets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

public class MenuUi : MonoBehaviour {
	[SerializeField] protected Image[]  _images;
	[SerializeField] protected string[] _names;
	[SerializeField] protected Button   _startButton;
	[SerializeField] protected Button   _quitButton;

	private Dictionary<string, Sprite[]> danceSequences { get; } = new Dictionary<string, Sprite[]>();

	public UnityEvent onClick       => _startButton.onClick;
	public UnityEvent onQuitClicked => _quitButton.onClick;

	private void Start() {
#if UNITY_WEBGL
		_quitButton.gameObject.SetActive(false);
#endif

		danceSequences.SetAll(_names.Select(
				t => (t, new[] { AssetLibrary.vegetableSheet[$"{t}.dance.000"], AssetLibrary.vegetableSheet[$"{t}.dance.001"], AssetLibrary.vegetableSheet[$"{t}.dance.002"] }))
			.ToDictionary(t => t.t, t => t.Item2));
	}

	private void Update() {
		for (var i = 0; i < _images.Length; ++i) {
			_images[i].sprite = danceSequences[_names[i]][Mathf.FloorToInt(Time.time * 3) % 3];
		}
	}
}