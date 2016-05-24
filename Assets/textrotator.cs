using UnityEngine;
using System.Collections;

public class textrotator : MonoBehaviour
{
	void Start ()
	{
	
	}
	void FixedUpdate()
	{
		transform.Rotate(0, Time.deltaTime * 100, 0);

		/*Vector3 rot = transform.rotation.eulerAngles;
		float incr = 6.6f;
		rot.x += incr;
		rot.y += incr;
		rot.z += incr;
		transform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);//turn slowly
		*/
	}
}
