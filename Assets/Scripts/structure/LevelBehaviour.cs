using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Declares behaviour of every Level, so that the rest 
/// of the software has fixed reference points for interacting.
/// </summary>
public abstract class LevelBehaviour : MonoBehaviour {

	/// <summary>
	/// Gets the timer, that determines how much time 
	/// the user have to complete the level, before timeout.
	/// </summary>
	/// <returns>The timer.</returns>
	abstract protected int GetTimer ();
	/// <summary>
	/// Finishs the level.
	/// </summary>
	abstract protected void FinishLevel ();
	/// <summary>
	/// Declares the starting positions for the users.
	/// </summary>
	/// <returns>The positions.</returns>
	/// <param name="index">Index.</param>
	abstract protected Vector3 StartPositions (int index);

	/// <summary>
	/// Gets the start position for a specific index of player in UserHandler.
	/// </summary>
	/// <returns>The start position.</returns>
	/// <param name="index">Index.</param>
	public Vector3 GetStartPosition (int index) {
		
		return StartPositions (index);
	}
	/// <summary>
	/// Creates the SocketObject, when the scene starts.
	/// Uses the GetTimer to determine time until timeout in level.
	/// </summary>
	void Start () {

		GameObject.Find (Constants.softwareModel).GetComponent<SoftwareModel> ().CreateSocketObject (GetTimer());
	}
}
