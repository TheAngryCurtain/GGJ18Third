using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using RewiredConsts;
using System;

public class InputManager : Singleton<InputManager>
{
    ///////////////////////////////////////////////////////////////////
    /// Constants
    ///////////////////////////////////////////////////////////////////
    public static readonly string Identifier = "InputManager";

    public static readonly int PrimaryPlayerId = 0;

    private static readonly float MAX_MOUSE_RAYCAST_DISTANCE = Mathf.Infinity;

    ///////////////////////////////////////////////////////////////////
    /// Serialized Field Member Variables
    ///////////////////////////////////////////////////////////////////


    ///////////////////////////////////////////////////////////////////
    /// Private Member Variables
    ///////////////////////////////////////////////////////////////////

    private Vector3 m_MousePosition = Vector3.zero;                     //! The current cached mouse position.
    public Vector3 MousePosition { get { return m_MousePosition; } }

    private Player m_RewiredPlayer;                                     //! Reference of the main player.
    public Player RewiredPlayer { get { return m_RewiredPlayer; } }

    private List<Action<InputActionEventData>> m_InputDelegateCache;    //! Use this to ensure we clear all delegates if the input manager is shut down before they are removed from the player.
    private int m_MouseInteractionLayerMask;                            //! Layer mask for mouse casting.

    ///////////////////////////////////////////////////////////////////
    /// Singleton MonoBehaviour Implementation
    ///////////////////////////////////////////////////////////////////

    public override void Awake()
    {
        m_RewiredPlayer = ReInput.players.GetPlayer(InputManager.PrimaryPlayerId);
        m_InputDelegateCache = new List<Action<InputActionEventData>>();

        m_MouseInteractionLayerMask = LayerMask.GetMask("Mouse Interactable");

        Debug.AssertFormat(ValidateManager() != false, "{0} : Failed to validate, please ensure that all required components are set and not null.", InputManager.Identifier);
        base.Awake();
    }

    void Start()
    {
        m_RewiredPlayer.AddInputEventDelegate(OnMouseInput, UpdateLoopType.FixedUpdate);
    }

    void Update()
    {
    }

    public override void OnDestroy()
    {
        // Handle the mouse input
        m_RewiredPlayer.RemoveInputEventDelegate(OnMouseInput);

        // When we shut down, let's take care of all this junk.
        if (m_RewiredPlayer != null && m_InputDelegateCache != null && m_InputDelegateCache.Count > 0)
        {
            for (int i = 0; i < m_InputDelegateCache.Count; ++i)
            {
                m_RewiredPlayer.RemoveInputEventDelegate(m_InputDelegateCache[i]);
            }
            m_InputDelegateCache.Clear();
        }
        else if (m_InputDelegateCache != null && m_InputDelegateCache.Count > 0)
        {
            m_InputDelegateCache.Clear();
        }

        base.OnDestroy();
    }

    /// <summary>
    /// Validate that any serialized fields are properly set.
    /// A Valid Manager should function properly.
    /// </summary>
    /// <returns></returns>
    protected override bool ValidateManager()
    {
        bool isValid = true;

        isValid = isValid && (m_RewiredPlayer != null);
        isValid = isValid && base.ValidateManager();

        return isValid;
    }

    ///////////////////////////////////////////////////////////////////
    /// InputManager Implementation
    ///////////////////////////////////////////////////////////////////


    /// <summary>
    /// Add an input event delegate to the player. Use Rewired to handle this.. so great!
    /// </summary>
    /// <param name="inputDelegate"></param>
    /// <param name="updateType"></param>
    public void AddInputEventDelegate(Action<InputActionEventData> inputDelegate, UpdateLoopType updateType)
    {
        Debug.Assert(m_RewiredPlayer != null, "Rewired Player is null, cannot add input delegate!");

        if(m_RewiredPlayer != null)
        {
            m_RewiredPlayer.AddInputEventDelegate(inputDelegate, updateType);

            m_InputDelegateCache.Add(inputDelegate);
        }
    }

    /// <summary>
    /// Get a 2D Axis.
    /// </summary>
    /// <param name="xAxis"></param>
    /// <param name="yAxis"></param>
    /// <returns></returns>
    public Vector2 GetAxis2D(int xAxis, int yAxis)
    {
        return m_RewiredPlayer.GetAxis2D(xAxis, yAxis);
    }

    /// <summary>
    /// Remove an input event delegate from the player. Use Rewired to handle this.. even better!
    /// </summary>
    /// <param name="inputDelegate"></param>
    public void RemoveInputEventDelegate(Action<InputActionEventData> inputDelegate)
    {
        Debug.Assert(m_RewiredPlayer != null && m_InputDelegateCache != null, "Rewired Player is null, or input cache is null cannot add input delegate!");

        if (m_RewiredPlayer != null && m_InputDelegateCache != null)
        {
            m_RewiredPlayer.RemoveInputEventDelegate(inputDelegate);

            bool didRemove = m_InputDelegateCache.Remove(inputDelegate);

            Debug.Assert(didRemove == true, "Attempted to remove delegate from the input cache but it was not found. This is odd, investigate.");
        }
    }

    public void SetInputType(string type)
    {
        ControllerMap cUIMap = m_RewiredPlayer.controllers.maps.GetMap(ControllerType.Joystick, 0, "Default", "UI");
        ControllerMap cDMap = m_RewiredPlayer.controllers.maps.GetMap(ControllerType.Joystick, 0, "Default", "Default");
        if (cUIMap != null && cDMap != null)
        {
        }
        else
        {
            cUIMap = m_RewiredPlayer.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "UI");
            cDMap = m_RewiredPlayer.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "Default");
        }

        switch (type)
        {
            case "UI":
                cUIMap.enabled = true;
                cDMap.enabled = false;
                break;
            case "Default":
                cUIMap.enabled = false;
                cDMap.enabled = true;
                break;
        }
    }

    /// <summary>
    /// Callback for Mouse Input
    /// </summary>
    /// <param name="data"></param>
    private void OnMouseInput(InputActionEventData data)
    {
     /*   switch(data.actionId)
        {
            case RewiredConsts.Action.PRIMARY_ACTION:
            case RewiredConsts.Action.SECONDARY_ACTION:
                if (data.GetButtonDown())
                {
                    HandleMouseInput(data.actionId);
                }
                break;
        }*/
    }

    /// <summary>
    /// Handle casting from screen and telling the gameobject that an interaction has occured.
    /// </summary>
    /// <param name="actionId"></param>
    private void HandleMouseInput(int actionId)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, MAX_MOUSE_RAYCAST_DISTANCE, m_MouseInteractionLayerMask))
        {
            if(hit.collider.gameObject != null)
            {
                MouseInteractable interactComp = hit.collider.gameObject.GetComponent<MouseInteractable>();

                if(interactComp != null)
                {
                    interactComp.OnMouseInteraction(actionId);
                }
            }
        }
    }
}