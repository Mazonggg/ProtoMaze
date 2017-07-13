using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateData {

	private int id;
	public int Id {
		get { return id; }
	}
	private Vector3 position;
	private Vector3 rotation;
	public Vector3 Position {
		get { return position; }
	}
	public Vector3 Rotation {
		get { return rotation; }
	}

	private UpdateData objectHeld;
	public UpdateData ObjectHeld {
		get { return objectHeld; }
	}

	public UpdateData(int id, Vector3 position, Vector3 rotation) {

		this.id = id;
		this.position = position;
		this.rotation = rotation;
	}

	public UpdateData(int id, Vector3 position, Vector3 rotation, UpdateData objectHeld) {

		this.id = id;
		this.position = position;
		this.rotation = rotation;
		if (objectHeld != null) {
			this.objectHeld = objectHeld;
		}
	}
}
