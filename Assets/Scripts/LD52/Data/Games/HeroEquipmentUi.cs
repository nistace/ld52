using LD52.Data.Attributes;
using LD52.Data.Cards;
using LD52.Data.Characters;
using LD52.Data.Characters.Heroes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Utils.Extensions;

public class HeroEquipmentUi : MonoBehaviour {
	public class DragAndDropEvent : UnityEvent<Hero, int, GameObject> { }

	[SerializeField] protected CharacterPortraitUi _portrait;
	[SerializeField] protected TMP_Text            _nameText;
	[SerializeField] protected TMP_Text[]          _attributes;
	[SerializeField] protected SimpleCardUi[]      _cards;

	public Hero hero { get; private set; }

	public static DragAndDropEvent onDrag { get; } = new DragAndDropEvent();
	public static DragAndDropEvent onDrop { get; } = new DragAndDropEvent();

	private void Start() {
		_cards.ForEach(t => {
			t.onPointerDown.AddListenerOnce(HandlePointerDownOnCard);
			t.onPointerUp.AddListenerOnce(HandlePointerUpOnCard);
		});
	}

	private void HandlePointerDownOnCard(SimpleCardUi card, RaycastResult raycast) => onDrag.Invoke(hero, _cards.IndexOf(card), raycast.gameObject);
	private void HandlePointerUpOnCard(SimpleCardUi card, RaycastResult raycast) => onDrop.Invoke(hero, _cards.IndexOf(card), raycast.gameObject);

	public void Set(Hero hero) {
		this.hero?.onDeckChanged.RemoveListener(Refresh);
		this.hero = hero;
		gameObject.SetActive(this.hero);
		if (!this.hero) return;

		_nameText.text = this.hero.displayName;
		_portrait.Set(this.hero.character);
		Refresh();
		this.hero.onDeckChanged.AddListenerOnce(Refresh);
	}

	private void Refresh() {
		if (!hero) return;
		foreach (var attribute in EnumUtils.Values<CharacterAttribute>()) {
			_attributes[(int)attribute].text = $"<sprite name={attribute.ToString().ToLowerFirst()}> {hero.character.attributeSet[attribute]}";
		}
		for (var i = 0; i < _cards.Length; ++i) {
			_cards[i].Set(hero.deck.allCards[i], hero.character);
			_cards[i].color = Color.white;
			_cards[i].backgroundColor = hero.IsInitialCard(i) ? Color.grey : Color.white;
		}
	}

	public int GetIndexOf(SimpleCardUi card) => _cards.IndexOf(card);

	public void ShowCardAsBeingDragged(int index) => _cards[index].color = new Color(1, 1, 1, .5f);

	public void CancelAllCardsBeingDragged() => Refresh();
}