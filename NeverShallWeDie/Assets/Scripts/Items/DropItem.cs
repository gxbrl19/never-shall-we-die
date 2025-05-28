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
        Instantiate(_item, transform.position, Quaternion.identity);
    }
}
