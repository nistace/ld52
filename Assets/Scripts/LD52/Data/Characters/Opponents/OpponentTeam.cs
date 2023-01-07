using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LD52.Data.Characters.Opponents {
	[Serializable]
	public class OpponentTeam {
		[SerializeField] protected Opponent[] _opponents;

		public IReadOnlyList<Opponent> opponents => _opponents;

		public OpponentTeam(IEnumerable<Opponent> opponents) => _opponents = opponents.ToArray();
	}
}