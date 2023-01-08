using System.Collections.Generic;
using LD52.Data.Cards;
using UnityEngine;
using UnityEngine.Events;

namespace LD52.Data.Characters.Opponents {
	[RequireComponent(typeof(GenericCharacter))]
	public class Opponent : MonoBehaviour {
		[SerializeField] protected GenericCharacter       _character;
		[SerializeField] protected Card[]                 _actions;
		[SerializeField] protected int                    _nextActionIndex;
		[SerializeField] protected List<GenericCharacter> _upcomingActionTargets = new List<GenericCharacter>();

		public GenericCharacter                character             => _character ? _character : GetComponent<GenericCharacter>();
		public string                          displayName           => character.displayName;
		public int                             armor                 => character.armor;
		public int                             health                => character.health;
		public int                             mana                  => character.mana;
		public int                             maxHealth             => character.maxHealth;
		public int                             maxMana               => character.maxMana;
		public Card[]                          actions               => _actions;
		public int                             nextActionIndex       => _nextActionIndex;
		public IReadOnlyList<GenericCharacter> upcomingActionTargets => _upcomingActionTargets;
		public Card                            upcomingAction        => _actions[nextActionIndex];
		public bool                            actionDone            { get; private set; }

		public UnityEvent onUpcomingActionChanged { get; } = new UnityEvent();

		private void Reset() {
			_character = GetComponent<GenericCharacter>();
		}

		public void PrepareForBattle() {
			character.PrepareForBattle();
			_nextActionIndex = 0;
			_upcomingActionTargets.Clear();
			onUpcomingActionChanged.Invoke();
		}

		public void PrepareNextAction() {
			_nextActionIndex = (_nextActionIndex + 1) % _actions.Length;
			_upcomingActionTargets.Clear();
			actionDone = false;
			onUpcomingActionChanged.Invoke();
		}

		public void SetTargetsForUpcomingAction(IEnumerable<GenericCharacter> targets) {
			_upcomingActionTargets.Clear();
			_upcomingActionTargets.AddRange(targets);
			onUpcomingActionChanged.Invoke();
		}

		public void SetActionAsDone() {
			actionDone = true;
			_upcomingActionTargets.Clear();
			onUpcomingActionChanged.Invoke();
		}
	}
}