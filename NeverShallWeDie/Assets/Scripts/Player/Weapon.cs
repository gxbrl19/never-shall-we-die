using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public SkillObject _skillObject;
    private Damager _damager;

    private void Awake()
    {
        _damager = GetComponent<Damager>();
    }

    private void Update()
    {
        //aqui será definido o dano de acordo com o level da arma
        _damager._power = _skillObject.level1;
    }
}
