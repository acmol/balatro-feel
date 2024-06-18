using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;




public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public IList<Sprite> cardSprites;
    public Dictionary<(Card.Type, int), Sprite> spritesDict;
    public IList<Card.CardInfo> cardInfos;

    void Start()
    {
        cardSprites = new List<Sprite>();
        spritesDict = new Dictionary<(Card.Type, int), Sprite>();
        
        List<string> names = new List<string>();

        cardInfos = new List<Card.CardInfo>();
        List<Card.Type> card_types = new List<Card.Type> {
            Card.Type.Heart, Card.Type.Club, Card.Type.Diamond, Card.Type.Spade, Card.Type.Operator, Card.Type.GOAL
        };
        string format = "Assets/Sprites/8BitDeck.png[8BitDeck_{0}]";
        for (int i = 0; i != 52; ++i)
        {
            names.Add(string.Format(format, i));
            cardInfos.Add(new Card.CardInfo(card_types[i / 13], (i + 2 ) % 13));
        }
        string goal_format = "Assets/Sprites/numbers.png[numbers_{0}]";
        for (int i = 0; i != 100; ++i)
        {
            names.Add(string.Format(goal_format, i));
            cardInfos.Add(new Card.CardInfo(Card.Type.GOAL, i));
        }
        string op_format = "Assets/Sprites/operators.png[operators_{0}]";
        for (int i = 0; i != 4; ++i)
        {
            names.Add(string.Format(op_format, i));
            cardInfos.Add(new Card.CardInfo(Card.Type.Operator, i));
        }

        Addressables.LoadAssetsAsync<Sprite>(names, null, Addressables.MergeMode.Union).Completed += (handle) => {
            cardSprites = handle.Result;
            for (int i = 0; i != cardSprites.Count; ++i)
            {
                spritesDict[cardInfos[i].AsKey()] = cardSprites[i];
            }

            StartCoroutine(AfterInit());
        };

        IEnumerator AfterInit()
        {
            InitDeck();
            DrawCards();
            yield break;
        }
    }


    void DrawCards()
    {
        var card_holders = gameObject.GetComponentsInChildren<HorizontalCardHolder>();
        foreach (var holder in card_holders)
        {
            holder.DrawCards();
        }
    }


    void LoadSprites()
    {

    }

    void InitDeck()
    {
        SetCardPile("Deck", cardInfos.Take(52).ToList());
        SetCardPile("GoalPile", cardInfos.Skip(52).Take(100).ToList());
        SetCardPile("OperatorDeck", cardInfos.Skip(152).Take(4).SelectMany((item)=> Enumerable.Repeat(item, 10)).ToList());
    }

    void SetCardPile(string pile_obj_name, List<Card.CardInfo> cards)
    {
        var deck = GameObject.Find(pile_obj_name);
        var pile = deck.GetComponent<Pile>();
        pile.SetCards(cards);
        pile.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
