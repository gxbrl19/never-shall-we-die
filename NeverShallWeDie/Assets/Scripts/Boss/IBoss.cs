using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoss
{
    void TakeHit(int power, Vector2 hitDirection, float knockbackForce = 5f);
    void TakeDamage(int amount);
}
