using System;
using UnityEngine;

namespace LD52.Data.Attributes {
	[Serializable]
	public class CharacterAttributeSet : IReadCharacterAttributeSet {
		[SerializeField] protected int[] _values = Array.Empty<int>();

		public int this[CharacterAttribute attribute] => _values.Length > (int)attribute ? _values[(int)attribute] : 0;

		public void Add(CharacterAttributeValue attributeValue) {
			if (_values.Length < (int)attributeValue.attribute) Array.Resize(ref _values, (int)attributeValue.attribute);
			_values[(int)attributeValue.attribute] += attributeValue.value;
		}

		public void Remove(CharacterAttributeValue attributeValue) {
			if (_values.Length < (int)attributeValue.attribute) Array.Resize(ref _values, (int)attributeValue.attribute);
			_values[(int)attributeValue.attribute] -= attributeValue.value;
		}
	}
}