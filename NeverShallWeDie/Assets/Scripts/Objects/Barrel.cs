using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Barrel : MonoBehaviour
{
	public int _idBarrel;

	int _lifeBarrel = 4;
	Animator _animation;
	DropItem _dropItem;

	[Header("FMOD Events")]
	[SerializeField] EventReference barrelbreak;

	void Start()
	{
		if (GameManager.instance._barrels[_idBarrel] == 1) { gameObject.SetActive(false); }

		_animation = GetComponent<Animator>();
		_dropItem = GetComponent<DropItem>();
	}

	void Life(int _value)
	{
		_lifeBarrel -= _value;

		if (_lifeBarrel > 0)
		{
			RuntimeManager.PlayOneShot(barrelbreak);
			_animation.SetTrigger("Hit");
		}
		else
		{
			RuntimeManager.PlayOneShot(barrelbreak);
			_animation.SetBool("Break", true);
			_dropItem._dropRate = 15;
			_dropItem.DropGold();
		}
	}

	public void FinishBreak()
	{
		GameManager.instance._barrels[_idBarrel] = 1;
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		if (_other.gameObject.tag == "SwordAtk")
		{
			Life(1);
		}
	}
}
