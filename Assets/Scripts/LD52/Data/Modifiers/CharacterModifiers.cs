using System;

namespace LD52.Data.Modifiers {
	[Flags]
	public enum CharacterModifiers {
		Buried   = 1 << 0,
		Poisoned = 1 << 1,
		Confused = 1 << 2,
		Stinging = 1 << 3,
		Worm     = 1 << 4,
	}
}	