using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Constants {

	// Holds all types of GObject forms.
	public static string[] objectForms = {"Cube", "Cone", "Ball", "Sphere"};

	public static Color userColor = new Color (0.25f, 0.5f, 0.9f); //64;128;230 / #4080e6
	public static Color secondaryColor = new Color (0.9f, 0.5f, 0.25f);
	public static Color textColorHover = userColor;//new Color (1.0f, 1.0f, 1.0f);
	public static Color textColor = new Color(0.9f, 0.9f, 0.9f);
	// Movement speed of Users.
	public static float moveSpeed = 6f;
	public static float runSpeed = 12f;
	/// <summary>
	/// The factor for rotation speed.
	/// </summary>
	public static float rotationFactor = 5f;
	//public static string softwareModel = "SoftwareModel";
	public static string noUser = "noUser";
	public static string freeUser = "free";

	// Server Flags.
	public static string sfTimer = "timer";
	public static string sfState = "state";
	public static string sfPaused = "PAUSED";
	public static string sfLoading = "ISLOADING";
	public static string sfStarting = "ISSTARTING";
	public static string sfRunning = "RUNNING";
	public static string sfFinished = "FINISHED";
	public static string sfPConnected = "CONNECTED";
	public static string sfError = "ERROR";
	public static string sfHint = "HINT";
	public static string sfLevelindex = "levelindex";

	public static Color ColorForUser(int index) {
		switch (index) {
			case 2:
				return new Color (0.7f, 0.0f, 0.0f);
			case 3:
				return new Color (0.0f, 0.7f, 0.4f);
			case 4:
				return new Color (0.9f, 0.9f, 0.0f);
			default:
				return userColor;
		}
	}
}
