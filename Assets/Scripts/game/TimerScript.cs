using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

	// Update is called once per frame
	public void SetTimer (int time) {
		if (time > 0) {
			gameObject.GetComponent<Text> ().text = ConvertSeconds (time);
			gameObject.GetComponent<Text> ().color = Constants.textColor;
		} else {
			gameObject.GetComponent<Text> ().text = "Time over";
			gameObject.GetComponent<Text> ().color = Constants.secondaryColor;
		}

	}
	/// <summary>
	/// Converts the seconds to clock time.
	/// </summary>
	/// <returns>The seconds.</returns>
	/// <param name="time">Time.</param>
	private string ConvertSeconds(int time) {
		return "Time: " + (time / 60 < 10 ? "0" : "") + (time / 60) + ":" + (time % 60 < 10 ? "0" : "") + (time % 60);
	}
}
