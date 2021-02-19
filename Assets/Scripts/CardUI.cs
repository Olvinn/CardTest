using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Vector3 position;
    public CardEvent onEndDrag, onBeginDrag;

    [SerializeField] CanvasGroup _cg;
    [SerializeField] TextMeshProUGUI _manaText, _atkText, _hpText;
    [SerializeField] Image _art;

    int _curMana, _curAtk, _curHp;

    bool _drag = false;
    bool _return = true;

    Card card;

    private void Awake()
    {
        onEndDrag = new CardEvent();
    }

    void Start()
    {
        position = transform.position;
    }

    public void UpdateCard(Card card)
    {
        this.card = card;
        UpdateAtack(card.atk);
        UpdateHealth(card.hp);
        UpdateMana(card.mana);
        UpdateArt();
    }

    void Update()
    {
        if (_drag)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float scale = Mathf.Abs(Camera.main.transform.position.z) / ray.direction.z;
            Vector3 pos = Input.mousePosition;
            pos.z = 0;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 10);
        }
        //else if (_return)
        //{
        //    transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 10);
        //}
    }

    public void SetUpMana(int value)
    {
        StopCoroutine("CounterAnimation");
        UpdateMana(_curMana); UpdateAtack(_curAtk); UpdateHealth(_curHp);
        StartCoroutine(CounterAnimation(_curMana, value, _manaText));
        _curMana = value;
    }

    public void SetUpAtack(int value)
    {
        StopCoroutine("CounterAnimation");
        UpdateMana(_curMana); UpdateAtack(_curAtk); UpdateHealth(_curHp);
        StartCoroutine(CounterAnimation(_curAtk, value, _atkText));
        _curAtk = value;
    }

    public void SetUpHP(int value)
    {
        StopCoroutine("CounterAnimation");
        UpdateMana(_curMana); UpdateAtack(_curAtk); UpdateHealth(_curHp);
        StartCoroutine(CounterAnimation(_curHp, value, _hpText));
        _curHp = value;
    }

    void UpdateMana(int value)
    {
        _curMana = value;
        if (card != null)
            _manaText.text = value.ToString();
    }

    void UpdateAtack(int value)
    {
        _curAtk = value;
        if (card != null)
            _atkText.text = value.ToString();
    }

    void UpdateHealth(int value)
    {
        _curHp = value;
        if (card != null)
            _hpText.text = value.ToString();
    }

    void UpdateArt()
    {
        if (card != null)
            if (card.sprite == null)
                StartCoroutine(WaitForArt());
            else
                _art.sprite = Sprite.Create(card.sprite, new Rect(0,0,8,8), Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _drag = true;
        position = transform.position;
        _return = false;
        _cg.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _drag = false;
        _cg.blocksRaycasts = true;
        onEndDrag?.Invoke(this);
    }

    public void Return()
    {
        _return = true;
    }

    IEnumerator WaitForArt()
    {
        int i = 0;
        while (card.sprite == null && i < 10)
        {
            i++;
            yield return new WaitForSeconds(1);
        }

        _art.sprite = Sprite.Create(card.sprite, new Rect(0, 0, card.sprite.width, card.sprite.height), new Vector2(.5f,.5f));
    }

    IEnumerator CounterAnimation(int curValue, int newValue, TextMeshProUGUI text)
    {
        float temp = curValue;
        while ((int)temp != newValue)
        {
            temp = Mathf.MoveTowards(temp, newValue, 1);
            text.text = ((int)temp).ToString();
            yield return new WaitForSeconds(.1f);
        }

        _art.sprite = Sprite.Create(card.sprite, new Rect(0, 0, card.sprite.width, card.sprite.height), new Vector2(.5f, .5f));
    }

    [System.Serializable]
    public class CardEvent : UnityEvent<CardUI>
    {
    }
}
