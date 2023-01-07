using System;
using System.Linq;
using LD52.Data.Characters.Opponents;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LD52.Data.Games {
	[Serializable]
	public class ScenarioStep : IReadScenarioStep {
		[SerializeField] protected bool               _startWithHeroInventory = true;
		[SerializeField] protected Opponent[]         _opponentPrefabs;
		[SerializeField] protected ScenarioStepReward _reward;

		public bool               equipmentStep => _startWithHeroInventory;
		public ScenarioStepReward rewards       => _reward;

		public OpponentTeam InstantiateOpponentTeam() => new OpponentTeam(_opponentPrefabs.Select(Object.Instantiate).ToArray());

		public bool HasIntro() => false;
	}
}