using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class DisplayWebcam : MonoBehaviour
{


        public RawImage rawimage;
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
        try {
            IBarcodeReader barcodeReader = new BarcodeReader();
            // decode the current frame
            var result = barcodeReader.Decode(webcamTexture.GetPixels32(),
              webcamTexture.width, webcamTexture.height);
            print(result);
            if (result != null) {
                Debug.Log("DECODED TEXT FROM QR: " +result.Text);
            }

        }
        catch (System.Exception ex) { Debug.LogWarning(ex.Message); }
    }
}

