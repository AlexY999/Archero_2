using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Game panel")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI hpScore;
    [SerializeField] private TextMeshProUGUI scoreText;
    [Header("WinLose panel")]
    [SerializeField] private GameObject winLosePanel;
    [SerializeField] private TextMeshProUGUI winLoseText;
    [SerializeField] private TextMeshProUGUI winLoseScoreText;
    [Header("Scene changer")]
    [SerializeField] private SceneChanger sceneChanger;

    private string _loadSceneName;
    private int _coinsNum;
    
    public void SetGamePanel()
    {
        OfAllPanels();
        gamePanel.SetActive(true);
        scoreText.text = "0";
        ChangeHPValue(1);
    }
    
    public void SetWinLosePanel(bool isWin, int score)
    {
        OfAllPanels();
        winLosePanel.SetActive(true);
        if (isWin)
            winLoseText.text = "WIN !!!";
        else
                winLoseText.text = "LOSE !!!";

        winLoseScoreText.text = $"SCORE: {score}";
    }
    
    public void ChangeScore(int score)
    {
        scoreText.text = score.ToString();
    }
    
    public void ChangeHPValue(float hp)
    {
        hpBar.fillAmount = hp;
        hpScore.text = $"{hp:P1}";
    }
    
    public void OnMenuButtonClick()
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(gameObject);
        Invoke(nameof(LoadMenuScene), 1.5f);
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }
    
    public void OnRestartButtonClick()
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(gameObject);
        Invoke(nameof(LoadGameScene), 1.5f);
        AudioManager.Instance().OnButtonClickPlayAudioClip();
    }

    public void ChangeScene(string sceneName, int coinsNum)
    {
        sceneChanger.gameObject.SetActive(true);
        sceneChanger.StartSceneChange(gameObject);
        Invoke(nameof(LoadScene), 1.5f);
        _loadSceneName = sceneName;
        _coinsNum = coinsNum;
    }
    
    private void LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_loadSceneName);
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }
    
    private void LoadMenuScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnMenuSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger changer = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        changer.StartSceneChange();
        gameManager.SetStartScore(_coinsNum);
    }
    
    private void LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene1");
        asyncLoad.completed += OnMenuSceneLoadCompleted;
    }

    private void OnGameSceneLoadCompleted(AsyncOperation obj)
    {
        SceneChanger changer = GameObject.Find("LoadingWindow").GetComponent<SceneChanger>();
        changer.StartSceneChange();
    }

    private void OfAllPanels()
    {
        gamePanel.SetActive(false);
        winLosePanel.SetActive(false);
    }
}
