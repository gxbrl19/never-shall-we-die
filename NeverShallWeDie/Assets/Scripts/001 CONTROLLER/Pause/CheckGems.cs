using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckGems : MonoBehaviour
{
    [SerializeField] ItemObject _itemObject;

    Items _item;
    Image _imageButton;

    private void Awake()
    {
        _imageButton = transform.Find("image_gems").GetComponent<Image>();
    }

    void Start()
    {
        _item = _itemObject.item;
    }

    private void Update()
    {
        if (_item == Items.WaterGem)
        {
            _imageButton.enabled = InventorySystem.instance.items.Contains(Items.WaterGem);
        }
        else if (_item == Items.FireGem)
        {
            _imageButton.enabled = InventorySystem.instance.items.Contains(Items.FireGem);
        }
        else if (_item == Items.AirGem)
        {
            _imageButton.enabled = InventorySystem.instance.items.Contains(Items.AirGem);
        }
    }
}
