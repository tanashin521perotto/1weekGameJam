using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // シングルトン
    // ゲーム内に１つしか存在しない物（音を管理する物とか）
    // 利用場所：シーン間でのデータ共有/オブジェクト共有
    // 書き方
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    //--シングルトン終わり--

    public AudioSource audioSourceSE; // SEのスピーカー
    public AudioClip[] audioClipsSE; // ならす素材

    public void PlaySE(int index)
    {
        audioSourceSE.PlayOneShot(audioClipsSE[index]); // SEを一度だけならす
    }
}