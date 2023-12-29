using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*アイテムプレハブ*/
    [SerializeField] List<BubbleController> prefabBubbles;
    /*UI*/
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] GameObject panelResult;
    /*Audio*/
    [SerializeField] AudioClip seDrop;
    [SerializeField] AudioClip seMerge;
    /*スコア*/
    private int score;
    /*現在のアイテム*/
    BubbleController currentBubble;
    /*生成位置*/
    const float SpawnItemY = 3.5f;
    /*Audioの再生装置*/
    AudioSource audioSource;

    private void Start( )
    {
        panelResult.SetActive( false );

        StartCoroutine(SpawnCurrentItem());
    }

    private void Update( )
    {
        
    }

    /// <summary>
    /// アイテム生成
    /// </summary>
    /// <param name="position"></param>
    /// <param name="colorType"></param>
    /// <returns></returns>
    BubbleController SpawnItem( Vector2 position , int colorType = -1 )
    {
        int index = Random.Range(0 , prefabBubbles.Count / 2);

        //色の指定があれば上書きさせる
        if(0 < colorType)
        {
            index = colorType;
        }

        BubbleController bubble = Instantiate(prefabBubbles[index], position, Quaternion.identity);

        bubble.manager   = this;
        bubble.colorType = index;

        return bubble;
    }

    /// <summary>
    /// 所持アイテムの生成
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCurrentItem()
    {
        yield return new WaitForSeconds(1.0f);

        currentBubble = SpawnItem(new Vector2(0 , SpawnItemY));
        
        //取得したコンポーネントは重力フリーにして、持ってる時に下に落ちないように設定。
        currentBubble.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    

}
