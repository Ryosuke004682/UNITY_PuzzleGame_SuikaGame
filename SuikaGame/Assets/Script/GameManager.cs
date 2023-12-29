using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    const float SPAWN_ITEM_Y = 3.5f;
    /*Audioの再生装置*/
    AudioSource audioSource;


    public float LeapLinearInterpolation = 0.5f;//補間するまでの時間


    private void Start( )
    {
        StartCoroutine(SpawnCurrentItem());
        panelResult.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update( )
    {
        if (!currentBubble) return;
        MoveItem();
    }

    /// <summary>
    /// アイテムの移動処理
    /// </summary>
    private void MoveItem()
    {
        //マウス座標からWorld座標に変換
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //x座標をマウスに合わせる
        Vector2 bubblePosition = new Vector2(worldPoint.x , SPAWN_ITEM_Y);
        currentBubble.transform.position = bubblePosition;

        //タッチ処理
        if (Input.GetMouseButtonUp(0))
        {
            //重力をセットして落とす
            currentBubble.GetComponent<Rigidbody2D>().gravityScale = 1;
            currentBubble = null;

            //次のアイテムをセット
            StartCoroutine(SpawnCurrentItem());

            //SEの設定
            audioSource.PlayOneShot(seDrop);
        }
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

        currentBubble = SpawnItem(new Vector2(0 , SPAWN_ITEM_Y));
        
        //取得したコンポーネントは重力フリーにして、持ってる時に下に落ちないように設定。
        currentBubble.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    
    /// <summary>
    /// アイテムを合体させる
    /// </summary>
    /// <param name="bubbleA"></param>
    /// <param name="bubbleB"></param>
    public void Merage(BubbleController bubbleA , BubbleController bubbleB)
    {
        //操作中のアイテムとぶつかった場合GameOver
        if(currentBubble == bubbleA || currentBubble == bubbleB)
        {
            enabled = false;
            panelResult.SetActive(true);

            return;
        }

        
        if (bubbleA.IsMarged  || bubbleB.IsMarged)  return;
        if (bubbleA.colorType != bubbleB.colorType) return;

        int nextColor = bubbleA.colorType + 1;
        if (prefabBubbles.Count - 1 < nextColor) return;

        Vector2 lerpPosition = 
            Vector2.Lerp
            (
                bubbleA.transform.position ,
                bubbleB.transform.position ,
                LeapLinearInterpolation
            );

        //新しいアイテムの生成
        BubbleController newBubble = SpawnItem(lerpPosition , nextColor);

        //マージ済みのフラグをあげる
        bubbleA.IsMarged = true;
        bubbleB.IsMarged = true;

        //シーンから削除
        Destroy(bubbleA.gameObject);
        Destroy(bubbleB.gameObject);

        //点数計算と表示計算（スコア処理）

        score += newBubble.colorType * 10;
        textScore.text = "" + score;

        //SE再生
        audioSource.PlayOneShot(seMerge);

    }

    /// <summary>
    /// リトライボタンの処理
    /// </summary>
    public void OnClickRetry()
    {
        SceneManager.LoadScene("GameScene");
    }

}
