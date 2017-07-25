using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinSessionButtonPrefab : MonoBehaviour {

    public Button joinSessionButton;
    public Text sessionIDText;
    public Text leaderText;
    public GameObject joinSessionCanvas, joinSessionController, createSessionCanvas;


	// Use this for initialization
	void Start () {
        joinSessionButton.onClick.AddListener(MakeRequest);
    }

    public void SetUp (string sessionID, string leader) {
        sessionIDText.text = sessionID;
        leaderText.text = leader;
    }


    public void MakeRequest() {
		string userId = UserStatics.IdSelf.ToString();
		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
            SetSessionIdAndGoToLobby,
            new string[] { "req", "sessionId", "userId" },
            new string[] { "joinSession", sessionIDText.text, userId });
    }

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
