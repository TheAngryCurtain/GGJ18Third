using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    //Clothes Swap
    protected ClothesController clothesController;
    public Collider2D colInteractive;

    protected const string NPC_TAG = "NPC";

    //Movement
    public float fWalkSpeed = 2;
    public float fRunSpeed = 3;
    protected bool bRunning = false;
    public int iDirection = 0;

    protected int iMaxItemsATM = 3;

    protected Rigidbody2D rigidbody2D;

    protected Vector2 vecInput = new Vector2(0, 0);
    protected Vector2 vecMovement = new Vector2(0, 0);

    protected List<NPCController> interactables = new List<NPCController>();

    protected Animator animator;

    // Use this for initialization
    void Start ()
    {
		
	}

    public void Init()
    {
        rigidbody2D = this.gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            rigidbody2D = this.gameObject.AddComponent<Rigidbody2D>();
        }

        clothesController = this.gameObject.GetComponent<ClothesController>();
        if (clothesController == null)
        {
            clothesController = this.gameObject.AddComponent<ClothesController>();
        }

        clothesController.Init();

        //NOT CHANGEABLE
        clothesController.SetClothes(ClothesHolder.SKIN_INDEX, Random.Range(0, Game.Instance.cClothes.dictTypeList[ClothesHolder.SKIN_INDEX].Count) % Game.Instance.cClothes.dictTypeList[ClothesHolder.SKIN_INDEX].Count,  Color.red);

        animator = this.gameObject.GetComponent<Animator>();
    }

    public void SetClothes(CharacterCreationData eventData)
    {

        clothesController.SetClothes(eventData);
    }

    // Update is called once per frame
    void Update ()
    {
		
	}

    private void FixedUpdate()
    {
        rigidbody2D.velocity = vecMovement;

        int iNewDirection = -1;
        if (vecMovement.y > 0)
        {
            iNewDirection = 0;
        }
        else if (vecMovement.y < 0)
        {
            iNewDirection = 2;
        }
        else if (vecMovement.x < 0)
        {
            iNewDirection = 1;
        }
        else if (vecMovement.x > 0)
        {
            iNewDirection = 3;
        }

        if (iNewDirection != iDirection && iNewDirection > -1)
        {
            iDirection = iNewDirection;

            if (clothesController)
            {
                clothesController.UpdateSpriteFacing(iDirection);
            }

        }

        if(animator != null)
        {
            animator.SetBool("bIsWalking", vecMovement != Vector2.zero);
        }
    }

    public void SetVectorMovement(Vector2 move)
    {
        vecMovement = move;
    }

    public Vector2 GetVectorMovement()
    {
        return vecMovement;
    }
}