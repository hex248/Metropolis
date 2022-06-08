using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSaveSelection : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameSave gameSave;
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject.Find("SaveManager").GetComponent<LoadSaves>().ChooseSave(gameSave);
    }
}
