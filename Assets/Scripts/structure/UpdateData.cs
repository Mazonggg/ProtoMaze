using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data class that collects data to update a GObject.
/// </summary>
public class UpdateData {
	/// <summary>
	/// The identifier for the database on server.
	/// </summary>
	private int id;
	public int Id {
		get { return id; }
	}
	/// <summary>
	/// The position as three dimensional vector.
	/// </summary>
	private Vector3 position;
	private Vector3 rotation;
	public Vector3 Position {
		get { return position; }
	}
	/// <summary>
	/// The rotation as three dimensional vector.
	/// </summary>
	public Vector3 Rotation {
		get { return rotation; }
	}

	/// <summary>
	/// A potentially held object by a user.
	/// </summary>
	private UpdateData objectHeld;
	public UpdateData ObjectHeld {
		get { return objectHeld; }
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="UpdateData"/> class with empty data.
	/// 
	/// </summary>
	public UpdateData() {

		this.id = -1;
		this.position = new Vector3 (0, 0, 0);
		this.rotation = new Vector3 (0, 0, 0);
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UpdateData"/> class without a held object.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	public UpdateData(int id, Vector3 position, Vector3 rotation) {

		this.id = id;
		this.position = position;
		this.rotation = rotation;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="UpdateData"/> class including a held object.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="position">Position.</param>
	/// <param name="rotation">Rotation.</param>
	/// <param name="objectHeld">Object held.</param>
	public UpdateData(int id, Vector3 position, Vector3 rotation, UpdateData objectHeld) {

		this.id = id;
		this.position = position;
		this.rotation = rotation;
		if (objectHeld != null) {
			this.objectHeld = objectHeld;
		}
	}
}
