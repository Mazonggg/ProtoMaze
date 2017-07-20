using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as player controller.
/// </summary>
public class User : GObject {

	public GameObject userInfo;
	private Animator animator;

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
	/// Counts the currently "empty" updates.
	/// </summary>
	private byte standCounter = 0;
	/// <summary>
	/// Current limit for "empty" updates depending on "streak" of valid updates.
	/// </summary>
	private byte standLimit = 0;
	/// <summary>
	/// Predifined max for "empty" updates before transitioning to idle animation.
	/// </summary>
	private byte standMax = 3;
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
			Debug.Log("x=" + (transform.position.x - value.Position.x) + " / y=" + (transform.position.y - value.Position.y) + " / z=" + (transform.position.z - value.Position.z));
			if (transform.position.x != value.Position.x || transform.position.y != value.Position.y || transform.position.z != value.Position.z) {
				transform.position = value.Position;
				animator.SetFloat ("Forward", 1f);
				standCounter = 0;
				if (standLimit < standMax) {
					standLimit++;
				} else {
					animator.SetBool ("QuickTrans", false);
				}
			} else {
				if (standCounter >= standLimit) {
					animator.SetFloat ("Forward", 0f);
					standCounter = 0;
					standLimit = 0;
					animator.SetBool ("QuickTrans", true);
				} else {
					standCounter++;
				}
			}
			//transform.rotation = Quaternion.Euler (value.Rotation.x, value.Rotation.y, value.Rotation.z);
		}
	}

	void Start() {
		animator = GetComponent<Animator> ();
	}

	void Update(){

		if (isPlayed) {
			if ((Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)) {
				Vector3 dir = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
				Move (dir, Constants.moveSpeed);
			} 
			animator.SetFloat ("Forward", Input.GetAxis ("Vertical"));
		}
	}
}
