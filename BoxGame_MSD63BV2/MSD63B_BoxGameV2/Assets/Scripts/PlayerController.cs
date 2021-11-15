using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class PlayerController : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
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


        if (!photonView.IsMine)
        {
            //we don't own this instance (player (clone)), therefore destroy rigidbody2d
            Destroy(GetComponent<Rigidbody2D>());
        }
        else
        {
            body = GetComponent<Rigidbody2D>();

            fixedJoystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
        }

    
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

        transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);


    }

    private void FixedUpdate()
    {
        if(photonView.IsMine)
           body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //we own this player (this instance), therefore send others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
           // print("sending data");
         
        }
        else
        {
            //receive data from other player
            this.playerPos = (Vector3)stream.ReceiveNext();
            this.playerScale = (Vector3)stream.ReceiveNext();

           // print("received player:" + this.playerPos);
        }
    }

    private void UpdatePlayerName(string nickname)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = nickname;
    }

    public void ChangeSizeFromMasterClient(List<PlayerInfo> playerInfos)
    {
        foreach(PlayerInfo playerInfo in playerInfos)
        {
            if (photonView.Owner.ActorNumber == playerInfo.actorNumber)
                this.playerScale = playerInfo.size;
        }
    }


    //is called automatically when a box is instaniated
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        UpdatePlayerName(info.photonView.Owner.NickName);

        float size = Random.Range(0.5f, 1.5f);
        this.playerScale = new Vector3(size, size, 1);

        //load the colour selected by the player
        object[] instantiationData = info.photonView.InstantiationData;
        string colour = (string)instantiationData[0];

        if (colour == "Red")
            GetComponent<SpriteRenderer>().color = Color.red;
        else if (colour == "Blue")
            GetComponent<SpriteRenderer>().color = Color.blue;
        else if (colour == "Green")
            GetComponent<SpriteRenderer>().color = Color.green;

    }
}
