using System;

using UnityEngine;

using CookingPrototype.Kitchen;
using CookingPrototype.UI;

using JetBrains.Annotations;

namespace CookingPrototype.Controllers {
	public sealed class GameplayController : MonoBehaviour {
		public static GameplayController Instance { get; private set; }

		public static bool isPause = true;

		public GameObject  TapBlock    = null;
		public StartWindow StartWindow = null;
		public WinWindow   WinWindow   = null;
		public LoseWindow  LoseWindow  = null;
		


		int _ordersTarget = 0;

		public int OrdersTarget {
			get { return _ordersTarget; }
			set {
				_ordersTarget = value;
				OrdersTargetChanged?.Invoke();
				TotalOrdersServedChanged?.Invoke();
			}
		}

		public event Action OrdersTargetChanged;

		public int        TotalOrdersServed { get; private set; } = 0;

		public event Action TotalOrdersServedChanged;

		void Awake() {
			if ( Instance != null ) {
				Debug.LogError("Another instance of GameplayController already exists");
			}
			Instance = this;

			StartWindow.Show();
		}

		void OnDestroy() {
			if ( Instance == this ) {
				Instance = null;
			}
		}

		void Init() {
			TotalOrdersServed = 0;
			Time.timeScale = 0f;
			isPause = true;

			TotalOrdersServedChanged?.Invoke();
		}

		public void CheckGameFinish() {
			if ( CustomersController.Instance.IsComplete ) {
				EndGame(TotalOrdersServed >= OrdersTarget);
			}
		}

		void EndGame(bool win) {
			Time.timeScale = 0f;
			TapBlock?.SetActive(true);
			if ( win ) {
				WinWindow.Show();
			} else {
				LoseWindow.Show();
			}
		}

		void HideWindows() {
			TapBlock?.SetActive(false);
			StartWindow?.Hide();
			WinWindow?.Hide();
			LoseWindow?.Hide();
		}

		[UsedImplicitly]
		public bool TryServeOrder(Order order) {
			if ( !CustomersController.Instance.ServeOrder(order) ) {
				return false;
			}

			TotalOrdersServed++;
			TotalOrdersServedChanged?.Invoke();
			CheckGameFinish();
			return true;
		}

		public void StartGame() {
			Time.timeScale = 1f;
			isPause = false;

			HideWindows();
		}

		public void RestartGame() {
			Init();
			CustomersController.Instance.Init();
			HideWindows();

			StartWindow.Show();

			foreach ( var place in FindObjectsOfType<AbstractFoodPlace>() ) {
				place.FreePlace();
			}
		}

		public void CloseGame() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
