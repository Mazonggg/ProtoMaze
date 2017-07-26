using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base-object for every movable object in a level.
/// </summary>
public class GObject : SoftwareBehaviour {

	private BoxCollider boxCollider;
	/// <summary>
	/// Database Id of this object on server.
	/// </summary>
	private int id = -1;

	/// <summary>
	/// Tells, if the object currently moves or is moved by a user.
	/// </summary>
	private bool active = false;
	public bool Active {
		get { return active; }
		set { active = value; }
	}

	/// <summary>
	/// true, if changed since last time data where referenced.
	/// </summary>
	private bool updated = false;
	public bool Updated {
		get { return updated; }
		set { updated = value; }
	}
	/// <summary>
	/// Returns the relevant data for updating the server, for this object.
	/// </summary>
	/// <value>The update data.</value>
	public UpdateData UpdateData {
		get { 
			return new UpdateData (
				id, 
				new Vector3(transform.position.x,transform.position.y,transform.position.z), 
				new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z)); 
		}
	}

	/// <summary>
	/// Resets all parameter of this GObject.
	/// </summary>
	public void Restart() {
		transform.position = new Vector3 (0, 0, 0);
		transform.rotation = Quaternion.Euler (0, 0, 0);
		updated = true;
	}

	/// <summary>
	/// Place the specified pos and rot.
	/// </summary>
	/// <param name="pos">Position.</param>
	/// <param name="rot">Rot.</param>
	public void Place(Vector3 pos, Vector3 rot, bool visible) {

		transform.position = pos;
		transform.rotation = Quaternion.Euler (rot);

		GetComponent<MeshRenderer> ().enabled = visible;
	}

	// Use this for initialization
	void Start () {
		boxCollider = GetComponent<BoxCollider>();
	}

	/*
	 * Moves GObject according to given direction and pace parameters.
	 */
	protected void Move(Vector3 dir, float pace){
		transform.position += dir * pace * Time.deltaTime;
		updated = true;
	}

	/// <summary>
	/// Rotate the user according to mouseRotation.
	/// </summary>
	/// <param name="mouseRotation">Mouse rotation.</param>
	protected void Rotate(float mouseRotation, float rotationFactor) {

		if (Time.timeScale != 0) {
			Vector3 localRot = transform.localRotation.eulerAngles;
			//localRot.x += mouseRotation.y * rotationFactor;
			localRot.y += mouseRotation * rotationFactor;
			transform.localRotation = Quaternion.Euler (localRot);
			updated = true;
		}
	}
}
