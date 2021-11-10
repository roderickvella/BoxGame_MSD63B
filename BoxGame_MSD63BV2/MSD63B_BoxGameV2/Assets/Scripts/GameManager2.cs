using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class GameManager2 : MonoBehaviour
{
    public GameObject playerPrefab;


    // Start is called before the first frame update
    void Start()
    {
        //we are in a room, spawn box for the local player. it gets synced auto by using PhotonNetwork.Instantiate
        //PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f)
        //, Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Test()
    {

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
