using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSession : SoftwareBehaviour {
	
	// Update is called once per frame
	void Update () {
		
	}
		
	/// <summary>
	/// Starts the next scene.
	/// </summary>
	public void LoadNewScene() {
		gameObject.SetActive (false);
		SceneManager.LoadScene ( gameObject.GetComponent<CreateSession> ().levelBehaviour.SceneName ());
	}
}
