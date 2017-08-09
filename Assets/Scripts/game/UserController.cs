using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the users in direct relation to the unity-engine.
/// Handles creation of GameObjects representing the users in a game-session.
/// </summary>
public class UserController: SoftwareBehaviour {

	public GameObject userPrefab, pauseMenuController;

	private List<User> users = new List<User>();
	public User ThisUser {
		get { 
			int uid = -1;
			for (int i = 0; i < users.Count; i++) {
				uid = UserStatics.GetUserId (i);
				if (uid == UserStatics.IdSelf) {
					return users [i]; 
				}
			}
			return null;
		}
	}

	/// <summary>
	/// Gets the user count in current session.
	/// </summary>
	/// <value>The user count.</value>
	public int UserCount {
		get { return users.Count; }
	}

	/*/// <summary>
	/// Gets the identifier of the user in database.
	/// </summary>
	/// <returns>The identifier.</returns>
	/// <param name="usr">Usr.</param>
	public int GetIndex(User usr) {

		return users.IndexOf (usr);
	}*/

	/// <summary>
	/// Adds the user to the gamescene and this handlers list.
	/// </summary>
	/// <param name="user_ref">User reference in session (a, b, c or d).</param>
	/// <param name="user_id">User identifier in database.</param>
	/// <param name="user_name">User name.</param>
	public void AddUser(string user_ref, int user_id, string user_name) {

		if (users.Count < 4) {
			GameObject usr = GameObject.Instantiate(
				userPrefab, 
				new Vector3(0,0,0), 
				Quaternion.Euler(0, 0, 0), 
				gameObject.transform);

			usr.GetComponent<Rigidbody>().MovePosition( SoftwareModel.GetComponent<LevelBehaviour> ().GetStartPosition (users.Count));
			usr.transform.localRotation = Quaternion.Euler (SoftwareModel.GetComponent<LevelBehaviour> ().GetStartRotation (users.Count));
			usr.name = "USER: " + user_name;

			users.Add (usr.GetComponent<User> ());
			UserStatics.SetUserInfo(users.IndexOf(usr.GetComponent<User> ()),user_id, user_name, user_ref); 
			if (UserStatics.IsMySelf(users.IndexOf(usr.GetComponent<User> ()))) {
				usr.GetComponent<User> ().IsPlayed = true;
			}
			usr.GetComponent<User>().userInfo.GetComponent<TextMesh>().text = user_ref + " : " + user_name;
			pauseMenuController.GetComponent<PauseMenu> ().AddAnimator (usr.GetComponent<Animator> ());
		}
		// Set the timeOut of PressurePlate according to number of players in game.
		PressurePlate.TimeOut = SoftwareModel.GetComponent<LevelBehaviour> ().GetPlateTimeOut (users.Count);
	}

	/// <summary>
	/// Updates the user.
	/// </summary>
	/// <param name="user_update">User update.</param>
	public void UpdateUser (UpdateData user_update) {

		for (int i = 0; i < users.Count; i++) {
			if (UserStatics.GetUserId (i) == user_update.Id) {
				users [i].UpdateUser(user_update);
			}
		}
	}
}
