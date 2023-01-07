using System;
using UnityEngine;

namespace LD52.Data.Attributes {
	[Serializable]
	public struct CharacterAttributeValue  {
		[SerializeField] private CharacterAttribute _attribute;
		[SerializeField] private int                _value;

		public CharacterAttribute attribute => _attribute;
		public int                value     => _value;

		public CharacterAttributeValue(CharacterAttribute attribute, int value) {
			_attribute = attribute;
			_value = value;
		}
	}
}