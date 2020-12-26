using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using Debug = UnityEngine.Debug;

public class CaptureManager : MonoBehaviour, IMixedRealityGestureHandler
{
    [SerializeField] private MixedRealityInputAction validateAction;
    [SerializeField] private MixedRealityInputAction cancelAction;

    private bool activeWithoutBackground;
    private bool activeWithBackground;

    private GameObject withoutBackgroundIcon;
    private GameObject withBackgroundIcon;

    private Camera camera;
    private PhotoCapture photoCaptureObject;

    [SerializeField] private GameObject infoTooltip;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        activeWithoutBackground = false;
        activeWithBackground = false;
        withoutBackgroundIcon = GameObject.Find("CaptureWithoutBackground").transform.Find("BackPlate").gameObject;
        withBackgroundIcon = GameObject.Find("CaptureWithBackground").transform.Find("BackPlate").gameObject;
        infoTooltip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleCaptureWithBackground()
    {
        if (activeWithBackground)
        {
            DeactivateCaptureWithBackground();
        }
        else
        {
            if (activeWithoutBackground)
            {
                DeactivateCaptureWithoutBackground();
            }
            ActivateCaptureWithBackground();
        }
    }

    private void ActivateCaptureWithBackground()
    {
        CoreServices.InputSystem.RegisterHandler<IMixedRealityGestureHandler>(this);
        ToggleIcon(withBackgroundIcon, true);
        activeWithBackground = true;
        infoTooltip.SetActive(true);
    }
    
    public void DeactivateCaptureWithBackground()
    {
        CoreServices.InputSystem.UnregisterHandler<IMixedRealityGestureHandler>(this);
        ToggleIcon(withBackgroundIcon, false);
        activeWithBackground = false;
        infoTooltip.SetActive(false);
    }

    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        Resolution cameraResolution =
            PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        photoCaptureObject = captureObject;
        CameraParameters cameraParameters = new CameraParameters();
        cameraParameters.hologramOpacity = 1.0f;
        cameraParameters.cameraResolutionWidth = cameraResolution.width;
        cameraParameters.cameraResolutionHeight = cameraResolution.height;
        cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
        
        captureObject.StartPhotoModeAsync(cameraParameters, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            var tempFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

            var filePath = Path.Combine(Application.persistentDataPath, tempFileName);
            var tempFilePathAndName = filePath;

            try
            {
                HideUI();
                infoTooltip.SetActive(false);
                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.PNG, captureResult =>
                {
                    ShowUI();
                    if (result.success)
                    {
                        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
                        #if !UNITY_EDITOR && UNITY_WINRT_10_0
                            var cameraRollFolder = Windows.Storage.KnownFolders.CameraRoll.Path;
                            File.Move(tempFilePathAndName, Path.Combine(cameraRollFolder, tempFileName));
                        #endif
                    }
                } );
            }
            catch (System.ArgumentException e)
            {
                Debug.LogError("System.ArgumentException:\n" + e.Message);
            }
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }
    
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
        DeactivateCaptureWithBackground();
    }

    public void CaptureWithBackground()
    {
        PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);
    }
    
    public void ToggleCaptureWithoutBackground()
    {
        if (activeWithoutBackground)
        {
            DeactivateCaptureWithoutBackground();
        }
        else
        {
            if (activeWithBackground)
            {
                DeactivateCaptureWithBackground();
            }
            ActivateCaptureWithoutBackground();
        }
    }
    
    private void ActivateCaptureWithoutBackground()
    {
        CoreServices.InputSystem.RegisterHandler<IMixedRealityGestureHandler>(this);
        ToggleIcon(withoutBackgroundIcon, true);
        activeWithoutBackground = true;
        infoTooltip.SetActive(true);
    }
    public void DeactivateCaptureWithoutBackground()
    {
        CoreServices.InputSystem.UnregisterHandler<IMixedRealityGestureHandler>(this);
        ToggleIcon(withoutBackgroundIcon, false);
        activeWithoutBackground = false;
        infoTooltip.SetActive(false);
    }

    public void CaptureWithoutBackground()
    {
        var renderTexture = new RenderTexture(1920, 1080, 24);
        camera.targetTexture = renderTexture;
        var image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGB24, false);
        
        
        HideUI();
        infoTooltip.SetActive(false);
        
        camera.Render();
        
        ShowUI();
        RenderTexture.active = camera.targetTexture;
        
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        RenderTexture.active = null;
        camera.targetTexture = null;
        Destroy(renderTexture);
        
        var bytes = image.EncodeToPNG();

        var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        var filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, bytes);

        #if !UNITY_EDITOR && UNITY_WINRT_10_0
            var cameraRollFolder = Windows.Storage.KnownFolders.CameraRoll.Path;
            File.Move(filePath, Path.Combine(cameraRollFolder, fileName));
        #endif
        DeactivateCaptureWithoutBackground();
    }
    
    private void ToggleIcon(GameObject icon, bool activated)
    {
        icon.SetActive(activated);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public void OnGestureStarted(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == validateAction)
        {
            if (activeWithBackground)
            {
                CaptureWithBackground();
            }
            else
            {
                CaptureWithoutBackground();
            }
        }
        else if (eventData.MixedRealityInputAction == cancelAction)
        {
            if (activeWithBackground)
            {
                DeactivateCaptureWithBackground();
            }
            else
            {
                DeactivateCaptureWithoutBackground();
            }
        }
    }

    private void HideUI()
    {
        var mainMenu = GameObject.Find("MainMenu");
        var cursor = GameObject.FindWithTag("Cursor");
        
        SetLayerRecursively(mainMenu, 8);
        SetLayerRecursively(cursor, 8);
    }

    private void ShowUI()
    {
        var mainMenu = GameObject.Find("MainMenu");
        var cursor = GameObject.FindWithTag("Cursor");
        
        SetLayerRecursively(mainMenu, 9);
        SetLayerRecursively(cursor, 0);
    }

    public void OnGestureUpdated(InputEventData eventData)
    {
        
    }

    public void OnGestureCompleted(InputEventData eventData)
    {
        
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
        
    }
}
