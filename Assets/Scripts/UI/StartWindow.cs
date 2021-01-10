using UnityEngine;
using UnityEngine.UI;

using CookingPrototype.Controllers;

using TMPro;

namespace CookingPrototype.UI {
	public sealed class StartWindow : MonoBehaviour {
		public Image    GoalBar     = null;
		public TMP_Text GoalText    = null;
		public Button   OkButton    = null;
		public Button   CloseButton = null;

		bool _isInit = false;

		void Init() {
			var gc = GameplayController.Instance;

			gc.OrdersTargetChanged += OnOrdersTargetChanged;

			OkButton.onClick.AddListener(gc.StartGame);
			CloseButton.onClick.AddListener(gc.CloseGame);
		}

		public void Show() {
			if ( !_isInit ) {
				Init();
			}								

			gameObject.SetActive(true);
		}

		public void Hide() {
			gameObject.SetActive(false);
		}

		void OnOrdersTargetChanged() {
			var gc = GameplayController.Instance;
			GoalText.text = $"{gc.OrdersTarget}";
		}
	}
}
