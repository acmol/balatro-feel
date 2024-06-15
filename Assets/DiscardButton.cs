using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;


public class DiscardButton : MonoBehaviour, IPointerDownHandler
{
    // Start is called before the first frame update
    GameObject playingCardGroup;
    HorizontalCardHolder playingCardHolder;
    void Start()
    {
        playingCardGroup = GameObject.Find("PlayingCardGroup");
        playingCardHolder = playingCardGroup.GetComponent<HorizontalCardHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData data)
    {
        StartCoroutine(Discard());
        IEnumerator Discard()
        {
            foreach (var card in playingCardHolder.GetSelectedCards())
            {
                card.transform.DOLocalMove(new Vector3(2000, 0, 0), 0.3f).SetEase(Ease.OutBack).onComplete += () => {
                    DOTween.Complete(card.transform);
                    playingCardHolder.DestroyCard(card);
                    playingCardHolder.DrawCards();
                };
            }
            yield break;
        }
    }
}
