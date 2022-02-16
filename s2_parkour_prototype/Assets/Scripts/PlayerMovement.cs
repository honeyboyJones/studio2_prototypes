using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerStates
    {
        grounded,
        inair,
        onwall,
        ledgegrab,
    }

    //control and see which state the player is in
    public PlayerStates CurrentState;

    [Header("Physics")]
    public float MaxSpeed;
    public float BackwardsMovementSpeed; //for left, right, or backwards
    public float InAirControl; //how much control while in air

    private float ActSpeed; //actual speed

    public float Acceleration;
    public float Decceleration;
    public float DirectionalControl; //how fast we can change direction
    private float InAirTime;
    private float GroundedTimer;
    private float AdjustmentAmt; //how much player can adjust to any movement (0 when sliding)

    [Header("Turning")]
    public float TurnSpeed;
    public float TurnSpeedInAir;
    public float TurnSpeedOnWalls;

    public float LookUpSpeed; //how fast we can look upwards
    public Camera Head; //reference to our head to move

    private float yTurn;
    private float xTurn;
    public float MaxLookAngle;
    public float MinLookAngle;

    private PlayerCollision Colli;
    private Rigidbody Rigid;
    private Animator Anim;

    private void Start()
    {
        Colli = GetComponent<PlayerCollision>();
        Rigid = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();

        AdjustmentAmt = 1; //reset any adjustement amounts
    }

    private void Update() 
    {
        float xMov = Input.GetAxis("Horizontal");
        float yMov = Input.GetAxis("Vertical");

        if(CurrentState == PlayerStates.grounded)
        {
            //check for jumping
            if(Input.GetButtonDown("Jump"))
            {
                //JumpUp();
            }

            //check for the ground
        }
        else if(CurrentState == PlayerStates.inair)
        {
            //check for ledges to grab onto

            //check for walls to run on

            //check for the ground
        }
        else if(CurrentState == PlayerStates.ledgegrab)
        {
            //remove any movement in velocity
        }
        else if(CurrentState == PlayerStates.onwall)
        {
                
        }
    }

    private void FixedUpdate()
    {
        float Del = Time.deltaTime;

        float xMov = Input.GetAxis("Horizontal");
        float yMov = Input.GetAxis("Vertical");

        float CamX = Input.GetAxis("Mouse X");
        float CamY = Input.GetAxis("Mouse Y");

        LookUpDown(CamY, Del);

        if(CurrentState == PlayerStates.grounded)
        {
            //increase grounded timer
            if(GroundedTimer < 10)
               GroundedTimer += Del;

            //get magnitude of inputs
            float inputMag = new Vector2(xMov, yMov).normalized.magnitude;
            //get which speed to apply to player (forwards or backwards)
            float targetSpd = Mathf.Lerp(BackwardsMovementSpeed, MaxSpeed, yMov);
            //check for crouching (if so apply crouch speed)
            lerpSpeed(inputMag, Del, targetSpd);

            MovePlayer(xMov, yMov, Del);
            TurnPlayer(CamX, Del, TurnSpeed);
            
        }
        else if(CurrentState == PlayerStates.inair)
        {

        }
        else if(CurrentState == PlayerStates.ledgegrab)
        {
                
        }
        else if(CurrentState == PlayerStates.onwall)
        {
                
        }
    }

    void lerpSpeed(float Mag, float d, float spd)
    {
        //current speed timed by the magnitude of inputs
        float LaMT = spd * Mag;

        //if moving or stopping
        float Accel = Acceleration;
        if(Mag == 0)
           Accel = Decceleration;

        //lerp actual speed
        ActSpeed = Mathf.Lerp(ActSpeed, LaMT, d * Accel);
    }

    void MovePlayer(float hor, float ver, float d)
    {
        //find direction to move
        Vector3 MovDir = (transform.forward * ver) + (transform.right * hor);
        MovDir = MovDir.normalized;

        //if not pressing an input, continue in direction of velocity
        if(hor == 0 && ver == 0)
           MovDir = Rigid.velocity.normalized;

        //mulilply direction by speed
        MovDir = MovDir * ActSpeed;

        MovDir.y = Rigid.velocity.y;

        //apply acceleration
        float Acel = DirectionalControl * AdjustmentAmt; //how much control
        Vector3 LerpVel = Vector3.Lerp(Rigid.velocity, MovDir, Acel * d);
        Rigid.velocity = LerpVel;
    }

    void TurnPlayer(float xAmt, float D, float Spd)
    {
        yTurn += (xAmt * D) * Spd;

        transform.rotation = Quaternion.Euler(0, yTurn, 0);
    }

    void LookUpDown(float yAmt, float d)
    {
        xTurn -= (yAmt * d) * LookUpSpeed;
        xTurn = Mathf.Clamp(xTurn, MinLookAngle, MaxLookAngle);

        Head.transform.localRotation = Quaternion.Euler(xTurn, 0, 0);
    }

}
