using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class describes the behaviour of FinisherPlates,
/// that complete the session, when stepped on.
/// </summary>
public class FinisherPlate : GObject {

	public GameObject lightInactive, lightActive;

	/// <summary>
	/// Sets the state of the plate.
	/// </summary>
	/// <param name="plateIsActive">If set to <c>true</c> plate is active.</param>
	public void SetPlateActive () {
		lightActive.SetActive (true);
		lightInactive.SetActive (false);
	}

	/// <summary>
	/// Catches the event, that a collision has started touching this collider.
	/// Sends request to finish the session
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter (Collision collision) {
		SoftwareModel.netwRout.TCPRequest (
			NetworkRoutines.EmptyCallback,
			new string[] { "req", "sessionId" }, 
			new string[] { "finishSession", UserStatics.SessionId.ToString () });
	}
}
