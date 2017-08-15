using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PlateController : SoftwareBehaviour {

	private List<PressurePlate> platesInScene = new List<PressurePlate> ();
	private DoorScript doorScript;

	public void AddPlates(PressurePlate[] plates) {

		for (int i = 0; i < plates.Length; i++) {
			platesInScene.Add (plates[i]);
		}
	}

	public int GetPlateCount() {
		return platesInScene.Count;
	}

	/// <summary>
	/// Sets the states for all PressurePlates in the current Scene.
	/// </summary>
	/// <param name="platesAreActive">Plates are active.</param>
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
	/// Callback for Network, that assigns the database Ids to the plates.
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
