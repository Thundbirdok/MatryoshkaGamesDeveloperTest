using System;

using UnityEngine;

using JetBrains.Annotations;

namespace CookingPrototype.Kitchen {
	[RequireComponent(typeof(FoodPlace))]
	public sealed class FoodTrasher : MonoBehaviour {

		FoodPlace _place = null;
		float	  _timer = 0f;

		int			   clickCounter = 0;
		float		   clickTime = 0;
		readonly float clickDelay = 0.5f;

		void Start() {
			_place = GetComponent<FoodPlace>();
			_timer = Time.realtimeSinceStartup;
		}

		/// <summary>
		/// Освобождает место по двойному тапу если еда на этом месте сгоревшая.
		/// </summary>
		[UsedImplicitly]
		public void TryTrashFood() {
			if ( clickCounter == 0 ) {
				++clickCounter;
				clickTime = Time.time;				
				return;
			}

			if ( Time.time > clickTime + clickDelay ) {
				++clickCounter;
				clickTime = Time.time;				
				return;
			}

			var food = _place.CurFood;
			if ( food == null ) {
				return;
			}

			if ( food.CurStatus == Food.FoodStatus.Overcooked ) {				
				_place.FreePlace();
			}
		}
	}
}
