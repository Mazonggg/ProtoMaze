using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class MainMenu : MonoBehaviour {

	public GameObject logInCanvas, mainMenuCanvas, createSessionCanvas, joinSessionCanvas;
    public GameObject joinSessionController;

	/// <summary>
	/// Called when object is "started",
	/// Sets the timescale to realtime and hides this menu,
	/// to correctly implement order of login etc. in game.
	/// </summary>
	void Start(){
		Time.timeScale = 1f;
		mainMenuCanvas.SetActive (false);
	}
		
	public void LogoutUser() {

		string userId = UserStatics.IdSelf.ToString();

		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
            HandleLogout, 
			new string[] {"req", "userId"},
			new string[] {"logoutUser", userId});
		GameObject.Find ("ErrorText").GetComponent<ErrorText> ().ClearText ();
	}

	private void HandleLogout(string[][] response) {

        logInCanvas.SetActive(true);
		mainMenuCanvas.SetActive(false);

		UserStatics.SetUserInfo(0, -1, "", "");
	}


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

		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
			createSessionCanvas.GetComponentInChildren<CreateSession>().AssignSessionToUser, 
			new string[] {"req", "userId"},
			new string[] {"createSession", userId});
	}
}
