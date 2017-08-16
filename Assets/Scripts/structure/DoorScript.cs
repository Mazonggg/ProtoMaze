using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the behaviour of the door, that is blocking the FinisherPlate.
/// </summary>
public class DoorScript : SoftwareBehaviour {

	/// <summary>
	/// Opens the door, that hinders the users from leaving the level.
	/// </summary>
	/// <param name="openIt">If set to <c>true</c> open it.</param>
	public void OpenTheDoor(bool openIt) {
		
		gameObject.SetActive (!openIt);
	}
}
