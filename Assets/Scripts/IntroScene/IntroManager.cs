using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.SoundPlay("BGM_A");
    }

    #region Method
    
    /// <summary>
    /// 게임 시작
    /// </summary>
    public void OnClickGameStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// 설정 열기
    /// </summary>
    public void OnClickSettings()
    {
        Popup_Settings.Show(null);
    }

    #endregion
}
