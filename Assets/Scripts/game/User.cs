using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlls the user gameObject.
/// </summary>
public class User : GObject {

	// GameObject parameters:
	public GameObject userInfo, shortsMesh;
	private Animator animator;

	/// <summary>
	/// Tells, if this User is played by oneself.
	/// </summary>
	private bool isPlayed = false;
	public bool IsPlayed {
		get { return isPlayed; }
		set { 
			isPlayed = value;
			userInfo.SetActive (false);
		}
	}
	/// <summary>
	/// Attached GameObject, when this user picks up an object.
	/// 
	/// Not implemented in this stage of development.
	/// </summary>
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

	/// <summary>
	/// Fetches identifier of one self for object internal processes.
	/// </summary>
	/// <value>The identifier.</value>
	private int Id {
		get { return UserStatics.IdSelf; }
	}

	/// <summary>
	/// Fetches username of one self for object internal processes.
	/// </summary>
	/// <value>The identifier.</value>
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
	public new UpdateData GetUpdateData () {

		if (rigTrans != null) {
			Updated = false;
			if (objectHeld == null) {
				return new UpdateData (
					Id, 
					new Vector3 (rigTrans.position.x, rigTrans.position.y, rigTrans.position.z), 
					new Vector3 (rigTrans.localRotation.eulerAngles.x, rigTrans.localRotation.eulerAngles.y, rigTrans.localRotation.eulerAngles.z)); 
			} else {
				return new UpdateData (
					Id, 
					new Vector3 (rigTrans.position.x, rigTrans.position.y, rigTrans.position.z), 
					new Vector3 (rigTrans.localRotation.eulerAngles.x, rigTrans.localRotation.eulerAngles.y, rigTrans.localRotation.eulerAngles.z),
					objectHeld.GetUpdateData ()); 
			}
		} else {
			return new UpdateData ();
		}
	}

	/// <summary>
	/// Updates the GameObject of not self played users.
	/// To avoid stuttering switch between animations, 
	/// there is a differentiation between short lasting
	/// and longer lasting movement patterns, that call
	/// different speeds of animation accordingly.
	/// </summary>
	/// <param name="upData">Up data.</param>
	public void UpdateUser (UpdateData upData) {

		bool xEquals = (int)(rigTrans.position.x * 10) == (int)(upData.Position.x * 10);
		bool yEquals = (int)(rigTrans.position.y * 10) == (int)(upData.Position.y * 10);
		bool zEquals = (int)(rigTrans.position.z * 10) == (int)(upData.Position.z * 10);
		Vector3 direction = upData.Position - rigTrans.position;

		if (!(xEquals && yEquals && zEquals)) {

			// TODO Logic to differentiate wheter the user is running or walking; forward, sideward or backward.
			float alpha = Mathf.Rad2Deg * Mathf.Atan ( direction.x / direction.z ) - 90f;


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
		bool running = direction.magnitude > Constants.moveSpeed;
		animator.SetBool ("Running", running);
		Move(direction, running ? Constants.runSpeed : Constants.moveSpeed);
		rigTrans.localRotation = Quaternion.Euler (upData.Rotation);
	}

	/// <summary>
	/// Called by unity engine, when gameObject is instantiated.
	/// 
	/// stores the Animator object to avoid unneeded Method calls,
	/// which improves performance.
	/// </summary>
	void Start() {
		animator = GetComponent<Animator> ();
	}

	/// <summary>
	/// Recolor the skinMesh of the user.
	/// </summary>
	/// <param name="color">Color.</param>
	public void Recolor(Color color){
		shortsMesh.GetComponent<Renderer> ().material.color = color;
	}

	/// <summary>
	/// Frequently called by unity engine.
	/// 
	/// Handles movement of self played user.
	/// Also only reacts, if game is currently running.
	/// </summary>
	protected new void Update(){
		// Allow base class to store RigidBody.
		base.Update ();
		if (isPlayed && SoftwareModel.GameRunning) {
			// Capture movement:
			if ((Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)) {
				float angle = rigTrans.localRotation.eulerAngles.y;
				Vector3 hor = new Vector3(
					Mathf.Cos ((0 - angle) * Mathf.PI / 180f) * Input.GetAxis ("Horizontal"),
					0,
					Mathf.Sin ((0 - angle) * Mathf.PI / 180f) * Input.GetAxis ("Horizontal"));
				Vector3 ver = new Vector3(
					Mathf.Sin (angle * Mathf.PI / 180f) * Input.GetAxis ("Vertical"),
					0, 
					Mathf.Cos (angle * Mathf.PI / 180f) * Input.GetAxis ("Vertical"));
				Vector3 dir = hor + ver;
				Move (dir, animator.GetBool("Running") ? Constants.runSpeed : Constants.moveSpeed);
			} 
			animator.SetFloat ("Forward", Input.GetAxis ("Vertical"));
			// Capture rotation:
			float rotation = Input.GetAxisRaw ("Mouse X");
			if (rotation != 0) {
				Rotate (rotation, Constants.rotationFactor);
			}
			// Capture running:
			animator.SetBool ("Running", Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
		}
	}
}
