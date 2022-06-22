using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "STAGE",menuName = "UI/Stage")]
public class StageUIData : ScriptableObject
{
    public Sprite CurrentImage;
    public int DiamondsRequired;
    public List<LevelUIData> Levels;
}
