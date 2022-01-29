using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DebugPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    private Camera playerCamera = default;

    [SerializeField]
    private Canvas playerCanvas = default;

    [SerializeField]
    private Slider hpSlider = default;

    private int MAX_HP = 1000;
    private int hp = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        this.hp = this.MAX_HP;
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine == true) // my turn
        {
            this.playerCamera.gameObject.SetActive(true); // enable my camera

            // -------------------------- for debug BEGIN -------------------------------
            if(Input.GetKey(KeyCode.UpArrow) == true) {
                this.gameObject.transform.position += this.gameObject.transform.forward * 0.05f;
            }
            if(Input.GetKey(KeyCode.LeftArrow) == true) {
                this.gameObject.transform.Rotate(0, -1.0f, 0);
            }
            if(Input.GetKey(KeyCode.RightArrow) == true) {
                this.gameObject.transform.Rotate(0, 1.0f, 0);
            }

            if(Input.GetKey(KeyCode.Space) == true) {
                this.hp++;
            } else {
                this.hp--;
            }
            if (this.hp > this.MAX_HP) {
                this.hp = this.MAX_HP;
            } else if (this.hp < 0) {
                this.hp = 0;
            }
            // -------------------------- for debug END -------------------------------

        } 
        else { // others' turn
            this.playerCamera.gameObject.SetActive(false); // disable my camera
        }


        // ----------------- Common Procedure BEGIN ---------------------------
        this.playerCanvas.transform.rotation = Camera.main.transform.rotation; // keep direction of HP bar
        this.hpSlider.value = (float)this.hp / (float)this.MAX_HP; // change value of HP bar
        // ----------------- Common Procedure END ---------------------------
    }

    // Communication function
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(this.hp);
        } 
        else {
            this.hp = (int)stream.ReceiveNext();
        }
    }
}
