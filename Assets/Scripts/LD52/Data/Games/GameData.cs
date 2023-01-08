using System.Collections.Generic;
using LD52.Assets;
using LD52.Data.Cards;
using LD52.Data.Characters;
using LD52.Data.Characters.Heroes;
using UnityEngine;

namespace LD52.Data.Games {
	[CreateAssetMenu]
	public class GameData : ScriptableObject {
		[SerializeField] protected Hero[]         _allHeroes;
		[SerializeField] protected Hero           _firstHero;
		[SerializeField] protected ScenarioStep[] _scenarioSteps;
		[SerializeField] protected Card[]         _lootCards;
		[SerializeField] protected Card[]         _initialCardsInReserve;

		public IEnumerable<Hero>           allHeroes             => _allHeroes;
		public Hero                        firstHero             => _firstHero;
		public IReadOnlyList<ScenarioStep> scenarioSteps         => _scenarioSteps;
		public IEnumerable<Card>           lootCards             => _lootCards;
		public Card[]                      initialCardsInReserve => _initialCardsInReserve;
	}
}