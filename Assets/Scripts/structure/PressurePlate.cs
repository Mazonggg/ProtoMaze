using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class that describes 
/// </summary>
public abstract class PressurePlate : MonoBehaviour {

	/// <summary>
	/// Determines whether this PressurePlate is permanently activated.
	/// </summary>
	/// <returns><c>true</c> if this instance is permanent; otherwise, <c>false</c>.</returns>
	protected abstract bool IsPermanent ();
	/// <summary>
	/// Sets the time elapsed, before plate is deactivated.
	/// </summary>
	/// <returns>The out.</returns>
	protected abstract int GetTimeOut();
	/// <summary>
	/// Determines, if this PressurePlate is active, 
	/// so the game can react accordingly.
	/// </summary>
	private bool isActive = false;
	public bool IsActive {
		get { return isActive; }
	}
	/// <summary>
	/// Activate this PressurePlate.
	/// </summary>
	public void Activate() {
		isActive = true;
	}
}
