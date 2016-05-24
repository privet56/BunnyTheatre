using UnityEngine;
using System.Collections;


//[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]
public class touchHandler : MonoBehaviour
{
	public GameObject lea;
	public GameObject leo;
	public GameObject floor;
	public GameObject roof;
	public GameObject death;
	public GameObject leaText;
	public GameObject leoText;
	public ParticleSystem hearts;
	public Texture2D cursorTexture;

	public Camera _camera;
	public Rigidbody _rigidbody = null;

	private bool grounded = false;
	private bool bOnRoof = false;
	//private Vector3 initialPosition;

	void Start()
	{
		hearts.Stop();
		death.SetActive(false);

		if(leaText)leaText.SetActive(false);	if(!leaText)print ("ERROR:!leaText");
		if(leoText)leoText.SetActive(false);	if(!leoText)print ("ERROR:!leoText");
#if !MOBILE_INPUT
		Cursor.visible = false;
#endif

		_rigidbody = GetComponent<Rigidbody>();
		StartCoroutine(onFallFromRoof(true));
		//initialPosition = transform.position;
	}

	void OnGUI()
	{
#if !MOBILE_INPUT
		int cursorWidth = 32;
		int cursorHeight = 32;
		GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, cursorWidth, cursorHeight), cursorTexture);
#endif
	}

	void Update()
	{

	}

	void Awake ()
	{

	}

	void FixedUpdate ()
	{
		if (transform.position.y < -99.9f)
		{
			//transform.position = initialPosition;				//TODO: make it work!
			Application.LoadLevel(0);							//for now, just restart
		}
		/*try
		{
			Ray ray = _camera.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.Raycast (ray.origin, ray.direction);
			Debug.DrawLine(new Vector3(-2,0,0), Vector3.zero, Color.green, Time.deltaTime, true);
			if(hitInfo)Debug.DrawLine (new Vector3(-2,0,0), hitInfo.point, Color.white, Time.deltaTime, true);	//does not work
		}
		catch(UnityException e){print ("exception:"+e+"\n"+e.Message);}*/

		if (grounded)
		{

		}
		
		grounded = false;
	}

	void OnCollisionStay ()
	{
		grounded = true;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject == floor)
		{
			if(bOnRoof)
			{
				bOnRoof = false;
				StartCoroutine(onFallFromRoof(false));
			}
			bOnRoof = false;
			return;
		}
		if (col.gameObject == roof)
		{
			bOnRoof = true;
			return;
		}

		if (col.gameObject && col.gameObject.name.ToLower().StartsWith("mmgroup0"))
		{
			leaText.SetActive(true);
			leoText.SetActive(true);
			hearts.Play();
			StartCoroutine(onHitStop());
		}
	}
	IEnumerator onHitStop ()
	{
		yield return new WaitForSeconds(6);
		leaText.SetActive(false);
		leoText.SetActive(false);
		hearts.Stop();
	}
	IEnumerator onFallFromRoof(bool yieldOnStart)
	{
		if(yieldOnStart) {yield return new WaitForSeconds(1); }
		death.SetActive(true);
		for(int i=0;i<3;i++)
		{
			float takes = 0.5f;
			StartCoroutine(Fade(0.0f, 0.8f, takes, death));
			yield return new WaitForSeconds(takes);
			StartCoroutine(Fade(0.8f, 0.0f, 0.9f, death));
			yield return new WaitForSeconds(takes);
		}
		death.SetActive(false);
	}
	IEnumerator Fade(float start, float end, float length, GameObject o)
	{
		CanvasRenderer r = o.GetComponent<CanvasRenderer>();
		r.SetAlpha(start);
		yield return new WaitForFixedUpdate();		//yield;
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/length))
		{
			float a = r.GetAlpha();
			a = Mathf.Lerp(start, end, i);
			r.SetAlpha(a);
			yield return new WaitForFixedUpdate();		//yield;
		}
		r.SetAlpha(end);
	}
}
