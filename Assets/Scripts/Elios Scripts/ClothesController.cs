using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterCreationData : GameEvent
{
    public Dictionary<int, int> lID = new Dictionary<int, int>();
    public Dictionary<int, Color> lColor = new Dictionary<int, Color>();
}

public class YEO_CLOTHES_SWAP_EVENT : GameEvent
{
    public int iType;
    public int iID;

    public Color iColor;
}

public class ClothesController : MonoBehaviour
{
    private Dictionary<int, List<List<Sprite>>> dictTypeList; //All Clothing
    private Dictionary<int, int> dictClothing = new Dictionary<int, int>(); //Currently Worn Clothing values
    private Dictionary<int, Color> dictClothingColor = new Dictionary<int, Color>(); //Currently Worn Clothing values
    private Dictionary<int, SpriteRenderer> dictPlayerSprites = new Dictionary<int, SpriteRenderer>(); //Current Sprites

    private PlayerSpritesHolder ClothingGameObjects;
    private ClothesHolder cClothesHolderReference;

    private int iCurrDirection = 2;
    private Color colItemColor;

    // Use this for initialization
    void Start()
    {

    }

    public void Init()
    { 
        //Get References
        cClothesHolderReference = Game.Instance.cClothes;
        ClothingGameObjects = this.GetComponent<PlayerSpritesHolder>();
        if (ClothingGameObjects == null)
        {
            Debug.Log("***WARNING*** ClothesHolder: Start: ClothingGameObjects is null and it shouldnt be...");
        }

        //Get Data
        //Debug.Log("ClothesController: Start: cClothesHolderReference.dictTypeList: " + cClothesHolderReference.dictTypeList.Count);
        dictTypeList = cClothesHolderReference.dictTypeList;

        //Set Instance Data
        dictPlayerSprites[ClothesHolder.HATS_INDEX] = ClothingGameObjects.HAT_GAMEOBJECT.GetComponent<SpriteRenderer>();

        dictPlayerSprites[ClothesHolder.TOPS_INDEX] = ClothingGameObjects.TOP_GAMEOBJECT.GetComponent<SpriteRenderer>();

        dictPlayerSprites[ClothesHolder.BOTTOMS_INDEX] = ClothingGameObjects.BOTTOM_GAMEOBJECT.GetComponent<SpriteRenderer>();

        dictPlayerSprites[ClothesHolder.SKIN_INDEX] = ClothingGameObjects.SKIN_GAMEOBJECT.GetComponent<SpriteRenderer>();
    }

    //Change clothes if we dont have it and its valid
    public void SetClothes(int iType, int iID, Color iColor)
    {
        if (dictTypeList[iType].Count > iID)
        {
            if (iType != ClothesHolder.SKIN_INDEX)
            {
                dictPlayerSprites[iType].color = iColor; //Change color of item
            }

            dictClothing[iType] = iID;
            dictClothingColor[iType] = iColor;
            //UpdateSpriteFacing(iCurrDirection);
        }
        else
        {
            //Make naked
        }
    }

    public void SetClothes(CharacterCreationData eventData)
    {
        SetClothes(ClothesHolder.HATS_INDEX, eventData.lID[ClothesHolder.HATS_INDEX], eventData.lColor[ClothesHolder.HATS_INDEX]);
        SetClothes(ClothesHolder.TOPS_INDEX, eventData.lID[ClothesHolder.TOPS_INDEX], eventData.lColor[ClothesHolder.TOPS_INDEX]);
        SetClothes(ClothesHolder.BOTTOMS_INDEX, eventData.lID[ClothesHolder.BOTTOMS_INDEX], eventData.lColor[ClothesHolder.BOTTOMS_INDEX]);

        Redraw();
    }

    public YEO_CLOTHES_SWAP_EVENT GetClothes(int iType)
    {
        if (dictClothing[iType] < 0 || dictClothing[iType] > dictTypeList[iType].Count)
        {
            Debug.Log("***WARNING*** ClothesController: GetClothes: Clothes of type: " + iType + " is invalid: " + dictClothing[iType]);
        }

        YEO_CLOTHES_SWAP_EVENT lData = new YEO_CLOTHES_SWAP_EVENT();
        lData.iType = iType;
        lData.iID = dictClothing[iType];
        lData.iColor = dictClothingColor[iType];

        return lData;
    }

    public void Redraw()
    {
        UpdateSpriteFacing(iCurrDirection, true);
    }

    public void UpdateSpriteFacing(int iDirection, bool bForceRedraw = false)
    {
        //0 up, 1, left, 2 down, 3 right
        //do stuff here to make the hates switch to the left/right/up/down sprites appropriately
        if ((!bForceRedraw || iCurrDirection != iDirection) && iDirection == -1) { return; }
        iCurrDirection = iDirection;
        
        if(dictClothing[ClothesHolder.HATS_INDEX] >= 0)
        {
            dictPlayerSprites[ClothesHolder.HATS_INDEX].sprite = dictTypeList[ClothesHolder.HATS_INDEX][dictClothing[ClothesHolder.HATS_INDEX]][iDirection];
        }

        if (dictClothing[ClothesHolder.TOPS_INDEX] >= 0)
        {
            dictPlayerSprites[ClothesHolder.TOPS_INDEX].sprite = dictTypeList[ClothesHolder.TOPS_INDEX][dictClothing[ClothesHolder.TOPS_INDEX]][iDirection];
        }

        if (dictClothing[ClothesHolder.BOTTOMS_INDEX] >= 0)
        {
            dictPlayerSprites[ClothesHolder.BOTTOMS_INDEX].sprite = dictTypeList[ClothesHolder.BOTTOMS_INDEX][dictClothing[ClothesHolder.BOTTOMS_INDEX]][iDirection];
        }

        //Nonswappable
        if(dictClothing[ClothesHolder.SKIN_INDEX] >= 0)
        {
            dictPlayerSprites[ClothesHolder.SKIN_INDEX].sprite = dictTypeList[ClothesHolder.SKIN_INDEX][dictClothing[ClothesHolder.SKIN_INDEX]][iDirection];
        }
    }
}