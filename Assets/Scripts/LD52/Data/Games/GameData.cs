﻿using System.Collections.Generic;
using LD52.Assets;
using LD52.Data.Characters;
using LD52.Data.Characters.Heroes;
using UnityEngine;

namespace LD52.Data.Games {
	[CreateAssetMenu]
	public class GameData : ScriptableObject {
		[SerializeField] protected Hero[]         _allHeroes;
		[SerializeField] protected ScenarioStep[] _scenarioSteps;

		public IEnumerable<Hero>           allHeroes     => _allHeroes;
		public IReadOnlyList<ScenarioStep> scenarioSteps => _scenarioSteps;
	}
}