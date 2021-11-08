using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
		//we are in a room, spawn box for the local player. it gets synced auto by using PhotonNetwork.Instantiate
		PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f)
			, Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
