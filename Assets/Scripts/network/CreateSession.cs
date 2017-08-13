using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class CreateSession : SoftwareBehaviour {

	public GameObject logInCanvas, mainMenuCanvas, createSessionCanvas, toggleLevelText, explanationText; //, buttonHolder;
	public LevelBehaviour levelBehaviour;
	public Text user_a, user_b, user_c, user_d, headline;//, leveltext;
    private Text[] users;

	private IEnumerator updateLobby;

    void Start () {
        createSessionCanvas.SetActive(false);
        users = new Text[] { user_a, user_b, user_c, user_d };
    }

    public void GoBack() {
		SoftwareModel.netwRout.TCPRequest(
            ResetUserInfo,
            new string[] { "req", "userId" },
            new string[] { "leaveSession", UserStatics.IdSelf.ToString() });

		//leveltext.gameObject.SetActive (false);
		//buttonHolder.SetActive (true);
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
				ChooseLevel (LevelConstants.GetLevel(currentLevelIndex));
                StartUpdateLobby();
                return;
			}
		}
	}

	/// <summary>
	/// The index of the current level.
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
	private void ChooseLevel (LevelBehaviour lvlBhvr) {

		AssignLevel (lvlBhvr);
		//leveltext.text = "Level:   " + levelBehaviour.SceneName ();
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

			// Check if this client occupies the first seat in session.
			// This grants right to choose level and start the game.
			/*Debug.Log("usernames[0] = " + usernames[0] + "     nameSelf = " + UserStatics.NameSelf);
			if (!usernames [0].Equals (UserStatics.NameSelf)) {
				buttonHolder.SetActive (false);
				leveltext.gameObject.SetActive (true);
			} else {
				buttonHolder.SetActive (true);
				leveltext.gameObject.SetActive (false);
			}*/
		}
    }

    public void StartUpdateLobby() {
		updateLobby = UpdateLobby ();
		StartCoroutine(updateLobby);
    }
}
