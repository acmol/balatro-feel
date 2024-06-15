using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;




public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public IList<Sprite> cardSprites;
    public Dictionary<(Card.Type, int), Sprite> spritesDict;

    void Start()
    {
        cardSprites = new List<Sprite>();
        spritesDict = new Dictionary<(Card.Type, int), Sprite>();
        
        List<string> names = new List<string>();
        string format = "Assets/Sprites/8BitDeck.png[8BitDeck_{0}]";
        for (int i = 0; i != 52; ++i)
        {
            names.Add(string.Format(format, i));
        }
        Addressables.LoadAssetsAsync<Sprite>(names, null, Addressables.MergeMode.Union).Completed += (handle) => {
            cardSprites = handle.Result;
            List<Card.Type> card_types =
            new List<Card.Type> {
                Card.Type.Heart, Card.Type.Club, Card.Type.Diamond, Card.Type.Spade
            };
            for (int i = 0; i != 52; ++i)
            {
                var card_info = new Card.CardInfo();
                card_info.rank = (i + 2) % 13;
                card_info.type = card_types[i / 13];
                spritesDict[card_info.AsKey()] = cardSprites[i];
            }
            print("xxxx === " + spritesDict.Count);

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


    void InitDeck()
    {
        var deck = GameObject.Find("Deck");
        var pile = deck.GetComponent<Pile>();
        List<Card.CardInfo> card_infos = new List<Card.CardInfo>();
        foreach (var (k, v) in spritesDict.Keys)
        {
            card_infos.Add(new Card.CardInfo(k, v));
        }
        pile.SetCards(card_infos);
        pile.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
