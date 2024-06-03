using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerPosition : ScriptableObject
{
    bool _sceneTransition = false;
    int _direction;
    int _indexStartPosition;

    public void SetAttributes(bool sceneTransition, int direction, int index)
    {
        _sceneTransition = sceneTransition;
        _direction = direction;
        _indexStartPosition = index;
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
}
