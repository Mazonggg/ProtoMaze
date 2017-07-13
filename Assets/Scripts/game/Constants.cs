using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Constants {

	// Holds all types of GObject forms.
	public static string[] objectForms = {"Cube", "Cone", "Ball", "Sphere"};

	public static Color userColor = new Color (0.25f, 0.5f, 0.9f);
	public static Color secondaryColor = new Color (0.9f, 0.5f, 0.25f);
	public static Color defaultColor = new Color (0.0f, 0.0f, 0.0f);
	// Movement speed of Users.
	public static float moveSpeed = 10f;
	public static string softwareModel = "SoftwareModel";
	public static string noUser = "noUser";
	public static string freeUser = "free";

	// Server Flags.
	public static string sfTimer = "timer";
	public static string sfState = "state";
	public static string sfPaused = "PAUSED";
	public static string sfStarting = "ISSTARTING";
	public static string sfRunning = "RUNNING";
	public static string sfPConnected = "CONNECTED";
	public static string sfError = "ERROR";
	public static string sfHint = "HINT";
}
