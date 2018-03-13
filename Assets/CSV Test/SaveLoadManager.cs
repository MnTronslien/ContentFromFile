using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour {

    [System.Serializable]
    public class SaveFile
    {
        public string username;
        public int score;
        //whatever we feel like
    }

    public static SaveLoadManager instance;
    public string _username;
    public SaveFile _currentSave;

	// Use this for initialization
	void Start () {
        instance = this;
        Load();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            Save();
        }
	}
    public void Save()
    {
        PlayerPrefs.SetString("UserName", _username);
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(_currentSave));
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("UserName"))
        {
            _username = PlayerPrefs.GetString("UserName");
            _currentSave = JsonUtility.FromJson<SaveFile>(PlayerPrefs.GetString("SaveData"));
        }
        else
        {
            //New User
        }
    }


}
