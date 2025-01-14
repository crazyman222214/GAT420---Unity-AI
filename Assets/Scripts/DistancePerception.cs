using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception
{
    override
    public GameObject[] GetGameObjects()
    {
        List<GameObject> result = new();

        //Get all colliders inside sphere
        var colliders = Physics.OverlapSphere(transform.position, maxDistance);
        foreach ( var collider in colliders )
        {

            //Do not include ourselves
            if ( collider.gameObject == gameObject ) continue;

            //Check for matching tag
            if (tagName == "" || collider.tag == tagName)
            {
                //Check if in angle range
                Vector3 direction = collider.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle <= maxAngle)
                {
                    result.Add(collider.gameObject);
                }
            }
        }
        return result.ToArray();
    }

}
