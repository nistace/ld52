using System;

namespace LD52.Data.Games {
	[Flags]
	public enum ScenarioStepReward {
		Nothing   = 0,
		Character = 1 << 0,
		Card      = 1 << 1,
	}
}