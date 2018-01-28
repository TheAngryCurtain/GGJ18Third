using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoticedEvent : GameEvent
{
    public bool Noticed;

    public PlayerNoticedEvent(bool noticed)
    {
        Noticed = noticed;
    }
}

public class AIState
{
    public enum eIdleActionType { Stand, Walk };

    protected CharController mController;
    protected eIdleActionType mActionType = eIdleActionType.Stand;

    public bool mInRangeOfPlayer;

    public virtual void Enter(CharController controller)
    {
        
    }

    public virtual AIState LocalUpdate()
    {
        return null;
    }

    public virtual void Collision(Collision2D collision)
    {

    }

    public virtual void Trigger(bool enter, Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mInRangeOfPlayer = enter;

            EventManager.Instance.FireEvent(new PlayerNoticedEvent(enter));
        }
    }

    public virtual void Exit()
    {

    }
}

public class IdleState : AIState
{
    private float mMinDecideTime = 5f;
    private float mMaxDecideTime = 10f;

    private float mStartTime = 0;
    private float mCurrentDecideTime = 0;

    public override void Enter(CharController controller)
    {
        mController = controller;

        mStartTime = Time.time;
        mCurrentDecideTime = UnityEngine.Random.Range(mMinDecideTime, mMaxDecideTime);

        mController.SetVectorMovement(Vector2.zero);
        // listen for distraction-based events
    }

    public override AIState LocalUpdate()
    {
        if (mStartTime + mCurrentDecideTime < Time.time)
        {
            mStartTime = Time.time;
            mCurrentDecideTime = UnityEngine.Random.Range(mMinDecideTime, mMaxDecideTime);

            mActionType = (mActionType == eIdleActionType.Stand ? eIdleActionType.Walk : eIdleActionType.Stand);

            // if standing, just wait again
            if (mActionType == eIdleActionType.Stand)
            {
                mController.SetVectorMovement(Vector2.zero);
            }
            else
            {
                // pick random direction
                int x = UnityEngine.Random.Range(0, 2);
                int y = UnityEngine.Random.Range(0, 2);

                Vector2 move = new Vector2(x, y);

                // no diagonals
                if (move.x == 1) y = 0;
                if (move.y == 1) x = 0;

                mController.SetVectorMovement(move);
            }
        }

        if (mInRangeOfPlayer)
        {
            float playerSuspicion = Game.Instance.cSuspicionController.GetSuspicion();
            if (playerSuspicion > 25)
            {
                return new AlertState();
            }
        }

        return this;
    }

    public override void Collision(Collision2D collision)
    {
        if (mActionType == eIdleActionType.Walk)
        {
            // reverse direction
            Vector2 currentMove = mController.GetVectorMovement();
            currentMove *= -1;

            mController.SetVectorMovement(currentMove);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // remove distraction-based events
    }
}

public class AlertState : AIState
{
    private Transform mPlayerTransform;

    public override void Enter(CharController controller)
    {
        mController = controller;
        mPlayerTransform = Game.Instance.playerTransform;

        // listen for stealth-related events?
    }

    public override AIState LocalUpdate()
    {
        if (mInRangeOfPlayer)
        {
            float playerSuspicion = Game.Instance.cSuspicionController.GetSuspicion();
            Vector3 directionToPlayer = (mPlayerTransform.position - mController.transform.position).normalized;
            if (playerSuspicion > 25 && playerSuspicion < 50)
            {
                // look at the player
                Vector2 lookButSecretlyMoveDirection = new Vector2(directionToPlayer.x / 1000f, directionToPlayer.y / 1000f);
                mController.SetVectorMovement(lookButSecretlyMoveDirection);
            }
            else if (playerSuspicion > 50 && playerSuspicion < 75)
            {
                // move towards player
                Vector2 moveDirection = new Vector2(directionToPlayer.x, directionToPlayer.y);
                mController.SetVectorMovement(moveDirection);
            }
        }
        else
        {
            return new IdleState();
        }

        return this;
    }

    public override void Exit()
    {
        base.Exit();

        // remove stealth-related events
    }
}

public class AIController : MonoBehaviour
{
    [SerializeField] private CharController mCharController;

    AIState mPrevState = null;
    AIState mActiveState = new IdleState();

    private void Update()
    {
        if (mPrevState != mActiveState)
        {
            if (mPrevState != null)
            {
                // for some reason, these states base variables don't persist...
                mActiveState.mInRangeOfPlayer = mPrevState.mInRangeOfPlayer;

                mPrevState.Exit();
            }

            mActiveState.Enter(mCharController);
        }

        // debug
        if (Input.GetKeyDown(KeyCode.C))
        {
            YEO_CLOTHES_SUS_EVENT susEvent = new YEO_CLOTHES_SUS_EVENT();
            susEvent.iSuspicion = 5;

            EventManager.Instance.FireEvent(susEvent);
        }

        mPrevState = mActiveState;
        mActiveState = mActiveState.LocalUpdate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        mActiveState.Collision(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mActiveState.Trigger(true, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        mActiveState.Trigger(false, collision);
    }
}
