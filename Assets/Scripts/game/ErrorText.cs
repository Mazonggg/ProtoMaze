using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script, that handles displaying of errors to user, while in MainMenu.
/// </summary>
public class ErrorText : MonoBehaviour {

	private float startTime = 0;
	private static int showTime = 3;
	private static bool show = false;

	/// <summary>
	/// Shows the error to the user.
	/// </summary>
	/// <param name="error">Error.</param>
	public void ShowError(string error) {

		gameObject.GetComponent<Text> ().text = error;
		startTime = Time.realtimeSinceStartup;
		show = true;
	}

	/// <summary>
	/// Frequently called by unity engine.
	/// 
	/// hides the error text after showTime has expired.
	/// </summary>
	void Update() {

		if (show) {
			if (Time.realtimeSinceStartup > startTime + showTime) {
				show = false;
				gameObject.GetComponent<Text> ().text = "";
			}
		}
	}

	/// <summary>
	/// Clears the text shown to user.
	/// </summary>
	public void ClearText() {
		show = false;
		gameObject.GetComponent<Text> ().text = "";
	}
}
