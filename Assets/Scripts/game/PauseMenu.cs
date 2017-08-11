using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using UnityEngine.SceneManagement;

public class PauseMenu : SoftwareBehaviour {
	
	public GameObject resumeButton, quitButton, pauseMenuCanvas, startMenuCanvas, mainCamera, startText, startButton;
	public GameObject pingText, timerText;

	private bool gamePaused = false;
	private bool GamePaused {
		set {
			gamePaused = value;
			SoftwareModel.GameRunning = !gamePaused && GameHasStarted;
		}
	}

	private bool gameHasStarted = false;
	private bool startedBefore = false;
	public bool GameHasStarted {
		get { return gameHasStarted; }
		set { 
			if (!startedBefore) {
				mainCamera.GetComponent<MainCameraBehaviour> ().PositionCamera ();
				gameHasStarted = true;
				startedBefore = value;
			}
		}
	}

	private List<Animator> animators = new List<Animator>();
	public void AddAnimator(Animator anim) {
		animators.Add (anim);
	}

	void Start() {

		TogglePause (false);
		ShowStartingMenu ();
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

	public void StartGame() {

		startText.GetComponent<Text> ().text = "WAIT...";
		startButton.SetActive (false);
		SoftwareModel.netwRout.TCPRequest(
			NetworkRoutines.EmptyCallback,
			new string[] { "req", "sessionId" },
			new string[] { "startTheGame", UserStatics.SessionId.ToString() });
	}
		
	public void Pause() {
		if (gameHasStarted) {
			SoftwareModel.netwRout.TCPRequest (
				NetworkRoutines.EmptyCallback,
				new string[] { "req", "sessionId" },
				new string[] { "pauseSession", UserStatics.SessionId.ToString () });
			TogglePause (true);
		}
	}

	public void Resume() {
		if (gameHasStarted) {
			SoftwareModel.netwRout.TCPRequest (
				NetworkRoutines.EmptyCallback,
				new string[] { "req", "sessionId" },
				new string[] { "resumeSession", UserStatics.SessionId.ToString () });
			TogglePause (false);
		}
	}

	public void End() {

		if (gameHasStarted) {
			resumeButton.SetActive (false);
			Pause ();
		}
	}

	public void Quit() {

		SoftwareModel.netwRout.TCPRequest(
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
		startMenuCanvas.SetActive (false);
		GamePaused = stop;

		ToggleAnimators (stop);
		//Time.timeScale = (stop ? 0f : 1f);        erstmal nur rausgenommen weil die Couruntines damit ebenfalls gestopt werden, müssen eine andere Loesung finden
	}

	private void ToggleAnimators(bool stop) {

		foreach (Animator anim in animators) {
			anim.speed = (stop ? 0f : 1f);
		}
	}
	public void ShowStartingMenu() {

		startText.GetComponent<Text> ().text = "Start Game";
		startButton.SetActive (true);
		startMenuCanvas.SetActive (true);
		pauseMenuCanvas.SetActive (false);
	}

	/// <summary>
	/// Sets the ping, that is displayed in game.
	/// </summary>
	/// <param name="state">State.</param>
	public void SetPing(string state) {

		pingText.GetComponent<Text> ().text = "Ping: " + state;
	}
}
