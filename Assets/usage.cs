using UnityEngine;
using System.Collections;

public class usage : MonoBehaviour
{

	void Start ()
	{
		StartCoroutine(removeUsage());
	}
	
	void Update ()
	{
	
	}
	IEnumerator removeUsage ()
	{
		yield return new WaitForSeconds(5);
		DestroyObject (this.gameObject);
	}
}
