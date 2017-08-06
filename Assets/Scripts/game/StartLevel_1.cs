using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel_1 : LevelBehaviour {

	protected override Vector3 StartPositions (int index) {
		Vector3[] arr = new Vector3[] {

			new Vector3 (-52f, 0.7f, 46.8f),
			new Vector3 (-52f, 0.7f, 55.9f),
			new Vector3 (-59.8f, 0.7f, 46.8f),
			new Vector3 (-59.8f, 0.7f, 55.9f)
		};
		if (index < arr.Length) {
			return arr [index];
		} else {
			return new Vector3();
		}
	}

	protected override Vector3 StartRotations (int index) {
		Vector3[] arr = new Vector3[] {

			new Vector3 (0, 135f, 0),
			new Vector3 (0, 90f, 0),
			new Vector3 (0, 180f, 0),
			new Vector3 (0, 135f, 0)
		};
		if (index < arr.Length) {
			return arr [index];
		} else {
			return new Vector3();
		}
	}

	protected override int PlateTimeOuts (int playerCount) {
		switch (playerCount) {
		case 2:
			return 30;
			break;
		case 3:
			return 10;
			break;
		case 4:
			return 0;
			break;
		default:
			return -1;
			break;
		}
	}

	protected override void FinishLevel() {

	}

	/// <summary>
	/// Gets the timer, that determines how much time 
	/// the user have to complete the level, before timeout.
	/// </summary>
	/// <returns>The timer.</returns>
	protected override int GetTimer () {
		
		return 60;
	}
}
