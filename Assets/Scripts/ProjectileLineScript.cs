using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLineScript : MonoBehaviour
{
    private  LineRenderer line;
    private bool drawing = true;
    private ProjectileScript projetile;
    static List<ProjectileLineScript> proj_line = new List<ProjectileLineScript>();
    private const float dim_mult = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        line.SetPosition(0, transform.position);
        projetile = GetComponentInParent<ProjectileScript>();
        ADD_LINE(this);

    }

    private void FixedUpdate()
    {
        if (drawing)
        {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, transform.position);

            if(projetile != null)
            {
                if (!projetile.awake)
                {
                    drawing = false;
                    projetile = null;
                }
            }
        }
    }
    private void OnDestroy()
    {
        proj_line.Remove(this);
    }

    static void ADD_LINE(ProjectileLineScript newLine)
    {
        Color col;
        foreach(ProjectileLineScript pl in proj_line)
        {
            col = pl.line.startColor;
            col = col * dim_mult;
            pl.line.startColor = pl.line.endColor = col;
        }
        proj_line.Add(newLine);
    }
}
