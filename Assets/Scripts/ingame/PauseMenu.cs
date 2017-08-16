using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behaviour of the pause menu, while playing a level.
/// </summary>
public class PauseMenu : SoftwareBehaviour {

	// GameObject parameters.
	public GameObject resumeButton, quitButton, pauseMenuCanvas, startMenuCanvas, mainCamera, startText, startButton, itemHolder;
	public Text winText;
	public GameObject pingText;

	/// <summary>
	/// Determines, if game is currently paused.
	/// </summary>
	private bool gamePaused = false;
	private bool GamePaused {
		set {
			gamePaused = value;
			SoftwareModel.GameRunning = !gamePaused && GameHasStarted;
		}
	}

	/// <summary>
	/// Tells, if the game has been started before in the current level.
	/// </summary>
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

	/// <summary>
	/// Called by unity engine, when gameObject is instantiated.
	/// 
	/// Hides the PauseMenu and shows the menu for starting the game in the current level.
	/// </summary>
	void Start() {

		TogglePause (false);
		ShowStartingMenu ();
	}

	/// <summary>	
	/// Frequently called by unity engine.
	/// 
	/// Listens if the escape button was pressed and toggles the PauseMenu accordingly.
	/// </summary>
	void Update(){
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (gamePaused) {
				Resume ();
			} else {
				Pause ();
			}
		}
	}

	/// <summary>
	/// Starts the game in the current level with TCP request to server.
	/// </summary>
	public void StartGame() {

		startText.GetComponent<Text> ().text = "WAIT...";
		startButton.SetActive (false);
		SoftwareModel.netwRout.TCPRequest(
			NetworkRoutines.EmptyCallback,
			new string[] { "req", "sessionId" },
			new string[] { "startTheGame", UserStatics.SessionId.ToString() });
	}
		
	/// <summary>
	/// Pauses the current game with TCP request to server.
	/// </summary>
	public void Pause() {
		if (gameHasStarted) {
			SoftwareModel.netwRout.TCPRequest (
				NetworkRoutines.EmptyCallback,
				new string[] { "req", "sessionId" },
				new string[] { "pauseSession", UserStatics.SessionId.ToString () });
			TogglePause (true);
		}
	}

	/// <summary>
	/// Resumes the current game with TCP request to server.
	/// </summary>
	public void Resume() {
		if (gameHasStarted) {
			SoftwareModel.netwRout.TCPRequest (
				NetworkRoutines.EmptyCallback,
				new string[] { "req", "sessionId" },
				new string[] { "resumeSession", UserStatics.SessionId.ToString () });
			TogglePause (false);
		}
	}

	/// <summary>
	/// Response to an ended level, by showing part of PauseMenu, that allows to quit the level in game.
	/// </summary>
	public void End() {

		if (gameHasStarted) {
			resumeButton.SetActive (false);
			Pause ();
		}
	}

	/// <summary>
	/// Quits the level in game with TCP request.
	/// </summary>
	public void Quit() {

		SoftwareModel.netwRout.TCPRequest(
			StartMainMenu,
			new string[] { "req", "userId" },
			new string[] { "leaveSession", UserStatics.IdSelf.ToString() });
	}

	/// <summary>
	/// Sends user back to MainMenu.
	/// 
	/// CALLBACK FUNCTION FOR TCP-Request.
	/// </summary>
	/// <param name="response">Response.</param>
	private void StartMainMenu(string[][] response){
		Time.timeScale = 1f;
		SceneManager.LoadScene ("Menu");
	}

	/// <summary>
	/// Toggles the PauseMenu on and off according to given bool.
	/// </summary>
	/// <param name="stop">If set to <c>true</c> stop.</param>
	public void TogglePause(bool stop) {

		pauseMenuCanvas.SetActive(stop);
		startMenuCanvas.SetActive (false);
		GamePaused = stop;

		ToggleAnimators (stop);
	}

	/// <summary>
	/// Toggles the animators, which stops and restarts animation of users.
	/// </summary>
	/// <param name="stop">If set to <c>true</c> stop.</param>
	private void ToggleAnimators(bool stop) {

		foreach (Animator anim in animators) {
			anim.speed = (stop ? 0f : 1f);
		}
	}

	/// <summary>
	/// Shows the starting menu.
	/// </summary>
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

	/// <summary>
	/// Called when session is finished. Shows menu and elapsed time accordingly.
	/// </summary>
	/// <param name="timeElapsed">Time elapsed.</param>
	public void FinishSession(int timeElapsed) {

		if (gameHasStarted) {
			winText.text = "You have won in " + timeElapsed + " seconds !";
			itemHolder.SetActive (false);
			resumeButton.SetActive (false);
			Pause ();
		}
	}
}
