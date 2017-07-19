using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorText : MonoBehaviour {

	private float startTime = 0;
	private static int showTime = 3;
	private static bool show = false;

	public void ShowError(string error) {

		gameObject.GetComponent<Text> ().text = error;
		startTime = Time.realtimeSinceStartup;
		show = true;
	}

	void Update() {

		if (show) {
			if (Time.realtimeSinceStartup > startTime + showTime) {
				show = false;
				gameObject.GetComponent<Text> ().text = "";
			}
		}
	}

	public void ClearText() {
		show = false;
		gameObject.GetComponent<Text> ().text = "";
	}
}
