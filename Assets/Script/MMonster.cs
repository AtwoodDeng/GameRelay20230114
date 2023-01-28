using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class MMonster : MonoBehaviour
{
    [BoxGroup("Visual")]
    [ReadOnly]
    public Color skinColor = Color.yellow;

    [BoxGroup("Visual")]
    public SpriteRenderer skinRender;

    public void InitVisual( Color col )
    {
        skinColor = col;
        skinRender.color = col;
    }

    [BoxGroup("Eyes")]
    public Transform eyeL;
    [BoxGroup("Eyes")]
    public Transform eyeR;
    [BoxGroup("Eyes")]
    [ReadOnly]
    public float angleL;
    [BoxGroup("Eyes")]
    [ReadOnly]
    public float angleR;
    [BoxGroup("Eyes")]
    public float eyeSpeed = 20f;


    public void SetupEye()
    {
        angleL = Random.Range(0, 360f);
        angleR = angleL + 180f;
    }

    public void UpdateEye()
    {
        angleL += eyeSpeed * Time.deltaTime;
        angleR += eyeSpeed * Time.deltaTime;

        eyeL.localEulerAngles = new Vector3(0, 0, angleL);
        eyeR.localEulerAngles = new Vector3(0, 0, angleR);
    }


    [BoxGroup("Move")]
    public float moveSpeed = 2f;

    [BoxGroup("Move")]
    [ReadOnly]
    public float tempSpeed = 2f;

    [BoxGroup("Move")]
    public float turnSpeed = 2f;
    [BoxGroup("Move")]
    public float turnFrequency = 10f;

    [BoxGroup("Move")]
    [ReadOnly]
    public float tempAngle = 0f;

    [BoxGroup("Move")]
    [ReadOnly]
    public Vector2 tempDir;

    [BoxGroup("Move")]
    public float turnNoise = 0f;

    [BoxGroup("Move")]
    public Vector2 worldSize = new Vector2(10f, 5f);

    [BoxGroup("Move")]
    public float worldEdge = 0.5f;



    public void SetupMove()
    {
        tempAngle = Random.Range(0, 360f);
        turnNoise = Random.Range(0, 1000f);
    }

    public void UpdateMove()
    {
        // constrain in ground
        var pos = transform.position;

        tempFratSpeedUp = Mathf.Lerp(tempFratSpeedUp, 0, Time.deltaTime * 1.5f);

        var currentTurnSpeed = turnSpeed + tempFratSpeedUp;

        tempAngle += (Mathf.PerlinNoise(turnFrequency * Time.deltaTime + turnNoise , 0 ) * 2f - 1f ) * currentTurnSpeed * Time.deltaTime;

        Vector2 turnDir = new Vector2(Mathf.Cos(tempAngle*Mathf.Deg2Rad),Mathf.Sin(tempAngle*Mathf.Deg2Rad));

        {
            float rightLerp = 1.0f - Mathf.Clamp01((pos.x + worldSize.x) / worldEdge);
            turnDir = Vector2.Lerp(turnDir,Vector2.right, rightLerp * 0.5f );
        }
        {
            float leftLerp = Mathf.Clamp01((pos.x - worldSize.x) / worldEdge);
            turnDir = Vector2.Lerp(turnDir, Vector2.left, leftLerp * 0.5f );
        }
        {
            float upLerp = 1.0f - Mathf.Clamp01((pos.y + worldSize.y) / worldEdge);
            turnDir = Vector2.Lerp(turnDir, Vector2.up, upLerp * 0.5f);
        }
        {
            float downLerp = Mathf.Clamp01((pos.y - worldSize.y ) / worldEdge);
            turnDir = Vector2.Lerp(turnDir, Vector2.down, downLerp * 0.5f );
        }

        tempDir = turnDir.normalized;

        transform.localScale = new Vector3( Mathf.Sign(tempDir.x) * 1.0f , 1.0f , 1.0f );

        transform.position += new Vector3(tempDir.x, tempDir.y, 0) * ( tempSpeed + tempFratSpeedUp ) * Time.deltaTime;

        Debug.DrawRay(transform.position, tempDir.normalized * 2f);

        //TODO: There is a bug when Monster collide with the edge, need to fix that

    }

    [BoxGroup("Frat")]
    public List<AudioClip> sounds;

    [BoxGroup("Frat")]
    public GameObject fratPrefab;

    [BoxGroup("Frat")]
    public float fratAnimLocalX = -0.66f;

    [BoxGroup("Frat")]
    public float fratAnimDuration = 1f;

    [BoxGroup("Frat")]
    public Vector2 fratInterval = new Vector2(2f, 5f);

    [BoxGroup("Frat")]
    public float fratTimer = 2f;

    [BoxGroup("Frat")]
    public float fratSpeedUp = 1f;

    [BoxGroup("Frat")]
    [ReadOnly]
    public float tempFratSpeedUp = 1f;

    public void Frat()
    {
        var sound = sounds[Random.Range(0,sounds.Count)];

        AudioSource.PlayClipAtPoint(sound, transform.position);

        var frat = Instantiate(fratPrefab) as GameObject;
        // frat.transform.parent = transform;
        frat.transform.position = transform.position;
        // frat.transform.localScale = transform.localScale;
        MFart fart = frat.GetComponent<MFart>();
        fart.Setup(this, fratAnimDuration);
        Farts.Add(fart);

        frat.transform.DOMove(tempDir.normalized * fratAnimLocalX, fratAnimDuration).SetRelative(true).SetEase(Ease.OutCubic);

        /*var seq = DOTween.Sequence();
        seq.Append(fratSprite.DOFade(0, 0));
        seq.Append(fratSprite.DOFade(0.5f, fratAnimDuration * 0.1f));
        seq.AppendInterval(fratAnimDuration * 0.7f);
        seq.Append(fratSprite.DOFade(0, fratAnimDuration * 0.2f).SetDelay(dur - fratAnimDuration));
        seq.AppendCallback(delegate () {
            Farts.Remove(fart);
            GameObject.Destroy(frat,2f);
        });*/

        DOVirtual.DelayedCall(fratAnimDuration, () =>
        {
            Farts.Remove(fart);
            GameObject.Destroy(frat);
        });

        tempFratSpeedUp = fratSpeedUp;
    }

    public void SetupFrat()
    {
        fratTimer = Random.Range(0, fratInterval.y);
    }


    public void UpdateFrat()
    {
        fratTimer -= Time.deltaTime;

        if ( fratTimer < 0  )
        {
            fratTimer = Random.Range(fratInterval.x, fratInterval.y);
            Frat();
        }
    }

    public void SetupWordInfo()
    {
        CurrentWordListIndex = Random.Range(0, Words.Count);
    }

    // Start is called before the first frame update
    void Start()
    {
        // setup eye
        SetupEye();

        SetupMove();

        SetupFrat();

        SetupWordInfo();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEye();

        UpdateMove();

        UpdateFrat();
    }

    //-------------------------------------------------------------------------------------------------------------------------------------
    [HideInInspector] public List<MFart> Farts;
    [HideInInspector] int CurrentWordListIndex;
    [SerializeField] List<WordInfo> Words;

    public void CallOnClicked()
    {
        //If a fart is selected and monster gets selected, kill Monster
        bool isFartSelected = false;
        foreach (var fart in Farts)
        {
            if(fart.IsSelected)
            {
                isFartSelected = true;
                break;
            }
        }
        if(isFartSelected)
        {
            MWorldManager.Instance.CallKill(this);
            foreach(var fart in Farts)
            {
                Destroy(fart.gameObject);
            }
        }  
    }

    /// <summary>
    /// Set states for all monster's farts
    /// </summary>
    /// <param name="seletedFart"></param>
    public void SetFartsState(MFart seletedFart = null)
    {
        foreach(var fart in Farts)
        {
            if(seletedFart!= null && fart == seletedFart)
            {
                fart.SetState(true);
                continue;
            }
            fart.SetState(false);
        }
    }

    public string GetRandomWord()
    {
        var worldIndex = Random.Range(0, Words[CurrentWordListIndex].WordList.Count);
        return Words[CurrentWordListIndex].WordList[worldIndex];
    }
    //-------------------------------------------------------------------------------------------------------------------------------------
}


[System.Serializable]
public struct WordInfo
{
    public string Label;
    public List<string> WordList;
}