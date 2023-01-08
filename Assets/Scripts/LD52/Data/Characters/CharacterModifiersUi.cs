using LD52.Assets;
using LD52.Data.Characters;
using LD52.Data.Modifiers;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class CharacterModifiersUi : MonoBehaviour {
	[SerializeField] protected Image[]          _images;
	[SerializeField] protected GenericCharacter _character;

	public void Set(GenericCharacter character) {
		_character?.onModifiersChanged.RemoveListener(Refresh);
		_character = character;
		_character?.onModifiersChanged.AddListenerOnce(Refresh);
		Refresh();
	}

	private void Refresh() {
		if (!_character) return;

		var index = 0;
		foreach (var modifier in EnumUtils.Values<CharacterModifiers>()) {
			if (_character.HasModifier(modifier)) {
				_images[index].enabled = true;
				_images[index].sprite = AssetLibrary.modifiers[$"{modifier.ToString().ToLowerFirst()}.default.000"];
				index++;
			}
		}
		while (index < _images.Length) {
			_images[index].enabled = false;
			index++;
		}
	}
}