using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Base-object for every movable object in a level.
/// </summary>
public class GObject : SoftwareBehaviour {


	/// <summary>
	/// Database Id of this object on server.
	/// </summary>
	private int id = -1;
	/// <summary>
	/// Tells, if the object currently moves or is moved by a user.
	/// </summary>
	private bool active = false;
	/// <summary>
	/// Store the RigidBody, to avoid unnecassary references to other objects during play.
	/// </summary>
	protected Rigidbody rigBody;
	protected Transform rigTrans;
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
	public UpdateData GetUpdateData () {
			return new UpdateData (
			id, 
			new Vector3(rigTrans.position.x, rigTrans.position.y, rigTrans.position.z), 
			new Vector3(rigTrans.localRotation.eulerAngles.x, rigTrans.localRotation.eulerAngles.y, rigTrans.localRotation.eulerAngles.z)); 
	}

	/// <summary>
	/// Resets all parameter of this GObject.
	/// </summary>
	public void Restart() {
		rigTrans.position = new Vector3 (0, 0, 0);
		rigTrans.localRotation = Quaternion.Euler (0, 0, 0);
		updated = true;
	}

	/// <summary>
	/// Move the GObject according to direction and given pace.
	/// Applies changes to RigidBody, to ensure correct physical behaviour,
	/// only apply to GameObject itself, if RigidBody not available.
	/// </summary>
	/// <param name="dir">Dir.</param>
	/// <param name="pace">Pace.</param>
	protected void Move(Vector3 dir, float pace){
		// Apply change in Position to rigBody
		if (rigBody != null) {
			Debug.Log ("Move(): pace=" + pace);
			Debug.Log ("rigTrans.position=" + rigTrans.position + "  +  dir=" + dir + "  *  pace=" + pace + "  *  Time.deltaTime=" + Time.deltaTime);
			rigBody.MovePosition (rigTrans.position + dir * pace * Time.deltaTime);
		} 
		updated = true;
	}

	/// <summary>
	/// Rotate the user according to mouseRotation.
	/// </summary>
	/// <param name="mouseRotation">Mouse rotation.</param>
	protected void Rotate(float mouseRotation, float rotationFactor) {
		
		if (Time.timeScale != 0) {
			Vector3 localRot = rigTrans.localRotation.eulerAngles;
			localRot.y += mouseRotation * rotationFactor;
			rigTrans.localRotation = Quaternion.Euler (localRot);
			updated = true;
		}
	}

	/// <summary>
	/// Cache the RigidBody once, for improved performance.
	/// </summary>
	protected void Update () {
		if (gameObject.GetComponent<Rigidbody> () != null && rigBody == null) {
			rigBody = gameObject.GetComponent<Rigidbody> ();
			rigTrans = rigBody.transform;
		} 
	}
}
