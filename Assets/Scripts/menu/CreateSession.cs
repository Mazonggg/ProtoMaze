using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Holds all function for creating a Session and controlling the respective Menu.
/// </summary>
public class CreateSession : SoftwareBehaviour {
	// GameObject parameters:
	public GameObject logInCanvas, mainMenuCanvas, createSessionCanvas, toggleLevelText, explanationText; //, buttonHolder;
	public LevelBehaviour levelBehaviour;
	public Text user_a, user_b, user_c, user_d, headline;//, leveltext;
    private Text[] users;

	/// <summary>
	/// IEnumerator that handles the updating of the lobby.
	/// </summary>
	private IEnumerator updateLobby;

	/// <summary>
	/// Called by unity engine, when gameObject is instantiated.
	/// 
	/// Hides the menu when game is opened and stores users.
	/// </summary>
    void Start () {
        createSessionCanvas.SetActive(false);
        users = new Text[] { user_a, user_b, user_c, user_d };
    }

	/// <summary>
	/// Navigates back to MainMenu, includes TCP request to leave the session.
	/// </summary>
    public void GoBack() {
		SoftwareModel.netwRout.TCPRequest(
            ResetUserInfo,
            new string[] { "req", "userId" },
            new string[] { "leaveSession", UserStatics.IdSelf.ToString() });

        mainMenuCanvas.SetActive(true);
        createSessionCanvas.SetActive(false);
    }

	/// <summary>
	/// Called by unity engine, when gameObject is destroyed.
	/// 
	/// Stops the updating of the lobby, when destroyed.
	/// </summary>
	void OnDestroy() {
		StopCoroutine (updateLobby);
	}

	/// <summary>
	/// Resets the information of one self when leaving a session.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
	/// </summary>
	/// <param name="response">Response.</param>
    private void ResetUserInfo(string[][] response) {
        UserStatics.SetUserInfo(0, UserStatics.IdSelf, UserStatics.GetUserName(UserStatics.IdSelf), "");
    }


    /// <summary>
	/// Assigns the session just created (on server) to the user on this client.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
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
				ChooseLevel (LevelConstants.GetLevel(currentLevelIndex));
                StartUpdateLobby();
                return;
			}
		}
	}

	/// <summary>
	/// The index of the current level in list of all available levels.
	/// </summary>
	private int currentLevelIndex = 0;
	/// <summary>
	/// Toggles to the next level in list of all levels in game.
	/// </summary>
	public void ToggleNextLevel () {
		currentLevelIndex = currentLevelIndex > LevelConstants.NumberOfLevels () - 2 ? 0 : ++currentLevelIndex;
		ChooseLevel(LevelConstants.GetLevel (currentLevelIndex));
	}

	/// <summary>
	/// Chooses level given, for playing in session.
	/// </summary>
	/// <param name="lvlBhvr">Lvl bhvr.</param>
	private void ChooseLevel (LevelBehaviour lvlBhvr) {

		AssignLevel (lvlBhvr);
		SoftwareModel.netwRout.TCPRequest(
			NetworkRoutines.EmptyCallback,
			new string[] { "req", "sessionId", "levelIndex" },
			new string[] { "changeLevel", UserStatics.SessionId.ToString() , currentLevelIndex.ToString() });
	}

	/// <summary>
	/// Assigns the level to be played in session.
	/// </summary>
	/// <param name="lvlBhvr">Lvl bhvr.</param>
	private void AssignLevel(LevelBehaviour lvlBhvr) {

		levelBehaviour = lvlBhvr;
		toggleLevelText.GetComponent<Text> ().text = "Choose level:   " + levelBehaviour.SceneName ();
	}

	/// <summary>
	/// Updates the lobby by sending TCP requests every second.
	/// </summary>
	/// <returns>The lobby.</returns>
    private IEnumerator UpdateLobby(){

		while (true) {
			string userSession = UserStatics.SessionId.ToString ();
			SoftwareModel.netwRout.TCPRequest (
				UpdateView,
				new string[] { "req", "sessionId" },
				new string[] { "getPlayerInSession", userSession });
			yield return new WaitForSeconds (1f);
			// Change of explanationText allows for hypothetical level-change, if further levels are implemented later:
			explanationText.GetComponent<Text>().text = levelBehaviour.ExplanationText ();
		}
    }

	/// <summary>
	/// Updates the overview of the lobby and receives the signal to start the level per remote.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
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

			if (pair [0].Equals (Constants.sfLevelindex)) {
				int levelindex = 0;
				int.TryParse (pair [1], out levelindex);
				currentLevelIndex = levelindex;
				AssignLevel(LevelConstants.GetLevel (levelindex));
			}

			// Check if the session is ment to be started.
			if (pair [0].Equals (Constants.sfState) && pair [1].Equals (Constants.sfStarting)) {
				// Start the session.
				gameObject.GetComponent<StartSession> ().StartTheSession ();
			}
		}
    }

	/// <summary>
	/// Starts the updating of the lobby.
	/// </summary>
    public void StartUpdateLobby() {
		updateLobby = UpdateLobby ();
		StartCoroutine(updateLobby);
    }
}
