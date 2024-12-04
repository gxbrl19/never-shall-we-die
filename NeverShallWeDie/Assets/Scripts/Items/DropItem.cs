using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private GameObject _item;
    [SerializeField] private Transform _dropPoint;
    [HideInInspector] public int _dropRate; //para os inimigos essa informação vem do scriptable object

    public void DropGold()
    {
        if (_item == null) { return; } //retorna se não tiver nenhum item

        for (int i = 0; i < _dropRate; i++)
        {
            Instantiate(_item, _dropPoint.position, Quaternion.identity);
        }
    }

    public void DropChest(int chestID)
    {
        if (_item == null) { return; } //retorna se não tiver nenhum item
        Key _key = _item.GetComponent<Key>();

        if (_key != null)
        {
            switch (chestID)
            {
                case 6: //TODO: pegar os IDs certos depois de definidos
                    _key._keyID = 1;
                    break;
                case 7:
                    _key._keyID = 2;
                    break;
                case 8:
                    _key._keyID = 3;
                    break;
                case 9:
                    _key._keyID = 4;
                    break;
                case 10:
                    _key._keyID = 5;
                    break;
                case 11:
                    _key._keyID = 6;
                    break;
            }
        }

        Instantiate(_item, transform.position, Quaternion.identity);
    }
}
