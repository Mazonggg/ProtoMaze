using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehaviour : SoftwareBehaviour {
	
	
	// Update is called once per frame
	void Update () {

		if (SoftwareModel.GameRunning) {
			Vector3 localRot = transform.localRotation.eulerAngles;
			localRot.x += Input.GetAxisRaw ("Mouse Y") * Constants.rotationFactor;
			transform.localRotation = Quaternion.Euler (localRot);
		}
	}
}
