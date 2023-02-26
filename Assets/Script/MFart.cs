using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MFart : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] TextMesh WordText;

    MMonster monster;
    Color fartColor;

    [HideInInspector] public bool IsSelected = false;

    public void Setup(MMonster monster, float fadeDur)
    {
        this.monster = monster;
        WordText.text = monster.GetRandomWord();
        fartColor = monster.skinColor;

        //Set it the same color as the monster color
        Color targetColor = monster.skinColor;
        targetColor.a = spriteRenderer.color.a;
        spriteRenderer.color = targetColor;

        if (fadeDur > 0)
            SetFade(fadeDur);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    public void CallOnClicked()
    {
        MWorldManager.Instance.CallOnSelectedFart(monster, this);
    }

    public void SetState(bool isSelected)
    {
        IsSelected = isSelected;
        Color targetColor = isSelected ? Color.white : monster.skinColor;
        targetColor.a = spriteRenderer.color.a;
        spriteRenderer.color = targetColor;
    }

    void SetFade(float dur)
    {
        spriteRenderer.DOFade(0.1f, dur);
    }
    //-------------------------------------------------------------------------------------------------------------------------------------
}
