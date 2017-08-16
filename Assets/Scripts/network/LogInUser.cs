using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

/// <summary>
/// Handles the LoginMenu.
/// </summary>
public class LogInUser : SoftwareBehaviour {
	// GameObject parameters:
	public GameObject inputName, inputPwd, logInCanvas, mainMenuCanvas;

	/// <summary>
	/// Called by unity engine, when gameObject is instantiated.
	/// 
	/// Hides the menu if already logged in.
	/// </summary>
	void Start() {

		if (UserStatics.IdSelf > -1) {
			HideMenu ();
		}
	}

	/// <summary>
	/// Sends TCP request to log the user in.
	/// </summary>
	public void LoginUser() {

		string name = inputName.GetComponent<InputField>().text;
		string pwd = SoftwareModel.netwRout.Md5Sum(inputPwd.GetComponent<InputField> ().text);

		SoftwareModel.netwRout.TCPRequest(
			HandleLogin, 
			new string[] {"req", "userName", "pwd"},
			new string[] {"loginUser", name, pwd});
	}

	/// <summary>
	/// Processes the login information from server and clears input fields, if successfull.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
	/// </summary>
	/// <param name="response">Response.</param>
	private void HandleLogin (string[][] response){

        foreach( string[] pair in response) {

			if (pair[0].Equals("userId")) {
				
                int IdTmp = -1;
				int.TryParse(pair[1], out IdTmp);
				HideMenu ();

				UserStatics.SetUserLoggedIn (IdTmp, inputName.GetComponent<InputField> ().text);

				inputName.GetComponent<InputField> ().text = "";
				inputPwd.GetComponent<InputField> ().text = "";
                return;
            }
        }
	}

	/// <summary>
	/// Navigates to MainMenu.
	/// </summary>
	private void HideMenu() {

		logInCanvas.SetActive(false);
		mainMenuCanvas.SetActive(true);
	}
}
