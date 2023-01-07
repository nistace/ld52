﻿using System;
using System.Collections;
using System.Linq;
using LD52.Data.Cards;
using LD52.Data.Characters;
using LD52.Data.Characters.Heroes;
using LD52.Data.Games;
using UnityEngine;
using Utils.Coroutines;
using Utils.Extensions;

namespace LD52.Scenes.GameScene {
	public class BattleGameState : AbstractGameState {
		public static BattleGameState state { get; } = new BattleGameState();

		private Hero selectedCardHero { get; set; }
		private Card selectedCard     { get; set; }

		private BattleGameState() { }

		protected override void Enable() {
			var opponentTeam = game.InstantiateOpponentTeam();

			game.playerHeroes.ForEach(t => t.PrepareForBattle());
			opponentTeam.opponents.ForEach(t => t.PrepareForBattle());

			ui.battle.Initialize(game.playerHeroes, opponentTeam);
			ui.Show(GameUi.Panel.Battle);
			CoroutineRunner.Run(PlayIntro());
		}

		private IEnumerator PlayIntro() {
			// TODO HANDLE INTRO
			CardZoomUi.instanceEnabled = true;
			yield return CoroutineRunner.Run(StartTurn());
		}

		private static IEnumerator PlayOutro() {
			CardZoomUi.instanceEnabled = false;
			// TODO HANDLE OUTRO
			yield return new WaitForSeconds(1);
			ChangeStateToNextOutroStep(null);
		}

		private IEnumerator StartTurn() {
			Coroutine waitCoroutine = null;

			foreach (var hero in game.playerHeroes.Where(hero => hero.deck.stackSize == 0)) {
				hero.deck.Shuffle();
				waitCoroutine = CoroutineRunner.Run(ui.battle.GetHeroUi(hero).MoveDiscardToStack());
			}
			if (waitCoroutine != null) {
				yield return waitCoroutine;
				yield return new WaitForSeconds(.5f);
			}

			foreach (var hero in game.playerHeroes) {
				hero.deck.DrawNext();
				hero.deck.DrawNext();
				waitCoroutine = CoroutineRunner.Run(ui.battle.GetHeroUi(hero).DrawCards(hero.deck.drawnCards));
			}
			yield return waitCoroutine;
			yield return new WaitForSeconds(.5f);

			HeroUi.onDrawnCardClicked.AddListenerOnce(HandleHeroCardClicked);
			ui.battle.SetEndTurnButtonVisible(true);
			ui.battle.onEndTurnClicked.AddListenerOnce(EndTurn);
		}

		private void EndTurn() {
			HeroUi.onDrawnCardClicked.RemoveListeners(HandleHeroCardClicked);
			ui.battle.onEndTurnClicked.RemoveListeners(EndTurn);

			CoroutineRunner.Run(DoEndTurn());
		}

		private IEnumerator DoEndTurn() {
			Coroutine waitCoroutine = null;
			ui.battle.SetEndTurnButtonVisible(false);

			foreach (var hero in game.playerHeroes.Where(hero => hero.deck.drawnSize > 0)) {
				hero.deck.DiscardDrawnCards();
				waitCoroutine = CoroutineRunner.Run(ui.battle.GetHeroUi(hero).MoveDrawnToDiscard());
			}
			if (waitCoroutine != null) {
				yield return waitCoroutine;
				yield return new WaitForSeconds(.5f);
			}

			// TODO play opponent turn;

			yield return new WaitForSeconds(2);

			yield return CoroutineRunner.Run(StartTurn());
		}

		private void HandleHeroCardClicked(Hero hero, int cardIndex) {
			HeroUi.onDrawnCardClicked.RemoveListener(HandleHeroCardClicked);
			ui.battle.SetEndTurnButtonVisible(false);

			selectedCardHero = hero;
			selectedCard = selectedCardHero.deck.drawnCards[cardIndex];

			if (!CardEffectManager.CheckCardCanBePlayed(game, selectedCard, selectedCardHero, out var reason)) {
				ui.battle.ShowCardBeingPlayedCancelOnly(selectedCard, reason switch {
					CardEffectManager.CannotPlayReason.None          => "Cannot play this card",
					CardEffectManager.CannotPlayReason.NotEnoughMana => "Not enough mana",
					CardEffectManager.CannotPlayReason.CasterDead    => "Dead hero",
					CardEffectManager.CannotPlayReason.NoValidTarget => "No valid target",
					_                                                => throw new ArgumentOutOfRangeException()
				});
			}
			else if (CardEffectManager.IsTargetSelectionRequired(game, selectedCard, selectedCardHero, out var candidates)) {
				ui.battle.ShowCardBeingPlayedWithTargets(selectedCard, "Select target", candidates);
				TargetSelectionUi.onAnyClicked.AddListenerOnce(HandleTargetSelected);
			}
			else {
				ui.battle.ShowCardBeingPlayedWithConfirm(selectedCard);
				ui.battle.onPlayingCardConfirmed.AddListenerOnce(HandlePlayingCardConfirmed);
			}
			ui.battle.onPlayingCardCancelled.AddListenerOnce(HandlePlayingCardCancelled);
		}

		private void HandlePlayingCardConfirmed() {
			HidePlayingCard();
			selectedCardHero.deck.DiscardDrawnCards();
			CoroutineRunner.Run(ui.battle.GetHeroUi(selectedCardHero).MoveDrawnToDiscard());
			CoroutineRunner.Run(CardEffectManager.PlayEffect(game, selectedCardHero, selectedCard, null, ResumePlayerTurn));
		}

		private void HandleTargetSelected(GenericCharacter target) {
			HidePlayingCard();
			selectedCardHero.deck.DiscardDrawnCards();
			CoroutineRunner.Run(ui.battle.GetHeroUi(selectedCardHero).MoveDrawnToDiscard());
			CoroutineRunner.Run(CardEffectManager.PlayEffect(game, selectedCardHero, selectedCard, target, ResumePlayerTurn));
		}

		private void HidePlayingCard() {
			ui.battle.HideCardBeingPlayed();
			TargetSelectionUi.onAnyClicked.RemoveListener(HandleTargetSelected);
			ui.battle.onPlayingCardCancelled.RemoveListener(HandlePlayingCardCancelled);
			ui.battle.onPlayingCardConfirmed.RemoveListener(HandlePlayingCardConfirmed);
		}

		private static bool IsGameOver() {
			if (game.opponentTeam.opponents.All(t => t.character.dead)) return true;
			if (game.playerHeroes.All(t => t.character.dead)) return true;
			return false;
		}

		private void ResumePlayerTurn() {
			if (IsGameOver()) {
				CoroutineRunner.Run(PlayOutro());
				return;
			}
			if (game.playerHeroes.All(t => t.deck.drawnSize == 0 || t.character.dead)) {
				EndTurn();
				return;
			}
			ui.battle.SetEndTurnButtonVisible(true);
			HeroUi.onDrawnCardClicked.AddListenerOnce(HandleHeroCardClicked);
		}

		private void HandlePlayingCardCancelled() {
			HidePlayingCard();
			HeroUi.onDrawnCardClicked.AddListenerOnce(HandleHeroCardClicked);
		}

		protected override void Disable() { }
	}
}