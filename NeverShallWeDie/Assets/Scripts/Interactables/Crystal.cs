using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private GameObject _crystalPiece;
    private int _lifeCrystal = 2;
    DropItem _dropItem;    

    void Awake() {
        _dropItem = GetComponent<DropItem>();
    }

    void Life(int _value) {
		_lifeCrystal -= _value;
        

		switch (_lifeCrystal) {
			case 1:
				//TODO: apenas tremer o cristal
                DropPiece();                
				break;
			case 0:
				_dropItem.Drop("Healing");
                DropPiece();
                Destroy(gameObject);
				break;
		}
	}

    void DropPiece() {
        int _rand = Random.Range(4, 8);
        if (_crystalPiece != null)         
        {
            for (int i = 0; i < _rand; i++)
            {
                GameObject hit = Instantiate(_crystalPiece, transform.position, _crystalPiece.transform.rotation);
                Destroy(hit, 2f);
            }     
        } 
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("SwordAtk")) {
            Life(1);
        }
    }
}
