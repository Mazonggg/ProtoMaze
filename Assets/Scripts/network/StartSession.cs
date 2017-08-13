using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSession : SoftwareBehaviour {

	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sends the TCP Request to change Session status to "STARTING",
	/// Starts the next scene.
	/// </summary>
	public void StartTheSession(){

		string sessionId = UserStatics.SessionId.ToString();

		SoftwareModel.netwRout.TCPRequest (
			LoadNewScene,
			new string[] { "req", "sessionId" }, 
			new string[] { "loadSession", sessionId });
	}

	/// <summary>
	/// Starts the next scene.
	/// </summary>
	private void LoadNewScene(string[][] response) {
		gameObject.SetActive (false);
		SceneManager.LoadScene ( gameObject.GetComponent<CreateSession> ().levelBehaviour.SceneName ());
	}
}
