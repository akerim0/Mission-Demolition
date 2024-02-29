using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotScript : MonoBehaviour
{
    [Header("Inscribed")]
    //public GameObject projectilePrefab;
    public float velocity = 10f;
    public GameObject projLinePrefab;
    public GameObject launchPoint;

    [Header("Dynamic")]
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    public GameObject[] Projectiles;
    bool shotFired;

    GameObject projectilePrefab;
    private void Awake()
    {
        Transform launchPointTransf = launchPoint.transform;
        //launchPoint = launchPointTransf.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTransf.position;

    }

    private void Update()
    {
        if (!aimingMode) return;

        Vector3 mousepos2D = Input.mousePosition;
        mousepos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousepos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude){
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocity;
            CameraFollowScript.SWITCH_VIEW(CameraFollowScript.eView.slingshot);
            CameraFollowScript.pointOfInt = projectile;
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.ShotFired();
            shotFired = true;
        }
        
        //if((projectilePrefab.GetComponent<ProjectileScript>() != null))
        //{
        //    Debug.Log("There is a script here !");
        //    MissionDemolition.Projectiles.RemoveAt(MissionDemolition.Projectiles.Count -1);
        //}
    }
    private void OnMouseEnter()
    {
        print("ShlingShot : OnMouse Enter");
        launchPoint.SetActive(true);
        //CameraFollowScript.SWITCH_VIEW(CameraFollowScript.eView.both);

    }
    private void OnMouseExit()
    {
        print("ShlingShot : OnMouse Exit");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        aimingMode = true;
        int numOfProj = MissionDemolition.Projectiles.Count-1;
        Debug.Log("Num of projec in sling : " + (numOfProj +1));
        projectilePrefab = MissionDemolition.Projectiles[numOfProj];
        //MissionDemolition.Projectiles.RemoveAt(numOfProj);
        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        MissionDemolition.Projectiles[numOfProj].gameObject.SetActive(false);
        shotFired = false;
        MissionDemolition.Projectiles.RemoveAt(numOfProj);
    }
}
