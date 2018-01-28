using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ClothesHolder : MonoBehaviour
{
    public static ClothesHolder mainClothesHolder;

    public const int HATS_INDEX = 0;
    public const int TOPS_INDEX = 1;
    public const int BOTTOMS_INDEX = 2;
    public const int SKIN_INDEX = 3;
    public const int COLOR_INDEX = 4;

    public List<List<Sprite>> HATS_LIST = new List<List<Sprite>>();
    public List<List<Sprite>> TOPS_LIST = new List<List<Sprite>>();
    public List<List<Sprite>> BOTTOMS_LIST = new List<List<Sprite>>();

    public GameObject HATS_GAMEOBJECT;
    public GameObject TOPS_GAMEOBJECT;
    public GameObject BOTTOMS_GAMEOBJECT;

    public List<Color> COLOR_LIST = new List<Color>();

    //NOT SWAPPABLE
    public GameObject SKIN_GAMEOBJECT;
    public List<List<Sprite>> SKIN_LIST = new List<List<Sprite>>();

    public Dictionary<int, List<List<Sprite>>> dictTypeList = new Dictionary<int, List<List<Sprite>>>(); //All Clothing

    void Awake()
    {
        mainClothesHolder = this.gameObject.GetComponent<ClothesHolder>();
        if (mainClothesHolder == null)//Make main gameObject
        {
            mainClothesHolder = this.gameObject.AddComponent<ClothesHolder>();
        }
        else if (this != mainClothesHolder)
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        
    }

    public void Init()
    {
        int iIter;
        //HATS
        HolderHolder[] HatHolders = HATS_GAMEOBJECT.GetComponents<HolderHolder>();
        for (iIter = 0; iIter < HatHolders.Length; ++iIter)
        {
            HATS_LIST.Add(HatHolders[iIter].listDirections);
        }

        //TOPS
        HolderHolder[] TopHolders = TOPS_GAMEOBJECT.GetComponents<HolderHolder>();
        for (iIter = 0; iIter < TopHolders.Length; ++iIter)
        {
            TOPS_LIST.Add(TopHolders[iIter].listDirections);
        }

        //BOTTOMS
        HolderHolder[] BottomHolders = BOTTOMS_GAMEOBJECT.GetComponents<HolderHolder>();
        for (iIter = 0; iIter < BottomHolders.Length; ++iIter)
        {
            BOTTOMS_LIST.Add(BottomHolders[iIter].listDirections);
        }

        //COLORS
        COLOR_LIST.Add(Color.red);
        COLOR_LIST.Add(Color.blue);
        COLOR_LIST.Add(Color.green);

        //SKIN
        HolderHolder[] SkinHolders = SKIN_GAMEOBJECT.GetComponents<HolderHolder>();
        for (iIter = 0; iIter < SkinHolders.Length; ++iIter)
        {
            SKIN_LIST.Add(SkinHolders[iIter].listDirections);
        }

        //Make assets more accessible
        dictTypeList[HATS_INDEX] = HATS_LIST; //Get Hat Data
        dictTypeList[TOPS_INDEX] = TOPS_LIST; //Get Top Data
        dictTypeList[BOTTOMS_INDEX] = BOTTOMS_LIST; //Get Bottoms Data
        dictTypeList[SKIN_INDEX] = SKIN_LIST; //Get Skin Data
    }
}