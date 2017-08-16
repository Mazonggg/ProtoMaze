using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Holds all function for joining a Session and controlling the respective Menu.
/// </summary>
public class JoinSession : SoftwareBehaviour {
	// GameObject parameters:
    public GameObject logInCanvas, mainMenuCanvas, JoinSessionCanvas, backButton;
    public GameObject JoinButton;
    public GameObject content;

	/// <summary>
	/// Called by unity engine, when gameObject is instantiated.
	/// 
	/// Hides the menu when game is opened.
	/// </summary>
    void Start () {

        JoinSessionCanvas.SetActive(false);
	}

	/// <summary>
	/// Navigates back to MainMenu.
	/// </summary>
    public void GoBack() {

        mainMenuCanvas.SetActive(true);
        JoinSessionCanvas.SetActive(false);
    }

	/// <summary>
	/// Delegates to get the current list of all available sessions on server.
	/// </summary>
	public void RefreshSessions() {

		GetSessions ();
	}

	/// <summary>
	/// Adds the buttons to the list of all available sessions on server.
	/// </summary>
	/// <param name="sessionList">Session list.</param>
    private void AddButtons(string[][] sessionList) {

        for (var i = content.transform.childCount - 1; i >= 1; i--) {

            var oldButton = content.transform.GetChild(i);
            oldButton.transform.parent = null;

        }

        int count = 0;
		if(sessionList != null){
        	for (int i = 0; i < sessionList.Length; i++) {
				
				GameObject newButton = (GameObject)Instantiate (JoinButton);
				newButton.GetComponent<JoinSessionButtonPrefab> ().SetUp (sessionList [i] [0], sessionList [i] [1]);

				Vector3 scale = JoinSessionCanvas.transform.lossyScale;  //die und folgende Zeile war eine schwere Geburt ...
				newButton.transform.localScale = scale;     
				newButton.transform.SetParent (content.transform);
				if (sessionList [i] [1].Equals ("_")) {
					newButton.GetComponent<Image> ().enabled = false;
					newButton.GetComponent<JoinSessionButtonPrefab> ().SetUp ("", "");
					newButton.GetComponent<JoinSessionButtonPrefab> ().enabled = false;
				} else {
					newButton.GetComponent<Image> ().enabled = true;
					newButton.GetComponent<PointerHandler> ().enabled = true;
				}
				count = i;
        	}
        	RectTransform newRT = content.GetComponent<RectTransform>();        //to get the RectTransform from content
        	newRT.sizeDelta = new Vector2(0, count * 100);                      //and strech it to fit all Buttons
		}
    }

	/// <summary>
	/// Fetches list of all available sessions from server with TCP request.
	/// 
	/// </summary>
    public void GetSessions() {
		
		SoftwareModel.netwRout.TCPRequest(
            ListAllSessions,
            new string[] { "req", "userId" },
			new string[] { "getSessions", UserStatics.IdSelf.ToString() });
    }

	/// <summary>
	/// Compiles datagram and causes the Menu to add the buttons for the sessions.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
	/// </summary>
	/// <param name="response">Response.</param>
    private void ListAllSessions(string[][] response) {

        string ret = "";

        foreach (string[] pair in response) {

          	if (pair[0].Equals("sessions")) {
             	ret += pair[1];
           	}
        }
		if (!ret.Equals ("")) { 
			string pattern = @"//|--";
			string[] sessionsAndLeader = Regex.Split (ret.TrimEnd ('-'), pattern);
			int i = 0;
			string[][] sessionList = new string[sessionsAndLeader.Length / 2][];
			for (int j = 0; j < sessionList.Length; j++) {
				sessionList [j] = new string[2];
			}

			foreach (string element in sessionsAndLeader) {
				sessionList [i / 2] [i % 2] = element;
				i++;
			}
			AddButtons (sessionList);
		} else {
			AddButtons (null);
		}
    }
}
