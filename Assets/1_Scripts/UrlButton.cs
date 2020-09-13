using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Button))]
public class UrlButton : MonoBehaviour
{
    [SerializeField] string url;

    void Start() {
        GetComponent<Button>().onClick.AddListener(()=>{
            Application.OpenURL(url);
        });
    }
}
