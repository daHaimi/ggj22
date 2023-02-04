using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;

[InitializeOnLoad]
public class THIMenu : MonoBehaviour
{
    private const string Terminal = "gnome-terminal";
    private const string ADBPath = "Data/PlaybackEngines/AndroidPlayer/SDK/platform-tools";
    private const string BashPath = "THI/Editor";
    private const string BashScript = "connect.bash";
    private const string VRActivity = "com.htc.vr.unity.WVRUnityVRActivity";
    private const int Port = 8098;

    private static Thread serverThread;
    private static HttpListener restServer;
    private static volatile bool connClosed;
    private static bool restEnabled = true;

    static THIMenu()
    {
        StartRESTServer();
    }
    
    [MenuItem ("Tools/THI/Vive/Start Study on Vive")]
    private static void StartProgramOnVive()
    {
        var package = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
        //adb shell am start -n com.package.name/com.package.name.ActivityName
        ExecuteScript("./adb shell am start -n " + package + "/" + VRActivity);
    }
    
    [MenuItem ("Tools/THI/Vive/Start Demo on Vive")]
    private static void StartDemoOnVive()
    {
        var package = "com.thi.hcig.VRDemo";
        ExecuteScript("./adb shell am start -n " + package + "/" + VRActivity);
    }
    
    [MenuItem ("Tools/THI/Vive/Connect ADB over WiFi")]
    private static void ConnectADB ()
    {
        ExecuteScript("bash -c " + Application.dataPath + "/" + BashPath + "/" + BashScript);
    }
    
    [MenuItem ("Tools/THI/Vive/Open Android Terminal")]
    private static void OpenTerminal ()
    {
        var path = EditorApplication.applicationPath;
        path = path.Remove(path.Length - 5) + ADBPath;
        Process.Start(Terminal, "--working-directory=" + path);
    }

    private static void ExecuteScript(string script)
    {
        var path = EditorApplication.applicationPath;
        path = path.Remove(path.Length - 5) + ADBPath;
        Process.Start(Terminal, "--working-directory=" + path + " -- " + script);
    }

    [MenuItem("Tools/THI/Fernsteuerung/Starte REST Steuerung")]
    private static void EnableRest()
    {
        if (restEnabled) return;
        restEnabled = true;
        StartRESTServer();
    }

    private static void StartRESTServer()
    {
        serverThread = new Thread(() =>
        {
            restServer = new HttpListener();
            var prefix = "http://localhost:" + Port + "/";
            restServer.Prefixes.Add(prefix + "start/");
            restServer.Prefixes.Add(prefix + "stop/");
            restServer.Prefixes.Add(prefix + "test/");
            restServer.Start();
            Debug.Log("Started REST server on port " + Port);
            connClosed = false;
            while (!connClosed)
            {
                var ctx = restServer.GetContext();
                var req = ctx.Request;
                Debug.Log($"Received request for {req.Url.AbsolutePath}");

                using var resp = ctx.Response;
                resp.Headers.Set("Content-Type", "text/plain");

                var response = "Not found";
                resp.StatusCode = 404;
                switch (req.Url.AbsolutePath)
                {
                    case "/start":
                        EditorApplication.delayCall += EditorApplication.EnterPlaymode;
                        response = "Started";
                        resp.StatusCode = 201;
                        break;
                    case "/stop":
                        EditorApplication.delayCall += EditorApplication.ExitPlaymode;
                        response = "Stopped";
                        resp.StatusCode = 200;
                        break;
                    case "/test":
                        response = "Running";
                        resp.StatusCode = 202;
                        break;
                }
                var buffer = Encoding.UTF8.GetBytes(response);
                resp.ContentLength64 = buffer.Length;
                using var ros = resp.OutputStream;
                ros.Write(buffer, 0, buffer.Length);
                Debug.Log("Sent " + response);
            }
            Debug.Log("Connection closed");
        })
        {
            IsBackground = true
        };
        serverThread.Start();
    }

    [MenuItem("Tools/THI/Fernsteuerung/Stoppe REST Steuerung")]
    private static void DisableRest()
    {
        if (!restEnabled) return;
        restEnabled = false;
        StopRESTServer();
    }

    private static void StopRESTServer()
    {
        connClosed = true;
        serverThread?.Abort();
    }

    static void StartPlayer()
    {
        EditorApplication.delayCall += EditorApplication.EnterPlaymode;
    }

    static void StopPlayer()
    {
        EditorApplication.delayCall += EditorApplication.ExitPlaymode;
    }
}
