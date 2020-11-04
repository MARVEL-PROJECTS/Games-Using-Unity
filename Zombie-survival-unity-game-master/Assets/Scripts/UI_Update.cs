using UnityEngine;
using UnityEngine.UI;

public class UI_Update : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private  RectTransform waveText;
    [SerializeField]
    private  RectTransform scoreText;
    [SerializeField]
    private RectTransform waveCleartext;

    private void Start()
    {
        //  check if referencees are completed in inspector
        if (healthBarRect == null)
        {
            Debug.LogError("UI UPDATE : No healtBar object referenced in inspector.");
        }
        if (waveText == null)
        {
            Debug.LogError("UI UPDATE : No waveText object referenced in inspector.");
        }
        if (scoreText == null)
        {
            Debug.LogError("UI UPDATE : No scoreText object referenced in inspector.");
        }
        if (waveCleartext == null)
        {
            Debug.LogError("UI UPDATE : No waveClearText object referenced in inspector.");
        }
    }

    public void SetWaveNumber(int _value)
    {
        string temp = "Wave: " + _value;
        waveText.GetComponent<Text>().text = temp;
    }

    public  void SetScoreNumber(int _value)
    {
        string temp = "Score: " + _value;
        scoreText.GetComponent<Text>().text = temp;
    }

    public void SetHealth(int _cur, int _max)
    {
        float value = (float) _cur / _max;
        healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
    }

    public void SetWaveClearOpacity(float _value)
    {
        CanvasGroup temp = waveCleartext.GetComponent<CanvasGroup>();
        temp.alpha = _value;
    }

	public void SetWaveClearText(string _string)
	{
		waveCleartext.GetComponent<Text> ().text = _string;
	}
}
