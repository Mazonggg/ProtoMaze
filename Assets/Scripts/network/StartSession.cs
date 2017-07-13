using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSession : MonoBehaviour {

	public GameObject /*createSessionCanvas,*/ startSessionButton;

    void Start () {
        //createSessionCanvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sends the TCP Request to change Session status to "STARTING",
	/// Starts the next scene.
	/// </summary>
	public void StartTheSession(){

		UserStatics.IsCreater = true;
		LoadNewScene ();
	}

	/// <summary>
	/// Starts the next scene.
	/// </summary>
	public void LoadNewScene() {
		SceneManager.LoadScene ("Level_1");
	}
}
