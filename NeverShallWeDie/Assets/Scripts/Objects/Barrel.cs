using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
	[SerializeField] private int _lifeBarrel = 2;

	public SpriteRenderer _barrel;
	public Sprite[] _barrelImages = new Sprite[2];
	public AudioClip _barrelBreak;

	private AudioSource _audio;
	private bool _break;

	void Start()
    {
		_barrel = GetComponent<SpriteRenderer>();
		_audio = GetComponent<AudioSource>();

	}

   
    void Update()
    {

    }

	void Life(int _value)
    {
		_lifeBarrel -= _value;

		switch (_lifeBarrel)
		{
			case 2:
				_barrel.sprite = _barrelImages[1];
				break;
			case 1:
				_barrel.sprite = _barrelImages[0];
				break;
			case 0:
				_break = true;
				_audio.PlayOneShot(_barrelBreak);
				GetComponent<Animator>().enabled = true;
				Destroy(GetComponent<Animator>(), 1);
				Destroy(GetComponent<AudioSource>(), 1);
				Destroy(GetComponent<BoxCollider2D>());
				Destroy(GetComponent<Barrel>(), 1);
				break;
		}
	}

	void BarrelAudio()
    {
        if (_break)
        {
			_audio.PlayOneShot(_barrelBreak);
			_break = false;
		}
	}

	void OnTriggerEnter2D(Collider2D _other)
	{
		if (_other.gameObject.tag == "SwordAtk")
		{
			Life(2);
		}
		if (_other.gameObject.tag == "Pistol")
		{
			Life(1);
		}
	}
}
