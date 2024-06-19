using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;

public class HorizontalCardHolder : MonoBehaviour
{

    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    // This setting is used when no pile is set (Test purpose only)
    [Header("Spawn Settings")]
    [SerializeField] private int cardsToSpawn = 7;
    public List<Card> cards;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    [SerializeField] public List<GameObject> pile;

    private List<HashSet<Card>> cardsBySourcePile;
    private Dictionary<Card, int> cardPileIndex;

    public Card DrawCard(Card.CardInfo info, GameObject pile_object = null)
    {
        var card_prefab = slotPrefab.GetComponentInChildren<Card>();
        card_prefab.pile = pile_object;
        var obj = Instantiate(slotPrefab, transform);
        var card = obj.GetComponentInChildren<Card>();
        AddCard(card);
        card.cardInfo = info;
        return card;

    }

    void AddCard(Card card)
    {
        card.PointerEnterEvent.AddListener(CardPointerEnter);
        card.PointerExitEvent.AddListener(CardPointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
        card.SelectEvent.AddListener(OnSelect);
        card.RightSelectEvent.AddListener(OnRightSelect);
        card.name = cards.Count.ToString();
        cards.Add(card);
    }
    void TransferInCard(GameObject slot)
    {
        Card card = slot.GetComponentInChildren<Card>();
        slot.transform.SetParent(transform);
        AddCard(card);
    }

    int GetLastSelect(Card current)
    {
        var indexes = cards.Where((c) => c.selected && c != current).Select((c) => c.ParentIndex());
        return indexes.Count() > 0 ? indexes.Max() + 1: 0;
    }

    void DoSelect(Card card, int pos_modify=0)
    {
        int last_select_index = GetLastSelect(card) + pos_modify;
        int cur_index = card.ParentIndex();

        var ordered_cards = gameObject.GetComponentsInChildren<Card>();


        if (cur_index <= last_select_index)
        {
            return;
        }
        for (int i = cur_index - 1; i >= last_select_index; --i)
        {
            selectedCard = card;
            Swap(ordered_cards[i]);
            EndDrag(card);
        }
    }

    public void OnSelect(Card card, bool select)
    {
        if (select)
        {
            DoSelect(card);
        }
    }


    public void OnRightSelect(Card card, bool select)
    {
        if (select)
        {
            DoSelect(card, 1);
        }
    }


    public void DrawCards()
    {
        if (pile == null || pile.Count == 0)
        {
            for (int i = cards.Count; i < cardsToSpawn; i++)
            {
                DrawCard(new Card.CardInfo(Card.Type.Diamond, 2));
            }
        } else
        {
            for (int i = 0; i != pile.Count; ++i)
            {
                var pile_obj = pile[i];
                var pile_comp = pile_obj.GetComponent<Pile>();
                int need_refill = pile_comp.refill_amount - cardsBySourcePile[i].Count;
                for (int j = 0; j < need_refill; ++j)
                {
                    var card = DrawCard(pile_comp.DrawCard(), pile_obj);
                    cardsBySourcePile[i].Add(card);
                    cardPileIndex[card] = i;
                }
            }
        }
    }

    public IEnumerable<Card> GetSelectedCards()
    {
        return cards.Where((Card card) => card.selected).OrderBy((c)=>c.ParentIndex());
    }

    void Start()
    {
        cards = new List<Card>();

        rect = GetComponent<RectTransform>();

        if (pile != null && pile.Count > 0)
        {
            cardsBySourcePile = new List<HashSet<Card>>(pile.Count);
            for(int i = 0; i != pile.Count; ++i)
            {
                cardsBySourcePile.Add(new HashSet<Card>());
            }
        }
        cardPileIndex = new Dictionary<Card, int>();

        // StartCoroutine(Frame());

        //IEnumerator Frame()
        //{
        //    yield return new WaitForSecondsRealtime(.1f);
        //    for (int i = 0; i < cards.Count; i++)
        //    {
        //        if (cards[i].cardVisual != null)
        //            cards[i].cardVisual.UpdateIndex(transform.childCount);
        //    }
        //}
    }

    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }

    void CardGoHome(Card card)
    {
        card.transform.DOLocalMove(card.selected ? new Vector3(0, card.selectionOffset, 0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);
    }

    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        CardGoHome(selectedCard);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;

    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    public void DestroyCard(Card card)
    {
        Destroy(card.transform.parent.gameObject);
        TransferOutCard(card);
    }

    void TransferOutCard(Card card)
    {
        cards.Remove(card);
        if (pile != null && pile.Count > 0)
        {
            int index = cardPileIndex[card];
            cardPileIndex.Remove(card);
            cardsBySourcePile[index].Remove(card);
        }
    }

    public void TransferOutCard(Card card, HorizontalCardHolder target)
    {
        print("Transfer out");
        TransferOutCard(card);
        target.TransferInCard(card.transform.parent.gameObject);
        target.CardGoHome(card);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (hoveredCard != null)
            {
                DestroyCard(hoveredCard);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var pointerEvent = new PointerEventData(EventSystem.current);
            pointerEvent.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, results);

            if (results.Count > 0)
            {
                GameObject hitObject = results[0].gameObject;
                if (hitObject.name == "Trippy-BG")
                {
                    foreach (Card card in cards)
                    {
                        card.Deselect();
                    }
                }
            } 
        }

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < cards.Count; i++)
        {

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    void Swap(int index)
    {
        Swap(cards[index]);
    }

    void Swap(Card card)
    {
        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = card.transform.parent;

        card.transform.SetParent(focusedParent);
        card.transform.localPosition = card.selected ? new Vector3(0, card.selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        if (card.cardVisual == null)
            return;

        bool swapIsRight = card.ParentIndex() > selectedCard.ParentIndex();
        card.cardVisual.Swap(swapIsRight ? -1 : 1);

        //Updated Visual Indexes
        foreach (Card c in cards)
        {
            c.cardVisual.UpdateIndex(transform.childCount);
        }
    }

    public void UpdateIndex()
    {
        foreach (Card c in cards)
        {
            c.cardVisual.UpdateIndex(transform.childCount);
        }
    }

}
