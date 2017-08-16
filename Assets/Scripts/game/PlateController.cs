using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// Controlls the behaviour of the PressurePlates
/// </summary>
public class PlateController : SoftwareBehaviour {

	private List<PressurePlate> platesInScene = new List<PressurePlate> ();
	private DoorScript doorScript;

	/// <summary>
	/// Adds plates to the list of contained PressurePlates
	/// </summary>
	/// <param name="plates">Plates.</param>
	public void AddPlates(PressurePlate[] plates) {

		for (int i = 0; i < plates.Length; i++) {
			platesInScene.Add (plates[i]);
		}
	}

	/// <summary>
	/// Gets the plate count.
	/// </summary>
	/// <returns>The plate count.</returns>
	public int GetPlateCount() {
		return platesInScene.Count;
	}

	/// <summary>
	/// Sets the states for all PressurePlates in the current Scene.
	/// Tells the DoorScript, if all PressurePlates are currently active.
	/// </summary>
	/// <param name="plateId">Plate identifier.</param>
	/// <param name="plateIsActive">If set to <c>true</c> plate is active.</param>
	public void SetPlateState(int plateId, bool plateIsActive) {
		
		bool allPlatesAreActive = true;
		for (int i = 0; i < platesInScene.Count; i++) {
			if (platesInScene [i].Id == plateId && i < platesInScene.Count) {
				platesInScene [i].SetPlateActive (plateIsActive);
			}
			// check, if one of the plates is inactive, meaning the door is closed.
			if (!platesInScene [i].IsActive) {
				allPlatesAreActive = false;
			}
		}
		// Tell the door, if it is supposed to open:
		SoftwareModel.doorScript.OpenTheDoor (allPlatesAreActive);
	}

	/// <summary>
	/// Assigns the database ids of server to the PressurePlates.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
	/// </summary>
	/// <param name="response">Response.</param>
	public void AssignThePlateIds(string[][] response) {

		foreach (string[] pair in response) {
			if (pair [0].Equals ("plateIds") && pair [1] != null) {
				string pattern = @"//";
				string[] plateIds = Regex.Split (pair[1], pattern);
				for (int i = 0; i < plateIds.Length; i++) {
					if(i < platesInScene.Count) {
						int tmpId = -1;
						int.TryParse (plateIds [i], out tmpId);
						platesInScene [i].Id = tmpId;
					}
				}
				return;
			}
		}
	}
}
