using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MCollider : MonoBehaviour
{
    [SerializeField] UnityEvent OnClicked;

    void OnMouseDown()
    {
        OnClicked?.Invoke();
    }
}
