using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    [SerializeField] GameObject mainParent;
    [SerializeField] GameObject loadSavesParent;
    [SerializeField] GameObject newSaveParent;

    public void PlayButtonPressed()
    {
        mainParent.SetActive(false);
        newSaveParent.SetActive(false);
        loadSavesParent.SetActive(true);
    }

    public void BackButtonPressed()
    {
        loadSavesParent.SetActive(false);
        newSaveParent.SetActive(false);
        mainParent.SetActive(true);
    }

    public void NewButtonPressed()
    {
        loadSavesParent.SetActive(false);
        mainParent.SetActive(false);
        newSaveParent.SetActive(true);
    }

    public void SwitchToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
