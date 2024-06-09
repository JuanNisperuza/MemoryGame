using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI playTimeText, scoreText, totalClicksText, pairsText;
    [SerializeField] public GameObject WinPanel, PausePanel, MenuPanel, HUD;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void UpdatePlayTimeUI(float time)
    {
        playTimeText.text = "Time: " + ((int)time) ;
    }

    public void UpdateTotalClicksUI(int clicks)
    {
        totalClicksText.text = "Moves: " + clicks.ToString();
    }

    public void UpdatePairsUI(int pairs)
    {
        pairsText.text = "Pairs: " + pairs.ToString();
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }



}
