using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerPosition : ScriptableObject {
    public bool _sceneTransition = false;
    public int _direction;
    public int _indexStartPosition;
    public Vector3 _initialValue;
}
