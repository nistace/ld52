using UnityEngine;
using Utils.Aseprite;

namespace LD52.Assets {
	public class AssetLibrary : MonoBehaviour {
		private static AssetLibrary  instance       { get; set; }
		public static  AsepriteSheet vegetableSheet => instance._vegetableSheet;
		public static  AsepriteSheet cardSheet      => instance._cardSheet;
		public static  AsepriteSheet modifiers      => instance._modifiers;

		[SerializeField] protected AsepriteSheet _vegetableSheet;
		[SerializeField] protected AsepriteSheet _cardSheet;
		[SerializeField] protected AsepriteSheet _modifiers;

		private void Awake() {
			instance = this;
		}
	}
}