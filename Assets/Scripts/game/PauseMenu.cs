using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	
	public GameObject resumeButton, quitButton, pauseMenuCanvas;
	public GameObject debugText, timerText;

	private bool gamePaused = false;

	void Start() {

		TogglePause (false);
	}


	void Update(){
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (gamePaused) {
				Resume ();
			} else {
				Pause ();
			}
		}
	}

	public void Pause() {
		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
			NetworkRoutines.EmptyCallback,
			new string[] { "req", "sessionId" },
			new string[] { "pauseSession", UserStatics.SessionId.ToString() });
		TogglePause (true);
	}

	public void Resume() {
		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
			NetworkRoutines.EmptyCallback,
			new string[] { "req", "sessionId" },
			new string[] { "resumeSession", UserStatics.SessionId.ToString() });
		TogglePause (false);
	}

	public void Quit() {

		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().netwRout.TCPRequest(
			StartMainMenu,
			new string[] { "req", "userId" },
			new string[] { "leaveSession", UserStatics.IdSelf.ToString() });
	}

	private void StartMainMenu(string[][] response){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Menu");
	}

	public void TogglePause(bool stop) {

		pauseMenuCanvas.SetActive(stop);
		gamePaused = stop;

		Time.timeScale = (stop ? 0f : 1f);
	}

	// TODO dev. helper
	public void ShowState(string state) {

		debugText.GetComponent<Text> ().text = "Game state: " + state;
	}
}
