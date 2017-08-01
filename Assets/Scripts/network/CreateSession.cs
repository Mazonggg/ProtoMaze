using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CreateSession : SoftwareBehaviour {

	public GameObject logInCanvas, mainMenuCanvas, createSessionCanvas, startSessionButton, backButton;
    public Text user_a, user_b, user_c, user_d, headline;
    private Text[] users;

	private IEnumerator updateLobby;

    void Start () {
        createSessionCanvas.SetActive(false);
        users = new Text[] { user_a, user_b, user_c, user_d };
    }

    public void goBack() {
		SoftwareModel.netwRout.TCPRequest(
            ResetUserInfo,
            new string[] { "req", "userId" },
            new string[] { "leaveSession", UserStatics.IdSelf.ToString() });

        mainMenuCanvas.SetActive(true);
        createSessionCanvas.SetActive(false);
    }

	void OnDestroy() {
		StopCoroutine (updateLobby);
	}

    private void ResetUserInfo(string[][] response) {

        UserStatics.SetUserInfo(0, UserStatics.IdSelf, UserStatics.GetUserName(UserStatics.IdSelf), "");
    }


    /// <summary>
    /// Assigns the session just created (on server) to the user on this client.
    /// </summary>
    /// <param name="response">Response.</param>
    public void AssignSessionToUser(string[][] response) {

		int SsIdTmp = -1;

		foreach (string[] pair in response) {

			if (pair [0].Equals ("type") && pair [1].Equals ("ERROR")) {
				
				GameObject.Find ("ErrorText").GetComponent<ErrorText> ().ShowError ("Couldn't create Session!");
				return;
			}
			if(pair[0].Equals("sessionId")) {
				int.TryParse(pair[1], out SsIdTmp);
				UserStatics.SessionId = SsIdTmp;
				mainMenuCanvas.SetActive(false);
				createSessionCanvas.SetActive(true);
                StartUpdateLobby();
                return;
			}
		}
	}

    private IEnumerator UpdateLobby(){

		while (true) {
			string userSession = UserStatics.SessionId.ToString ();
			SoftwareModel.netwRout.TCPRequest (
				UpdateView,
				new string[] { "req", "sessionId" },
				new string[] { "getPlayerInSession", userSession });
			yield return new WaitForSeconds (1f);
		}
    }

	/// <summary>
	/// Updates the overview of the lobby and receives the signal to start the level per remote.
	/// </summary>
	/// <param name="response">Response.</param>
	private void UpdateView(string[][] response){
		
		headline.text ="Wait for Players in Session " + UserStatics.SessionId.ToString();
		string ret = "";
        foreach (string[] pair in response){

			if (pair [0].Equals ("playerInSession")) {

				ret += pair [1];
			}
			string pattern = @"//";
			string[] usernames = Regex.Split (ret, pattern);

			for (int i = 0; i < usernames.Length; i++) {

				Debug.Log ("UpdateView: UserStatics.GetUserName (UserStatics.IdSelf)=" + UserStatics.GetUserName (UserStatics.IdSelf));
				users [i].text = usernames [i];
				users [i].fontStyle = FontStyle.Bold;
				if (UserStatics.GetUserName (UserStatics.IdSelf).Equals (usernames [i])) {
					users [i].color = Constants.userColor;
				} else if(usernames[i].Equals(Constants.noUser)) {
					users [i].text = Constants.freeUser;
					users [i].fontStyle = FontStyle.Normal;
					users [i].color = Constants.textColor;
				} else {
					users [i].color = Constants.textColor;
				}
			}

			// Check if the session is ment to be started.
			if (pair [0].Equals (Constants.sfState) && pair [1].Equals (Constants.sfStarting)) {
				// Start the session.
				gameObject.GetComponent<StartSession> ().LoadNewScene ();
			}
		}
    }

    public void StartUpdateLobby() {
		updateLobby = UpdateLobby ();
		StartCoroutine(updateLobby);
    }
}
