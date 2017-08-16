using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

/// <summary>
/// Represents the interface between the unity-game and the functional model of the Software in itself.
/// Stores controllers and such of different object groups.
/// Establishes connection to server.
/// </summary>
public class SoftwareModel : SoftwareBehaviour {
	// References for controllers and similar
	// First four similar to GameObject parameters and public because of that.
	public UserController userController;
	public NetworkRoutines netwRout;
	public DoorScript doorScript;
	public FinisherPlate finisherPlate;

	private SocketObject socketObj;
	public SocketObject SocketObj {
		get { return socketObj; }
	}
	private PlateController plateContr;
	public PlateController PlateContr {
		get { return plateContr; }
	}

	/// <summary>
	/// Tells if the session is currently in RUNNING state.
	/// </summary>
	private bool gameRunning = false;
	public bool GameRunning {
		get { return gameRunning; }
		set { gameRunning = value; }
	}

	/// <summary>
	/// Creates the socket object in runtime.
	/// </summary>
	/// <param name="timer">Timer.</param>
	public void CreateSocketObject(int timer){
		gameObject.AddComponent<SocketObject> ();
		gameObject.GetComponent<SocketObject> ().SetSocket (timer);
        socketObj = gameObject.GetComponent<SocketObject>();
    }

	/// <summary>
	/// Creates the plate controller in runtime.
	/// </summary>
	public void CreatePlateController() {
		gameObject.AddComponent<PlateController> ();
		plateContr = gameObject.GetComponent<PlateController>();

		plateContr.AddPlates(FindObjectsOfType (typeof(PressurePlate)) as PressurePlate[]);
	}
}