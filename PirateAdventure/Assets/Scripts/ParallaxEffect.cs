using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    //Starting position for the parallax game object
    Vector2 startingPosition;

    //Start Z value of the parallax game object
    float startingZ;

    //Distance the cam moved from starting position of parallax object
    Vector2 camMovesSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    //if object = in front of target, use near clip plane, and vice versa
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //The closer the object from the player, the faster the ParallaxEffect will move. Drag Z value closer to target to make it slower.
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
        
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + camMovesSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
