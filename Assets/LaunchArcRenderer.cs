using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput.PlatformSpecific;
using UnityEngine.EventSystems;
using UnityStandardAssets;
using UnityStandardAssets._2D;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRenderer : MonoBehaviour
{
    //
    LineRenderer lr;

    public Platformer2DUserControl playerController;
    public PlatformerCharacter2D playerCharacterScript;
 
    public float velocity;
    public float angle;
    public int resolution = 20;


    float g; // force of gravity of y axis
    float radianAngle;
    void Awake()
    {
        playerController = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
        playerCharacterScript = gameObject.transform.parent.GetComponent<PlatformerCharacter2D>();
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }


  


    // Use this for initialization
    void Start()
    {
        RenderArc();
    }

    void RenderArc()
    {
        lr.SetVertexCount(resolution + 1);
        lr.SetPositions(CalculateArcArray());
    }

    Vector3[] CalculateArcArray()
    {
        List<Vector3> arcArray = new List<Vector3>(resolution * 4 + 1);

        for (int i = 0; i <= resolution * 4; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray.Add(FindPointOnParabola(new Vector2(playerController.h, playerController.v) * playerCharacterScript.m_JumpForce, t));
        }

        return arcArray.ToArray();
    }

    private Vector2 FindPointOnParabola(Vector2 initialVelocity, float time, float gravityAcceleration = -9.81f)
    {
        return new Vector2(initialVelocity.x * time, (initialVelocity.y + ((gravityAcceleration * 10)/2) * time) * time);
    }
    

    

    void Update()
    {
       
        //angle = Vector2.SignedAngle(Vector2.right, new Vector2(playerCharacterScript.h, playerCharacterScript.v));
        if (lr != null && Application.isPlaying)
        {
            RenderArc();
        }

    }



}



