using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the behaviour of GUI components in menu according to mouse actions.
/// </summary>
public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
	// GameObject parameters:
	public GameObject text, text2, border, placeholder;

	private static float highlight = 1.0f;
	private bool entered = false;
	private bool downed = false;
	/// <summary>
	/// Called by unity engine, when mouse enters the object.
	/// 
	/// Delegates to internal function.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter(PointerEventData eventData) {

		Enter ();
	}	
	/// <summary>
	/// Processes the change of GUI elements color, according to mouse entering the object.
	/// </summary>
	private void Enter() {

		entered = true;

		text.GetComponent<Text> ().color = Constants.textColorHover;
		if (text2 != null) {
			text2.GetComponent<Text> ().color = Constants.textColorHover;
		}	
		if (border != null) {
			border.GetComponent<Image> ().color = Constants.textColorHover;
		}
		if (placeholder != null) {
			placeholder.GetComponent<Text> ().color = Constants.textColorHover;
		}
	}
	/// <summary>
	/// Called by unity engine, when mouse exits the object.
	/// 
	/// Delegates to internal function.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerExit(PointerEventData eventData) {

		Exit ();
	}
	/// <summary>
	/// Processes the change of GUI elements color, according to mouse exiting the object.
	/// </summary>
	private void Exit() {

		entered = false;

		text.GetComponent<Text> ().color = Constants.textColor;
		if (text2 != null) {
			text2.GetComponent<Text> ().color = Constants.textColor;
		}
		if (border != null) {
			border.GetComponent<Image> ().color = Constants.textColor;
		}
		if (placeholder != null) {
			placeholder.GetComponent<Text> ().color = Constants.textColor;
		}
	}
	/// <summary>
	/// Called by unity engine, when mouse click was started on object.
	/// 
	/// Delegates to internal function.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerDown(PointerEventData eventData) {

		Down ();
	}
	/// <summary>
	/// Processes the change of GUI elements color, according to mouse starting to click on the object.
	/// </summary>
	private void Down() {

		downed = true;
		if (gameObject.GetComponent<Image> () != null) {
			Color imageColor = gameObject.GetComponent<Image> ().color;
			imageColor.r += highlight;
			imageColor.g += highlight;
			imageColor.b += highlight;
			gameObject.GetComponent<Image> ().color = imageColor;
		}
	}
	/// <summary>
	/// Called by unity engine, when mouse click ended on object.
	/// 
	/// Delegates to internal function.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerUp(PointerEventData eventData) {

		Up ();
	}
	/// <summary>
	/// Processes the change of GUI elements color, according to mouse click stopping on the object.
	/// </summary>
	private void Up() {

		downed = false;
		if (gameObject.GetComponent<Image> () != null) {
			Color imageColor = gameObject.GetComponent<Image> ().color;
			imageColor.r -= highlight;
			imageColor.g -= highlight;
			imageColor.b -= highlight;
			gameObject.GetComponent<Image> ().color = imageColor;
		}
	}
	/// <summary>
	/// Resets the fields, if prematurely disabled. E.g. when clicked and disabled in one event.
	/// </summary>
	void OnDisable () {

		if (entered) {
			Exit ();
		}
		if (downed) {
			Up ();
		}
	}
}
