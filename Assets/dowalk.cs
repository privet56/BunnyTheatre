using UnityEngine;
using System.Collections;

public class dowalk : MonoBehaviour
{
	public GameObject floor;		//TODO: check for floor-plane end instead of the hard-coded 8
	public GameObject heart;
	public ParticleSystem hearts;
	private bool bDown = true;
	private bool bHit = false;
	private float hitStart;
	private Transform _transform;

	void Start()
	{
		heart.SetActive (false);														//hide heart initially
		_transform = transform;															//do this for better performance
	}
	
	void FixedUpdate()
	{
		var pos = _transform.position;
		if(!bHit)
		{
			pos.z -= bDown ? 0.01f : -0.01f;
			_transform.position = pos;													//do walk
		}
		if(!bHit && Mathf.Abs(pos.z) > 8)
		{
			bDown = !bDown;
			_transform.rotation = Quaternion.Euler(270, bDown ? 90 : 270, 0);			//turn around at the end of the floor-plane
			/*			//never ever do this!
			Quaternion rot = _transform.rotation;
			rot.y = !bDown ? 0.5f : -0.5f;
			rot.z = rot.y;
			_transform.rotation = rot;													
			*/
		}
		if(bHit)
		{
			if((Time.time - hitStart) > 6)
			{
				hearts.Stop();
				bHit = false;
				heart.SetActive (false);
				_transform.rotation = Quaternion.Euler(270, bDown ? 90 : 270, 0);		//finish being hit
			}
			else
			{
				{
					Vector3 eRot = _transform.rotation.eulerAngles;
					eRot.y += 3.3f * Time.deltaTime;
					eRot.z = eRot.y;
					_transform.rotation = Quaternion.Euler(eRot.x, eRot.y, eRot.z);		//hit -> turn around all the time
				}
				{
					Vector3 hRot = heart.transform.rotation.eulerAngles;
					var incr = 9.9f * Time.deltaTime;
					hRot.x -= incr;
					hRot.y -= incr;
					hRot.z -= incr;
					heart.transform.rotation = Quaternion.Euler(hRot.x, hRot.y, hRot.z);//turn heart
				}
			}
		}
	}
	public void OnCollisionEnter(Collision col)
	{
		//print("HIT:>"+col.gameObject.name+"<");
		if (!bHit && col.gameObject && col.gameObject.name == "RigidBodyFPSController")
		{
			onHit();
		}
	}
	public void onHit()
	{
		if(bHit)return;
		{
			var pos = _transform.position;
			pos.z += bDown ? 1.6f : -1.6f;
			_transform.position = pos;													//go back
		}
		bHit = true;
		hitStart = Time.time;
		heart.SetActive (true);
		hearts.Play();
	}
}
