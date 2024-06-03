using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	public int _idBarrel;
	public AudioClip _barrelBreak;

	int _lifeBarrel = 4;
	Animator _animation;
	AudioSource _audio;
	DropItem _dropItem;

	void Start()
	{
		if (GameManager.instance._barrels[_idBarrel] == 1) { gameObject.SetActive(false); }

		_audio = GetComponent<AudioSource>();
		_animation = GetComponent<Animator>();
		_dropItem = GetComponent<DropItem>();
	}

	void Life(int _value)
	{
		_lifeBarrel -= _value;

		if (_lifeBarrel > 0)
		{
			_audio.PlayOneShot(_barrelBreak);
			_animation.SetTrigger("Hit");
		}
		else
		{
			_audio.PlayOneShot(_barrelBreak);
			_animation.SetBool("Break", true);
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
