using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class SliderController : MonoBehaviour
{
    [SerializeField] string busPath = "";
    private FMOD.Studio.Bus bus;

    [SerializeField] private Slider slider = null;

    private void Start()
    {
        if (busPath != "")
        {
            bus = RuntimeManager.GetBus(busPath);
        }

        float volume;
        bus.getVolume(out volume);
        slider.value = volume * slider.maxValue;

        UpddateSliderOutput();
    }

    public void UpddateSliderOutput()
    {
        bus.setVolume(slider.value / slider.maxValue);
    }
}
