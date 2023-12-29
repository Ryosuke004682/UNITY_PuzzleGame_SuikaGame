using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public GameManager manager;


    public int colorType;
    public bool IsMarged;

    private void Start( )
    {
        
    }

    private void Update( )
    {

        //‰æ–ÊŠO‚É—‚¿‚½‚çÁ‚·ˆ—
        if(transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
    }



}
