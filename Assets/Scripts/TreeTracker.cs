using UnityEngine;


public class TreeTracker : MonoBehaviour{
    public bool isRunning = true;
    public float gameSpeed = 1.0f;
    
    void OnGUI() {
        if( GUI.Button( new Rect(110, 10, 80, 20), "Start/Stop") ){
            isRunning = !isRunning;
        }
        GUI.Label( new Rect(120, 30, 80, 50), "" + (isRunning ? "Running" : "Paused"));

        GUI.Box( new Rect(10, 10, 100, 50), "Trees in Scene");
        GUI.Label( new Rect( 55, 35, 50, 50), "" + transform.childCount );
    }
}