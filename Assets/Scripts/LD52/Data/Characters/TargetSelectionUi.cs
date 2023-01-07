using UnityEngine;
using UnityEngine.EventSystems;

namespace LD52.Data.Characters {
	public class TargetSelectionUi : MonoBehaviour, IPointerClickHandler {
		[SerializeField] protected GenericCharacter _character;

		public static GenericCharacter.Event onAnyClicked { get; } = new GenericCharacter.Event();

		public GenericCharacter character {
			get => _character;
			set => _character = value;
		}

		public void OnPointerClick(PointerEventData eventData) => onAnyClicked.Invoke(_character);
	}
}