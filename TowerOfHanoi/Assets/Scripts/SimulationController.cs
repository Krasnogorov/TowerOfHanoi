using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Main class of simulation. It calculates all changes required to task, moving and update of status
 */
public class SimulationController : MonoBehaviour
{
    /** 
     * Struct for saving required changes
     */
    struct State
    {
        public int n;
        public int src;
        public int dest;
        public int tmp;
        public int step;
    }
    // List of all tower
    [SerializeField]
    private Tower[] towerList;
    // Count of disk
    private int mMaxDiskCount = 0;
    // List with all changes step-by-step
    private List<State> mStateList = new List<State>();
    // Index of current step
    private int mCurrentStep = 0;
    // Notification for UI about update index of current step
    public Action<int, int> OnStepChanged;
    // Notification for UI about update state of simulation
    public Action OnSimulationFinished;
    /**
     * We should subscribe to notifications on start of scene
     */
    public void Start()
    {
        foreach (Tower tower in towerList)
        {
            tower.OnStepFinished += StepFinished;
        }
    }
    /**
     * We should unsubscribe from notifications
     */
    public void OnDestroy()
    {
        foreach (Tower tower in towerList)
        {
            tower.OnStepFinished += StepFinished;
        }
    }
    /**
     * Removes all disks on scene.
     */
    public void Cleanup()
    {
        foreach (Tower tower in towerList)
        {
            tower.Cleanup();
        }
        Disk[] diskList = GameObject.FindObjectsOfType<Disk>();
        foreach (Disk disk in diskList)
        {
            DestroyImmediate(disk.gameObject);
        }
        diskList = null;
    }
    /**
     * Initialise start values and calculate list of changes
     */
    public void InitWithCount(int count)
    {
        towerList[0].InitWithCount(count);
        mMaxDiskCount = count;
        mCurrentStep = 0;
        mStateList.Clear();
        MoveStack(mMaxDiskCount, 0, 1, 2);
    }
    /**
     * Start simulation and notification to UI
     */
    public void StartSimulation()
    {
        State state = mStateList[mCurrentStep];
        towerList[state.src].MoveRingTo(towerList[state.dest]);
        OnStepChanged(mCurrentStep, mStateList.Count);
        mCurrentStep++;
    }
    /**
     * Stop current simulation and cleanup all variables
     */
    public void StopSimulation()
    {
        Cleanup();
    }
    /**
     * Callback for finish current step. We should try to choose next state or finish simulation
     */
    public void StepFinished()
    {
        OnStepChanged(mCurrentStep, mStateList.Count);
        if (mCurrentStep <= mStateList.Count - 1)
        {
            State state = mStateList[mCurrentStep];
            towerList[state.src].MoveRingTo(towerList[state.dest]);
            mCurrentStep++;            
        }
        else
        {
            SimulationFinished();
        }
    }
    /**
     * Notify that simulation was finished
     */
    public void SimulationFinished()
    {
        OnSimulationFinished();
    }
    /**
     * Calculate list of states which should be implemented for resolve puzzle.
     */
    private void MoveStack(int n, int src, int dest, int tmp)
    {
        List<State> stack = new List<State>();
        State firstState;
        firstState.n = n;
        firstState.src = src;
        firstState.dest = dest;
        firstState.tmp = tmp;
        firstState.step = 0;
        stack.Add(firstState);
        while (stack.Count > 0)
        {
            State state = stack[stack.Count - 1];
            switch (state.step)
            {
                case 0:
                    if (state.n == 0)
                    {
                        stack.RemoveAt(stack.Count - 1);
                    }
                    else
                    {
                        State tmpSt = stack[stack.Count - 1];
                        tmpSt.step += 1;
                        stack[stack.Count - 1] = tmpSt;

                        State newState;
                        newState.n = state.n-1;
                        newState.src = state.src;
                        newState.dest = state.tmp;
                        newState.tmp = state.dest;
                        newState.step = 0;
                        stack.Add(newState);
                    }
                    break;
                case 1:
                    {

                        mStateList.Add(state);
                        State tmpSt = stack[stack.Count - 1];
                        tmpSt.step += 1;
                        stack[stack.Count - 1] = tmpSt;

                        State newState;
                        newState.n = state.n - 1;
                        newState.src = state.tmp;
                        newState.dest = state.dest;
                        newState.tmp = state.src;
                        newState.step = 0;
                        stack.Add(newState);
                    }
                    break;
                case 2:
                    stack.RemoveAt(stack.Count - 1);
                    break;
            }
        }
    }
}
