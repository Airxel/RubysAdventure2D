using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance {  get; private set; }

    private VisualElement m_Healthbar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();

        m_Healthbar = uiDocument.rootVisualElement.Query<VisualElement>("HealthBar");

        SetHealthValue(1f);
    }

    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(percentage * 100f);
    }
}
