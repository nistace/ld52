using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LD52.Data.Cards;
using LD52.Data.Characters;
using LD52.Data.Characters.Heroes;
using LD52.Data.Characters.Opponents;
using LD52.Data.Games;
using LD52.Data.Modifiers;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;
using Utils.StaticUtils;

namespace LD52.Scenes.GameScene {
	public static class CardEffectManager {
		public enum CannotPlayReason {
			None          = 0,
			NotEnoughMana = 1,
			CasterDead    = 2,
			NoValidTarget = 3,
		}

		public static bool CheckCardCanBePlayed(Game game, Card card, Hero caster, out CannotPlayReason reason) {
			if (caster.health <= 0) return CannotPlayReason.CasterDead.False(out reason);
			if (caster.mana < card.manaCost) return CannotPlayReason.NotEnoughMana.False(out reason);
			return card.target switch {
				CardTarget.Ally when game.playerHeroes.Count(t => t != caster) == 0      => CannotPlayReason.NoValidTarget.False(out reason),
				CardTarget.AllAllies when game.playerHeroes.Count(t => t != caster) == 0 => CannotPlayReason.NoValidTarget.False(out reason),
				_                                                                        => CannotPlayReason.None.True(out reason)
			};
		}

		public static bool IsTargetSelectionRequired(Game game, Card card, Hero hero, out IEnumerable<GenericCharacter> candidates) => card.target switch {
			CardTarget.Self             => ((GenericCharacter[])null).False(out candidates),
			CardTarget.Ally             => game.playerHeroes.Except(t => t == hero).Select(t => t.character).True(out candidates),
			CardTarget.AllyOrSelf       => game.playerHeroes.Select(t => t.character).True(out candidates),
			CardTarget.AllAllies        => ((GenericCharacter[])null).False(out candidates),
			CardTarget.AllAlliesAndSelf => ((GenericCharacter[])null).False(out candidates),
			CardTarget.Opponent         => game.opponentTeam.opponents.Select(t => t.character).True(out candidates),
			CardTarget.AllOpponents     => ((GenericCharacter[])null).False(out candidates),
			_                           => throw new ArgumentOutOfRangeException()
		};

		public static IEnumerator PlayEffect(Game game, Hero caster, Card card, GenericCharacter target, UnityAction callback) {
			var targets = target
				? new[] { target }
				: card.target switch {
					CardTarget.Self             => new[] { caster.character },
					CardTarget.AllAllies        => game.playerHeroes.Except(t => t == caster).Select(t => t.character),
					CardTarget.AllAlliesAndSelf => game.playerHeroes.Select(t => t.character),
					CardTarget.AllOpponents     => game.opponentTeam.opponents.Select(t => t.character),
					CardTarget.Opponent         => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					CardTarget.AllyOrSelf       => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					CardTarget.Ally             => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					_                           => throw new ArgumentOutOfRangeException()
				};

			return PlayEffect(game, caster.character, card, targets, callback);
		}

		public static IEnumerator PlayEffect(Game game, Opponent caster, Card card, GenericCharacter target, UnityAction callback) {
			var targets = target
				? new[] { target }
				: card.target switch {
					CardTarget.Self             => new[] { caster.character },
					CardTarget.AllAllies        => game.opponentTeam.opponents.Except(t => t == caster).Select(t => t.character),
					CardTarget.AllAlliesAndSelf => game.opponentTeam.opponents.Select(t => t.character),
					CardTarget.AllOpponents     => game.playerHeroes.Select(t => t.character),
					CardTarget.Opponent         => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					CardTarget.AllyOrSelf       => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					CardTarget.Ally             => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					_                           => throw new ArgumentOutOfRangeException()
				};

			return PlayEffect(game, caster.character, card, targets, callback);
		}

		private static IEnumerator PlayEffect(Game game, GenericCharacter caster, Card card, IEnumerable<GenericCharacter> targets, UnityAction callback) {
			var targetArray = targets as GenericCharacter[] ?? targets.ToArray();
			Debug.Log($"{caster.displayName} casts {card.displayName} on {string.Join(", ", targetArray.Select(t => t.displayName))}");
			yield return null;
			// TODO Play animation
			var solver = GetActionSolver(card);
			targetArray.ForEach(t => solver.Invoke(caster, t));
			callback?.Invoke();
		}

		private static Action<GenericCharacter, GenericCharacter> GetActionSolver(Card card) => card.action switch {
			CardAction.Attack  => (caster, target) => target.Damage(card.GetStrength(caster.attributeSet)),
			CardAction.Heal    => (caster, target) => target.Heal(card.GetStrength(caster.attributeSet)),
			CardAction.Protect => (caster, target) => target.AddArmor(card.GetStrength(caster.attributeSet)),
			CardAction.Bury    => (_, target) => target.AddModifier(CharacterModifier.Buried),
			CardAction.GetMana => (caster, target) => target.ProduceMana(card.GetStrength(caster.attributeSet)),
			_                  => throw new ArgumentOutOfRangeException()
		};
	}
}