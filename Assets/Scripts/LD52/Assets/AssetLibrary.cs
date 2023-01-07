using UnityEngine;
using Utils.Aseprite;

namespace LD52.Assets {
	public class AssetLibrary : MonoBehaviour {
		private static AssetLibrary  instance       { get; set; }
		public static  AsepriteSheet vegetableSheet => instance._vegetableSheet;
		public static  AsepriteSheet cardSheet      => instance._cardSheet;

		[SerializeField] protected AsepriteSheet _vegetableSheet;
		[SerializeField] protected AsepriteSheet _cardSheet;

		private void Awake() {
			instance = this;
		}
	}
}