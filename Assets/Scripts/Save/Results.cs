[System.Serializable]
public class Results
{
    public int total_clicks;
    public int total_time;
    public int pairs;
    public int score;


    public Results(int total_clicks, int total_time, int pairs, int score)
    {
        this.total_clicks = total_clicks;
        this.total_time = total_time;
        this.pairs = pairs;
        this.score = score;
    }
}
