using UnityEngine;
using UnityEngine.UI;

/**
 * Class to control ingame ui and start/stop simulation
 */
public class InGameUI : MonoBehaviour
{
    // Input field with count of disk
    [SerializeField]
    private InputField diskCountInput;
    // GameObject - panel with start interfface
    [SerializeField]
    private GameObject startPanel;
    // Controller of simulation
    [SerializeField]
    private SimulationController controller;
    // Label with status of simulation
    [SerializeField]
    private Text finishLabel;
    // Label with current step
    [SerializeField]
    private Text stepLabel;
    /**
     * We should subscribe to notification from controller
     */
    void Start ()
    {
        diskCountInput.text = "";
        controller.OnSimulationFinished += OnSimulationFinished;
        controller.OnStepChanged += OnStepChanged;
	}
    /**
     * We should unsubscribe from notifications
     */
    public void OnDestroy()
    {
        controller.OnSimulationFinished -= OnSimulationFinished;
        controller.OnStepChanged -= OnStepChanged;
    }
    /**
        * This method returns integer value from content of input field. I changed ContentType property of input field to avoid validation of
        * content in every calls of OnValueChanged method. But if someone will change ContentType property, this method returns 0.
        */
    private int GetDiskCount()
    {
        int ret = 0;
        if (!int.TryParse(diskCountInput.text, out ret))
        {
            Debug.LogError("Please don't input nondecimal symbols and rollback content type of input field to Integer Number! ");
        }
        return ret;
    }
    /**
     * This method will be called from start panel.
     * It validates value in input field, displays error if needed and can starts simulation if everything is success.
     */
    public void OnStartButtonClick()
    {
        int diskCount = GetDiskCount();
        if (diskCount > 0)
        {
            finishLabel.text = "Simulation started";
            startPanel.SetActive(false);
            controller.InitWithCount(diskCount);
            controller.StartSimulation();
        }
        else
        {
            Debug.LogError("Count of disks should be greater then 0. Please input correct value.");
        }
    }

    /**
     * This method will be called from simulation process when user clock on button "Restart".
     * It stops simulation and cleans input field for new simulation
     */
    public void OnRestartClick()
    {
        controller.StopSimulation();
        diskCountInput.text = "";
    }
    /** 
     * Callback for finish of simulation
     */
    public void OnSimulationFinished()
    {
        finishLabel.text = "Finished";
    }
    /**
     * Callback for change index of step
     */
    public void OnStepChanged(int step, int max)
    {
        stepLabel.text = step.ToString() + "/" + max.ToString();
    }
}
