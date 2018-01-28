using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum eGameState
{
    Boot,
    FrontEnd,
    GamePlay,
}

public class Game : Singleton<Game>
{
    public const int MAX_NPC = 10;

    public GameObject playerPrefab;
    public GameObject NPCPrefab;
    public GameObject goTargetManager;
    public GameObject goSpawns;

    public List<GameObject> npcs = new List<GameObject>();

    public ClothesHolder cClothes;
    public SuspicionController cSuspicionController;
    public TargetManager cTargetManager;
    public Transform playerTransform;
    public SpawnPointHolder cSpawns;
        
    public eGameState CurrentState;

    public override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start ()
    {
        LoadGameState(eGameState.FrontEnd);
        //load in mikes level
        //Application.LoadLevelAdditive("Elios Town Test");
    }

    public void StartGame()
    {
        cClothes = this.gameObject.GetComponent<ClothesHolder>();
        if (cClothes == null)
        {
            Debug.Log("Game: Start: ClothesHolder is not attached! HELLO?");
        }
        cClothes.Init();

        cSuspicionController = this.gameObject.GetComponent<SuspicionController>();
        if (cSuspicionController == null)
        {
            cSuspicionController = this.gameObject.AddComponent<SuspicionController>();
        }
        cSuspicionController.Init();

        cTargetManager = goTargetManager.GetComponent<TargetManager>();
        if (cTargetManager == null)
        {
            Debug.Log("***WARNING*** cTargetManager NULL");
        }
        cTargetManager.Init();

        goSpawns = GameObject.Find("Spawn Point Container");
        cSpawns = goSpawns.GetComponent<SpawnPointHolder>();
        if(cSpawns == null)
        {
            Debug.Log("***WARNING*** CSpawns null");
        }
        cSpawns.Reset();

        //Characters
        GameObject player = Instantiate(playerPrefab);
        playerTransform = player.transform;
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetClothes(cTargetManager.AgentData);
        }

        npcs.Add(Instantiate(NPCPrefab));
        npcs[npcs.Count - 1].gameObject.transform.position = cSpawns.GetSpawnPoint().position;
        NPCController cNPCController = npcs[npcs.Count - 1].GetComponent<NPCController>();
        if (cNPCController != null)
        {
            cNPCController.SetClothes(cTargetManager.TargetData);
        }

        CharacterCreationData npcData;
        for (int i = 0; i < MAX_NPC; ++i)
        {
            npcs.Add(Instantiate(NPCPrefab));
            npcs[npcs.Count - 1].gameObject.transform.position = cSpawns.GetSpawnPoint().position;
            cNPCController = npcs[npcs.Count - 1].GetComponent<NPCController>();

            if (cNPCController != null)
            {
                npcData = new CharacterCreationData();
                npcData.lID[ClothesHolder.HATS_INDEX] = 0;
                npcData.lColor[ClothesHolder.HATS_INDEX] = cTargetManager.iHatID == i ? cTargetManager.TargetData.lColor[ClothesHolder.HATS_INDEX] : new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                npcData.lID[ClothesHolder.TOPS_INDEX] = 0;
                npcData.lColor[ClothesHolder.TOPS_INDEX] = cTargetManager.iTopID == i ? cTargetManager.TargetData.lColor[ClothesHolder.TOPS_INDEX] : new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                npcData.lID[ClothesHolder.BOTTOMS_INDEX] = 0;
                npcData.lColor[ClothesHolder.BOTTOMS_INDEX] = cTargetManager.iBottomID == i ? cTargetManager.TargetData.lColor[ClothesHolder.BOTTOMS_INDEX] : new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                cNPCController.SetClothes(npcData);
            }
        }

        GameObject.Find("FollowCamera").GetComponent<ShittyFollowCam>().StartCamera();
    }

    public void LoadGameState(eGameState state)
    {
        if(state != CurrentState)
        {
            CurrentState = state;

            switch (state)
            {
                case eGameState.Boot:
                    SceneManager.LoadScene("FrontEnd", LoadSceneMode.Additive);
                    break;
                case eGameState.FrontEnd:
                    SceneManager.LoadScene("FrontEnd", LoadSceneMode.Additive);
                    break;
                case eGameState.GamePlay:
                    SceneManager.UnloadSceneAsync("FrontEnd");
                    SceneManager.LoadScene("BackEnd", LoadSceneMode.Additive);
                    break;
            }
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
