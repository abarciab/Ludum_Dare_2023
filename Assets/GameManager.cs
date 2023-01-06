using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int frameTarget = 60;

    private void Start()
    {
        SetFrameRate();
    }

    private void OnValidate()
    {
        if (Application.isPlaying) SetFrameRate();
    }

    void SetFrameRate()
    {
        Application.targetFrameRate = frameTarget;
    }
}
