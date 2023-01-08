using System.Collections.Generic;
using System.Linq;
using LD52.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

public class GameOverUi : MonoBehaviour {
	[SerializeField] protected TMP_Text _outcomeText;
	[SerializeField] protected Image[]  _images;
	[SerializeField] protected string[] _names;
	[SerializeField] protected Button   _menuButton;

	private Dictionary<string, Sprite[]> animationSequences { get; } = new Dictionary<string, Sprite[]>();

	public UnityEvent onClick => _menuButton.onClick;

	private static List<Sprite> LoadSequence(string prefix) {
		var sprites = new List<Sprite>();
		var sprite = AssetLibrary.vegetableSheet[$"{prefix}000"];
		for (var i = 1; sprite; i++) {
			sprites.Add(sprite);
			sprite = AssetLibrary.vegetableSheet[$"{prefix}{i:000}"];
		}
		return sprites;
	}

	public void Initialize(bool won) {
		_outcomeText.text = won ? "Good job,<br>Brigade!" : "You've been<br>Harvested.";
		animationSequences.SetAll(_names.Select(t => (t, LoadSequence($"{t}.{(won ? "dance" : "dead")}."))).ToDictionary(t => t.t, t => t.Item2.ToArray()));
	}

	private void Update() {
		for (var i = 0; i < _images.Length; ++i) {
			_images[i].sprite = animationSequences[_names[i]][Mathf.FloorToInt(Time.time * 3) % animationSequences[_names[i]].Length];
		}
	}
}