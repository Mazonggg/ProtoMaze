using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds static information about users, 
/// that are not directly related to the unity-engine.
/// This is necessary to avoid loss of information 
/// when changing scenes in the game, without implementing
/// a local database.
/// </summary>
public class UserStatics {

	private static int idSelf = -1;
	public static int IdSelf {
		get { return idSelf; }
		set { idSelf = value; }
	}
	private static int[] userIds = {-1, -1, -1, -1};
	private static string[] userNames = {"", "", "", ""};
	private static string[] userRefs = {"", "", "", ""};

	/// <summary>
	/// Gets the user identifier.
	/// </summary>
	/// <returns>The user identifier.</returns>
	/// <param name="index_in_game">Index in game.</param>
	public static int GetUserId(int index_in_game) {

		return userIds [index_in_game];
	}

	/// <summary>
	/// Checks if a user id is in session.
	/// </summary>
	/// <returns><c>true</c>, if is in was usered, <c>false</c> otherwise.</returns>
	/// <param name="user_id">User identifier.</param>
	public static bool UserIsIn(int user_id) {

		foreach (int userId in userIds) {
			if (userId == user_id) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Determines if the given index of the player in the game represents myself.
	/// </summary>
	/// <returns><c>true</c> if is my self the specified index_in_game; otherwise, <c>false</c>.</returns>
	/// <param name="index_in_game">Index in game.</param>
	public static bool IsMySelf(int index_in_game){

		return idSelf == userIds [index_in_game];
	}

	private static bool isCreater = false;
	public static bool IsCreater {
		get { return isCreater; }
		set { isCreater = value; }
	}
		
	/// <summary>
	/// Gets the name of the user for a given user_id.
	/// </summary>
	/// <returns>The user name.</returns>
	/// <param name="user_id">User identifier.</param>
	public static string GetUserName (int user_id) {

		for (int i = 0; i < userIds.Length; i++) {
			if (userIds [i] == user_id) {
				return userNames [i];
			}
		}
		return "nothing found";
	}

	/// <summary>
	/// Gets the user ref of the user for a given user_id.
	/// </summary>
	/// <returns>The user ref.</returns>
	/// <param name="user_id">User identifier.</param>
	public static string GetUserRef (int user_id) {

		for (int i = 0; i < userIds.Length; i++) {
			if (userIds [i] == user_id) {
				return userRefs [i];
			}
		}
		return "nothing found";
	}

	/// <summary>
	/// Sets the name of the user in reference to his/her Id.
	/// </summary>
	/// <returns><c>true</c>, if user name was set, <c>false</c> otherwise.</returns>
	/// <param name="user_id">User identifier.</param>
	/// <param name="user_name">User name.</param>
	public static void SetUserInfo(int index_of_user, int user_id, string user_name, string user_ref) {

		// Debug.Log ("SetUserInfo: index_of_user=" + index_of_user + " / user_id=" + user_id + " / user_name=" + " / user_ref=" + user_ref);
		userIds [index_of_user] = user_id;
		userNames [index_of_user] = user_name;
		userRefs [index_of_user] = user_ref;
	}

	/// <summary>
	/// Sets the information for the user logged in on this instance of the game.
	/// </summary>
	/// <param name="user_id">User identifier.</param>
	/// <param name="user_name">User name.</param>
	public static void SetUserLoggedIn(int user_id, string user_name) {
		SetUserInfo(0, user_id, user_name, "");
		IdSelf = user_id;
	}

	/// <summary>
	/// Id of session in database.
	/// </summary>
	private static int sessionId = 0;
	public static int SessionId {
		get { return sessionId; }
		set { sessionId = value; }
	}
}
