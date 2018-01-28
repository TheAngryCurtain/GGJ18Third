using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;
using UI.Constants;
using UI.Enums;

public class UIManager : Singleton<UIManager>
{
    // Constants
    private static readonly string Identifier = "UIManager";

    // Inspector Serialized Fields
    [SerializeField]
    private Canvas m_Canvas;                                    //! The Game Canvas. We'll only use one and load prefabs for Screens and other elements Under it.

    [SerializeField]
    private UnityEngine.EventSystems.EventSystem m_EventSys;    //! The Unity Event System.

    [SerializeField]
    private GameObject m_PopupLayer;                            //! The layer for popups. This allows me to scrim out a background.

    [SerializeField]
    private Canvas m_PopupCanvas;                               //! The Game Canvas. We'll only use one and load prefabs for Screens and other elements Under it.  

    [SerializeField]
    private GameObject m_PromptsLayer;                          //! The layer for prompts. This allows me to scrim out a background.

    [SerializeField]
    private Canvas m_PromptsCanvas;                             //! The Game Canvas. We'll only use one and load prefabs for Screens and other elements Under it.         

    [SerializeField]
    private List<UIScreenPrefabInfo> m_PrefabInfo;              //! List of Prefab Information: Includes ID to Prefab For Loading.

    [SerializeField]
    private UIBasePopup m_GenericPopupPrefab;                   //! Generic Popup Prefab?

    // Private Member Variables
    private Animator m_ScreenAnimator;                  //! Reference Member Variable for the current screen animator. Use this to play animations and transitions.
    private Stack<ScreenId> m_ScreenStack;              //! Screen Stack by Id, so we can navigate the screen history.
    private UIBaseScreen m_CurrentScreen;               //! Reference to the current screen. We can use this to get data or input, and set up event listeners.
    private Stack<UIPopupData> m_PopupStack;            //! A popupstack that is used to track the stack of popups that have been queued to show.
    private UIBasePopup m_CurrentPopup;                 //! Reference to the current popup.
    private UIPrompts m_Prompts;                        //! Reference to the prompts class

    // Locks
    private bool m_AnimationLock;                       //! Animation Lock, do not allow screens to animate while this is locked.
    private bool m_InputLock;                           //! Input Lock, block input while this is locked.
    private bool m_PrefabLoadingLock;                   //! Lock when loading a prefab, so we don't do anything else or load another prefab.

    public bool IsAnimationLocked { get { return m_AnimationLock; } set { m_AnimationLock = value; } }
    public bool IsInputLocked { get { return m_InputLock; } set { m_InputLock = value; } }
    public bool IsPrefabLoadingLocked { get { return m_PrefabLoadingLock; } set { m_PrefabLoadingLock = value; } }

    public UnityEngine.EventSystems.EventSystem EventSystem { get { return m_EventSys; } }
    public UIPrompts Prompts { get { return m_Prompts; } }
    public Canvas MainCanvas { get { return m_Canvas; } }
    public Canvas PopupCanvas { get { return m_PopupCanvas; } }

    public override void Awake()
    {
        m_ScreenStack = new Stack<ScreenId>();
        m_PopupStack = new Stack<UIPopupData>();
        m_CurrentScreen = null;
        m_CurrentPopup = null;

        Debug.AssertFormat(ValidateManager() != false, "{0} : Failed to validate, please ensure that all required components are set and not null.", UIManager.Identifier);

        m_Prompts = m_PromptsCanvas.GetComponent<UIPrompts>();

        base.Awake();
    }

    void Start()
    {
        // These things need to be active / inactive for the base state of our manager.
        m_Canvas.gameObject.SetActive(true);
        m_PopupLayer.SetActive(false);
        m_PromptsLayer.SetActive(false);

        // TODO: This needs to be set by some sort of data.. default to keyboard? 
        m_Prompts.SetPromptsPlatform(InputPlatform.Joystick);

        TransitionToScreen(ScreenId.Title);
        //TransitionToScreen(ScreenId.Letter, new UILetterScreenData { BodyText = "It's the body boy!", SenderText = "MaMaC" });

        EventManager.Instance.AddEventListener<YEO_TALK_EVENT>(OnSpeechBubble);
    }

    public override void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener<YEO_TALK_EVENT>(OnSpeechBubble);

        base.OnDestroy();
    }

    private void OnSpeechBubble(YEO_TALK_EVENT eventData)
    {
        Debug.Log(eventData.message);

        UIBubblePopupData testPopup = new UIBubblePopupData();

        testPopup.Title = "Change Hat";
        testPopup.Body = eventData.message;

        testPopup.BubblePos = Vector3.zero;

        List<UIPopupButtonData> buttonData = new List<UIPopupButtonData>();
        buttonData.Add(new UIBubbleButtonData { Text = "Change it", Selection = PopupSelection.Okay, Button = InputButton.A });
        buttonData.Add(new UIBubbleButtonData { Text = "Keep yours", Selection = PopupSelection.Cancel, Button = InputButton.B });

        testPopup.ButtonData = buttonData;
        testPopup.Callback = (PopupSelection s, object data) => { if (s == PopupSelection.Okay) { Debug.Log("POPUP one CLOSED"); ClosePopup(); } };

        UIManager.Instance.QueuePopup(testPopup);
    }

    void Update()
    {

    }

    /// <summary>
    /// Validate that any serialized fields are properly set, and that
    /// the screens are valid.
    /// A Valid Manager should function properly.
    /// </summary>
    /// <returns></returns>
    protected override bool ValidateManager()
    {
        //TODO: Put asserts after each line to explain what's missing / wrong with my prefab.

        bool isValid = true;

        isValid = isValid && (m_Canvas != null);

        isValid = isValid && (m_PopupCanvas != null);

        isValid = isValid && (m_PopupLayer != null);

        isValid = isValid && (m_PromptsCanvas != null);

        isValid = isValid && (m_PromptsLayer != null);

        isValid = isValid && (m_ScreenStack != null);

        isValid = isValid && (m_PopupStack != null);

        isValid = isValid && ValidateScreenList();

        isValid = isValid && base.ValidateManager();

        return isValid;
    }

    /// <summary>
    /// Validates that the screen list contains no duplicate screens.
    /// </summary>
    /// <returns>True if Valid.</returns>
    private bool ValidateScreenList()
    {
        bool isValid = false;

        isValid = (m_PrefabInfo != null) && (m_PrefabInfo.Count > 0);

        List<ScreenId> tempIds = new List<ScreenId>();

        if (isValid)
        {
            for (int i = 0; i < m_PrefabInfo.Count; i++)
            {
                if (!tempIds.Contains(m_PrefabInfo[i].ScreenId))
                {
                    tempIds.Add(m_PrefabInfo[i].ScreenId);
                }
                else
                {
                    isValid = false;
                    break;
                }
            }
        }

        return isValid;
    }

    /// <summary>
    /// Returns the prefab associated with a screen id.
    /// </summary>
    /// <param name="screenId"></param>
    /// <returns></returns>
    private GameObject GetPrefabFromScreenId(ScreenId screenId)
    {
        GameObject screenPrefab = null;

        if (screenId != ScreenId.None)
        {
            UIScreenPrefabInfo info = m_PrefabInfo.Find(p => p.ScreenId == screenId);
            screenPrefab = info.Prefab;
            return screenPrefab;
        }

        Debug.AssertFormat(true, "{0} : Couldn't find a prefab for screenid : {1}. Make sure it is serialzied in mPrefabInfo.", UIManager.Identifier, screenId.ToString());
        return screenPrefab;
    }

    /// <summary>
    /// Loads a screen based on it's screen id.
    /// </summary>
    /// <param name="screenId">Unique identifier for a screen.</param>
    private UIBaseScreen LoadScreen(ScreenId screenId)
    {
        GameObject screenPrefab = GetPrefabFromScreenId(screenId);

        if (screenPrefab != null)
        {
            // Instantiate the screen in the canvas.
            GameObject instantiatedPrefab = GameObject.Instantiate(screenPrefab, m_Canvas.transform);

            if (instantiatedPrefab != null)
            {
                UIBaseScreen screen = instantiatedPrefab.GetComponent<UIBaseScreen>();

                if (screen != null)
                {
                    // Call set defaults and assign the current screen.
                    screen.Initialize();

                    return screen;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Unloads the current screen.
    /// Will likely do more stuff in the future.
    /// </summary>
    private void UnloadCurrentScreen()
    {
        if (m_CurrentScreen != null)
        {
            m_CurrentScreen.Shutdown();
            GameObject.Destroy(m_CurrentScreen.gameObject);
        }
    }

    // Go back! But only if we actually can.
    private void DoBackTransition()
    {
        if (m_ScreenStack.Count >= General.MINIMUM_NUM_SCREENS_FOR_BACK)
        {
            m_ScreenStack.Pop();
            TransitionToScreen(m_ScreenStack.Peek());
        }
    }

    /// <summary>
    /// Clear the current screen stack.. maybe when shutting down the UI?
    /// </summary>
    public void ClearScreenStack()
    {
        m_ScreenStack.Clear();
    }

    /// <summary>
    /// Transition to a screen by id.
    /// </summary>
    /// <param name="screenId"></param>
    public void TransitionToScreen(ScreenId screenId, object data = null)
    {
        StartCoroutine(DoScreenTransition(screenId, data));
    }

    /// <summary>
    /// Transition Work Enumerator.
    /// Carries out the screen loading process and locks the system until a screen has been loaded.
    /// </summary>
    /// <param name="screenId"></param>
    /// <returns></returns>
    private IEnumerator DoScreenTransition(ScreenId screenId, object data = null)
    {
        // if this is a new screen... (it should always be.)
        if (screenId != ScreenId.None)
        {
            m_InputLock = true;
            bool canNavigateBackwards = false;

            if (m_CurrentScreen != null)
            {
                canNavigateBackwards = m_CurrentScreen.CanNavigateBack;
                yield return StartCoroutine(m_CurrentScreen.DoScreenAnimation(UIScreenAnimState.Outro));
                UnloadCurrentScreen();
            }

            m_PrefabLoadingLock = true;
            UIBaseScreen loadedScreen = LoadScreen(screenId);

            if (loadedScreen != null)
            {
                while (m_PrefabLoadingLock)
                {
                    yield return null;
                }

                // If the current screen doesn't support back navigation, remove it from the stack.
                if (!canNavigateBackwards && m_ScreenStack.Count > 0)
                {
                    m_ScreenStack.Pop();
                }

                m_CurrentScreen = loadedScreen;

                // Back transitions can't add the screen twice.
                if (m_ScreenStack.Count == 0 || (m_ScreenStack.Count > 0 && screenId != m_ScreenStack.Peek()))
                {
                    // Push the new screen onto the stack.
                    m_ScreenStack.Push(screenId);
                }

                yield return StartCoroutine(m_CurrentScreen.DoScreenAnimation(UIScreenAnimState.Intro));

                m_CurrentScreen.SetData(data);
                m_CurrentScreen.SetPrompts();
                m_CurrentScreen.SetupComplete();
            }
            m_InputLock = false;
        }
    }

    /// <summary>
    /// Queue a popup.
    /// </summary>
    /// <param name="popup"></param>
    public void QueuePopup(UIPopupData popup)
    {
        m_PopupStack.Push(popup);
        if (m_CurrentPopup == null)
        {
            ShowNextPopup();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="popup"></param>
    public void ShowNextPopup()
    {
        // Close any existing popups.
        ClosePopup();

        if (m_PopupStack.Count > 0)
        {
            InputManager.Instance.SetInputType("UI");

            UIPopupData nextPopup = m_PopupStack.Pop();
            m_CurrentPopup = GameObject.Instantiate<UIBasePopup>(m_GenericPopupPrefab, m_PopupCanvas.transform);
            m_CurrentPopup.SetData(nextPopup);
            m_CurrentPopup.Initialize();

            m_PopupLayer.SetActive(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClosePopup()
    {
        if (m_CurrentPopup != null)
        {
            m_CurrentPopup.Shutdown();
            GameObject.Destroy(m_CurrentPopup.gameObject);
            m_CurrentPopup = null;

            // Show the next popop
            if (m_PopupStack.Count > 0)
            {
                ShowNextPopup();
            }
            else
            {
                InputManager.Instance.SetInputType("Default");
                m_PopupLayer.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttons"></param>
    public void SetLeftPrompts(List<InputButton> buttons)
    {
        m_PromptsLayer.SetActive(true);

        if(m_Prompts != null)
        {
            m_Prompts.SetLeftPrompts(buttons);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="buttons"></param>
    public void SetRightPrompts(List<InputButton> buttons)
    {
        m_PromptsLayer.SetActive(true);

        if (m_Prompts != null)
        {
            m_Prompts.SetRightPrompts(buttons);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ClearPrompts()
    {
        m_PromptsLayer.SetActive(false);

        if (m_Prompts != null)
        {
            m_Prompts.ClearPrompts();
        }
    }
}