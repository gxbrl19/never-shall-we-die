using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{    
    [SerializeField] private GameObject _item;
    [SerializeField] private bool _interactable; //verifica se é o barril e atribui o droprate
    [HideInInspector] public int _dropRate; //para os inimigos essa informação vem do scriptable object

    public void DropGold()
    {
        if (_item == null) { return; } //retorna se não tiver nenhum item
        if (_interactable) { _dropRate = 10; }

        for (int i = 0; i < _dropRate; i++)
        {
            Instantiate(_item, transform.position, transform.rotation);
        }
    }
}
