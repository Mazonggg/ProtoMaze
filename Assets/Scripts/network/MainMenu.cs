using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

/// <summary>
/// Handles the whole MainMenu with all its function.
/// </summary>
public class MainMenu : SoftwareBehaviour {
	// GameObject parameters:
	public GameObject logInCanvas, mainMenuCanvas, createSessionCanvas, joinSessionCanvas;
    public GameObject joinSessionController;

	/// <summary>
	/// Called by unity engine, when gameObject is instantiated.
	/// 
	/// Hides this menu,
	/// to correctly implement order of login etc. in game.
	/// </summary>
	void Start(){
		mainMenuCanvas.SetActive (false);
	}
		
	/// <summary>
	/// Sends TCP request to log one self out from the game.
	/// </summary>
	public void LogoutUser() {

		string userId = UserStatics.IdSelf.ToString();

		SoftwareModel.netwRout.TCPRequest(
            HandleLogout, 
			new string[] {"req", "userId"},
			new string[] {"logoutUser", userId});
		GameObject.Find ("ErrorText").GetComponent<ErrorText> ().ClearText ();
	}

	/// <summary>
	/// Navigates to LogInMenu and clears static user information.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
	/// </summary>
	/// <param name="response">Response.</param>
	private void HandleLogout(string[][] response) {

        logInCanvas.SetActive(true);
		mainMenuCanvas.SetActive(false);

		UserStatics.SetUserInfo(0, -1, "", "");
	}

	/// <summary>
	/// Navigates to the menu for joining a session.
	/// </summary>
	public void JoinSession() {

		GameObject.Find ("ErrorText").GetComponent<ErrorText> ().ClearText ();
		mainMenuCanvas.SetActive(false);
		joinSessionCanvas.SetActive(true);

		joinSessionController.GetComponent<JoinSession>().GetSessions();   
	}


	/// <summary>
	/// Sets up request to server, that creates the session
	/// </summary>
	public void CreateSession() {

		GameObject.Find ("ErrorText").GetComponent<ErrorText> ().ClearText ();
		string userId = UserStatics.IdSelf.ToString();

		SoftwareModel.netwRout.TCPRequest(
			createSessionCanvas.GetComponentInChildren<CreateSession>().AssignSessionToUser, 
			new string[] {"req", "userId"},
			new string[] {"createSession", userId});
	}
}
