using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CaptorInfo : MonoBehaviour
{
    private string MAC_ADDRESS = "20:18:08:08:02:30";
    public int sampling_rate = 100;
    private PluxDeviceManager pluxDevManager;
    private List<int> collected_data = new List<int>();

    private int heartrate;
    // Start is called before the first frame update
    void Start()
    {// Initialise object
        pluxDevManager = new PluxDeviceManager(ScanResults, ConnectionDone, AcquisitionStarted, OnDataReceived, OnEventDetected, OnExceptionRaised);

        // Important call for debug purposes by creating a log file in the root directory of the project.
        pluxDevManager.WelcomeFunctionUnity();
        
    }

    // Update is called once per frame
    void Update()
    { }
    
    /**
     * =================================================================================
     * ============================= Function =========================================
     * =================================================================================
     */   
    
    private void OnApplicationQuit()
    {
        try
        {
            // Disconnect from device.
            if (pluxDevManager != null)
            {
                pluxDevManager.DisconnectPluxDev();
                Console.WriteLine("Application ending after " + Time.time + " seconds");
            }
        }
        catch (Exception exc)
        {
            Console.WriteLine("Device already disconnected when the Application Quit.");
        }
    }

    private void Connect()
    {
        pluxDevManager.PluxDev(MAC_ADDRESS);
    }

    private void Disconnect()
    {
        pluxDevManager.DisconnectPluxDev();
    }
    
    private void StartAq()
    { //only for channel A2
        pluxDevManager.StartAcquisitionUnity(sampling_rate, new List<int> { 2}, 10);
    }
    
       /**
     * =================================================================================
     * ============================= Callbacks =========================================
     * =================================================================================
     */

    // Callback that receives the list of PLUX devices found during the Bluetooth scan.
    public void ScanResults(List<string> listDevices)
    {
        
        if (listDevices.Count > 0)
        {
            Debug.Log("Scan completed.\nNumber of devices found: " + listDevices.Count);
        }
        else
        {
            // Show an informative message stating the none devices were found.
            Debug.Log("Bluetooth device scan didn't found any valid devices.");
        }
    }
    
    // Callback invoked once the connection with a PLUX device was established.
    // connectionStatus -> A boolean flag stating if the connection was established with success (true) or not (false).
    public void ConnectionDone(bool connectionStatus)
    {
        if (connectionStatus)
        {
            Debug.Log("Connection sucessful.");
        }
        else
        {
            Debug.Log("It was not possible to establish a connection with the device.");
        }
    }
    
    // Callback invoked once the data streaming between the PLUX device and the computer is started.
    // acquisitionStatus -> A boolean flag stating if the acquisition was started with success (true) or not (false).
    // exceptionRaised -> A boolean flag that identifies if an exception was raised and should be presented in the GUI (true) or not (false).
    public void AcquisitionStarted(bool acquisitionStatus, bool exceptionRaised = false, string exceptionMessage = "")
    {
        if (acquisitionStatus)
        { }
        else
        {
            // Present an informative message about the error.
            Debug.Log("It was not possible to start a real-time data acquisition. Try to repeat the scan/connect/start workflow.");
        }
    }
    
    // Callback invoked every time an exception is raised in the PLUX API Plugin.
    // exceptionCode -> ID number of the exception to be raised.
    // exceptionDescription -> Descriptive message about the exception.
    public void OnExceptionRaised(int exceptionCode, string exceptionDescription)
    {
        if (pluxDevManager.IsAcquisitionInProgress())
        {
            // Present an informative message about the error.
            Debug.Log(exceptionDescription);
        }
    }
    
    // Callback that receives the data acquired from the PLUX devices that are streaming real-time data.
    // nSeq -> Number of sequence identifying the number of the current package of data.
    // data -> Package of data containing the RAW data samples collected from each active channel ([sample_first_active_channel, sample_second_active_channel,...]).
    public void OnDataReceived(int nSeq, int[] data)
    {
        collected_data.Add(data[0]);
        if (collected_data.Count == 200) //2seconds
            {
                heartrate = detectHR(collected_data);
                collected_data.Clear();
            }

        if (nSeq % sampling_rate == 0)
        {
            string outputString = heartrate.ToString();
            //TODO put the heartbeat in a gameobject 
        }
    }
    
    // Callback that receives the events raised from the PLUX devices that are streaming real-time data.
    // pluxEvent -> Event object raised by the PLUX API.
    public void OnEventDetected(PluxDeviceManager.PluxEvent pluxEvent)
    {
        if (pluxEvent is PluxDeviceManager.PluxDisconnectEvent)
        {
            // Present an error message.
            Debug.Log(
                "The connection between the computer and the PLUX device was interrupted due to the following event: " +
                (pluxEvent as PluxDeviceManager.PluxDisconnectEvent).reason);

            // Securely stop the real-time acquisition.
            pluxDevManager.StopAcquisitionUnity(-1);
        }
        else if (pluxEvent is PluxDeviceManager.PluxDigInUpdateEvent)
        {
            PluxDeviceManager.PluxDigInUpdateEvent digInEvent = (pluxEvent as PluxDeviceManager.PluxDigInUpdateEvent);
            Console.WriteLine("Digital Input Update Event Detected on channel " + digInEvent.channel + ". Current state: " + digInEvent.state);
        }
    }
    
    private int detectHR(List<int> data)
    {
        int[] f = FilterECG(data.ToArray());
        int[] rPeaks = DetectRPeaks(f).ToArray();

        double[] rPeakIntervals = new double[rPeaks.Length - 1];
        for (int i = 0; i < rPeaks.Length - 1; i++)
        {
            rPeakIntervals[i] = rPeaks[i + 1] - rPeaks[i];
        }
        double averageInterval = rPeakIntervals.Average();
        double heartRate = 60.0 / (averageInterval / sampling_rate);
        return (int)heartRate;
    }
    
    static int[] FilterECG(int[] ecgData)
    {
        int[] filteredData = new int[ecgData.Length];
        for (int i = 0; i < ecgData.Length; i++)
        {
            int value = ecgData[i] - 500;
            if (value is > -20 and < 20)
            {
                filteredData[i] = 0;
            }
            else
            {
                filteredData[i] = value;
            }
        }
        return filteredData;
    }

    
    static List<int> DetectRPeaks(int[] filteredData)
    {
        List<int> rPeaks = new List<int>();
        for (int i = 0; i < filteredData.Length-1; i++)
        {
            if (filteredData[i] == 0 && filteredData[i + 1] > 20)
            {
                rPeaks.Add(i);
            } 
        }

        return rPeaks;
    }

    IEnumerable wait(int time)
    {
        yield return new WaitForSeconds(time);
    }
}
