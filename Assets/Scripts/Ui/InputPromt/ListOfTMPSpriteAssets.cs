using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfTMPSpriteAssets", menuName = "Scriptable Objects/ListOfTMPSpriteAssets")]
public class ListOfTMPSpriteAssets : ScriptableObject
{
    public List<TMP_SpriteAsset> SpriteAssets;
}
