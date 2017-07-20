using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using System.Text;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Handles all UDP communication with the server, while a session is RUNNING, PAUSED or ISSTARTING.
/// </summary>
public class SocketObject {

	private static string serverError = "Error";

	private Thread socketThread;
	private Socket socket;

	private static int port = 8050;
	private static IPAddress IPv4 = IPAddress.Parse("81.169.245.94");

	private IPEndPoint endPoint = new IPEndPoint(IPv4, port);

	private UserController userController;
	private TimerScript timerScript;
	private PauseMenu pauseMenu;

	public SocketObject(int timer){
		
		// Create the socket, that communicates with server.
		socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		string userId = UserStatics.GetUserId(0).ToString();
		string sessionId = UserStatics.SessionId.ToString();

		if (UserStatics.IsCreater) {
			
			GameObject.Find (Constants.softwareModel).GetComponent<SoftwareModel> ().netwRout.UDPRequest (
				NetworkRoutines.EmptyCallback,
				new string[] { "userId", "timer", "sessionId" }, 
				new string[] { userId, timer.ToString (),  sessionId });
		}

		GameObject.Find (Constants.softwareModel).GetComponent<SoftwareModel> ().netwRout.TCPRequest (
			HandleSessionStart,
			new string[] { "req", "sessionId", "userId" }, 
			new string[] { "startSession", sessionId, userId });

		userController = GameObject.Find (Constants.softwareModel).GetComponent<SoftwareModel> ().userController;
		pauseMenu = GameObject.Find ("PauseMenuController").GetComponent<PauseMenu> ();
		timerScript = GameObject.Find ("TimerText").GetComponent<TimerScript> ();

		WorkOnSocket ();
	}

	/// <summary>
	/// Handles the session start. Assigns the other users in the game-session to the respective GameObjects.
	/// </summary>
	/// <param name="response">Response.</param>
	private void HandleSessionStart(string[][] response) {

		string user_ref = "";
		int user_id = 0;
		string user_name = "";

		foreach (string[] pair in response) {
			if (pair[0].Equals ("ur")) {
					user_ref= pair[1]; 
			} else if (pair[0].Equals ("ui")) {
				int.TryParse(pair[1], out user_id);
			} else if (pair[0].Equals ("un")) {
				user_name = pair[1];
				userController.AddUser (user_ref, user_id, user_name);
			}
		}
	}

	/// <summary>
	/// Starts to work on socket.
	/// Connection issues can be caught here.
	/// </summary>
	private void WorkOnSocket(){
		
		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().StartCoroutine (TellSocket());
		GameObject.Find(Constants.softwareModel).GetComponent<SoftwareModel>().StartCoroutine (ListenToSocket());
    }

	// storage for upstream data.
	byte[] sendBuf = new byte[128];
	int sendBytes = 0;
	float lastDatagram = 0;
	float currentTime = 1;
	int countTicks = 0;
	/// <summary>
	/// Tells change in state of CObjects to server.
	/// </summary>
	/// <returns>The socket.</returns>
	private IEnumerator TellSocket(){

		yield return new WaitForSeconds(1f);

		SendDatagram();
		while (true) {
			// Transmitted data
			currentTime = Time.realtimeSinceStartup;
			// Only tick, if changes in game state is found and time since last tick fits tickrate.
			User usr = userController.ThisUser;

			if (usr.Updated && (currentTime - lastDatagram > 0.04)) {
				SendDatagram ();
				lastDatagram = currentTime;
			}
			yield return null;
		}
	}

	private int counting = 0;
	private void SendDatagram() {
		
		string info = CollectUserData ();
		sendBuf = System.Text.ASCIIEncoding.ASCII.GetBytes (info);
		sendBytes = socket.SendTo (sendBuf, endPoint);
	}


	// storage for upstream data.
	byte[] receiveBuf = new byte[128];
	/// <summary>
	/// Listens to downstream socket, connected to server, and updates state of CObjects accordingly.
	/// </summary>
	/// <returns>The to socket.</returns>
	private IEnumerator ListenToSocket (){

		yield return new WaitForSeconds(1f);

		while (true) {
			yield return null;
			if (socket.Poll(0, SelectMode.SelectRead)) {
				int bytesReceived = socket.Receive(receiveBuf, 0, receiveBuf.Length, SocketFlags.None);

				if (bytesReceived > 0) {
					ProcessDownBuf (receiveBuf);
				}
			}
		}		
	}

	/// <summary>
	/// Processes the content of the buf, received from server.
	/// checks, if content is valid, and sorts the information.
	/// </summary>
	/// <param name="buf">Buffer.</param>
	private void ProcessDownBuf(byte[] buf) {

		string bufString = System.Text.ASCIIEncoding.ASCII.GetString (buf);
		//Debug.Log ("ProcessDownBuf: " + bufString);
		string[] pairs = bufString.Split('&');

		for (int i = 0; i < pairs.Length; i++) {
			string[] pair = pairs [i].Split ('=');
			if (pair [0].Equals ("ui")) {
				
				int user_id = -1;
				int.TryParse (pair [1], out user_id);
				string[] posRot = pairs [i + 1].Split ('=')[1].Split(';');
				string[] pos = posRot [0].Split('_');
				string[] rot = posRot [1].Split('_');

				float posX = 999;
				float posY = 999;
				float posZ = 999;

				float.TryParse (pos [0], out posX);
				float.TryParse (pos [1], out posY);
				float.TryParse (pos [2], out posZ);

				float rotX = 999;
				float rotY = 999;
				float rotZ = 999;

				float.TryParse (pos [0], out rotX);
				float.TryParse (pos [1], out rotY);
				float.TryParse (pos [2], out rotZ);

				userController.UpdateUser(new UpdateData(user_id, new Vector3(posX, posY, posZ), new Vector3(rotX, rotY, rotZ)));
			} else if (pair [0].Equals (Constants.sfState)) {

				if (pair [1].Equals (Constants.sfPaused)) {
					// LOGIC FOR PAUSING THE GAME.	
					pauseMenu.TogglePause (true);
				} else if (pair [1].Equals (Constants.sfRunning)) {
					// LOGIC TO RESUME THE GAME.
					pauseMenu.TogglePause (false);
					// Checks if game was started before and acts accordingly to start it.
					if (!pauseMenu.GameHaseStarted) {
						pauseMenu.GameHaseStarted = true;
					}
				} else if (pair [1].Equals (Constants.sfStarting)) {
					// LOGIC TO SHOW STARTING MENU.
					pauseMenu.ShowStartingMenu ();
				} 
				pauseMenu.ShowState (pair [1]);
			} else if(pair[0].Equals (Constants.sfTimer)) {
				int time = 0;
				int.TryParse (pair [1], out time);
				timerScript.SetTimer (time);
				if (time <= 0) {
					pauseMenu.End (0);
				}
			}
		}
	}

	/// <summary>
	/// Collects the data relevant for server update of this player and converts it to string convention.
	/// </summary>
	/// <returns>The user data.</returns>
	private string CollectUserData() {
		
		UpdateData userData = userController.ThisUser.UpdateData;

		string msg = "t=";
		if (userData.ObjectHeld == null) {
			msg += "1";
		} else {
			msg += "2";
		}

		msg += "&ui=" + userData.Id;
		msg += "&up=" +
			userData.Position.x + "_" +
			userData.Position.y + "_" +
			userData.Position.z + ";" +
			userData.Rotation.x + "_" +
			userData.Rotation.y + "_" +
			userData.Rotation.z;

		if (userData.ObjectHeld != null) {
			msg += "&oi=" + userData.ObjectHeld.Id;
			msg += "&op=" +
				userData.ObjectHeld.Position.x + "_" +
				userData.ObjectHeld.Position.y + "_" +
				userData.ObjectHeld.Position.z + ";" +
				userData.ObjectHeld.Rotation.x + "_" +
				userData.ObjectHeld.Rotation.y + "_" +
				userData.ObjectHeld.Rotation.z;
		}
		return msg;
	}
}
