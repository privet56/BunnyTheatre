using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof (Rigidbody))]
public class ctrl : MonoBehaviour
{
	public GameObject fpsController;
	public GameObject fpsCamera;
	public GameObject ctrlArea;
	public GameObject usageWASDText;
	public GameObject bLeft;
	public GameObject bRight;
	public GameObject bForward;


	public GameObject leaText;
	public GameObject leoText;

	public Rigidbody _rigidbody = null;
	public Camera _camera = null;

	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_camera = fpsCamera.GetComponent<Camera>();
		ctrlArea.SetActive (false);
		DestroyObject(ctrlArea);ctrlArea=null;
#if MOBILE_INPUT
		DestroyObject(usageWASDText);usageWASDText=null;
#endif
	}
	
	void Update ()
	{
	
	}

	void FixedUpdate()
	{
		bool leftPressed	= false;
		bool rightPressed	= false;
		bool forwardPressed = false;
		bool rabbitHit 		= false;

		if (ctrlArea != null)
		{
			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					RaycastHit hit = new RaycastHit();
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					bool b = Physics.Raycast(ray, out hit);
					if (b)
						rabbitHit = rabbitClicked(hit);
					if (b && !rabbitHit)
					{
						if (hit.collider.gameObject == bLeft)
							leftPressed = true;
						else if (hit.collider.gameObject == bRight)
							rightPressed = true;
						else if (hit.collider.gameObject == bForward)
							forwardPressed = true;
					}
				}
			}
			if (Input.GetMouseButtonDown (0))
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				bool b = Physics.Raycast(ray, out hit);
				if (b)
					rabbitHit = rabbitClicked(hit);
				if (b && !rabbitHit)
				{
					if (hit.collider.gameObject == bLeft)
						leftPressed = true;
					else if (hit.collider.gameObject == bRight)
						rightPressed = true;
					else if (hit.collider.gameObject == bForward)
						forwardPressed = true;
				}
			}
		}
		else
		{
			int iScreenPart1 = Screen.width / 3;
			int iScreenPart2 = iScreenPart1 * 2;

			if (Input.touchCount > 0)
			{
				foreach (Touch touch in Input.touches)
				{
					RaycastHit hit = new RaycastHit();
					Ray ray = Camera.main.ScreenPointToRay(touch.position);
					bool b = Physics.Raycast(ray, out hit);
					if (b)
						rabbitHit = rabbitClicked(hit);
					if(!rabbitHit)
					{
						if(touch.position.x > iScreenPart2)
							rightPressed = true;
						else if(touch.position.x > iScreenPart1)
							forwardPressed = true;
						else
							leftPressed = true;
					}
				}
			}
			if (Input.GetMouseButtonDown (0))
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				bool b = Physics.Raycast(ray, out hit);
				if (b)
					rabbitHit = rabbitClicked(hit);

				if(!rabbitHit)
				{
					if(Input.mousePosition.x > iScreenPart2)
						rightPressed = true;
					else if(Input.mousePosition.x > iScreenPart1)
						forwardPressed = true;
					else
						leftPressed = true;
				}
			}
		}

		if (forwardPressed)
		{
#if MOBILE_INPUT
			moveForward();
#endif
		}
		if(rabbitHit)
		{
			if(!leaText.activeSelf)
			{
				leaText.SetActive(true);
				leoText.SetActive(true);
				StartCoroutine(removeLeTexts());
			}
		}
		{	//handle rotation
#if MOBILE_INPUT
			if (leftPressed && rightPressed)
			{
				fpsController.SendMessage ("setButtonRotation", 2);
				return;
			}
			if (!leftPressed && !rightPressed)
			{
				fpsController.SendMessage ("setButtonRotation", 2);
				return;
			}
			fpsController.SendMessage ("setButtonRotation", leftPressed ? -1 : 1);
#endif
		}
	}

	public bool isPressed(GameObject o)
	{
		pressChecker pC = new pressChecker();
		o.SendMessage ("isButtonPressed", pC);
		return pC.isPressed;
	}

	private void moveForward()
	{
		transform.Translate(_camera.transform.forward * 0.02f);
	}

	private bool rabbitClicked(RaycastHit hitInfo)
	{
		//if(hitInfo == null)return false;
		if(!hitInfo.collider.gameObject)return false;
		if(hitInfo.collider.gameObject.name == null)return false;
		if((hitInfo.collider.gameObject.name.ToLower() == "walkingbunny") || (hitInfo.collider.gameObject.name.ToLower() == "rabbit"))
	   	{
			StartCoroutine(onWalkingBunnyClicked(hitInfo.collider.gameObject, hitInfo.point));
			return false;
		}
		if((hitInfo.collider.gameObject.name.ToLower() == "leo") || (hitInfo.collider.gameObject.name.ToLower() == "lea") || (hitInfo.collider.gameObject.name.ToLower().StartsWith("mmgroup0")))
		{
			StartCoroutine(faceHit(hitInfo.collider.gameObject, hitInfo.point));
			return true;
		}
		return false;
	}
	private IEnumerator removeLeTexts()
	{
		yield return new WaitForSeconds(5);
		leaText.SetActive(false);
		leoText.SetActive(false);
	}
	IEnumerator onWalkingBunnyClicked(GameObject o, Vector3 point)
	{
		yield return null;
		while((o.transform.parent != null) && (o.transform.parent.gameObject != null))
		{
			o = o.transform.parent.gameObject;
		}
		o.SendMessage("onHit", SendMessageOptions.DontRequireReceiver);
	}
	IEnumerator faceHit(GameObject o, Vector3 point)
	{
		yield return null;
		fpsController.SendMessage ("updateLook", o.transform);
		yield return new WaitForSeconds(2);
		fpsController.SendMessage ("resetLook");
	}
}
