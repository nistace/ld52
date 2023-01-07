using System;

namespace LD52.Data.Modifiers {
	[Flags]
	public enum CharacterModifier {
		None     = 0,
		Buried   = 1 << 0,
		Poisoned = 1 << 1
	}
}