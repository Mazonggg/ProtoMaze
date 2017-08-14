using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

/// <summary>
/// Represents the interface between the unity-game and the functional model of the Software in itself.
/// Establishes Connection to server.
/// </summary>
public class SoftwareModel : SoftwareBehaviour {

	public UserController userController;
	public NetworkRoutines netwRout;
	public DoorScript doorScript;

	private SocketObject socketObj;
	public SocketObject SocketObj {
		get { return socketObj; }
	}

	private PlateController plateContr;
	public PlateController PlateContr {
		get { return plateContr; }
	}

	private bool gameRunning = false;
	public bool GameRunning {
		get { return gameRunning; }
		set { gameRunning = value; }
	}

	public void CreateSocketObject(int timer){
		gameObject.AddComponent<SocketObject> ();
		gameObject.GetComponent<SocketObject> ().SetSocket (timer);
        socketObj = gameObject.GetComponent<SocketObject>();
    }

	public void CreatePlateController() {
		gameObject.AddComponent<PlateController> ();
		plateContr = gameObject.GetComponent<PlateController>();

		plateContr.AddPlates(FindObjectsOfType (typeof(PressurePlate)) as PressurePlate[]);
	}
}