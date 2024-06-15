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
        // 检查当前的透明度，如果为0则显示，如果不为0则隐藏
        if (canvasGroup.alpha == 0)
        {
            // 显示按钮
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

            // 隐藏按钮
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
