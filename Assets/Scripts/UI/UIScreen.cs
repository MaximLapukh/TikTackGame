using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField] GameObject _screen;
    public void View()
    {
        _screen.SetActive(true);
    }
}
