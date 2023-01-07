using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Ui;

namespace LD52.Data.Cards {
	public class FullCardUi : MonoBehaviourUi {
		[SerializeField] protected Image    _cardActionIcon;
		[SerializeField] protected TMP_Text _cardName;
		[SerializeField] protected TMP_Text _description;

		public void Set(Card card) {
			_cardActionIcon.sprite = card.icon;
			_cardName.text = card.displayName;
			_description.text = card.description;
		}
	}
}