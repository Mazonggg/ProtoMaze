using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftwareBehaviour : MonoBehaviour {

	private SoftwareModel softwareModel;
	private string swM = "SoftwareModel";
	public SoftwareModel SoftwareModel {
		get {
			if(softwareModel == null) {
				softwareModel = GameObject.Find (swM).GetComponent<SoftwareModel> ();
			}
			return softwareModel;
		}
	}
}
