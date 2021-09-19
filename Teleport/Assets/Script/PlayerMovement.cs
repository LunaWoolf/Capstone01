using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.18f;
    

    Vector3 gravityVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;
    public bool isGrounded;
    public bool Aimming;
    public bool Teleporting;

    public GameObject Phantom;
    public GameObject Camera;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        if (isGrounded && gravityVelocity.y > 0)
        {
            gravityVelocity.y = -2f;

        }

        if (!Teleporting)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            gravityVelocity.y += gravity * Time.deltaTime;
            controller.Move(gravityVelocity * Time.deltaTime);
        }


        if (Input.GetKeyDown(KeyCode.Space) && !Teleporting)
        {
            Aimming = true;
        }
        CheckAimming();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Aimming = false;
            StartCoroutine(Teleport(0f, Phantom.transform));
        }
        //OnDrawGizmos();
    }

    void CheckAimming()
    {
        if (Aimming)
        {
            Phantom.GetComponent<MeshRenderer>().enabled = true;
            //Phantom.transform.position = Camera.transform.forward * 10f;

            Camera camera = Camera.GetComponent<Camera>();
            //Vector3 p = camera.ViewportToWorldPoint(camera.transform.forward);
            Phantom.transform.position = camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 5f));

            //Phantom.transform.position = Camera.transform.forward * 2f;

            //Phantom.GetComponent<Transform>().localRotation = Camera.transform.localRotation;

        }

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(Camera.transform.position, p * 10f);
    }



    IEnumerator Teleport(float delayTime, Transform teleportPosition)
    {
        Teleporting = true;
        this.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(delayTime); 
        float startTime = Time.time; 
        while (Time.time - startTime <= 1)
        { 
            transform.position = Vector3.Lerp(transform.position,teleportPosition.position, Time.time - startTime);
            yield return 1; 
        }
        StartCoroutine(Float(1f));
    }

    IEnumerator Float(float floatTime)
    {
        gravity = -3f;
        Phantom.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CharacterController>().enabled = true;
        Teleporting = false;
        gravityVelocity.y = 0f;
        yield return new WaitForSeconds(floatTime);
        gravity = -9.18f;
    }

}
