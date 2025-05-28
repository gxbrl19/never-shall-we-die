using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DrawbridgeMechanism : MonoBehaviour
{
    public static DrawbridgeMechanism instance;
    [SerializeField] int[] _slots;
    [SerializeField] Image[] _keyImages;
    bool _triggered;
    PlayerInputs _input;
    [SerializeField] Drawbridge _drawbridge;

    void Awake()
    {
        instance = this;

        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
        _slots = new int[6];
        CancelSelect();
    }

    void Start()
    {
        if (GameManager.instance._drawbridge == 1) { gameObject.GetComponent<Collider2D>().enabled = false; }
    }

    private void Update()
    {
        if (_triggered && _input.interact)
        {
            UIManager.instance.ActivePanelDrawbridge();
            _triggered = false;
            _input.interact = false;
        }
    }

    public void AddSlot(ItemObject key, int keyID)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == 6)
            {
                _keyImages[i].color = Color.white;
                _keyImages[i].sprite = key.sprite;
                _slots[i] = keyID;
                break;
            }
        }

        VerifySlots();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = true;
        }
    }

    public void CancelSelect()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = 6;
            _keyImages[i].color = Color.black;
        }
    }

    void VerifySlots() //chamado ao selecionar uma Key (configurar)
    {
        // enquanto não estiver todo preenchido, ele não compara com o secret
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] == 6) { return; }
        }

        //usando Equals para verificar se é igual ao secret
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!Equals(_slots[i], GameManager.instance._secret[i]))
            {
                CancelSelect();
                return;
            }
        }

        _drawbridge.EnabledBridge();
        UIManager.instance.UpDrawbridge();
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
