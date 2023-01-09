using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCoordinator : MonoBehaviour
{
    [System.Serializable]
    public class TutorialData
    {
        public string Main;
        public List<string> details;
    }
}
