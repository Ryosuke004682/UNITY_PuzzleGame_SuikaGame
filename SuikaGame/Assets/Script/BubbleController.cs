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
        //画面外に落ちたら消す処理
        if(transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D( Collision2D collision )
    {
        BubbleController bubble = collision.gameObject.GetComponent<BubbleController>();

        if (!bubble) return;

        //接触があった場合合体させる
        manager.Merage(this , bubble);
    }


}
