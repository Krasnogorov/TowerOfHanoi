using UnityEngine;

/**
 * Implements disk on the scene
 */
public class Disk : MonoBehaviour
{
    // Size of the disk
    private int mSize = 1;
    // Start tower for moving
    private Tower mStartTower;
    // Finish tower for moving
    private Tower mFinishTower;
    // Controller of moving on the scene
    [SerializeField]
    private DiskMovingController movingController;
    // getter for size variable
    public int size
    {
        get
        {
            return mSize;
        }
    }
    /**
     * Initialize element with selected size.
     * Update scale of game object.
     */
    public void InitWithSize(int size)
    {
        mSize = size;
        transform.localScale = new Vector3(size, 1, size);
    }
    /**
     * Starts moving disk from startTower to finishTower
     */
    public void MoveFromTo(Tower startTower, Tower finishTower)
    {
        mStartTower = startTower;
        mFinishTower = finishTower;
        movingController.enabled = true;
        movingController.StartMovingTo(mStartTower.movingHelper.transform.position);
    }
    /**
     * Get position on top of finish tower
     */
    public Vector3 GetSecondPosition()
    {
        return mFinishTower.movingHelper.transform.position;
    }
    /** 
     * Get position on top of disks inside pyramid
     */
    public Vector3 GetFinishPosition()
    {
        return mFinishTower.GetTopPosition();
    }
    /**
     * Notification from movingController about moving is finished
     */
    public void FinishMoving()
    {
        mFinishTower.FinishMovingDisk(this);
    }
}
