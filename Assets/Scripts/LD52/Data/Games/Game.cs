using System;
using System.Collections.Generic;
using System.Linq;
using LD52.Data.Cards;
using LD52.Data.Characters.Heroes;
using LD52.Data.Characters.Opponents;
using UnityEngine;
using Utils.Events;
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
		[SerializeField] protected CardReserve  _cardReserve = new CardReserve();

		public int stepCount => _gameData.scenarioSteps.Count;

		public IntEvent onStepChanged { get; } = new IntEvent();

		public Game(GameData data) {
			_gameData = data;
			RecruitHero(_gameData.firstHero);
			_gameData.initialCardsInReserve.ForEach(_cardReserve.AddCard);
		}

		public IReadOnlyList<Hero> playerHeroes => _playerHeroes;
		public OpponentTeam        opponentTeam => _opponentTeam;
		public CardReserve         cardReserve  => _cardReserve;

		public IEnumerable<Hero> GetAvailableHeroesToRecruit() => _gameData.allHeroes.Except(t => _playerHeroes.Any(u => u.displayName == t.displayName));
		public OpponentTeam InstantiateOpponentTeam() => _opponentTeam = _gameData.scenarioSteps[_currentScenarioStep].InstantiateOpponentTeam();

		public bool IsScenarioEnded() => _currentScenarioStep >= _gameData.scenarioSteps.Count;

		public void EndCurrentScenarioStep() {
			_currentScenarioStep++;
			onStepChanged.Invoke(_currentScenarioStep);
		}

		public IReadScenarioStep GetCurrentScenarioStep() => _gameData.scenarioSteps[_currentScenarioStep];

		public void RecruitHero(Hero heroPrefab) {
			var newHero = Object.Instantiate(heroPrefab);
			newHero.Initialize();
			_playerHeroes.Add(newHero);
		}

		public (Card, Card)[] GenerateBoosters() {
			_boosterLevels++;
			var candidateCards = _gameData.lootCards.Where(t => t.level <= _boosterLevels).ToList();
			return 3.CreateArray(_ => (candidateCards.Random(), candidateCards.Random()));
		}
	}
}