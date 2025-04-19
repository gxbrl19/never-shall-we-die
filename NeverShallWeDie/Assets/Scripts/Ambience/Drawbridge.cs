using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drawbridge : MonoBehaviour
{
    public static Drawbridge instance;
    [SerializeField] int[] _slots;
    [SerializeField] Image[] _keyImages;
    bool _triggered;
    Player _player;
    PlayerInputs _input;

    void Awake()
    {
        instance = this;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputs>();
        _slots = new int[6];
        CancelSelect();
    }

    private void Update()
    {
        if (_triggered && _input.interact)
        {
            UIManager.instance.ActivePanelDrawbridge();
            UIManager.instance.SaveBackup();
            _triggered = false;
            _input.interact = false;
        }
    }

    public void AddSlot(int slotID, int keyID)
    {
        if (_slots[slotID] < 6)
        {
            int key = _slots[slotID];
            GameManager.instance._keys[key] = 1;
            _slots[slotID] = keyID;
            GameManager.instance._keys[keyID] = 2;
        }
        else
        {
            _slots[slotID] = keyID;
            GameManager.instance._keys[keyID] = 2;
        }

        CheckColor(slotID, keyID);
        VerifySlots();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            _triggered = true;
        }
    }

    void CheckColor(int slotID, int keyID)
    {
        switch (keyID)
        {
            case 0:
                _keyImages[slotID].color = Color.yellow;
                break;
            case 1:
                _keyImages[slotID].color = Color.red;
                break;
            case 2:
                _keyImages[slotID].color = Color.green;
                break;
            case 3:
                _keyImages[slotID].color = Color.cyan;
                break;
            case 4:
                _keyImages[slotID].color = Color.magenta;
                break;
            case 5:
                _keyImages[slotID].color = Color.grey;
                break;
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
        //usando Equals para verificar se é igual ao secret
        for (int i = 0; i < _slots.Length; i++)
        {
            if (!Equals(_slots[i], GameManager.instance._secret[i]))
            {
                return;
            }
        }

        UIManager.instance.UpDrawbridge();
        Debug.Log("Iniciando Animação");
    }
}
