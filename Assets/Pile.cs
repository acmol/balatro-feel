using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pile : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        allCards = new List<Card.CardInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static System.Random rng = new System.Random(); 
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void Shuffle()
    {
        pos = 0;
        Shuffle(allCards);
    }

    public void SetCards(List<Card.CardInfo> cards)
    {
        allCards = cards;
    }

    public Card.CardInfo DrawCard()
    {
        return allCards[pos++];
    }

    public bool Empty()
    {
        return pos == allCards.Count;
    }

    List<Card.CardInfo> allCards;
    [SerializeField] int pos = 0;
}
