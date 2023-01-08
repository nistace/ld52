using System.Linq;
using LD52.Assets;
using LD52.Data.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;
using Utils.Ui;

namespace LD52.Data.Cards {
	public class FullCardUi : MonoBehaviourUi {
		[SerializeField] protected Image    _cardActionIcon;
		[SerializeField] protected TMP_Text _cardName;
		[SerializeField] protected TMP_Text _value;
		[SerializeField] protected Image    _targetIcon;
		[SerializeField] protected TMP_Text _manaCostText;
		[SerializeField] protected TMP_Text _description;
		[SerializeField] protected TMP_Text _equippedInfo;
		[SerializeField] protected bool     _showEquippedInfo;

		private void Start() => SetEquippedInfoVisible(_showEquippedInfo);

		public void Set(Card card, GenericCharacter cardOwner) {
			_cardActionIcon.sprite = card.icon;
			_cardName.text = card.displayName;
			_value.text = string.Join("<br>", new[] { card.DisplayCardValue(cardOwner?.attributeSet), card.DisplayImpactOnModifiers() }.Where(t => !string.IsNullOrEmpty(t)));
			_targetIcon.sprite = AssetLibrary.cardSheet[$"target_{card.target.ToString().ToLowerFirst()}.default.000"];
			_manaCostText.text = $"<sprite name=mana> {card.manaCost}";
			_description.text = card.description;
			_equippedInfo.text = card.attributeBonus.value > 0 ? $"Equipped: <sprite name={card.attributeBonus.attribute.ToString().ToLowerFirst()}> +{card.attributeBonus.value}" : string.Empty;
		}

		public void SetEquippedInfoVisible(bool visible) {
			_showEquippedInfo = visible;
			_equippedInfo.enabled = _showEquippedInfo;
		}
	}
}