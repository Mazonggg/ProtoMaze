using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Handles all UDP communication with the server, while a session is RUNNING, PAUSED or ISSTARTING.
/// </summary>
public class SocketObject: SoftwareBehaviour {
	
	private static string nothingFound = "nothingFound";

	private Thread socketThread;
	private Socket socket;

	private static int port = 8050;
	private static IPAddress IPv4 = IPAddress.Parse("81.169.245.94");

	private IPEndPoint endPoint = new IPEndPoint(IPv4, port);

	private TimerScript timerScript;
	private PauseMenu pauseMenu;

	private int levelTimer = 0;
	private bool socketRunning = false;

	public void SetSocket(int timer){
		
		levelTimer = timer;
		// Create the socket, that communicates with server.
		socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

		string userId = UserStatics.GetUserId(0).ToString();
		string sessionId = UserStatics.SessionId.ToString();

		SoftwareModel.netwRout.TCPRequest (
			HandleSessionStart,
			new string[] { "req", "sessionId", "userId" }, 
			new string[] { "startSession", sessionId, userId });
	}
	/// <summary>
	/// Handles the session start. Assigns the other users in the game-session to the respective GameObjects.
	/// 
	/// Adds the plates and objects to server and Starts socket, if necessary.
	/// </summary>
	/// <param name="response">Response.</param>
	private void HandleSessionStart(string[][] response) {
        
		string user_ref = "";
		int user_id = 0;
		string user_name = "";
		string sessionId = UserStatics.SessionId.ToString ();
		bool startedSocket = false;

		foreach (string[] pair in response) {
			// Check, if session is already in running state, otherwise start it.
			if (pair [0].Equals ("state") && !(pair [1].Equals (Constants.sfRunning) || pair [1].Equals (Constants.sfStarting))) {

				startedSocket = true;
				string plateIds = "";
				for (int i = 0; i < SoftwareModel.PlateContr.GetPlateCount (); i++) {
					plateIds += i + (i < SoftwareModel.PlateContr.GetPlateCount () - 1 ? "//" : "");
				}
				SoftwareModel.netwRout.TCPRequest (
					CollectPlateIds,
					new string[] { "req", "sessionId", "plateIds" }, 
					new string[] { "addPlatesToSession", sessionId, plateIds });

				string userId = UserStatics.GetUserId (0).ToString ();
				SoftwareModel.netwRout.UDPRequest (
					NetworkRoutines.EmptyCallback,
					new string[] { "userId", "timer", "sessionId" }, 
					new string[] { userId, levelTimer.ToString (),  sessionId });
			} else if (pair[0].Equals ("ur")) {
					user_ref= pair[1]; 
			} else if (pair[0].Equals ("ui")) {
				int.TryParse(pair[1], out user_id);
			} else if (pair[0].Equals ("un")) {
				user_name = pair[1];
				SoftwareModel.userController.AddUser (user_ref, user_id, user_name);
			}
		}
		// Collect the plateIds, if the session was started by somebody else:
		if (!startedSocket) {
			CollectPlateIds ();
		}
		pauseMenu = GameObject.Find ("PauseMenuController").GetComponent<PauseMenu> ();
		timerScript = GameObject.Find ("TimerText").GetComponent<TimerScript> ();

		WorkOnSocket ();
	}

	/// <summary>
	/// Collect the ids of the plates in the session.
	/// </summary>
	/// <param name="response">Response.</param>
	private void CollectPlateIds(params string[][] response) {

		string sessionId = UserStatics.SessionId.ToString ();
		SoftwareModel.netwRout.TCPRequest (
			SoftwareModel.PlateContr.AssignThePlateIds,
			new string[] { "req", "sessionId" }, 
			new string[] { "getPlatesInSession", sessionId });
	}

	/// <summary>
	/// Starts to work on socket.
	/// Connection issues can be caught here.
	/// </summary>
	private void WorkOnSocket(){

		StartCoroutine (TellSocket());
		StartCoroutine (ListenToSocket());
		socketRunning = true;
    }

	// storage for upstream data.
	byte[] sendBuf = new byte[128];
	float lastDatagram = 0;
	float currentTime = 1;
	/// <summary>
	/// Tells change in state of CObjects to server.
	/// </summary>
	/// <returns>The socket.</returns>
	private IEnumerator TellSocket(){

        yield return new WaitForSeconds(1f);
        SendDatagram();
        while (socketRunning) {
			// Transmitted data
			currentTime = Time.realtimeSinceStartup;
			// Only tick, if changes in game state is found and time since last tick fits tickrate.
			User usr = SoftwareModel.userController.ThisUser;

			if (usr.Updated && (currentTime - lastDatagram > 0.04)) {
				SendDatagram ();
				lastDatagram = currentTime;
			}
			yield return null;
		}
		
	}

	private void SendDatagram() {
        try {
            string info = CollectUserData();
            if (!info.Equals(nothingFound)) {
                sendBuf = System.Text.ASCIIEncoding.ASCII.GetBytes(info);
                socket.SendTo(sendBuf, endPoint);
            }
        } catch (Exception e) {
            Debug.Log("SOCKETEXEPTION GECTACHT"+ "Exception caught."+ e);
        }
    }     


	// storage for upstream data.
	byte[] receiveBuf = new byte[128];
    /// <summary>
    /// Listens to downstream socket, connected to server, and updates state of CObjects accordingly.
    /// </summary>
    /// <returns>The to socket.</returns>
    private IEnumerator ListenToSocket(){
		
            yield return null;
            while (socketRunning){

                yield return null;
                try {
                    while (socket.Poll(0, SelectMode.SelectRead) && socketRunning)
                    {
                        //  Debug.Log("GEht hier rein UUUND bricht ab?");

                        int bytesReceived = socket.Receive(receiveBuf, 0, receiveBuf.Length, SocketFlags.None);
                        if (bytesReceived > 0)
                        {
                            ProcessDownBuf(receiveBuf);
                        }
                    }
                }
                catch (Exception e) {
                	Debug.Log("SOCKETEXEPTION GECTACHT " +  socketRunning + "      blubb      "+e);
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
		string[] pairs = bufString.Split('&');
		for (int i = 0; i < pairs.Length; i++) {
			string[] pair = pairs [i].Split ('=');
			if (pair [0].Equals ("PING") && pair [1].Equals ("PING")) {
				pauseMenu.SetPing (((int) ((Time.realtimeSinceStartup - lastTime) * 1000)).ToString());
				return;
			} else if (pair [0].Equals ("ui")) {
				
				int user_id = -1;
				int.TryParse (pair [1], out user_id);
				string[] posRot = pairs [i + 1].Split ('=') [1].Split (';');
				string[] pos = posRot [0].Split ('_');
				string[] rot = posRot [1].Split ('_');

				float posX = 0;
				float posY = 0;
				float posZ = 0;

				float.TryParse (pos [0], out posX);
				float.TryParse (pos [1], out posY);
				float.TryParse (pos [2], out posZ);

				float rotX = 0;
				float rotY = 0;
				float rotZ = 0;

				float.TryParse (rot [0], out rotX);
				float.TryParse (rot [1], out rotY);
				float.TryParse (rot [2], out rotZ);

				SoftwareModel.userController.UpdateUser (new UpdateData (user_id, new Vector3 (posX, posY, posZ), new Vector3 (rotX, rotY, rotZ)));
			} else if (pair [0].Equals (Constants.sfState)) {

				if (pair [1].Equals (Constants.sfPaused)) {
					// LOGIC FOR PAUSING THE GAME.	
					pauseMenu.TogglePause (true);
				} else if (pair [1].Equals (Constants.sfRunning)) {
					// LOGIC TO RESUME THE GAME.
					pauseMenu.TogglePause (false);
					// Checks if game was started before and acts accordingly to start it.
					if (!pauseMenu.GameHasStarted) {
						pauseMenu.GameHasStarted = true;
					}
				} else if (pair [1].Equals (Constants.sfFinished)) {
					// LOGIC FOR FINISHING THE GAME.	
					socketRunning = false;
					SoftwareModel.finisherPlate.SetPlateActive ();	
					pauseMenu.FinishSession (levelTimer - TimerScript.RestTime);
				}
			} else if (pair [0].Equals (Constants.sfTimer)) {
				int time = 0;
				int.TryParse (pair [1], out time);
				socketRunning = timerScript.SetTimer (time);
			} else if (pair [0].Equals ("os")) {
				string pattern = @"//";
				string pattern2 = @"##";
				string[] statesStrings = Regex.Split (pair[1], pattern);

				foreach (string stateString in statesStrings) {
					string[] statePair = Regex.Split (stateString, pattern2);
					int plateId = -1;
					int.TryParse (statePair [0], out plateId);
					bool st = statePair [1].Equals ("1");
					SoftwareModel.PlateContr.SetPlateState (plateId, st);
				}
			}
		}
	}

	/// <summary>
	/// Collects the data relevant for server update of this player and converts it to string convention.
	/// </summary>
	/// <returns>The user data.</returns>
	private string CollectUserData() {

		if (SoftwareModel.userController.ThisUser != null) {
			UpdateData userData = SoftwareModel.userController.ThisUser.GetUpdateData ();

			string msg = "t=";
			if (userData.ObjectHeld == null) {
				msg += "A";
			} else {
				msg += "B";
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
		} else {
			return nothingFound;
		}
	}
		
	private float lastTime = 0;
	private float lastPing = 0;
	private float pingInt = 1;

    // War ein Grund warum er abgekackt ist, hat gesendet bevor ueberhaupt was aufgebaut war ..
    // Einfach eine Abfrage rein, ob der Socket schon am Arbeiten ist (y).

    /// <summary>
    /// Update method user to frequently count the ping between client and server
    /// and send updates of user once per second.
    /// </summary>
    void Update() {
        try {
            if (socketRunning)
            {
                if (Time.realtimeSinceStartup > lastPing + pingInt)
                {
                    lastPing = lastTime = Time.realtimeSinceStartup;
                    sendBuf = System.Text.ASCIIEncoding.ASCII.GetBytes("PING");
                    socket.SendTo(sendBuf, endPoint);
                    // Send update of User.
                    SendDatagram();
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("SOCKETEXEPTION GECTACHT"+ "Exception caught."+ e);
        }

    }

	/// <summary>
	/// Ends the communication of the socket and calls PauseMenu to end the session.
	/// </summary>
    public void KillSocket(){
        socketRunning = false;
        pauseMenu.End();
    }
}
