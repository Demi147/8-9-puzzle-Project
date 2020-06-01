using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{

    #region Varibles
    public TextMeshProUGUI _text;
    #endregion

    public void SetMessage(string _message)
    {
        _text.text = _message;
    }
}
