using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Inscribed")]
    public Text uiTLevel;
    public Text uiTShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }

        ProjectileScript.DestroyProjectiles();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        GoalScript.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
        CameraFollowScript.SWITCH_VIEW(CameraFollowScript.eView.both);
    }

    void UpdateGUI()
    {
        uiTLevel.text = "Level " + (level + 1);
        uiTShots.text = "Shots : " + shotsTaken;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
        if ((mode == GameMode.playing) && GoalScript.goalMet)
        {
            mode = GameMode.levelEnd;
            CameraFollowScript.SWITCH_VIEW(CameraFollowScript.eView.both);
            Invoke("NextLevel", 2f);

        }
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel();
    }
    static public void ShotFired()
    {
        S.shotsTaken++;
    }
    static public GameObject GetCastle()
    {
        return S.castle;
    }
}
