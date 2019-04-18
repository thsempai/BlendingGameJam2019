using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;

public class DisplayWebcam : MonoBehaviour
{


    public RawImage rawimage;
    public static Action<string> QRCodeMessage;

    WebCamTexture webcamTexture;
        void Start() {
            webcamTexture = new WebCamTexture();
            rawimage.texture = webcamTexture;
            rawimage.material.mainTexture = webcamTexture;
            webcamTexture.Play();

        }

    // Update is called once per frame
    void Update()
    {
        ZXing.Result result = null;
        try {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            result = barcodeReader.Decode(webcamTexture.GetPixels32(),
              webcamTexture.width, webcamTexture.height);
        }
        catch (System.Exception ex) { Debug.LogWarning(ex.Message); }

        if (result != null) {

                    QRCodeMessage?.Invoke(result.Text);
            }


    }
}

