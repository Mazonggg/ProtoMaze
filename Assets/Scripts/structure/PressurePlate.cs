using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class describes the behaviour of PressurePlates.
/// Written in a way, that circumvents convenient method calls,
/// that could potentially slow down the flow of the game. Thus
/// it mostly uses the methods given by unity engine.
/// </summary>
public class PressurePlate : GObject {
	// GameObject parameters
	public GameObject lightActive, lightInactive;
	/// <summary>
	/// Sets the time elapsed, before plate is deactivated.
	/// Implemented static to work with 
	/// </summary>
	/// <returns>The out.</returns>
	private static int timeOut = -1;
	public static int TimeOut {
		set { timeOut = value; }
	}
	/// <summary>
	/// Determines, if this PressurePlate is active, 
	/// so the game can react accordingly.
	/// </summary>
	private bool isActive = false;
	public bool IsActive {
		get { return isActive; }
	}
	/// <summary>
	/// Deactivate light, that signals "active" status of the plate.
	/// This prevents issues with initilisation of objects.
	/// </summary>
	void Start () {
		lightActive.SetActive (false);
	}
	/// <summary>
	/// Sets the state of the plate.
	/// </summary>
	/// <param name="plateIsActive">If set to <c>true</c> plate is active.</param>
	public void SetPlateActive (bool plateIsActive) {

		isActive = plateIsActive;	
		lightActive.SetActive (plateIsActive);
		lightInactive.SetActive (!plateIsActive);
	}
	/// <summary>
	/// Catches the event, that a collision has started touching this collider.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter (Collision collision) {
		SendActivation ();
	}
	/// <summary>
	/// Catches the event, that a collision has stopped touching this collider.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionExit (Collision collision) {
		SendActivation ();
	}
	/// <summary>
	/// Activates the plate on server via TCP request.
	/// </summary>
	private void SendActivation() {
		SoftwareModel.netwRout.TCPRequest (
			NetworkRoutines.EmptyCallback,
			new string[] { "req", "sessionId", "plateId", "timeout" }, 
			new string[] { "activatePlate", UserStatics.SessionId.ToString (), Id.ToString (), timeOut.ToString () });
	}
}
