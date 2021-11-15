using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Newtonsoft.Json;

public class GameManager2 : MonoBehaviour
{
    public GameObject playerPrefab;


    // Start is called before the first frame update
    void Start()
    {
        //we are in a room, spawn box for the local player. it gets synced auto by using PhotonNetwork.Instantiate
        //PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f)
        //, Quaternion.identity, 0);



        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("ButtonChangeSizes").SetActive(true);
            print("set active true");
        }
        else
        {
            GameObject.Find("ButtonChangeSizes").SetActive(false);
            print("set active false");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeSizesButton()
    {
        Photon.Realtime.Player[] allPlayers = PhotonNetwork.PlayerList;
        List<PlayerInfo> playerInfos = new List<PlayerInfo>();

        foreach(Photon.Realtime.Player player in allPlayers)
        {
            float size = Random.Range(0.5f, 1.5f);
            playerInfos.Add(new PlayerInfo() { size = new Vector3(size, size, 1), actorNumber = player.ActorNumber });

        }


        string json = JsonConvert.SerializeObject(playerInfos);
        GetComponent<NetworkManager>().ChangeSizes(json);
    }


    //called when we click the spawn button
    public void SpawnButton1()
    {
        TMP_Dropdown dropdown = GameObject.Find("DropdownColours").GetComponent<TMP_Dropdown>();
        string colour = dropdown.options[dropdown.value].text;

        object[] myCustomInitData = new object[1] { colour };

        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f)
            , Quaternion.identity, 0, myCustomInitData);

    }
}
