using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class describes the behaviour of PressurePlates.
/// Written in a way, that circumvents convenient method calls,
/// that could potentially slow down the flow of the game. Thus
/// it mostly uses the methods given by unity engine.
/// </summary>
public class PressurePlate : SoftwareBehaviour {

	public GameObject lightActive, ligtInactive;
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
	private float timeWhenLeft = 0;
	/// <summary>
	/// Deactivate light, that signals "active" status of the plate.
	/// This prevents issues with initilisation of objects.
	/// </summary>
	void Start () {
		lightActive.SetActive (false);
	}
	/// <summary>
	/// Constantly called while game is running.
	/// </summary>
	void Update () {
		if (timeOut > 0 && isActive && timeWhenLeft + timeOut <= Time.realtimeSinceStartup) {
			isActive = false;
			lightActive.SetActive (false);
			ligtInactive.SetActive (true);
		}
	}
	/// <summary>
	/// Catches the event, that a collision has started touching this collider.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter (Collision collision) {
		isActive = true;	
		lightActive.SetActive (true);
		ligtInactive.SetActive (false);
		timeWhenLeft = Time.realtimeSinceStartup;
	}

	/// <summary>
	/// Catches the event, that a collision has stopped touching this collider.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionExit (Collision collision) {
		timeWhenLeft = Time.realtimeSinceStartup;
	}
}
