using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD52.Data.Characters {
	public class CharacterBarUi : MonoBehaviour {
		[SerializeField] protected RectTransform    _healthBarContainer;
		[SerializeField] protected RectTransform    _armorBarContainer;
		[SerializeField] protected RectTransform    _manaBarContainer;
		[SerializeField] protected GenericCharacter _character;

		private List<Image> healthTokens { get; } = new List<Image>();
		private List<Image> armorTokens  { get; } = new List<Image>();
		private List<Image> manaTokens   { get; } = new List<Image>();

		public void Unset() {
			if (!_character) return;
			_character.onArmorChanged.RemoveListener(RefreshArmor);
			_character.onHealthChanged.RemoveListener(RefreshHealth);
			_character.onManaChanged.RemoveListener(RefreshMana);
		}

		public void Set(GenericCharacter character) {
			Unset();
			_character = character;
			if (!_character) return;
			_character.onArmorChanged.AddListener(RefreshArmor);
			_character.onHealthChanged.AddListener(RefreshHealth);
			_character.onManaChanged.AddListener(RefreshMana);
			RefreshArmor();
			RefreshHealth();
			RefreshMana();
		}

		private void RefreshMana() {
			while (manaTokens.Count < _character.maxMana) {
				var newToken = BarTokenPool.CreateToken(_manaBarContainer);
				manaTokens.Add(newToken);
			}

			while (manaTokens.Count > _character.maxMana) {
				BarTokenPool.Pool(manaTokens[0]);
				manaTokens.RemoveAt(0);
			}

			for (var i = 0; i < manaTokens.Count; ++i) {
				if (i < _character.mana) BarTokenPool.ColorizeMana(manaTokens[i]);
				else BarTokenPool.ColorizeEmptyMana(manaTokens[i]);
			}
		}

		private void RefreshHealth() {
			while (healthTokens.Count < _character.maxHealth) {
				var newToken = BarTokenPool.CreateToken(_healthBarContainer);
				healthTokens.Add(newToken);
			}

			while (healthTokens.Count > _character.maxHealth) {
				BarTokenPool.Pool(healthTokens[0]);
				healthTokens.RemoveAt(0);
			}

			for (var i = 0; i < healthTokens.Count; ++i) {
				if (i < _character.health) BarTokenPool.ColorizeHealth(healthTokens[i]);
				else BarTokenPool.ColorizeEmptyHealth(healthTokens[i]);
			}
		}

		private void RefreshArmor() {
			while (armorTokens.Count < _character.armor) {
				var newToken = BarTokenPool.CreateToken(_armorBarContainer);
				BarTokenPool.ColorizeArmor(newToken);
				armorTokens.Add(newToken);
			}

			while (armorTokens.Count > _character.armor) {
				BarTokenPool.Pool(armorTokens[0]);
				armorTokens.RemoveAt(0);
			}
		}
	}
}