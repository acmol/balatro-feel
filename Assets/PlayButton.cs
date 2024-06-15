using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerDownHandler
{

    CanvasGroup canvasGroup = null;
    GameObject playingCardGroup = null;
    // Start is called before the first frame update
    Vector2 playingCardGroupPosition;
    RectTransform playingCardTransform;
    void Start()
    {
        var group = GameObject.Find("GameButtonGroup");
        canvasGroup = group.GetComponent<CanvasGroup>();
        playingCardGroup = GameObject.Find("PlayingCardGroup");
        playingCardTransform = playingCardGroup.GetComponent<RectTransform>();
        playingCardGroupPosition = playingCardTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleButtons()
    {
        // ��鵱ǰ��͸���ȣ����Ϊ0����ʾ�������Ϊ0������
        if (canvasGroup.alpha == 0)
        {
            // ��ʾ��ť
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            playingCardTransform.anchoredPosition = playingCardGroupPosition;
            //var holder = playingCardGroup.GetComponent<HorizontalCardHolder>();
            //var card_info = new Card.CardInfo();
            //card_info.rank = 5;
            //card_info.type = Card.Type.Diamond;
            //holder.cards[0].cardInfo = card_info;
        }
        else
        {

            playingCardTransform.anchoredPosition = new Vector3(playingCardGroupPosition.x, 100, 0);

            // ���ذ�ť
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        StartCoroutine(HiddenButtons());
        IEnumerator HiddenButtons()
        {
            
            ToggleButtons();
            yield return new WaitForSecondsRealtime(.1f);
            ToggleButtons();
        }
    }
}
