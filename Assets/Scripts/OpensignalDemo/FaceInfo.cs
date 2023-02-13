using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.UI;

public class FaceInfo : MonoBehaviour
{
    private string folderPath = "Assets/StreamingAssets/Face";

    private string targetFile;

    private StreamReader _reader;

    private double[] model = new double[35];

    private double constant;

    public Text outPutMsg;

    private DateTime lastTime;
    
    // Start is called before the first frame update
    void Start()
    {
        //setting up the model
        using var f = new StreamReader(File.OpenRead("Assets/StreamingAssets/ML Models/decision.txt"));
        var m = f.ReadLine().Split(",");
        model = m.Select(d => double.Parse(d, CultureInfo.InvariantCulture)).ToArray();

        var c = f.ReadLine();
        constant = double.Parse(c, CultureInfo.InvariantCulture);
        
        lastTime = DateTime.Now;
        
        //seeting up the file read
        var info = new DirectoryInfo(folderPath);
        var fileinfo = info.GetFiles();
        foreach (var file in fileinfo)
        {
            if (file.Name.Contains(".csv") && !file.Name.Contains(".meta"))
            {
                targetFile = file.FullName;
            }
        }

        var fileStream = new FileStream(targetFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        _reader = new StreamReader(fileStream,Encoding.Default);
    }

    // Update is called once per frame
    private void Update()
    {
        var y = 0;
        var timeDiff = DateTime.Now - lastTime;
        if (timeDiff.TotalMilliseconds < 100)
        {
            return;
        }
        try
        {
            lastTime = DateTime.Now;
            string[] lines = _reader.ReadToEnd().Split(new string[] {Environment.NewLine}, StringSplitOptions.None);
            foreach (var t in lines)
            {
                if (t == "" || t.StartsWith("f")) continue;
                string[] data = t.Split(",");
                var x = data.Skip(data.Length - 35).Take(35).ToArray();
                try
                {
                    var X = x.Select(d => double.Parse(d, CultureInfo.InvariantCulture)).ToArray();
                    y += Predict(X);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
        catch (Exception e)
        {
            // ignored
        }

        outPutMsg.text = y < 0 ? "Fear" : "Neutral";
    }

    private void OnApplicationQuit()
    {
        _reader.Dispose();
    }
   
    private int Predict(double[] X)
    {
        double y = 0;
        for (var i = 0; i < X.Length; i++)
        {
            y += X[i] * model[i];
        }
        return (y+constant)>0? 1 : -1;
    }
    
}
