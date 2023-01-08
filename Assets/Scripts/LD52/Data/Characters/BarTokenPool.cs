using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LD52.Data.Characters {
	public class BarTokenPool : MonoBehaviour {
		private static BarTokenPool instance { get; set; }

		private static Queue<Image> pool { get; } = new Queue<Image>();

		[SerializeField] protected Image _prefab;
		[SerializeField] protected Color _healthColor;
		[SerializeField] protected Color _emptyHealthColor;
		[SerializeField] protected Color _armorColor;
		[SerializeField] protected Color _manaColor;
		[SerializeField] protected Color _emptyManaColor;

		private void Start() {
			instance = this;
			pool.Clear();
			gameObject.SetActive(false);
		}

		public static void Pool(Image token) {
			pool.Enqueue(token);
			token.transform.SetParent(instance.transform);
		}

		public static Image CreateToken(RectTransform parent) {
			var token = pool.Count == 0 ? Instantiate(instance._prefab, parent) : pool.Dequeue();
			token.transform.SetParent(parent);
			return token;
		}

		public static void ColorizeHealth(Image token) => token.color = instance._healthColor;
		public static void ColorizeEmptyHealth(Image token) => token.color = instance._emptyHealthColor;
		public static void ColorizeArmor(Image token) => token.color = instance._armorColor;
		public static void ColorizeMana(Image token) => token.color = instance._manaColor;
		public static void ColorizeEmptyMana(Image token) => token.color = instance._emptyManaColor;
	}
}