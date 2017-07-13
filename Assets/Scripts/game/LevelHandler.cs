using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler {

	private int playerCount = 2;
	private List<GObject> gObjects = new List<GObject>();
	public void AddGObject(GObject obj) {
		if (!gObjects.Contains (obj)) {
			gObjects.Add (obj);
		}
	}
	public int PlayerCount {
		get { return playerCount;}
	}




	/*
	 * Checks, if given GObject has collision with any of
	 * the other GObjects in the scene and returns the collided 
	 * GObject accordignly.
	 */
	public GObject hasCollision (GObject obj) {

		foreach (GObject collObj in gObjects) {
			if (obj != collObj) {

				return collObj;
			}
		}
		return null;
	}
}
