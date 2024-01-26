using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public enum eView
    {none, slingshot, castle, both};

    static private CameraFollowScript S;
    static public GameObject pointOfInt;

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    public GameObject viewBothGo;

    [Header("Dynamic")]
    public float camZ;
    public eView nextView = eView.slingshot;

    private void Awake()
    {
        S = this;
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 destination = Vector3.zero;

        if (pointOfInt != null)
        {
            Rigidbody poiRigid = pointOfInt.GetComponent<Rigidbody>();
            if((poiRigid !=null) && poiRigid.IsSleeping())
            {
                pointOfInt = null;
            }
        }
        if(pointOfInt != null)
        {
            destination = pointOfInt.transform.position;
        }
        
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination.z = camZ;
        
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }

    public void SwitchView(eView newView)
    {
        if(newView == eView.none)
        {
            newView = nextView;
        }
        switch (newView)
        {
            case eView.slingshot:
                pointOfInt = null;
                nextView = eView.castle;
                break;
            case eView.castle:
                pointOfInt = MissionDemolition.GetCastle();
                nextView = eView.both;
                break;
            case eView.both:
                pointOfInt = viewBothGo;
                nextView = eView.slingshot;
                break;
        }
    }
    public void SwitchView()
    {
        SwitchView(eView.none);
    }

    static public void SWITCH_VIEW(eView newView)
    {
        S.SwitchView(newView);
    }
}
