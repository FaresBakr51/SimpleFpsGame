using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPunCallbacks
{
    public Transform gunTransform;
    public ParticleSystem ps;
  
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("RPC_shot", RpcTarget.All);
            }
        }      
    }
    [PunRPC]
    void RPC_shot()
    {
        ps.Play();
        Ray ray = new Ray(gunTransform.position, gunTransform.forward);
        if(Physics.Raycast(ray,out RaycastHit hit, 100))
        {
            var enemyhealth = hit.collider.GetComponent<Health>();
            if (enemyhealth)
            {
                enemyhealth.TakeDamage(20);
            }
        }
    }
}
