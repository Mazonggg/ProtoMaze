using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as player controller.
/// </summary>
public class User : GObject {

	public GameObject userInfo, shortsMesh;
	private Animator animator;

	private bool isPlayed = false;
	public bool IsPlayed {
		get { return isPlayed; }
		set { 
			isPlayed = value;
			userInfo.SetActive (false);
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

	public void UpdateUser (UpdateData upData) {
		
		if (rigTrans.position.x != upData.Position.x ||
			rigTrans.position.y != upData.Position.y || 
			rigTrans.position.z != upData.Position.z) {


			// TODO Insert logic to differentiate between moving forward and backwards.
			// Vector3  direction = (upData.Position - rigPos).normalized - upData.Rotation.normalized;
			// Debug.Log ("direction: x=" + direction.x + "   y=" + direction.y + "   z=" + direction.z);

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
		bool running = (upData.Position - rigTrans.position).magnitude > Constants.moveSpeed;
		animator.SetBool ("Running", running);
		Move(upData.Position - rigTrans.position, running ? Constants.runSpeed : Constants.moveSpeed);
		rigTrans.localRotation = Quaternion.Euler (upData.Rotation);
	}

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

	protected void Update(){

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
			Rotate (Input.GetAxisRaw("Mouse X"), Constants.rotationFactor);
			// Capture running:
			animator.SetBool ("Running", Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
		}
	}
}
