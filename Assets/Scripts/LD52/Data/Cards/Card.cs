using LD52.Assets;
using LD52.Data.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LD52.Data.Cards {
	[CreateAssetMenu]
	public class Card : ScriptableObject {
		public class Event : UnityEvent<Card> { }

		[SerializeField] protected string                  _displayName;
		[SerializeField] protected string                  _iconName;
		[SerializeField] protected CardAction              _action;
		[SerializeField] protected CardTarget              _target;
		[SerializeField] protected CharacterAttribute      _attribute           = CharacterAttribute.Attack;
		[SerializeField] protected int                     _attributeMultiplier = 1;
		[SerializeField] protected int                     _constantStrength;
		[SerializeField] protected int                     _manaCost;
		[SerializeField] protected string                  _description;
		[SerializeField] protected int                     _level = 1;
		[SerializeField] protected CharacterAttributeValue _attributeBonus;

		public string                  displayName    => _displayName;
		public Sprite                  icon           => AssetLibrary.cardSheet[$"{_iconName}.default.000"];
		public CardAction              action         => _action;
		public string                  description    => _description;
		public int                     level          => _level;
		public CharacterAttributeValue attributeBonus => _attributeBonus;
		public CardTarget              target         => _target;
		public int                     manaCost       => _manaCost;

		public string ToStrengthLabel() {
			if (_attributeMultiplier == 0 && _constantStrength == 0) return string.Empty;
			if (_attributeMultiplier == 0) return $"{_constantStrength}";
			if (_constantStrength == 0) return $"{_attribute} x{_attributeMultiplier}";
			return $"{_attribute} x{_attributeMultiplier} + {_constantStrength}";
		}

		public int GetStrength(IReadCharacterAttributeSet attributeSet) => attributeSet[_attribute] * _attributeMultiplier + _constantStrength;
	}
}