using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInMap : MonoBehaviour
{
    [SerializeField] int _idChest;

    void Update()
    {
        // verifica se ainda não foi aberto
        bool enabled = GameManager.instance._chests[_idChest] == 0;
        gameObject.SetActive(enabled);
    }
}
