using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the behaviour of the main camera, while playing a level.
/// </summary>
public class MainCameraBehaviour : SoftwareBehaviour {
	
	private Vector3 cameraDefault = new Vector3 (3f, 15f, -15f);
	public GameObject userController;

	/// <summary>
	/// Frequently called by unity engine.
	/// 
	/// Tillts the camera according to vertical movement of the mouse input.
	/// </summary>
	void Update () {

		if (SoftwareModel.GameRunning) {
			Vector3 localRot = transform.localRotation.eulerAngles;
			localRot.x -= Input.GetAxisRaw ("Mouse Y") * Constants.rotationFactor;
			transform.localRotation = Quaternion.Euler (localRot);
		}
	}

	/// <summary>
	/// Positions the camera behind the played user GameObject, so it moves with it.
	/// </summary>
	public void PositionCamera() {
		transform.SetParent (userController.GetComponent<UserController> ().ThisUser.transform);
		transform.localPosition = cameraDefault;
		transform.localRotation = Quaternion.Euler (new Vector3(0, 0, 0));
	}
}
