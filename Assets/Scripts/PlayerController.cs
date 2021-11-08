using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour, IPunObservable
{
    Rigidbody2D body;

    float horizontal;
    float vertical;

    public float runSpeed = 5.0f;

    public FixedJoystick fixedJoystick;

    private PhotonView photonView;

    private Vector3 playerPos;
    private Vector3 playerScale;

    void Start()
    {


        photonView = PhotonView.Get(this);

        
        body = GetComponent<Rigidbody2D>();

        fixedJoystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
        

    
    }

    void Update()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");

        if (!photonView.IsMine)
        {
            //if player object is not mine, then we need to manually change its position with data from server
            transform.position = Vector3.Lerp(transform.position, this.playerPos, Time.deltaTime * 10); //lerp is created for smooth animation
      
        }
        else {

            horizontal = fixedJoystick.Horizontal;
            vertical = fixedJoystick.Vertical;
        }



    }

    private void FixedUpdate()
    {
      
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //we own this player (this instance), therefore send others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            print("sending data");
         
        }
        else
        {
            //receive data from other player
            this.playerPos = (Vector3)stream.ReceiveNext();
            this.playerScale = (Vector3)stream.ReceiveNext();

            print("received player:" + this.playerPos);
        }
    }
}
