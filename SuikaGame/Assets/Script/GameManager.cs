using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    const float SPAWN_ITEM_Y = 3.5f;
    /*Audio�̍Đ����u*/
    AudioSource audioSource;


    public float LeapLinearInterpolation = 0.5f;//��Ԃ���܂ł̎���


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
    /// �A�C�e���̈ړ�����
    /// </summary>
    private void MoveItem()
    {
        //�}�E�X���W����World���W�ɕϊ�
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //x���W���}�E�X�ɍ��킹��
        Vector2 bubblePosition = new Vector2(worldPoint.x , SPAWN_ITEM_Y);
        currentBubble.transform.position = bubblePosition;

        //�^�b�`����
        if (Input.GetMouseButtonUp(0))
        {
            //�d�͂��Z�b�g���ė��Ƃ�
            currentBubble.GetComponent<Rigidbody2D>().gravityScale = 1;
            currentBubble = null;

            //���̃A�C�e�����Z�b�g
            StartCoroutine(SpawnCurrentItem());

            //SE�̐ݒ�
            audioSource.PlayOneShot(seDrop);
        }
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

        currentBubble = SpawnItem(new Vector2(0 , SPAWN_ITEM_Y));
        
        //�擾�����R���|�[�l���g�͏d�̓t���[�ɂ��āA�����Ă鎞�ɉ��ɗ����Ȃ��悤�ɐݒ�B
        currentBubble.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
    
    /// <summary>
    /// �A�C�e�������̂�����
    /// </summary>
    /// <param name="bubbleA"></param>
    /// <param name="bubbleB"></param>
    public void Merage(BubbleController bubbleA , BubbleController bubbleB)
    {
        //���쒆�̃A�C�e���ƂԂ������ꍇGameOver
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

        //�V�����A�C�e���̐���
        BubbleController newBubble = SpawnItem(lerpPosition , nextColor);

        //�}�[�W�ς݂̃t���O��������
        bubbleA.IsMarged = true;
        bubbleB.IsMarged = true;

        //�V�[������폜
        Destroy(bubbleA.gameObject);
        Destroy(bubbleB.gameObject);

        //�_���v�Z�ƕ\���v�Z�i�X�R�A�����j

        score += newBubble.colorType * 10;
        textScore.text = "" + score;

        //SE�Đ�
        audioSource.PlayOneShot(seMerge);

    }

    /// <summary>
    /// ���g���C�{�^���̏���
    /// </summary>
    public void OnClickRetry()
    {
        SceneManager.LoadScene("GameScene");
    }

}
