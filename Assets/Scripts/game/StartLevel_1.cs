using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel_1 : MonoBehaviour {

	// Declares the stating positions of the users.
	private static Vector3[] startPositions = {
		new Vector3 (0, 0.5f, 0),
		new Vector3 (5, 0.5f, 0),
		new Vector3 (-5, 0.5f, 0),
		new Vector3 (0, 0.5f, -5)
	};
	// Declares the time given to complete the level.
	private static int timer = 10;
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
		
		GameObject.Find (Constants.softwareModel).GetComponent<SoftwareModel> ().CreateSocketObject (timer);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
