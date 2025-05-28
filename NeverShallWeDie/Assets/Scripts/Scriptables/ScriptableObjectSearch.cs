using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectSearch : MonoBehaviour
{
    public static T FindScriptableObjectByAttribute<T>(string attributeName, string attributeValue) where T : ScriptableObject
    {
        List<T> results = new List<T>();

        T[] allObjects = Resources.FindObjectsOfTypeAll<T>();
        foreach (T obj in allObjects) {
            if (obj.GetType().GetField(attributeName).GetValue(obj).ToString() == attributeValue) {
                results.Add(obj);
            }
        }

        return results.Count > 0 ? results[0] : null; // Retorna o primeiro encontrado ou null se não encontrar
    }
}
