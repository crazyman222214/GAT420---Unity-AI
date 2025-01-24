using UnityEditor;
using UnityEngine;

public class AutonomousAgent : AIAgent
{
    public Perception seekPerception;
    public Perception fleePerception;
    public Perception flockPerception;

    [Header("Wander")]
    public AutoAgentData data;
    float angle;


    private void Update()
    {


        //movement.ApplyForce(Vector3.forward * 10);
        transform.position = Utilities.Wrap(transform.position, new Vector3(-15, -15, -15), new Vector3(15, 15, 15));

        //Debug.DrawRay(transform.position, transform.forward * seekPerception.maxDistance, Color.green);

        // Seek
        if (seekPerception != null )
        {
            var gameObjects = seekPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Seek(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        if (fleePerception != null)
        {

            // Flee
            var gameObjects = fleePerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                Vector3 force = Flee(gameObjects[0]);
                movement.ApplyForce(force);
            }
        }

        if (flockPerception != null)
        {
            // Flock
            var gameObjects = flockPerception.GetGameObjects();
            if (gameObjects.Length > 0)
            {
                movement.ApplyForce( Cohesion(gameObjects));
                movement.ApplyForce(Seperation(gameObjects));
                movement.ApplyForce(Alignment(gameObjects));
            }
        }

        if (movement.Acceleration.sqrMagnitude == 0)
        {

            Vector3 force = Wander();
            
            movement.ApplyForce(force);
        }

        Vector3 acceleration = movement.Acceleration;
        acceleration.y = 0;
        movement.Acceleration = acceleration;

        if (movement.Direction.sqrMagnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(movement.Direction);
        }

    }

    private Vector3 Seek(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(direction);
        return force;
    }

    private Vector3 Flee(GameObject go)
    {
        Vector3 direction = go.transform.position - transform.position;
        Vector3 force = GetSteeringForce(-direction);
        return force;
    }

    private Vector3 Cohesion(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            positions += neighbor.transform.position;
        }
        Vector3 center = positions / neighbors.Length;
        Vector3 direction = center - transform.position;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }

    private Vector3 Seperation(GameObject[] neighbors)
    {
        Vector3 positions = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            positions += neighbor.transform.position;
        }
        Vector3 center = positions / neighbors.Length;
        Vector3 direction = transform.position -center;
        Vector3 force = GetSteeringForce(direction);

        return force;
    }
    private Vector3 Alignment(GameObject[] neighbors)
    {

        return Vector3.zero;
    }

    private Vector3 Wander()
    {
        angle += Random.Range(-data.displacement, data.displacement);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 point = rotation * (Vector3.forward * data.radius);
        Vector3 forward = transform.forward + movement.Direction;
        Vector3 force = GetSteeringForce(forward + point);

        return force;
    }

    private Vector3 GetSteeringForce(Vector3 direction)
    {
        Vector3 desired = direction.normalized * movement.data.maxSpeed;
        Vector3 steer = desired - movement.Velocity;
        Vector3 force = Vector3.ClampMagnitude(steer, movement.data.maxForce);

        return force;
    }
}
