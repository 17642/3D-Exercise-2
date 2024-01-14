using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{
    [SerializeField]
    private Rigidbody[] rb;
    [SerializeField]
    private GameObject go_meat;
    [SerializeField]
    private int damage;

    [SerializeField]
    private bool isActivated = false;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip sound_activate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(!isActivated)
        {
            if (other.transform.tag != "Untagged")
            {
                isActivated = true;
                audioSource.clip = sound_activate;
                audioSource.Play();
                Destroy(go_meat);//고기 제거

                for(int i=0;i<rb.Length; i++)
                {
                    rb[i].useGravity = true;
                    rb[i].isKinematic = false;//리지드바디에 물리 적용
                }
                if(other.transform.name == "Player")
                {
                    other.transform.GetComponent<StatusController>().DecreaseHP(damage);
                }
            }
        }
    }
}
