using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [SerializeField] private Light2D _light;
    
    void Update() {
        if (PlayerEquipment.instance.equipments.Contains(Equipments.Lantern)) {
            _light.intensity = 0.7f;
        }
        else {
            _light.intensity = 0f;
        }
    }
}
