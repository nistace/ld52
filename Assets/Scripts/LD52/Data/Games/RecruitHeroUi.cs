using LD52.Data.Attributes;
using LD52.Data.Cards;
using LD52.Data.Characters;
using LD52.Data.Characters.Heroes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class RecruitHeroUi : MonoBehaviour {
	[SerializeField] protected CharacterPortraitUi _portrait;
	[SerializeField] protected TMP_Text            _nameText;
	[SerializeField] protected TMP_Text[]          _attributeTexts;
	[SerializeField] protected SimpleCardUi[]      _cards;
	[SerializeField] protected Button              _recruitButton;

	private Hero hero { get; set; }

	public static Hero.Event onRecruit { get; } = new Hero.Event();

	private void Start() {
		_recruitButton.onClick.AddListenerOnce(() => onRecruit.Invoke(hero));
	}

	public void Set(Hero hero) {
		this.hero = hero;
		if (!this.hero) return;
		_portrait.Set(hero.character);
		_nameText.text = hero.displayName;
		foreach (var attribute in EnumUtils.Values<CharacterAttribute>()) {
			_attributeTexts[(int)attribute].text = $"<sprite name={attribute.ToString().ToLowerFirst()}> {hero.character.attributeSet[attribute]}";
		}
		for (var index = 0; index < hero.defaultCards.Count; index++) {
			_cards[index].Set(hero.defaultCards[index], hero.character);
		}
	}
}