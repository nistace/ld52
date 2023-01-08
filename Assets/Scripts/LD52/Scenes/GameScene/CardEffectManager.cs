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
using Utils.Coroutines;
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

		public static bool CheckCardCanBePlayed(Game game, Card card, GenericCharacter caster, out CannotPlayReason reason) {
			if (caster.health <= 0) return CannotPlayReason.CasterDead.False(out reason);
			if (caster.mana < card.manaCost) return CannotPlayReason.NotEnoughMana.False(out reason);
			switch (card.target) {
				case CardTarget.Self when !CheckRestrictions(card, caster):                                                                      return CannotPlayReason.NoValidTarget.False(out reason);
				case CardTarget.Ally when game.playerHeroes.Count(t => t.character != caster && CheckRestrictions(card, t.character)) == 0:      return CannotPlayReason.NoValidTarget.False(out reason);
				case CardTarget.AllyOrSelf when game.playerHeroes.Count(t => CheckRestrictions(card, t.character)) == 0:                         return CannotPlayReason.NoValidTarget.False(out reason);
				case CardTarget.AllAllies when game.playerHeroes.Count(t => t.character != caster && CheckRestrictions(card, t.character)) == 0: return CannotPlayReason.NoValidTarget.False(out reason);
				case CardTarget.AllAlliesAndSelf when game.playerHeroes.Count(t => CheckRestrictions(card, t.character)) == 0:                   return CannotPlayReason.NoValidTarget.False(out reason);
				case CardTarget.Opponent when game.opponentTeam.opponents.Count(t => CheckRestrictions(card, t.character)) == 0:                 return CannotPlayReason.NoValidTarget.False(out reason);
				case CardTarget.AllOpponents when game.opponentTeam.opponents.Count(t => CheckRestrictions(card, t.character)) == 0:             return CannotPlayReason.NoValidTarget.False(out reason);
				default:                                                                                                                         return CannotPlayReason.None.True(out reason);
			}
		}

		public static bool IsTargetSelectionRequired(Game game, Card card, Hero hero, out IEnumerable<GenericCharacter> candidates) => card.target switch {
			CardTarget.Self             => ((GenericCharacter[])null).False(out candidates),
			CardTarget.Ally             => game.playerHeroes.Except(t => t == hero).Select(t => t.character).Where(t => CheckRestrictions(card, t)).True(out candidates),
			CardTarget.AllyOrSelf       => game.playerHeroes.Select(t => t.character).Where(t => CheckRestrictions(card, t)).True(out candidates),
			CardTarget.AllAllies        => ((GenericCharacter[])null).False(out candidates),
			CardTarget.AllAlliesAndSelf => ((GenericCharacter[])null).False(out candidates),
			CardTarget.Opponent         => game.opponentTeam.opponents.Select(t => t.character).Where(t => CheckRestrictions(card, t)).True(out candidates),
			CardTarget.AllOpponents     => ((GenericCharacter[])null).False(out candidates),
			_                           => throw new ArgumentOutOfRangeException()
		};

		public static IEnumerable<GenericCharacter> SelectTargetsForUpcomingAction(Game game, Opponent opponent) => opponent.upcomingAction.target switch {
			CardTarget.Self => new[] { opponent.character }.Where(t => CheckRestrictions(opponent.upcomingAction, t)),
			CardTarget.Ally => new[] { game.opponentTeam.opponents.Where(t => CheckRestrictions(opponent.upcomingAction, t.character)).Except(t => t == opponent).RandomOrDefault()?.character }.NotNull(),
			CardTarget.AllyOrSelf => new[] { game.opponentTeam.opponents.Where(t => CheckRestrictions(opponent.upcomingAction, t.character)).RandomOrDefault()?.character }.NotNull(),
			CardTarget.AllAllies => game.opponentTeam.opponents.Where(t => CheckRestrictions(opponent.upcomingAction, t.character)).Except(t => t == opponent).Select(t => t.character),
			CardTarget.AllAlliesAndSelf => game.opponentTeam.opponents.Where(t => CheckRestrictions(opponent.upcomingAction, t.character)).Select(t => t.character),
			CardTarget.Opponent => new[] { game.playerHeroes.Where(t => CheckRestrictions(opponent.upcomingAction, t.character)).RandomOrDefault()?.character }.NotNull(),
			CardTarget.AllOpponents => game.playerHeroes.Select(t => t.character).Where(t => CheckRestrictions(opponent.upcomingAction, t)),
			_ => throw new ArgumentOutOfRangeException()
		};

		private static bool CheckRestrictions(Card card, GenericCharacter character) {
			if (card.targetMustBeAlive && character.dead) return false;
			foreach (var modifier in EnumUtils.Values<CharacterModifiers>()) {
				if (card.requiredModifiers.HasFlag(modifier) && !character.HasModifier(modifier)) return false;
				if (card.forbiddenModifiers.HasFlag(modifier) && character.HasModifier(modifier)) return false;
			}
			return true;
		}

		public static IEnumerator PlayEffect(Game game, Hero caster, Card card, GenericCharacter target, UnityAction callback) {
			var targets = target
				? new[] { target }
				: (card.target switch {
					CardTarget.Self             => new[] { caster.character },
					CardTarget.AllAllies        => game.playerHeroes.Except(t => t == caster).Select(t => t.character),
					CardTarget.AllAlliesAndSelf => game.playerHeroes.Select(t => t.character),
					CardTarget.AllOpponents     => game.opponentTeam.opponents.Select(t => t.character),
					CardTarget.Opponent         => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					CardTarget.AllyOrSelf       => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					CardTarget.Ally             => throw new ArgumentOutOfRangeException(nameof(target), "Cannot be null"),
					_                           => throw new ArgumentOutOfRangeException()
				}).Where(t => CheckRestrictions(card, t));

			return PlayEffect(caster.character, card, targets, callback);
		}

		public static IEnumerator PlayEffect(Opponent caster) => PlayEffect(caster.character, caster.upcomingAction, caster.upcomingActionTargets, null);

		private static IEnumerator PlayEffect(GenericCharacter caster, Card card, IEnumerable<GenericCharacter> targets, UnityAction callback) {
			var validTargets = targets.Where(t => CheckRestrictions(card, t)).ToArray();
			Debug.Log($"{caster.displayName} casts {card.displayName} on {string.Join(", ", validTargets.Select(t => t.displayName))}");
			caster.ConsumeMana(card.manaCost);
			var solver = GetActionSolver(card);
			yield return CoroutineRunner.Run(BattleUiData.uiPerCharacter[caster].portrait.Animate(card.animationData, card.icon, validTargets, () => validTargets.ForEach(t => {
				solver.Invoke(caster, t);
				if (card.action == CardAction.Attack && t.HasModifier(CharacterModifiers.Stinging)) caster.Damage(2);
				t.AddModifiers(card.additionModifiers);
			})));
			callback?.Invoke();
		}

		private static Action<GenericCharacter, GenericCharacter> GetActionSolver(Card card) => card.action switch {
			CardAction.Attack  => (caster, target) => target.Damage(card.GetStrength(caster.attributeSet)),
			CardAction.Heal    => (caster, target) => target.Heal(card.GetStrength(caster.attributeSet)),
			CardAction.Protect => (caster, target) => target.AddArmor(card.GetStrength(caster.attributeSet)),
			CardAction.GetMana => (caster, target) => target.ProduceMana(card.GetStrength(caster.attributeSet)),
			CardAction.LifeSteal => (caster, target) => {
				var amount = card.GetStrength(caster.attributeSet);
				caster.Heal(amount);
				target.Damage(amount);
			},
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}