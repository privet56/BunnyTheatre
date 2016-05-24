using UnityEngine;
using System.Collections;

public class pressChecker : MonoBehaviour
{
	public bool isPressed = false;
	public bool isMouseOver = false;

	void Start ()
	{
	
	}
	
	void FixedUpdate ()
	{
	}

	public bool isButtonPressed(pressChecker pC)
	{
		pC.isPressed = false;
		if(!this.isMouseOver)return false;
		pC.isPressed = Input.GetMouseButtonDown (0);
		if(!pC.isPressed)
			pC.isPressed = (Input.touchCount > 0);

		return pC.isPressed;
	}

	public void onMouseEnter()
	{
		this.isMouseOver = true;
	}
	public void onMouseExit()
	{
		this.isMouseOver = false;
	}
}
