using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
    }

    public void ChangeSizes(string jsonSizes)
    {
        //send message to all connected players (even master client) with random sizes
        photonView.RPC("ChangeSizesRPC", RpcTarget.All, jsonSizes);
    }

    [PunRPC]
    public void ChangeSizesRPC(string jsonSizes)
    {
        print(jsonSizes);

        List<PlayerInfo> playerInfos = JsonConvert.DeserializeObject<List<PlayerInfo>>(jsonSizes);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            //update the size of the box accordingly
            player.GetComponent<PlayerController>().ChangeSizeFromMasterClient(playerInfos);

        }


    }

    public void DestroyPlayer(int destroyPlayerId)
    {
        //send a message to all connected players, with id to destroy
        photonView.RPC("DestroyPlayerRPC", RpcTarget.All, destroyPlayerId);
    }

    [PunRPC]
    public void DestroyPlayerRPC(int destroyPlayerId)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.GetComponent<PhotonView>().Owner.ActorNumber == destroyPlayerId)
            {
                if(player.GetComponent<PhotonView>().AmOwner) //you have to be the owner of that PhotonView to destroy
                    PhotonNetwork.Destroy(player);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
