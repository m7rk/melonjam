using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public MovePattern EnemyData;
    private int MoveCounter = 0;
    [SerializeField] private Sprite[] DirectionalSprites;
    private List<Transform> ArrowsList = new List<Transform>();
    void Start()
    {
        ChangeEnemy(EnemyData);
    }

    void Update()
    {
        if (EnemyData.Actions.Count() <= MoveCounter) Debug.Log("Enemy Defeated");
        else
        {
            ActionDirections currentAction = EnemyData.Actions[MoveCounter];
            switch (currentAction)
            {
                case ActionDirections.LEFT:
                    if (Input.GetKeyDown(KeyCode.LeftArrow)) CountScore();
                    
                    break;
                case ActionDirections.UP:
                    if (Input.GetKeyDown(KeyCode.UpArrow)) CountScore();
                

                    break;
                case ActionDirections.RIGHT:
                    if (Input.GetKeyDown(KeyCode.RightArrow)) CountScore();

                    break;
                case ActionDirections.DOWN:
                    if (Input.GetKeyDown(KeyCode.DownArrow)) CountScore();

                    break;
                case ActionDirections.SKIP:
                    if (Input.GetKeyDown(KeyCode.Space)) CountScore();

                    break;
            }
        }
    }

    public void ChangeEnemy(MovePattern newPattern)
    {
        EnemyData = newPattern;
        MoveCounter = 0;
        for (int i = 0; i < EnemyData.Actions.Length; i++)
        {
            GameObject ArrowObject = new GameObject();
            ArrowObject.transform.position = new Vector3(1f * (i % 4) - 1.25f, 4 - (Mathf.FloorToInt(i / 4) * 1f), 0);
            ArrowObject.AddComponent<SpriteRenderer>().sprite = DirectionalSprites[(int)EnemyData.Actions[i]];
            ArrowsList.Add(ArrowObject.transform);

        }
    }

    private float CountScore()
    {
        float timeOffNote = 0f;

        MoveCounter++;
        Destroy(ArrowsList[0].gameObject);
        ArrowsList.RemoveAt(0);

        timeOffNote = Mathf.Min(Time.time - BeatManager.LastBeatTime, Mathf.Abs(Time.time - (BeatManager.LastBeatTime + BeatManager.BPMmeter)));
        Debug.Log(timeOffNote);
        return timeOffNote;
    }
}
