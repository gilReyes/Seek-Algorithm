using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class KinematicSeek : MonoBehaviour {
    //Data for character and target
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject target;

    //Max Speed at which character can travel
    [SerializeField] private float maxSpeed;

    //True for seeking false for fleeing
    [SerializeField] private bool seek;

    //Satisfaction radius
    [SerializeField] private float satisfactionRadius;

    //Makes the character slow down as it reaches the target. Higher values more gentle deaceleration.
    [SerializeField] private float timeToTarget;

    private Transform characterTransform;
    private Transform targetTransform;
    private KinematicSteeringOutput steering;
	// Use this for initialization
	void Start () {
        //Get Character and target transforms
        characterTransform = character.GetComponent<Transform>();
        targetTransform = target.GetComponent<Transform>();
        //Create Output holder
        steering = new KinematicSteeringOutput();
	}
	
	// Update is called once per frame
	void Update () {
        //Decide if we want to seek the target or flee from it
        if (seek)
        {
            //Calculate resulting vector between target and character
            steering.velocity = targetTransform.position - characterTransform.position;
        }
        else
        {
            //Calculate resulting vector between character and vector
            steering.velocity = characterTransform.position - targetTransform.position;
        }

        if(steering.velocity.magnitude < satisfactionRadius)
        {
            return;
        }

        //Scale Velocity
        steering.velocity /= timeToTarget;

        //Clip Velocity
        if(steering.velocity.magnitude > maxSpeed)
        {
            steering.velocity.Normalize();
            steering.velocity *= maxSpeed;
        }
        //Reset rotation and set the move Vector
        steering.rotation = 0;
        character.GetComponent<ThirdPersonCharacter>().Move(steering.velocity, false, false);
	}
}
