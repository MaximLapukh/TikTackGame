using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHideHumanGetFirst : MonoBehaviour
{
    [SerializeField] Toggle _PvEButton;
    [SerializeField] GameObject _humanGetFirst;
    private void FixedUpdate()
    {
        _humanGetFirst.SetActive(_PvEButton.isOn);
    }
}
