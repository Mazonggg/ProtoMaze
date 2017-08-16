using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds constant data concerning LevelBehaviours.
/// </summary>
public sealed class LevelConstants : SoftwareBehaviour{

	/// <summary>
	/// List of all levels contained in the game.
	/// 
	/// HOOK for adding new levels to the game.
	/// </summary>
	private static LevelBehaviour[] allLevels = new LevelBehaviour [] {
		new StartLevel_1 (),
		new StartLevel_2 ()
	};

	/// <summary>
	/// Total number of levels.
	/// </summary>
	/// <returns>The of levels.</returns>
	public static int NumberOfLevels () {
		return allLevels.Length;
	}

	/// <summary>
	/// Gets the level at the given index.
	/// </summary>
	/// <returns>The level.</returns>
	/// <param name="index">Index.</param>
	public static LevelBehaviour GetLevel (int index) {
		if (index >= 0 && index < allLevels.Length) {
			return allLevels [index];
		} else {
			return allLevels [0];
		}
	}
}
