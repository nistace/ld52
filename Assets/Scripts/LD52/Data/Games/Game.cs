﻿using System;
using System.Collections.Generic;
using System.Linq;
using LD52.Data.Cards;
using LD52.Data.Characters.Heroes;
using LD52.Data.Characters.Opponents;
using UnityEngine;
using Utils.Extensions;
using Object = UnityEngine.Object;

namespace LD52.Data.Games {
	[Serializable]
	public class Game {
		[SerializeField] protected GameData     _gameData;
		[SerializeField] protected List<Hero>   _playerHeroes = new List<Hero>();
		[SerializeField] protected int          _currentScenarioStep;
		[SerializeField] protected OpponentTeam _opponentTeam;
		[SerializeField] protected int          _boosterLevels;
		[SerializeField] protected List<Card>   _unusedCards = new List<Card>();

		public Game(GameData data) {
			_gameData = data;
			RecruitHero(_gameData.firstHero);
		}

		public List<Hero>   playerHeroes => _playerHeroes;
		public OpponentTeam opponentTeam => _opponentTeam;

		public IEnumerable<Hero> GetAvailableHeroesToRecruit() => _gameData.allHeroes.Except(t => _playerHeroes.Any(u => u.displayName == t.displayName));
		public OpponentTeam InstantiateOpponentTeam() => _opponentTeam = _gameData.scenarioSteps[_currentScenarioStep].InstantiateOpponentTeam();

		public bool IsScenarioEnded() => _currentScenarioStep >= _gameData.scenarioSteps.Count;

		public void EndCurrentScenarioStep() => _currentScenarioStep++;
		public IReadScenarioStep GetCurrentScenarioStep() => _gameData.scenarioSteps[_currentScenarioStep];
		public void RecruitHero(Hero heroPrefab) => _playerHeroes.Add(Object.Instantiate(heroPrefab));

		public (Card, Card)[] GenerateBoosters() {
			_boosterLevels++;
			var candidateCards = _gameData.lootCards.Where(t => t.level <= _boosterLevels).ToList();
			return 3.CreateArray(_ => (candidateCards.Random(), candidateCards.Random()));
		}

		public void ObtainCards((Card, Card) booster) {
			(var first, var second) = booster;
			_unusedCards.Add(first);
			_unusedCards.Add(second);
		}
	}
}