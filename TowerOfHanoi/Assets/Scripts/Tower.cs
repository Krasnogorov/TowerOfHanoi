using System.Collections.Generic;
using UnityEngine;

/**
 * Class described tower
 */
public class Tower : MonoBehaviour
{
    // List of disks on this tower
    private List<Disk> mDiskList = new List<Disk>();
    // Empty game object on the scene which helps to disks in moves
    public GameObject movingHelper;
    // Prefab of disk
    public GameObject diskPrefab;
    /**
     * Initialize tower with count of disks.
     * Instantiate disk object and setup correct information
     */
    public void InitWithCount(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject diskObj = GameObject.Instantiate<GameObject>(diskPrefab);
            Disk disk = diskObj.GetComponent<Disk>();
            disk.InitWithSize(count - i);
            SetDiskToTop(diskObj);
            mDiskList.Add(disk);
        }
    }
    /**
     * Setup disk position at top position of tower
     */
    private void SetDiskToTop(GameObject diskObj)
    {
        diskObj.transform.position = GetTopPosition();
    }
    /**
     * Start moving procedure for top disk
     */
    public void MoveRingTo(Tower newTower)
    {
        mDiskList[mDiskList.Count - 1].MoveFromTo(this, newTower);
        mDiskList.RemoveAt(mDiskList.Count - 1);
    }
    /**
     * Update list of disks in tower
     */
    public void FinishMovingDisk(Disk newDisk)
    {
        mDiskList.Add(newDisk);
        // TODO: Notify simulator - step finished
    }
    /**
     * Get position for top disk
     */
    public Vector3 GetTopPosition()
    {
        return new Vector3(transform.position.x, -3 + mDiskList.Count, transform.position.z);
    }
}
