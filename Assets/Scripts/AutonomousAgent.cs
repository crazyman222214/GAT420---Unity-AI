using UnityEngine;

public class AutonomousAgent : AIAgent
{
    public Perception perception;

    private void Update()
    {


        //movement.ApplyForce(Vector3.forward * 10);
        //transform.position = Utilities.Wrap(transform.position, new Vector3(-5, -5, -5), new Vector3(5, 5, 5));

        Debug.DrawRay(transform.position, transform.forward * perception.maxDistance, Color.green);

        var gameObjects = perception.GetGameObjects();
        foreach (var go in gameObjects)
        {

            Debug.DrawLine(transform.position, go.transform.position, Color.red);


            //Moving towards the player

            Vector3 moveDirection = go.transform.position - transform.position;

            if (moveDirection.magnitude < 2)
            {
                print("near player");
                movement.Velocity = Vector3.zero;
                movement.Acceleration = Vector3.zero;
            }
            else
            {
                transform.forward = movement.Direction;
                movement.ApplyForce((moveDirection * Time.deltaTime).normalized);


            }
        }
    }
}
