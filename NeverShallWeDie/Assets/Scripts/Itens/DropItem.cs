using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private GameObject _item;
    [HideInInspector] public int _dropRate; //pega no scriptable object

    public void Drop(string type) 
    {
        if(type == "Gold") 
        {
            if (_item != null)         
            {
                for (int i = 0; i < _dropRate; i++)
                {
                    Instantiate(_item, transform.position, transform.rotation);
                }     
            }  
        }
        else if(type == "Healing")
        {
            int _n = 4;
            if (_item != null)         
            {
                for (int i = 0; i < _n; i++)
                {
                    Instantiate(_item, transform.position, transform.rotation);
                }     
            }  
        }                  
    }
}
