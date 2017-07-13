using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel_1 : MonoBehaviour {

	// Declares the stating positions of the users.
	private static Vector3[] startPositions = {
		new Vector3 (0, 0, 0),
		new Vector3 (5, 0, 0),
		new Vector3 (-5, 0, 0),
		new Vector3 (0, -5, 0)
	};
	// Declares the time given to complete the level.
	private static int timer = 120;
	/// <summary>
	/// Gets the start position for a specific index of player in UserHandler.
	/// </summary>
	/// <returns>The start position.</returns>
	/// <param name="index">Index.</param>
	public static Vector3 GetStartPosition (int index) {

		return startPositions[index];
	}
	// Use this for initialization
	void Start () {
		
		GameObject.Find (Constants.softwareModel).GetComponent<SoftwareModel> ().CreateSocketObject (120);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
