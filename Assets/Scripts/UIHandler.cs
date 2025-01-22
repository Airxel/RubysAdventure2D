using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    public float displayTime = 5.0f;
    private VisualElement m_NPCDialogue;
    private Label m_NPCLabel;
    private float m_TimerDisplay;

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

        m_NPCDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NPCDialogue.style.display = DisplayStyle.None;
        m_NPCLabel = m_NPCDialogue.Q<Label>("Label");
        m_TimerDisplay = -1.0f;

        SetHealthValue(1f);   
    }

    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;

            if (m_TimerDisplay < 0)
            {
                m_NPCDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(percentage * 100f);
    }

    public void DisplayDialogue(string dialogueText)
    {
        m_NPCLabel.text = dialogueText;

        m_NPCDialogue.style.display = DisplayStyle.Flex;

        m_TimerDisplay = displayTime;
    }
}
