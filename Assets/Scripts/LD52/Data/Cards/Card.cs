using System;
using System.Linq;
using LD52.Assets;
using LD52.Data.Attributes;
using LD52.Data.Modifiers;
using UnityEngine;
using UnityEngine.Events;
using Utils.Extensions;

namespace LD52.Data.Cards {
	[CreateAssetMenu]
	public class Card : ScriptableObject {
		public class Event : UnityEvent<Card> { }

		[SerializeField]                       protected string                  _displayName;
		[SerializeField]                       protected string                  _iconName;
		[SerializeField]                       protected string                  _description;
		[SerializeField]                       protected CardAction              _action;
		[SerializeField]                       protected CharacterModifiers      _additionModifiers;
		[SerializeField]                       protected CharacterModifiers      _removedModifiers;
		[SerializeField]                       protected CardEffectAnimationData _animationData = new CardEffectAnimationData();
		[SerializeField]                       protected CardTarget              _target;
		[SerializeField]                       protected CharacterAttribute      _attribute           = CharacterAttribute.Attack;
		[SerializeField]                       protected int                     _attributeMultiplier = 1;
		[SerializeField]                       protected int                     _constantStrength;
		[SerializeField]                       protected int                     _level = 1;
		[SerializeField]                       protected CharacterAttributeValue _attributeBonus;
		[Header("Conditions"), SerializeField] protected int                     _manaCost;
		[SerializeField]                       protected bool                    _targetMustBeAlive = true;
		[SerializeField]                       protected CharacterModifiers      _requiredModifiers;
		[SerializeField]                       protected CharacterModifiers      _forbiddenModifiers = CharacterModifiers.Buried;

		public string                  displayName        => _displayName;
		public Sprite                  icon               => AssetLibrary.cardSheet[$"{_iconName}.default.000"];
		public CardAction              action             => _action;
		public string                  description        => _description;
		public int                     level              => _level;
		public CharacterAttributeValue attributeBonus     => _attributeBonus;
		public CardTarget              target             => _target;
		public bool                    targetMustBeAlive  => _targetMustBeAlive;
		public CharacterModifiers      requiredModifiers  => _requiredModifiers;
		public CharacterModifiers      forbiddenModifiers => _forbiddenModifiers;
		public int                     manaCost           => _manaCost;
		public CharacterModifiers      additionModifiers  => _additionModifiers;
		public CardEffectAnimationData animationData      => _animationData;

		public int GetStrength(IReadCharacterAttributeSet attributeSet) => attributeSet[_attribute] * _attributeMultiplier + _constantStrength;

		public string DisplayCardValue(IReadCharacterAttributeSet attributeSet) {
			if (attributeSet == null) return DisplayCardValue();
			if (GetStrength(attributeSet) == 0) return string.Empty;
			return $"{DisplayActionName()} {GetStrength(attributeSet)} ({DisplayGenericCardValue()})";
		}

		private string DisplayCardValue() {
			if (_attributeMultiplier == 0 && _constantStrength == 0) return "";
			return $"{DisplayActionName()} {DisplayGenericCardValue()}";
		}

		private string DisplayGenericCardValue() {
			if (_attributeMultiplier == 0 && _constantStrength == 0) return "";
			if (_attributeMultiplier == 0) return $"{_constantStrength}";
			if (_constantStrength == 0) return $"{_attributeMultiplier}<sprite name={_attribute.ToString().ToLowerFirst()}>";
			return $"{_attributeMultiplier}<sprite name={_attribute.ToString().ToLowerFirst()}> + {_constantStrength}";
		}

		public string DisplayImpactOnModifiers() => string.Join(", ",
			EnumUtils.Values<CharacterModifiers>().Where(t => additionModifiers.HasFlag(t)).Select(t => $"+<sprite name=modifier_{t.ToString().ToLowerFirst()}>")
				.Union(EnumUtils.Values<CharacterModifiers>().Where(t => _removedModifiers.HasFlag(t)).Select(t => $"-<sprite name=modifier_{t.ToString().ToLowerFirst()}>")));

		private string DisplayActionName() => _action switch {
			CardAction.Attack    => "Attack",
			CardAction.Heal      => "Heal",
			CardAction.Protect   => "Shield",
			CardAction.GetMana   => "Mana",
			CardAction.LifeSteal => "Life Steal",
			_                    => throw new ArgumentOutOfRangeException()
		};
	}
}