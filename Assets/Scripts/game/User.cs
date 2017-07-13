using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as player controller.
/// </summary>
public class User : GObject {

	public GameObject userInfo;

	private bool isPlayed = false;
	public bool IsPlayed {
		get { return isPlayed; }
		set { 
			isPlayed = value;
			userInfo.GetComponent<MeshRenderer> ().material.color = Constants.userColor;
		}
	}
	private GObject objectHeld;
	public GObject ObjectHeld {
		get { return objectHeld; }
		set { objectHeld = value; }
	}
	/// <summary>
	/// Represents the spot, the user takes in the gaming session.
	/// </summary>
	private string user_ref = "";
	public string User_ref {
		get { return user_ref; }
		set { user_ref = value; }
	}

	//private int id = -1;
	private int Id {
		get { return UserStatics.IdSelf; }
	}

	private string UserName {
		get { return UserStatics.GetUserName (Id); }
	}

	/// <summary>
	/// Returns the relevant data for updating the server, for this object.
	/// </summary>
	/// <value>The update data.</value>
	public UpdateData UpdateData {
		get {
			Updated = false;
			if (objectHeld == null) {
				return new UpdateData (
					Id, 
					new Vector3 (transform.position.x, transform.position.y, transform.position.z), 
					new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)); 
			} else {
				return new UpdateData (
					Id, 
					new Vector3 (transform.position.x, transform.position.y, transform.position.z), 
					new Vector3 (transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
					objectHeld.UpdateData); 
			}
		}
		set {
			transform.position = value.Position;
			transform.rotation = Quaternion.Euler (value.Rotation.x, value.Rotation.y, value.Rotation.z);
		}
	}

	void FixedUpdate(){
		
		if (isPlayed && (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)) {
			Vector3 dir = new Vector3 (Input.GetAxis ("Horizontal"), 0,Input.GetAxis ("Vertical"));
			Move (dir, Constants.moveSpeed);
		}
	}
}
