using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Interact : MonoBehaviour
{
    [SerializeField] string _ptInteract;
    [SerializeField] string _engInteract;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            var currentLocale = LocalizationSettings.SelectedLocale;

            if (currentLocale.Identifier.Code == "pt-BR")
            {
                UIManager.instance.InteractPanel(true, _ptInteract);
            }
            else if (currentLocale.Identifier.Code == "en")
            {
                UIManager.instance.InteractPanel(true, _engInteract);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Invencible"))
        {
            UIManager.instance.InteractPanel(false, "");
        }
    }
}
