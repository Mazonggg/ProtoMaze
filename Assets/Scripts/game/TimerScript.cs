using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that controlls the display of time left in game level.
/// </summary>
public class TimerScript : SoftwareBehaviour {

	/// <summary>
	/// Returns rest time for general requests.
	/// </summary>
	private static int restTime;
	public static int RestTime {
		get { return restTime; }
	}

	/// <summary>	
	/// Frequently called by unity engine.
	/// 
	/// Sets the timer according to given time.
	/// If time has elapsed, it tells the socket, to stop working.
	/// -> Thus functions as a pseudo callback, which circumvents timing issues.
	/// </summary>
    public bool SetTimer (int time) {
		restTime = time;
		if (time > 0) {
			gameObject.GetComponent<Text> ().text = ConvertSeconds (time);
			gameObject.GetComponent<Text> ().color = Constants.textColor;
            return true;
		} else {
			gameObject.GetComponent<Text> ().text = "Time over";        
			gameObject.GetComponent<Text> ().color = Constants.secondaryColor;
            SoftwareModel.SocketObj.KillSocket();
            return false;
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
