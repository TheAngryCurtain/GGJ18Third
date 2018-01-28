using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YEO_TALK_EVENT : GameEvent
{
    public string message;
}

public class YEO_GET_CLUE_EVENT : GameEvent
{
    public string message;
}

public class PlayerController : CharController
{
    [SerializeField] private float mSusDecayRate = 2f;
    [SerializeField] private int mDecayAmount = -1;

    private float mCurrentTime = 0;
    private bool mNoticed = false;

    //Event data
    YEO_CLOTHES_SUS_EVENT data = new YEO_CLOTHES_SUS_EVENT();
    YEO_CLOTHES_SUS_EVENT susEvent = new YEO_CLOTHES_SUS_EVENT();
    
    public void Init()
    {
        data.iSuspicion = 30;
        EventManager.Instance.FireEvent(data);

        susEvent.iSuspicion = mDecayAmount;

        base.Init();

        InputManager.Instance.AddInputEventDelegate(OnInputUpdate, UpdateLoopType.Update);

        //EventManager.Instance.AddEventListener<ClothesController.YEO_CLOTHES_SWAP_EVENT>(clothesController.SetClothes);
        EventManager.Instance.AddEventListener<PlayerNoticedEvent>(OnPlayerNoticed);
    }

    public void SetClothes(CharacterCreationData eventData)
    {
        Init();

        base.SetClothes(eventData);
    }

    private void OnPlayerNoticed(PlayerNoticedEvent e)
    {
        mNoticed = e.Noticed;
    }

    protected virtual void OnInputUpdate(InputActionEventData iData)
    {
        if (!iData.GetButtonUp()) return;

        int iClothesType = -1;
        data.iSuspicion = 0;
        if (iData.actionId == RewiredConsts.Action.HatSwap
            || iData.actionId == RewiredConsts.Action.BodySwap
            || iData.actionId == RewiredConsts.Action.LegSwap
            )
        {

            foreach (NPCController interactable in interactables)
            {
                if (interactable.IsPlayerBehindMe(this))
                {
                    if (iData.actionId == RewiredConsts.Action.HatSwap)
                    {
                        iClothesType = ClothesHolder.HATS_INDEX;
                    }
                    else if (iData.actionId == RewiredConsts.Action.BodySwap)
                    {
                        iClothesType = ClothesHolder.TOPS_INDEX;
                    }
                    else
                    {
                        iClothesType = ClothesHolder.BOTTOMS_INDEX;
                    }

                    data.iSuspicion = 1;
                }

                if (iClothesType > -1)
                {
                    YEO_CLOTHES_SWAP_EVENT lNPCData;
                    YEO_CLOTHES_SWAP_EVENT lOurData;

                    ClothesController clothesNPC = interactable.gameObject.GetComponent<ClothesController>();
                    if (clothesNPC != null)
                    {
                        lNPCData = clothesNPC.GetClothes(iClothesType);
                        lOurData = clothesController.GetClothes(iClothesType);

                        clothesNPC.SetClothes(iClothesType, lOurData.iID, lOurData.iColor); //Set NPC Clothing

                        clothesController.SetClothes(iClothesType, lNPCData.iID, lNPCData.iColor); //Set ours
                    }

                    //Suspicion
                    if (data.iSuspicion != 0)
                    {
                        EventManager.Instance.FireEvent(data);
                    }
                }
            }

        }
        else if (iData.actionId == RewiredConsts.Action.SELECT)
        {
            data.iSuspicion++;
            foreach (NPCController interactable in interactables)
            {
                interactable.HandleInteraction(data.iSuspicion);
            }
        }

        if (data.iSuspicion != 0)
        {
            EventManager.Instance.FireEvent(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //vecInput.x = Input.GetAxisRaw("Horizontal");
        //vecInput.y = Input.GetAxisRaw("Vertical");
        vecInput = InputManager.Instance.GetAxis2D(RewiredConsts.Action.LEFTSTICK_HORIZONTAL, RewiredConsts.Action.LEFTSTICK_VERTICAL);
        bRunning = Input.GetKey(KeyCode.LeftShift);

        float fSpeed = bRunning ? fRunSpeed : fWalkSpeed;
        vecMovement = vecInput * fSpeed;

        // slowly lower suspicion
        if (!mNoticed)
        {
            if (Time.time > mCurrentTime + mSusDecayRate)
            {
                mCurrentTime = Time.time;
                EventManager.Instance.FireEvent(susEvent);
            }
        }
        else
        {
            mCurrentTime = Time.time;
        }

        //data.iSuspicion = 0;

        ////TEST
        ////Debug.Log("ELIO: PlayerController: Update: colInteractive: " + colInteractive.gameObject.tag);
        //foreach (NPCController interactable in interactables)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        UIManager.Instance.ClosePopup();
        //    }
            
        //    int iClothesType = -1;
        //    if (Input.GetKeyDown(KeyCode.I))
        //    {
        //        iClothesType = ClothesHolder.HATS_INDEX;
                
        //        data.iSuspicion = 1;
        //    }
        //    else if (Input.GetKeyDown(KeyCode.O))
        //    {
        //        iClothesType = ClothesHolder.TOPS_INDEX;
        //        data.iSuspicion = 1;
        //    }
        //    else if (Input.GetKeyDown(KeyCode.P))
        //    {
        //        iClothesType = ClothesHolder.BOTTOMS_INDEX;
        //        data.iSuspicion = 1;
        //    }
        //    else if(Input.GetKeyDown(KeyCode.L))
        //    {
        //        data.iSuspicion++;
                
        //        Debug.Log("[cew]pressed a on this guy:" + interactable.gameObject.name);
        //        interactable.HandleInteraction(data.iSuspicion);
        //    }


        //    if (iClothesType > -1)
        //    {
        //        List<int> lNPCData;
        //        List<int> lOurData;
        //        int iNPCItem = -1;

        //        ClothesController clothesNPC = interactable.gameObject.GetComponent<ClothesController>();
        //        if(clothesNPC != null)
        //        {
        //            lNPCData = clothesNPC.GetClothes(iClothesType);
        //            lOurData = clothesController.GetClothes(iClothesType);
                    
        //            clothesNPC.SetClothes(iClothesType, lOurData[0], lOurData[1]); //Set NPC Clothing
                    
        //            clothesController.SetClothes(iClothesType, lNPCData[0], lNPCData[1]); //Set ours
        //        }
        //    }

        //    //Suspicion
        //    if (data.iSuspicion != 0)
        //    {
        //        EventManager.Instance.FireEvent(data);
        //    }
        //}


    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        interactables.Add(coll.GetComponent<NPCController>());
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        interactables.Remove(coll.GetComponent<NPCController>());
    }

    public void Awake()
    {
       // EventManager.Instance.AddEventListener<YEO_TALK_EVENT>(OnSpeechBubble);
    }

    public void OnDestroy()
    {
        InputManager.Instance.RemoveInputEventDelegate(OnInputUpdate);
        // EventManager.Instance.RemoveEventListener<YEO_TALK_EVENT>(OnSpeechBubble);
    }

    private void OnSpeechBubble(YEO_TALK_EVENT eventData)
    {
        Debug.Log(eventData.message);
    }
}