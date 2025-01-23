using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    // Variables utilizadas para mostrar elementos de la UI
    private VisualElement m_NPCDialogue;
    private Label m_NPCLabel;
    private float m_TimerDisplay;
    private VisualElement m_Healthbar;

    // Variable utilizada para el tiempo que los diálogos están siendo mostrados
    //public float displayTime = 5.0f;

    // Variable y función para poder usar este script en otros, como un Singleton
    public static UIHandler instance {  get; private set; }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Función para guardar referencias de los elementos y establecer valores iniciales de la UI
    /// </summary>
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

    /// <summary>
    /// Función que controla el tiempo que está siendo mostrado un diálogo
    /// </summary>
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

    /// <summary>
    /// Función que controla el tamaño de la barra de vida
    /// </summary>
    /// <param name="percentage"></param>
    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(percentage * 100f);
    }

    /// <summary>
    /// Función que controla la aparición de los diálogos
    /// </summary>
    /// <param name="dialogueText"></param>
    public void DisplayDialogue(string dialogueText)
    {
        m_NPCLabel.text = dialogueText;
        m_NPCDialogue.style.display = DisplayStyle.Flex;
    }

    public void DisplayTime(float displayTime)
    {
        m_TimerDisplay = displayTime;
    }
}
