using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActionDirections
{
    UP = 0, RIGHT = 1, LEFT = 2, DOWN = 3, SKIP = 4
}

[Serializable, CreateAssetMenu]
public class MovePattern : ScriptableObject
{
   [SerializeField] public string EnemyName;
    [SerializeField] public Sprite enemySprite;
   [SerializeField] public int BeatFrequency = 4;
   [SerializeField] public ActionDirections[] Actions;
}
