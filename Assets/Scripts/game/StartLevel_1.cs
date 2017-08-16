using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LevelBehaviour script for the first level in the game.
/// Override functions are explained in base class.
/// </summary>
public class StartLevel_1 : LevelBehaviour {

	public override string SceneName () {
		return "Level_1";
	}

	public override string ExplanationText () {
		return "You have " + GetTimer () + " seconds to reach the escape zone.";
	}

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
		case 3:
			return 10;
		case 4:
			return 0;
		default:
			return -1;
		}
	}

	protected override int GetTimer () {
		
		return 60;
	}
}
