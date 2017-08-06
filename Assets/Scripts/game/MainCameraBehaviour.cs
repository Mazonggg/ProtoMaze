using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehaviour : SoftwareBehaviour {
	
	private Vector3 cameraDefault = new Vector3 (3f, 15f, -15f);
	public GameObject userController;
	// Update is called once per frame
	void Update () {

		if (SoftwareModel.GameRunning) {
			Vector3 localRot = transform.localRotation.eulerAngles;
			localRot.x -= Input.GetAxisRaw ("Mouse Y") * Constants.rotationFactor;
			transform.localRotation = Quaternion.Euler (localRot);
		}
	}

	public void PositionCamera() {
		transform.SetParent (userController.GetComponent<UserController> ().ThisUser.transform);
		transform.localPosition = cameraDefault;
		transform.localRotation = Quaternion.Euler (new Vector3(0, 0, 0));
	}
}
