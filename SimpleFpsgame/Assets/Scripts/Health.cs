using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviourPunCallbacks,IPunObservable
{
    public int health = 100;
    Renderer [] visuals;
    void Start()
    {
        visuals = GetComponentsInChildren<Renderer>();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (int)stream.ReceiveNext();
        }
    }
    public  void TakeDamage(int damage)
    {
        health -= damage;
    }
    void SetRender(bool state)
    {
        foreach(var render in visuals)
        {
            render.enabled = state;
        }
    }
   IEnumerator respawn()
    {
        SetRender(false);
        health = 100;
        GetComponent<CharacterController>().enabled = false;     
        transform.position = new Vector3(0, 10, 0);
        yield return new WaitForSeconds(1);
        GetComponent<CharacterController>().enabled = true;
        SetRender(true);
    }
   void Update()
    {
        if(health <= 0)
        {
            StartCoroutine(respawn());
        }
    }
}
