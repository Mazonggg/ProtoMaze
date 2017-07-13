using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System.Net;
using System;

/// Contains function Md5Sum() from 'http://wiki.unity3d.com/index.php?title=MD5 opened on: 2017_04_26'.
/// Convention for responses:
/// response: "type=HINT&userId=5&userName=Protofox";
public class NetworkRoutines : MonoBehaviour {

	private static string serverError = "Error:";
	private static string serverResponse = "Response:";
	private static string serverHint = "Hint:";
	private static string serverRequest = "http://h2678361.stratoserver.net/scripts/";
	private static string connScript = "connection.php";
	private static string upstreamSocket = "upstream.php";

	private UnityWebRequest connection;


	/// <summary>
	/// Searches for own IP-Addres in DNS host entries.
	/// Picks last entry found, while in development.
	/// </summary>
	/// <returns>The local I pv4.</returns>
	internal static string GetLocalIPv4() {

		IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
		IPAddress[] ipAddr = ipHost.AddressList;

		string ip = "";
		for(int i = 0; i < ipAddr.Length; i++) {
			ip = ipAddr[i].ToString();
		}

		return ip;
	}

	public static void EmptyCallback(string[][] response){}

	public void UDPRequest(Action<string[][]> callback, string[] keys, string[] values) {

		string request = SerializeRequest (serverRequest + upstreamSocket, GenerateParams(keys, values));
		StartCoroutine (MakeRequest(callback, request));
	}

	public void TCPRequest(Action<string[][]> callback, string[] keys, string[] values) {
        
		string request = SerializeRequest (serverRequest + connScript, GenerateParams(keys, values));
		StartCoroutine (MakeRequest(callback, request));
	}
	/// <summary>
	/// Processes and returns all tcp-requests to server.
	/// </summary>
	/// <returns>The request.</returns>
	/// <param name="param">Parameter.</param>
	private IEnumerator MakeRequest(Action<string[][]> callback, string request) {
		
		// Debug.Log ("MakeRequest: " + request);
		using (connection = UnityWebRequest.Get (request)) {
			
			yield return connection.Send ();

				if (connection.isError) {
					Debug.Log (serverError + connection.error);
				} else {
					string response = connection.downloadHandler.text;
					// Checks if the request responses with an error
					if (response.StartsWith (serverError)) {
						// Debug.Log (serverError + response);
					} else {
						Debug.Log (serverResponse + response);
						callback (CompileResponse (response));
					}
				}
		}
	}

    /// <summary>
    /// Gets the type of the response, by extracting the respective part of it.
    /// </summary>
    /// <returns>The response type. eg. ERROR, HINT, SUCCESS</returns>
    /// <param name="response">Response.</param>
    private string GetResponseType (string response) {

        string step1 = response.Split('=')[1];
        return step1.Split('&')[0];
    }

	private string SerializeRequest(params string[] parts) {
		string serial = "";

		foreach (string part in parts) {
            
			serial += part;
		}
		return serial;
	}

    private string[][] CompileResponse(string response) {

        string[] pairs = response.Split('&');
        string[][] comp = new string[pairs.Length][];

        for (int i = 0; i < pairs.Length; i++) {
            comp[i] = pairs[i].Split('=');
        }

        return comp; 
    }

	/// <summary>
	/// Generates the parameters for a php request.
	/// </summary>
	/// <returns>The parameters.</returns>
	/// <param name="pars">Key-Value pairs for request.</param>
	private string GenerateParams(string[] keys, string[] values) {

		string gen = "?";

		for (int i = 0; i < keys.Length; i++) {
			gen += keys[i] + "=" + values[i] + "&";
		}

        gen = gen.Substring(0, gen.Length - 1);
		return gen;
	}



	/// <summary>
	/// Applies Md5s encryption to the string.
	/// </summary>
	/// <returns>The sum.</returns>
	/// <param name="strToEncrypt">String to encrypt.</param>
	public string Md5Sum(string strToEncrypt) {
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++) {
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}
}
