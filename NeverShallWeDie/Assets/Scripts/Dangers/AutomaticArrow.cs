using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticArrow : MonoBehaviour
{
    public Arrow _arrow;
    public Transform _spawnPoint;
    private float _interval = 2.2f;
    

    public void Awake()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            _arrow._direction = transform.localScale.x;

            Vector3 _scale = _arrow.transform.localScale;
            _scale.x = transform.localScale.x;
            _arrow.transform.localScale = _scale;

            GameObject _newArrow = Instantiate(_arrow.gameObject, _spawnPoint.position, _spawnPoint.rotation);
            yield return new WaitForSeconds(_interval);
        }
    }
}
