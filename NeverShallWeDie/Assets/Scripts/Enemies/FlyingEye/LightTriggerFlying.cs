using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTriggerFlying : MonoBehaviour
{
	[SerializeField] FlyingEye _flyingEye;

	private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "Player")
		{
			_flyingEye._playerTriggered = true;
		}
	}
}
