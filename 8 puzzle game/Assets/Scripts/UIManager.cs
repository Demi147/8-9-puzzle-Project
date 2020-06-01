using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Varibles
    [Header("GUI")]
    public TextMeshProUGUI movesHolder;

    [Header("pop Ups")]
    public GameObject PopUpPrefab;
    public GameObject canvas;
    public float popUpDeleteTime;

    [Header("Load From File Stuff")]
    public GameObject loadFromFileHolder;
    public Text _textHolder;
    #endregion

    #region Events
    public delegate void OnFileLoadedEventHandeler(object source, string path);
    public event OnFileLoadedEventHandeler OnFileLoaded;
    #endregion

    #region Singleton
    public static UIManager instance;
    #endregion

    private void Awake()
    {
        instance = this;
        GameManager.instance.OnGUIReset += Instance_OnGUIReset;
        GameManager.instance.OnMove += Instance_OnMove;
        GameManager.instance.OnPopUpMessageShow += Instance_OnPopUpMessageShow;
    }

    private void Instance_OnPopUpMessageShow(object source, string _message)
    {
        var pop = Instantiate(PopUpPrefab,canvas.transform);
        pop.GetComponent<Popup>().SetMessage(_message);
        Destroy(pop, popUpDeleteTime);
    }

    private void Instance_OnMove(object source, int _moves)
    {
        movesHolder.text = _moves.ToString();
    }

    private void Instance_OnGUIReset(object sender, System.EventArgs e)
    {
        movesHolder.text = "0";
    }

    public void ToggleLoadFilePanel(bool value)
    {
        loadFromFileHolder.SetActive(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        movesHolder.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFileMethod()
    {
        //get the text
        string _path = _textHolder.text;
        if (!(_path != null && _path.Length > 0))
        {
            Instance_OnPopUpMessageShow(this, "You must give a path to load!");
            return;
        }
        
        if (!File.Exists(_path))
            {
                //error message
                Instance_OnPopUpMessageShow(this, "Invalid Path!");
                return;
            }

        OnFileLoaded?.Invoke(this, _path);
        ToggleLoadFilePanel(false);
    }
}
