using System;
using System.Collections.Generic;
using LD52.Assets;
using LD52.Data.Attributes;
using LD52.Data.Modifiers;
using UnityEngine;
using UnityEngine.Events;

namespace LD52.Data.Characters {
	public class GenericCharacter : MonoBehaviour {
		public class Event : UnityEvent<GenericCharacter> { }

		[SerializeField] protected string                _portraitName;
		[SerializeField] protected string                _displayName;
		[SerializeField] protected CharacterAttributeSet _attributeSet = new CharacterAttributeSet();
		[SerializeField] protected CharacterModifiers    _characterModifiers;
		[SerializeField] protected int                   _health;
		[SerializeField] protected int                   _armor;
		[SerializeField] protected int                   _mana;

		public int                   health       => _health;
		public int                   armor        => _armor;
		public int                   mana         => _mana;
		public CharacterAttributeSet attributeSet => _attributeSet;
		public int                   maxHealth    => _attributeSet[CharacterAttribute.Health];
		public int                   maxMana      => _attributeSet[CharacterAttribute.Mana];

		public  string                                       displayName      => _displayName;
		private Dictionary<CharacterAnimation, List<Sprite>> sprites          { get; }      = new Dictionary<CharacterAnimation, List<Sprite>>();
		private CharacterAnimation                           currentAnimation { get; set; } = CharacterAnimation.Idle;

		public Sprite portrait => sprites.ContainsKey(currentAnimation) ? sprites[currentAnimation][Mathf.FloorToInt(Time.time * 3) % sprites[currentAnimation].Count] : default;

		public UnityEvent onHealthChanged    { get; } = new UnityEvent();
		public UnityEvent onArmorChanged     { get; } = new UnityEvent();
		public UnityEvent onManaChanged      { get; } = new UnityEvent();
		public UnityEvent onModifiersChanged { get; } = new UnityEvent();
		public bool       dead               => health <= 0;
		public bool       alive              => !dead;

		private void Reset() {
			_portraitName = name;
			_displayName = name;
		}

		private void Start() {
			foreach (var characterAnimation in EnumUtils.Values<CharacterAnimation>()) {
				var sprite = AssetLibrary.vegetableSheet[$"{_portraitName}.{characterAnimation.ToString().ToLower()}.000"];
				if (sprite) sprites.Add(characterAnimation, new List<Sprite>());
				for (var i = 1; sprite; i++) {
					sprites[characterAnimation].Add(sprite);
					sprite = AssetLibrary.vegetableSheet[$"{_portraitName}.{characterAnimation.ToString().ToLower()}.{i:000}"];
				}
			}
		}

		public void PrepareForBattle() {
			_health = maxHealth;
			_armor = 0;
			_mana = maxMana;
		}

		public void AddArmor(int armor) {
			if (armor == 0) return;
			_armor += armor;
			onArmorChanged.Invoke();
		}

		public void Heal(int heal) {
			if (heal == 0) return;
			if (_health == maxHealth) return;
			_health = Math.Clamp(_health + heal, 0, maxHealth);
			onHealthChanged.Invoke();
		}

		public void Damage(int damage) {
			if (damage == 0) return;
			if (health == 0) return;
			var damageToArmor = Math.Min(armor, damage);
			var damageToHealth = Math.Clamp(damage - damageToArmor, 0, _health);
			if (damageToArmor > 0) _armor -= damageToArmor;
			if (damageToHealth > 0) _health -= damageToHealth;
			currentAnimation = DetermineAnimation();
			if (damageToArmor > 0) onArmorChanged.Invoke();
			if (damageToHealth > 0) onHealthChanged.Invoke();
		}

		public void ConsumeMana(int mana) {
			if (mana == 0) return;
			if (_mana == 0) return;
			_mana = Math.Clamp(_mana - mana, 0, maxMana);
			onManaChanged.Invoke();
		}

		public void ProduceMana(int mana) {
			if (mana == 0) return;
			if (_mana == maxMana) return;
			_mana = Math.Clamp(_mana + mana, 0, maxMana);
			onManaChanged.Invoke();
		}

		public void AddModifiers(CharacterModifiers modifiers) {
			_characterModifiers |= modifiers;
			currentAnimation = DetermineAnimation();
			onModifiersChanged.Invoke();
		}

		public void RemoveModifiers(CharacterModifiers modifiers) {
			_characterModifiers &= ~modifiers;
			currentAnimation = DetermineAnimation();
			onModifiersChanged.Invoke();
		}

		public void RefreshModifiersForNewTurn() {
			if (dead) _characterModifiers = 0;
			if (_characterModifiers.HasFlag(CharacterModifiers.Poisoned)) Damage(1);
			_characterModifiers &= CharacterModifiers.Poisoned | CharacterModifiers.Worm;
			currentAnimation = DetermineAnimation();
			onModifiersChanged.Invoke();
		}

		private CharacterAnimation DetermineAnimation() {
			if (dead) return CharacterAnimation.Dead;
			if (_characterModifiers.HasFlag(CharacterModifiers.Buried)) return CharacterAnimation.Buried;
			return CharacterAnimation.Idle;
		}

		public bool HasModifier(CharacterModifiers modifier) => _characterModifiers.HasFlag(modifier);
	}
}