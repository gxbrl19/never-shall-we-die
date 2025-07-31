using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void TakeHit(int power, Vector2 hitDirection, float knockbackForce = 7f);
    void TakeDamage(int amount);
}

