using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Secret : MonoBehaviour
{
    void Start()
    {
        if (GameManager.instance._secret == null || GameManager.instance._secret.Length == 0)
        {
            GenerateSecret();
        }
    }

    void GenerateSecret()
    {
        List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5 };

        numbers = numbers.OrderBy(x => Random.value).ToList();

        GameManager.instance._secret = numbers.ToArray();
    }
}
