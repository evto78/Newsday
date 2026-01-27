using System.ComponentModel;
using UnityEngine;

/***********************************
* Description: To move the camera to a different location in the scene. 
* Last Person Edited: Ryan McBride
* Last Date Edited:Jan 26, 2026
************************************/

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;
    
    [SerializeField, Range(0, 3)]//let the designer change what game scene that they are looking at. 
    private int _scene = 0;

    //to let us know how far we need to jump over
    [SerializeField] private float sceneWidth = 1;
    [SerializeField] private float sceneBufferWidth = 0;

    [SerializeField] private GameObject [] _sceneUI;
    [SerializeField] private bool exactPosition = false;
    [SerializeField] private Vector2[] _scenePosition;

    public void jumpToScene(int scene)
    {
        if (scene >= _sceneUI.Length) return;

        if (exactPosition)//if we have the exact position we'll move to that location
        {
            _camera.transform.position = _scenePosition[scene];
        }
        else//we treat it like a slide show moving to the right
        {
            //we move to the next expected location of the scene based on the size of the
            //scene's width + and a little bit of extra buffer space if there is any
            float xCoordinate = (sceneWidth * scene) + (scene == 0 ? 0 : sceneBufferWidth * scene);
            _camera.transform.position = new Vector3(xCoordinate, _camera.transform.position.y, _camera.transform.position.z);
        }

        Debug.Log(_scene);
        turnOffAllUI();//turn off the current GUI
        
        _sceneUI[scene].SetActive(true);//turn on our desired GUI

        _scene = scene;//update the current scene position
    }


    //Takes the camera to the next scene. 
    public void nextScene() {
        jumpToScene((_scene + 1) % _sceneUI.Length);
    }

    public void Update()
    {
        jumpToScene(_scene);
    }

    public void turnOffAllUI()
    {
        for(int i = 0; i < _sceneUI.Length; i++)
        {
            _sceneUI[i].SetActive(false);
        }
    }
}
