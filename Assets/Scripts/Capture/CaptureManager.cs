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

    private Texture2D hololensTargetTexture;

    private CameraParameters hololensCameraParameters;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        activeWithoutBackground = false;
        activeWithBackground = false;
        withoutBackgroundIcon = GameObject.Find("CaptureWithoutBackground").transform.Find("BackPlate").gameObject;
        withBackgroundIcon = GameObject.Find("CaptureWithBackground").transform.Find("BackPlate").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("Activating");
        CoreServices.InputSystem.RegisterHandler<IMixedRealityGestureHandler>(this);
        ToggleIcon(withBackgroundIcon, true);
        activeWithBackground = true;

       
       
        
        PhotoCapture.CreateAsync(true, delegate(PhotoCapture captureObject)
        {
            Resolution cameraResolution =
                PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            hololensTargetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 1.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;
            hololensCameraParameters = cameraParameters;
        });
    }

    private void CaptureWithBackground()
    {
        photoCaptureObject.StartPhotoModeAsync(hololensCameraParameters, delegate(PhotoCapture.PhotoCaptureResult result)
        {
            photoCaptureObject.TakePhotoAsync("./test.png", PhotoCaptureFileOutputFormat.PNG, captureResult =>
            {
                DeactivateCaptureWithBackground();
            });
        });
    }
    private void ActivateCaptureWithoutBackground()
    {
        CoreServices.InputSystem.RegisterHandler<IMixedRealityGestureHandler>(this);
        ToggleIcon(withoutBackgroundIcon, true);
        activeWithoutBackground = true;
    }
    public void DeactivateCaptureWithoutBackground()
    {
        CoreServices.InputSystem.UnregisterHandler<IMixedRealityGestureHandler>(this);
        ToggleIcon(withoutBackgroundIcon, false);
        activeWithoutBackground = false;
    }

    public void DeactivateCaptureWithBackground()
    {
        photoCaptureObject?.StopPhotoModeAsync(result =>
        {
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        } );
    }

    private void ToggleIcon(GameObject icon, bool activated)
    {
        icon.SetActive(activated);
    }

    private void CaptureWithoutBackground()
    {
        var currentRenderTexture = RenderTexture.active;
        camera.targetTexture = new RenderTexture(1920, 1080, 8);
        RenderTexture.active = camera.targetTexture;
        
        var mainMenu = GameObject.Find("MainMenu");
        SetLayerRecursively(mainMenu, 8);
        
        camera.Render();
        
        SetLayerRecursively(mainMenu, 0);
        
        var image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRenderTexture;
        
        var bytes = image.EncodeToPNG();
        Destroy(image);
        
        File.WriteAllBytes("C:\\Data\\USERS\\holol\\Pictures" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", bytes);
        camera.targetTexture = null;
        DeactivateCaptureWithoutBackground();
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
