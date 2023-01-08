using System.Collections.Generic;

namespace LD52.Data.Characters {
	public static class BattleUiData {
		public static Dictionary<GenericCharacter, ICharacterUi> uiPerCharacter { get; } = new Dictionary<GenericCharacter, ICharacterUi>();
	}
}