using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NewItem : MonoBehaviour
{
    public ItemObject _itemObject;
    private Items _item;

    Rigidbody2D _body;
    SpriteRenderer _spriteRenderer;
    Collider2D _collider;

    [Header("FMOD Events")]
    [SerializeField] EventReference collected;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();

        //_item = _itemObject.item;
    }

    private void Start()
    {
        _collider.enabled = false;
        _body.AddForce(new Vector2(0f, 20f), ForceMode2D.Impulse);
        Invoke("EnabledCollider", 0.5f);
    }

    void EnabledCollider() //Invoke
    {
        _collider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            RuntimeManager.PlayOneShot(collected);
            UIManager.instance.FeedbackItem(_spriteRenderer.sprite);
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            GameManager.instance._inventory.Add(_item);
            InventorySystem.instance.items.Add(_item);
        }
    }
}
