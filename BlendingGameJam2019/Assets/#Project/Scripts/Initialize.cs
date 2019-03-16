﻿using UnityEngine;
using UnityEngine.iOS;
using System.Collections;

// Show WebCams and Microphones on an iPhone/iPad.
// Make sure NSCameraUsageDescription and NSMicrophoneUsageDescription
// are in the Info.plist.

public class Initialize : MonoBehaviour {
    IEnumerator Start() {
        findWebCams();

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
            Debug.Log("webcam found");
        }
        else {
            Debug.Log("webcam not found");
        }

        findMicrophones();

        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone)) {
            Debug.Log("Microphone found");
        }
        else {
            Debug.Log("Microphone not found");
        }
    }

    void findWebCams() {
        foreach (WebCamDevice device in WebCamTexture.devices) {
            Debug.Log("Name: " + device.name);
        }
    }

    void findMicrophones() {
        foreach (string device in Microphone.devices) {
            Debug.Log("Name: " + device);
        }
    }
}