using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : CharController
{
    private TalkyComponent Talkies;

    public enum INTERACTION_TYPE
    {
        TALK,
        SMASH,
        ACTIVATE,
        GET_CLUE
    }

    public INTERACTION_TYPE interactionType;
    
    public void Init()
    {
        base.Init();

        Talkies = this.gameObject.GetComponent<TalkyComponent>();
    }

    public void SetClothes(CharacterCreationData eventData)
    {
        Init();

        base.SetClothes(eventData);
    }

    public void HandleInteraction(int iThreatLevel)
    {
        switch (interactionType)
        {
            case INTERACTION_TYPE.SMASH:
                GameObject.DestroyImmediate(this.gameObject);
                break;
            case INTERACTION_TYPE.TALK:
                {
                    if (Talkies)
                    {
                        if (iThreatLevel >= Talkies.DialogList.Count)
                        {
                            iThreatLevel = 0;
                        }

                        EventManager.Instance.FireEvent(new YEO_TALK_EVENT()
                        {
                            message = Talkies.DialogList[iThreatLevel]
                        });
                    }
                }
                    break;
            case INTERACTION_TYPE.GET_CLUE:
                Debug.Log("[cew] INTERACTION_TYPE.ACTIVATE");
                if (Talkies)
                {
                    EventManager.Instance.FireEvent(new YEO_GET_CLUE_EVENT()
                    {
                        message = Talkies.Clue
                    }); 
                }
                break;
            case INTERACTION_TYPE.ACTIVATE: break;
            default: break;
        }
    }


    // Update is called once per frame
    void Update ()
    {
		
	}

    public bool IsPlayerBehindMe(PlayerController myPlayer)
    {
        bool bIsBehind = false;
        if (myPlayer.GetComponent<CharController>().iDirection != iDirection) { return false; }


        switch (iDirection)
        {
            case 0: //up
                {
                    bIsBehind = (myPlayer.transform.localPosition.y < transform.localPosition.y);
                    break;
                }
            case 1: //left
                {
                    bIsBehind = (myPlayer.transform.localPosition.x > transform.localPosition.x);
                    break;
                }
            case 2: //down
                {
                    bIsBehind = (myPlayer.transform.localPosition.y > transform.localPosition.y);
                    break;
                }
            case 3: //right
                {
                    bIsBehind = (myPlayer.transform.localPosition.x < transform.localPosition.x);
                    break;
                }
            default: break;
        }

        Debug.Log("[cew]IsPlayerBehindMe=" + bIsBehind);

        return bIsBehind;
    }

    public bool AmIInfrontOfPlayer(PlayerController myPlayer)
    {
        bool bIsInfront = false;
        switch (myPlayer.iDirection)
        {
            case 0: //up
                {
                    bIsInfront = (myPlayer.transform.localPosition.y < transform.localPosition.y);
                    break;
                }
            case 1: //left
                {
                    bIsInfront = (myPlayer.transform.localPosition.x > transform.localPosition.x);
                    break;
                }
            case 2: //down
                {
                    bIsInfront = (myPlayer.transform.localPosition.y > transform.localPosition.y);
                    break;
                }
            case 3: //right
                {
                    bIsInfront = (myPlayer.transform.localPosition.x < transform.localPosition.x);
                    break;
                }
            default: break;
        }

        return bIsInfront;
    }
}
