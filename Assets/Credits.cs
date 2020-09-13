using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] UICreditsButton[] buttons;

    private void OnEnable() {
        foreach (var button in buttons) {
            button.SetToInitialState();
        }
    }
}
