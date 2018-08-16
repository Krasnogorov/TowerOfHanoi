using System;
using UnityEngine;

/**
 * Class which implemented moving of game object from first position to last.
 * It uses something like FSM with different states to simplify validating of next step.
 * Moving is next way : top position on start tower -> movingHelper on startTower ->
 * movingHelper on finishTower -> top position on finish tower.
 */
public class DiskMovingController : MonoBehaviour
{
    // Enum described list of different states
    public enum MovingState
    {
        StateStay,
        StateMoveUp,
        StateMoveTo,
        StateMoveDown
    };
    // Current state of FSM
    private MovingState mCurrentState = MovingState.StateStay;
    // Next position of moving
    private Vector3 mNextPosition;
    /**
     * Save required position, change current state. 
     */
    public void StartMovingTo(Vector3 position)
    {
        mCurrentState = MovingState.StateMoveUp;
        mNextPosition = position;
    }
    /**
     * Update method.
     * I turn off and on this script to minimize count of unused calls.
     * Move gameObject with some value and validate is we received too small distance.
     * And change currentState
     */
    public void FixedUpdate()
    {
        float step = 10 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, mNextPosition, step);
        if (Math.Abs(transform.position.x - mNextPosition.x) < float.Epsilon &&
            Math.Abs(transform.position.y - mNextPosition.y) < float.Epsilon &&
            Math.Abs(transform.position.z - mNextPosition.z) < float.Epsilon )
        {
            switch (mCurrentState)
            {
                case MovingState.StateMoveUp:
                    mNextPosition = GetComponent<Disk>().GetSecondPosition();
                    mCurrentState = MovingState.StateMoveTo;
                    break;
                case MovingState.StateMoveTo:
                    mNextPosition = GetComponent<Disk>().GetFinishPosition();
                    mCurrentState = MovingState.StateMoveDown;
                    break;
                case MovingState.StateMoveDown:
                    GetComponent<Disk>().FinishMoving();
                    //Disable component to avoid call update functions
                    enabled = false;
                    break;
                default:
                    break;
            }
        }
    }
}
