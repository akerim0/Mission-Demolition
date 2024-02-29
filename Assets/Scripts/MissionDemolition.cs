using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd,
    gameOver
}

public enum ColorsList
{
    red,green,purple,blue
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    static public Dictionary<int, int> PROJ_LEVEL_DICT;
    public Dictionary<int, Color> COLORS_DICT = new Dictionary<int, Color>{ 
        { 1, Color.green }, { 2, Color.blue }, { 3, Color.red }, { 4, Color.black } };

    [Header("Inscribed")]
    public Text uiTLevel;
    public Text uiTShots;
    public Vector3 castlePos;
    public GameObject[] castles;
    public GameObject projectilePrefab;
    public GameObject GameOverText;
    public int startNumOfBalls = 3;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public static GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";  
    public Transform projectilesParent;
    static public List<GameObject> Projectiles;
    private void Awake()
    {
        GameOverText.SetActive(false);
        projectilesParent = GameObject.Find("ProjectilesParent").transform;
        PROJ_LEVEL_DICT = new Dictionary<int, int>();
        for(int i = 0; i<castles.Length; i++)
        {
            PROJ_LEVEL_DICT.Add(i,startNumOfBalls);
            startNumOfBalls += 2;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel(int level = 0)
    {
        if (mode == GameMode.gameOver) return;
        if (castle != null)
        {
            Destroy(castle);
        }

        ProjectileScript.DestroyProjectiles();
        GameOverText.SetActive(false);
        shotsTaken = 0;

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        GoalScript.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
        CameraFollowScript.SWITCH_VIEW(CameraFollowScript.eView.both);

        Projectiles = new List<GameObject>();
        Vector3 projPos = projectilesParent.position;

        for(int i = 0; i < PROJ_LEVEL_DICT[level]; i++)
        {
            GameObject go = Instantiate<GameObject>(projectilePrefab, projectilesParent);
            go.GetComponent<Renderer>().material.color = COLORS_DICT[Random.Range(1, COLORS_DICT.Count+1)];
            go.transform.position = projPos;
            go.GetComponent<Rigidbody>().isKinematic = true; 
            projPos.x -= go.GetComponent<SphereCollider>().radius*2.5f;
            Projectiles.Add(go);
        }
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
        
        if (mode == GameMode.gameOver && shotsTaken == PROJ_LEVEL_DICT[level])
        {
            GameOverText.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Escape)) QuitGame();
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
            shotsTaken = 0;
        }
        StartLevel(level);
    }
    static public void ShotFired()
    {
        S.shotsTaken++;
    }
    static public GameObject GetCastle()
    {
        return S.castle;
    }
    static public int  GetLevel()
    {
        return S.level;
    }
    public void RestartLevel()
    {
        
        StartLevel(level);
        mode = GameMode.playing;
        Debug.Log("Level " + level + "Restart");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
