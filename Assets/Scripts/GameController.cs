using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] CardUI _cardUIPattern;
    [SerializeField] Table _handField, _tableField;

    List<CardUI> _cards;
    int num = 0;

    private void Awake()
    {
        _cards = new List<CardUI>();
    }

    private void Start()
    {
        int count = UnityEngine.Random.Range(4, 7);
        for (int i = 0; i < count; i++)
        {
            CardUI cui = CreateCardUI();
            cui.UpdateCard(CreateCard());
            _cards.Add(cui);
            foreach (CardUI card in _cards)
            {
                card.transform.SetParent(_handField.transform);
            }
        }
    }

    public void ChangeCardValue()
    {
        if (num == _cards.Count)
            num = 0;
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                _cards[num].SetUpAtack(UnityEngine.Random.Range(-2, 10));
                break;
            case 1:
                _cards[num].SetUpHP(UnityEngine.Random.Range(-2, 10));
                break;
            case 2:
                _cards[num].SetUpMana(UnityEngine.Random.Range(-2, 10));
                break;
        }
        num++;
    }

    private Card CreateCard()
    {
        Card result = ScriptableObject.CreateInstance(typeof(Card)) as Card;
        result.atk = UnityEngine.Random.Range(0, 10);
        result.hp = UnityEngine.Random.Range(-1, 10);
        result.mana = UnityEngine.Random.Range(0, 10);
        return result;
    }

    private CardUI CreateCardUI()
    {
        CardUI result = Instantiate(_cardUIPattern.gameObject, transform).GetComponent<CardUI>();
        result.onEndDrag.AddListener(CardMove);
        return result;
    }

    private void CardMove(CardUI card)
    {
        if (_tableField.active)
            card.transform.SetParent(_tableField.transform);
        else if (_handField.active)
            card.transform.SetParent(_handField.transform);
        else
            card.Return();
    }
}
