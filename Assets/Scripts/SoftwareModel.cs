using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

/// <summary>
/// Represents the interface between the unity-game and the functional model of the Software in itself.
/// Establishes Connection to server.
/// </summary>
public class SoftwareModel : MonoBehaviour {

	public UserController userController;
	public NetworkRoutines netwRout;

	private SocketObject socketObj;

	public SocketObject SocketObj {
		get { return socketObj; }
		set { socketObj = value; }
	}
		
	public void PlaceUser() {

		if (userController != null) {

			userController.ThisUser.Place (new Vector3 (0, 0, 0), new Vector3(0, 0, 0), true);
		}
	}

	public void CreateSocketObject(int timer){
		
		socketObj = new SocketObject (timer);
	}
}