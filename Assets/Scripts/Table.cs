﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class Table : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool active;

    public void OnPointerEnter(PointerEventData eventData)
    {
        active = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
