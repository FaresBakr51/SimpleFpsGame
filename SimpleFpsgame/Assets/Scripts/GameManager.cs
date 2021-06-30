using UnityEngine;
using Photon.Pun;
public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerprefab;
    void Start()
    {
        PhotonNetwork.Instantiate(playerprefab.name, new Vector3(0, 5, 0), Quaternion.identity);    
    }
}
