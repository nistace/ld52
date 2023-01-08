using System.Linq;
using LD52.Data.Characters.Heroes;
using LD52.Data.Games;
using Utils.Extensions;

namespace LD52.Scenes.GameScene {
	public class RecruitHeroGameState : AbstractGameState {
		public static RecruitHeroGameState state     { get; } = new RecruitHeroGameState();

		private RecruitHeroGameState() { }

		protected override void Enable() {
			var randomHeroes = game.GetAvailableHeroesToRecruit().ToArray();

			if (randomHeroes.Length == 0) {
				ChangeStateToNextOutroStep(ScenarioStepReward.Character);
				return;
			}

			randomHeroes.Shuffle();
			ui.recruit.Set(randomHeroes[0], randomHeroes.Length > 1 ? randomHeroes[1] : null);
			ui.Show(GameUi.Panel.RecruitHero);

			RecruitHeroUi.onRecruit.AddListenerOnce(RecruitHero);
		}

		private static void RecruitHero(Hero hero) {
			game.RecruitHero(hero);
			ChangeStateToNextOutroStep(ScenarioStepReward.Character);
		}

		protected override void Disable() {
			RecruitHeroUi.onRecruit.RemoveListener(RecruitHero);
		}
	}
}