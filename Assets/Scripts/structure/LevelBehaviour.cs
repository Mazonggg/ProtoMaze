using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Declares behaviour of every Level, so that the rest 
/// of the software has fixed reference points for interacting.
/// </summary>
public abstract class LevelBehaviour : SoftwareBehaviour {

	/// <summary>
	/// Returns the name of the scene object, that this level represents.
	/// </summary>
	/// <returns>The scenes name.</returns>
	abstract public string SceneName ();
	/// <summary>
	/// Gives a brief explanation of the level.
	/// </summary>
	/// <returns>The text.</returns>
	abstract public string ExplanationText ();
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
	/// Declares the starting rotations for the users.
	/// </summary>
	/// <returns>The rotations.</returns>
	/// <param name="index">Index.</param>
	abstract protected Vector3 StartRotations (int index);
	/// <summary>
	/// Returns the seconds, the PressurePlates take to reset during play.
	/// </summary>
	/// <returns>The plate time out.</returns>
	/// <param name="playerCount">Player count.</param>
	abstract protected int PlateTimeOuts (int playerCount);
	/// <summary>
	/// Gets the start position for a specific index of player in UserHandler.
	/// </summary>
	/// <returns>The start position.</returns>
	/// <param name="index">Index.</param>
	public Vector3 GetStartPosition (int index) {

		return StartPositions (index);
	}	
	/// <summary>
	/// Gets the start rotation for a specific index of player in UserHandler.
	/// </summary>
	/// <returns>The start rotation.</returns>
	/// <param name="index">Index.</param>
	public Vector3 GetStartRotation (int index) {

		return StartRotations (index);
	}
	/// <summary>
	/// Returns the time, the PressurePlate take to reset during Play.
	/// </summary>
	/// <returns>The plate time outs.</returns>
	/// <param name="playerCount">Player count.</param>
	public int GetPlateTimeOut (int playerCount) {

		return PlateTimeOuts (playerCount);
	}
	/// <summary>
	/// Creates the SocketObject, when the scene starts.
	/// Uses the GetTimer to determine time until timeout in level.
	/// 
	/// Redefines Gravity in the Game.
	/// </summary>
	void Start () {

		SoftwareModel.CreateSocketObject (GetTimer());
		Physics.gravity = new Vector3 (0, - 30f, 0);
	}
}
