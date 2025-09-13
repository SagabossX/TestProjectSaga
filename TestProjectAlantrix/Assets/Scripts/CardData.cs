using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Game/Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private Sprite frontSprite;

    public string Id => id;
    public Sprite FrontSprite => frontSprite;
}