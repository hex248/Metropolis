using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NPC", menuName = "Scriptables/NPC", order = 1)]
public class NPC : ScriptableObject
{
    public string npcName;

    public Sprite npcIdleSprite;
    public Sprite npcHappySprite;
}
