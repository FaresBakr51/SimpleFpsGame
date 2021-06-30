using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviourPunCallbacks,IPunObservable
{
    float yVelocity = 0f; 
    public float gravity = 15f;   
    public float movementSpeed = 10f;  
    public float jumpSpeed = 10f;
    public  Transform cameraTransform;
    float pitch = 0f;  
    public float maxPitch = 85f;  
    public float minPitch = -85f; 
    public float mouseSensitivity = 2f;
    CharacterController cc;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        if (!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            Look();
            Move();
        }    
    }

    void Look()
    {     
        float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
        float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(0, xInput, 0);
        pitch -= yInput;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);    
        Quaternion rot = Quaternion.Euler(pitch, 0, 0);
        cameraTransform.localRotation = rot;
    }

    void Move()
    {  
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = Vector3.ClampMagnitude(input, 1f);
        Vector3 move = transform.TransformVector(input) * movementSpeed;
        if (cc.isGrounded)
        {
            yVelocity = -gravity * Time.deltaTime;
           
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity -= gravity * Time.deltaTime;
        move.y = yVelocity;
        cc.Move(move * Time.deltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(pitch);
        }
        else
        {
            pitch = (float)stream.ReceiveNext();
            Quaternion rot = Quaternion.Euler(pitch, 0, 0);
            cameraTransform.localRotation = rot;
        }
    }
}
