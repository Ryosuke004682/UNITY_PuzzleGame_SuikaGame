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

    private void OnCollisionEnter2D( Collision2D collision )
    {
        BubbleController bubble = collision.gameObject.GetComponent<BubbleController>();

        if (!bubble) return;

        //ÚG‚ª‚ ‚Á‚½ê‡‡‘Ì‚³‚¹‚é
        manager.Merage(this , bubble);
    }


}
