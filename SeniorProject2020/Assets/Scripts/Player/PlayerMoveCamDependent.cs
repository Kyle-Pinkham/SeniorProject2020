﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveCamDependent : MonoBehaviour
{
    public bool canMove = true;
	
	public float speed = 1;
	private float baseSpeed;
	public float jumpSpeed = 1;

    public GameObject orbitCamera;

	//private PlayerAnim playerAnim;
	public Rigidbody rb;
    public InputManager inputManager;

	void Start ()
	{
		baseSpeed = speed;
		//playerAnim = GetComponent<PlayerAnim>();
		//rb = GetComponent<Rigidbody>();
		StartCoroutine(Move());
	}

	IEnumerator Move()
    {
        while(canMove)
        {

			//groundcheck
         	inputManager.isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, 1 << LayerMask.NameToLayer("Ground"));

            //calculate new transform.position
            Vector3 newPos = Vector3.Normalize(new Vector3(inputManager.GetHorizontal(), 0, inputManager.GetVertical()));
            newPos = orbitCamera.transform.TransformDirection(newPos);
            newPos.y = 0;

            //move to new position
            rb.AddForce(newPos * speed);

            if(rb.velocity.magnitude > 0.1)
            {
                transform.forward = orbitCamera.transform.forward;
                Vector3 newRot = transform.rotation.eulerAngles;
                newRot.x = 0;
                transform.rotation = Quaternion.Euler(newRot);
                //playerAnim.Walk();
            }
            if(speed > baseSpeed)
            {
                //playerAnim.Run();
            }

            //Resets velocity to 0 each frame so character doesn't slide around
            rb.velocity = new Vector3(0, rb.velocity.y, 0);

            yield return null;
        }
    }

	public void ChangeToSprintSpeed()
	{
		speed = baseSpeed * 2;
	}

	public void ChangeToRegularSpeed()
	{
		speed = baseSpeed;
	}

	public void Jump()
	{
		if(inputManager.isGrounded)
		{
			rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);
			print("jumping");
		}
	}
}
