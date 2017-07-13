using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinSessionButtonPrefab : MonoBehaviour {

    public Button joinSessionButton;
    public Text sessionIDText;
    public Text leaderText;
    public GameObject joinSessionCanvas, createSessionCanvas;


	// Use this for initialization
	void Start () {
        joinSessionButton.onClick.AddListener(MakeRequest);
    }

    public void SetUp (string sessionID, string leader) {
        sessionIDText.text = sessionID;
        leaderText.text = leader;
    }


    public void MakeRequest() {
		string userId = UserStatics.GetUserId(0).ToString();
		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
            SetSessionIdAndGoToLobby,
            new string[] { "req", "sessId", "userId" },
            new string[] { "joinSession", sessionIDText.text, userId });
    }

	/// <summary>
	/// Changes the color of the field.
	/// </summary>
	/// <param name="color">Color.</param>
	public void ChangeColor() {

		ColorBlock colors = joinSessionButton.colors;
		colors.normalColor = new Color (0.8f, 0.9f, 1f);
		joinSessionButton.colors = colors;
	}


    public void SetSessionIdAndGoToLobby(string[][] response) {

        int SsIdTmp = -1;

        foreach (string[] pair in response) {

			/*if (pair[0].Equals("type") && pair[1].Equals(Constants.sfHint)) {

                Debug.Log("User already assigned to a session or Session does not exist!");
                return;
            }*/
            if (pair[0].Equals("sessionId")) {
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
