using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryUIItem : MonoBehaviour
{
    public TextMeshProUGUI survivalTimeTxt;
    public TextMeshProUGUI dateTxt;
    public void HistoryUIItemInit(string survivalTime, string dateText)
    {
        survivalTimeTxt.text = survivalTime;
        dateTxt.text = dateText;
    }
}
