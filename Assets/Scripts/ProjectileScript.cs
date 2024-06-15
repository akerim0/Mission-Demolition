using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileScript : MonoBehaviour
{
    const int lookbackCount = 10;
    static List<ProjectileScript> projectiles = new List<ProjectileScript>();

    [SerializeField]
    private bool _awake = true;
    private AudioSource audioS;
    public AudioClip wallHitAudio;
    bool audioPlayed = false;
    //GameMode mode;
    public bool awake
    {
        get { return _awake; }
        private set { _awake = value; }

    }
    private Vector3 prevpos;

    private List<float> deltas = new List<float>();
    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        audioS = GetComponent<AudioSource>();
        awake = true;
        prevpos = new Vector3(1000, 1000, 0);
        deltas.Add(1000);
        projectiles.Add(this);
    }
   
    private void FixedUpdate()
    {
        if (rigid.isKinematic || !awake) return;
       
        Vector3 deltaV3 = transform.position - prevpos;
        deltas.Add(deltaV3.magnitude);
        prevpos = transform.position;
        while (deltas.Count > lookbackCount)
        {
            deltas.RemoveAt(0);
        }

        float maxDelta = 0;
        foreach(float f in deltas)
        {
            if (f > maxDelta) maxDelta = f;
        }

        if(maxDelta <= Physics.sleepThreshold)
        {
            awake = false ;
            rigid.Sleep();
        }
        if ((MissionDemolition.Projectiles.Count < 1) && !GoalScript.goalMet && rigid.IsSleeping())
        {
            MissionDemolition.mode = GameMode.gameOver;
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if(collision.transform.tag == "wall" && !audioPlayed)
        {
            audioS.PlayOneShot(wallHitAudio);
            audioPlayed = true;
        }
    }
    private void OnDestroy()
    {
        projectiles.Remove(this);
    }
    static public void DestroyProjectiles()
    {
        foreach (ProjectileScript p in projectiles)
        {
            Destroy(p.gameObject);
        }
    }
    
}
