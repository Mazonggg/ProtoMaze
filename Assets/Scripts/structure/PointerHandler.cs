using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	public GameObject text, text2, border, placeholder;

	private static float highlight = 1.0f;
	private bool entered = false;
	private bool downed = false;

	public void OnPointerEnter(PointerEventData eventData) {

		Enter ();
	}	

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

	public void OnPointerExit(PointerEventData eventData) {

		Exit ();
	}

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

	public void OnPointerDown(PointerEventData eventData) {

		Down ();
	}

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

	public void OnPointerUp(PointerEventData eventData) {

		Up ();
	}

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
