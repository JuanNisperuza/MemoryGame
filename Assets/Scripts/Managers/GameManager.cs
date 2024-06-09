using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private UIManager uiManager;

    [SerializeField] private GridGenerator gridGenerator;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private AudioClip confettiSound;

    #region BlockGameplay
    private Block firstRevealedBlock;
    private Block secondRevealedBlock;
    private bool canClick = true;
    #endregion

    #region GeneralGameplayVariables
    private float playTime;
    private int totalClicks;
    private int pairs;
    private int score;

    public float PlayTime
    {
        get { return playTime; }
        set
        {
            playTime = value;
            uiManager.UpdatePlayTimeUI(value);
        }
    }

    public int TotalClicks
    {
        get { return totalClicks; }
        set
        {
            totalClicks = value;
            uiManager.UpdateTotalClicksUI(value);
        }
    }

    public int Pairs
    {
        get { return pairs; }
        set
        {
            pairs = value;
            uiManager.UpdatePairsUI(value);
        }
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            uiManager.UpdateScoreUI(value);
        }
    }
    #endregion

    private bool isPause = false;
    private bool inGame = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseState();
        }
    }

    public void InitGame()
    {
        inGame = true;
        StopAllCoroutines();
        gridGenerator.LoadBlocksFromJson();
        uiManager.HUD.SetActive(true);
        uiManager.MenuPanel.SetActive(false); 
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (inGame) 
        {
            PlayTime += Time.deltaTime;
            yield return null; 
        }
    }

    public void TogglePauseState()
    {
        if (inGame)
        {
            isPause = !isPause;
            if (isPause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    private void Pause()
    {
        uiManager.PausePanel.SetActive(true); 
        Time.timeScale = 0.0f;
    }

    private void Resume()
    {
        Time.timeScale = 1.0f; 
        uiManager.PausePanel.SetActive(false);
    }

    public void Win()
    {
        StopAllCoroutines();
        uiManager.WinPanel.SetActive(true);
        SaveSystem.Save(totalClicks, (int)playTime, pairs, score);
    }

    public void RestartGame()
    {
        inGame = false;
        SceneManager.LoadScene(0);
    }

    private void CalculateScore()
    {
        const int basePointsPerPair = 100;
        const int clickPenalty = 2;
        Score += basePointsPerPair - (totalClicks * clickPenalty) - (int)playTime;
    }


    public void OnBlockClicked(Block clickedBlock)
    {
        if (!canClick) return; 

        TotalClicks++;

        if (firstRevealedBlock == null)
        {
            firstRevealedBlock = clickedBlock;
            firstRevealedBlock.ShowBlock(); 
            firstRevealedBlock.blockAnimations.ClickAnimation(); 
        }
        else if (secondRevealedBlock == null && clickedBlock != firstRevealedBlock)
        {
            secondRevealedBlock = clickedBlock; 
            secondRevealedBlock.ShowBlock();
            secondRevealedBlock.blockAnimations.ClickAnimation();
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canClick = false; 
        yield return new WaitForSeconds(1f);

        if (firstRevealedBlock.id == secondRevealedBlock.id)
        {
            firstRevealedBlock.IsMatched = true;
            secondRevealedBlock.IsMatched = true;
            Pairs++; 
            CalculateScore(); 

            if (gridGenerator.totalPairs == Pairs)
            {
                StartCoroutine(WaitToWin());
            }
        }
        else
        {
            firstRevealedBlock.HideBlock();
            secondRevealedBlock.HideBlock();
        }
        firstRevealedBlock = null;
        secondRevealedBlock = null;
        canClick = true;
    }

    private IEnumerator WaitToWin()
    {
        confetti.Play();
        SoundManager.Instance.PlaySound(confettiSound);
        yield return new WaitForSeconds(1.5f);
        Win(); 
    }

    public void Exit()
    {
        Application.Quit();
    }
}