using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*�A�C�e���v���n�u*/
    [SerializeField] List<BubbleController> prefabBubbles;
    /*UI*/
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] GameObject panelResult;
    /*Audio*/
    [SerializeField] AudioClip seDrop;
    [SerializeField] AudioClip seMerge;
    /*�X�R�A*/
    private int score;
    /*���݂̃A�C�e��*/
    BubbleController currentBubble;
    /*�����ʒu*/
    const float SpawnItemY = 3.5f;
    /*Audio�̍Đ����u*/
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
    /// �A�C�e������
    /// </summary>
    /// <param name="position"></param>
    /// <param name="colorType"></param>
    /// <returns></returns>
    BubbleController SpawnItem( Vector2 position , int colorType = -1 )
    {
        int index = Random.Range(0 , prefabBubbles.Count / 2);

        //�F�̎w�肪����Ώ㏑��������
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
    /// �����A�C�e���̐���
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCurrentItem()
    {
        yield return new WaitForSeconds(1.0f);

        currentBubble = SpawnItem(new Vector2(0 , SpawnItemY));
        
        //�擾�����R���|�[�l���g�͏d�̓t���[�ɂ��āA�����Ă鎞�ɉ��ɗ����Ȃ��悤�ɐݒ�B
        currentBubble.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    

}
