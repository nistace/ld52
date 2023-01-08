using UnityEngine;

namespace LD52.Data.Characters {
	public interface ICharacterUi {
		public Vector2             position { get; }
		public CharacterPortraitUi portrait { get; }
	}
}