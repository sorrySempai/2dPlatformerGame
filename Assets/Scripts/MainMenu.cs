using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    int levelComplete;

    public Button lvl2;
    public Button lvl3;
    public Button lvl4;
    public Button lvl5;


    public void Start()
    {
        levelComplete = PlayerPrefs.GetInt("LevelComplete");
        lvl2.interactable = false;
        lvl3.interactable = false;
        lvl4.interactable = false;
        lvl5.interactable = false;

        switch (levelComplete)
        {
            case 1:
                lvl2.interactable = true;
                break;
            case 2:
                lvl2.interactable = true;
                lvl3.interactable = true;
                break;
            case 3:
                lvl2.interactable = true;
                lvl3.interactable = true;
                lvl4.interactable = true;
                break;
            case 4:
                lvl2.interactable = true;
                lvl3.interactable = true;
                lvl4.interactable = true;
                lvl5.interactable = true;
                break;
        }
    }

    public void LoadTo(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }
}