using System.Collections.Generic;
using System.Linq;
using LD52.Data.Cards;
using LD52.Data.Characters;
using LD52.Data.Characters.Heroes;
using LD52.Data.Characters.Opponents;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utils.Extensions;

namespace LD52.Data.Games {
	public class GameBattleUi : MonoBehaviour {
		[SerializeField] protected HeroUi[]            _heroUis;
		[SerializeField] protected OpponentUi[]        _opponentUis;
		[SerializeField] protected BattlePlayingCardUi _playingCardUi;
		[SerializeField] protected Button              _endTurnButton;

		private Dictionary<Hero, HeroUi>         uiPerHero     { get; } = new Dictionary<Hero, HeroUi>();
		private Dictionary<Opponent, OpponentUi> uiPerOpponent { get; } = new Dictionary<Opponent, OpponentUi>();

		public UnityEvent onPlayingCardConfirmed => _playingCardUi.onConfirmClicked;
		public UnityEvent onPlayingCardCancelled => _playingCardUi.onCancelClicked;
		public UnityEvent onEndTurnClicked       => _endTurnButton.onClick;

		public void Initialize(IReadOnlyList<Hero> playerHeroes, OpponentTeam opponentTeam) {
			uiPerHero.Clear();
			for (var i = 0; i < _heroUis.Length; ++i) {
				_heroUis[i].gameObject.SetActive(playerHeroes.Count > i);
				if (playerHeroes.Count > i) {
					_heroUis[i].SetHero(playerHeroes[i]);
					uiPerHero.Add(playerHeroes[i], _heroUis[i]);
				}
			}
			uiPerOpponent.Clear();
			for (var i = 0; i < _opponentUis.Length; ++i) {
				_opponentUis[i].gameObject.SetActive(opponentTeam.opponents.Count > i);
				if (opponentTeam.opponents.Count > i) {
					_opponentUis[i].Set(opponentTeam.opponents[i]);
					uiPerOpponent.Add(opponentTeam.opponents[i], _opponentUis[i]);
				}
			}
			HideCardBeingPlayed();
		}

		public HeroUi GetHeroUi(Hero hero) => uiPerHero.ContainsKey(hero) ? uiPerHero[hero] : null;

		public void ShowCardBeingPlayedWithConfirm(Card card, GenericCharacter caster) => _playingCardUi.Show(card, caster, BattlePlayingCardUi.ExpectedAction.ConfirmOrCancel);

		public void ShowCardBeingPlayedCancelOnly(Card card, GenericCharacter caster, string message) => _playingCardUi.Show(card, caster, BattlePlayingCardUi.ExpectedAction.CancelOnly, message);

		public void ShowCardBeingPlayedWithTargets(Card card, GenericCharacter caster, string message, IEnumerable<GenericCharacter> acceptedTargets) {
			_playingCardUi.Show(card, caster, BattlePlayingCardUi.ExpectedAction.SelectTargetOrCancel, message);
			uiPerHero.Where(t => acceptedTargets.Contains(t.Key.character)).ForEach(t => t.Value.targetSelection.enabled = true);
			uiPerOpponent.Where(t => acceptedTargets.Contains(t.Key.character)).ForEach(t => t.Value.targetSelection.enabled = true);
		}

		public void HideCardBeingPlayed() {
			uiPerHero.Values.ForEach(t => t.targetSelection.enabled = false);
			uiPerOpponent.Values.ForEach(t => t.targetSelection.enabled = false);
			_playingCardUi.Hide();
		}

		public void SetEndTurnButtonVisible(bool visible) => _endTurnButton.gameObject.SetActive(visible);
	}
}