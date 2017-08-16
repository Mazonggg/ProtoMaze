using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class that replaces MonoBehaviour mostly in this game, so every Behaviour 
/// inherits the SoftwareModel as datafield. Avoids excesive referencing of GameObjects.
/// </summary>
public class SoftwareBehaviour : MonoBehaviour {
	// Store the SoftwareModel.
	private SoftwareModel softwareModel;
	private string swM = "SoftwareModel";
	protected SoftwareModel SoftwareModel {
		get {
			if(softwareModel == null) {
				softwareModel = GameObject.Find (swM).GetComponent<SoftwareModel> ();
			}
			return softwareModel;
		}
	}
}
