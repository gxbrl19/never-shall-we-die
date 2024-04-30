using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerPosition : ScriptableObject
{
    bool _sceneTransition = false;
    int _direction;
    int _indexStartPosition;
    float _health;
    float _mana;

    public void SetAttributes(bool sceneTransition, int direction, int index, float health, float mana)
    {
        _sceneTransition = sceneTransition;
        _direction = direction;
        _indexStartPosition = index;
        _health = health;
        _mana = mana;
    }

    public bool SceneTransition
    {
        get { return _sceneTransition; }
        set { _sceneTransition = value; }
    }

    public int Index
    {
        get { return _indexStartPosition; }
        set { _indexStartPosition = value; }
    }

    public int Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public float Mana
    {
        get { return _mana; }
        set { _mana = value; }
    }
}
