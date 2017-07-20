using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel_1 : LevelBehaviour {

	// Declares the stating positions of the users.
	protected override Vector3 StartPositions (int index) {
		Vector3[] arr = new Vector3[] {
			new Vector3 (0, 0.5f, 0),
			new Vector3 (5, 0.5f, 0),
			new Vector3 (-5, 0.5f, 0),
			new Vector3 (0, 0.5f, -5)
		};
		if (index < arr.Length) {
			return arr [index];
		} else {
			return new Vector3();
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
		
		return 30;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
