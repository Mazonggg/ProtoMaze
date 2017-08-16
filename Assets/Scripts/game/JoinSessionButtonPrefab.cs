using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Prefab-script for a button in JoinSessionCanvas, that handles joining of a session.
/// </summary>
public class JoinSessionButtonPrefab : SoftwareBehaviour {

	// GameObject parameters:
    public Button joinSessionButton;
    public Text sessionIDText;
    public Text leaderText;
    public GameObject joinSessionCanvas, joinSessionController, createSessionCanvas;


	/// <summary>
	/// Called by unity engine, when gameObject is instantiated.
	/// 
	/// Enables the function of the button.
	/// </summary>
	void Start () {
        joinSessionButton.onClick.AddListener(MakeRequest);
    }

	/// <summary>
	/// Sets up the information, that is displayed in the buttons panel.
	/// </summary>
	/// <param name="sessionID">Session I.</param>
	/// <param name="leader">Leader.</param>
    public void SetUp (string sessionID, string leader) {
        sessionIDText.text = sessionID;
        leaderText.text = leader;
    }

	/// <summary>
	/// Makes the request to join the chosen session.
	/// </summary>
    public void MakeRequest() {
		string userId = UserStatics.IdSelf.ToString();
		SoftwareModel.netwRout.TCPRequest(
            SetSessionIdAndGoToLobby,
            new string[] { "req", "sessionId", "userId" },
            new string[] { "joinSession", sessionIDText.text, userId });
    }

	/// <summary>
	/// Stores the session id and opens menu for session.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
	/// </summary>
	/// <param name="response">Response.</param>
    public void SetSessionIdAndGoToLobby(string[][] response) {

        int SsIdTmp = -1;

        foreach (string[] pair in response) {
			// When ERROR on server is caught, refresh the list, instead of joining.
			if (pair[0].Equals("type") && pair[1].Equals(Constants.sfError)) {

				joinSessionController.GetComponent<JoinSession> ().RefreshSessions ();
                return;
            } else if (pair[0].Equals("sessionId")) {
                int.TryParse(pair[1], out SsIdTmp);
				UserStatics.SessionId = SsIdTmp;

                joinSessionCanvas.SetActive(false);
                createSessionCanvas.SetActive(true);
                createSessionCanvas.GetComponentInChildren<CreateSession>().StartUpdateLobby();
                return;
            }
        }
    }
}
