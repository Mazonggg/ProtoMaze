using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

public class MainMenu : MonoBehaviour {

	public GameObject logInCanvas, mainMenuCanvas, createSessionCanvas, joinSessionCanvas;
    public GameObject joinSessionController;

	void Start(){
		mainMenuCanvas.SetActive (false);
	}
		
	public void LogoutUser() {

		string userId = UserStatics.IdSelf.ToString();

		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
            HandleLogout, 
			new string[] {"req", "userId"},
			new string[] {"logoutUser", userId});
	}

	private void HandleLogout(string[][] response) {

        logInCanvas.SetActive(true);
		mainMenuCanvas.SetActive(false);

		UserStatics.SetUserInfo(0, -1, "", "");
	}


	public void JoinSession() {
		
		mainMenuCanvas.SetActive(false);
		joinSessionCanvas.SetActive(true);

		joinSessionController.GetComponent<JoinSession>().GetSessions();   
	}


	/// <summary>
	/// Sets up request to server, that creates the session
	/// </summary>
	public void CreateSession() {
		
		string userId = UserStatics.IdSelf.ToString();

		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
			createSessionCanvas.GetComponentInChildren<CreateSession>().AssignSessionToUser, 
			new string[] {"req", "userId"},
			new string[] {"createSession", userId});
	}
}
