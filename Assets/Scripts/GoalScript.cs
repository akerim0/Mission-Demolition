using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class GoalScript : MonoBehaviour
{
    static public bool goalMet = false;
    private AudioSource audioS;
    public AudioClip hitAudio;
    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ProjectileScript proj = other.GetComponent<ProjectileScript>();

        if(proj != null)
        {
            GoalScript.goalMet = true;
            audioS.PlayOneShot(hitAudio);
            Material mat = GetComponent<Renderer>().material;
            Color c = Color.green;
            c.a = 0.75f;
            mat.color = c;

        }
    }
}
