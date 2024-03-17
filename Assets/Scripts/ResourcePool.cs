using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ResourcePool : MonoBehaviour
{
    public static ResourcePool Instance { get; private set; }
    public GameObject indicatorPrefab;
    public GameObject IndicatorTextPrefab;
    private List<GameObject> indicators = new List<GameObject>();
    private List<GameObject> indicatorTexts = new List<GameObject>();
    public Transform indicatorParent;
    public Transform indicatorTextParent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(InitializeIndicators(10));
            StartCoroutine(InitializeIndicatorTexts(10));
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator InitializeIndicators(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateNewIndicator();
            yield return null;
        }
    }
    GameObject CreateNewIndicator()
    {
        GameObject newIndicator = Instantiate(indicatorPrefab);
        newIndicator.SetActive(false);
        newIndicator.transform.parent = indicatorParent;
        indicators.Add(newIndicator);
        return newIndicator;
    }
    public GameObject GetIndicator(GameObject parent)
    {
        foreach (var indicator in indicators)
        {
            if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
                indicator.transform.parent = parent.transform;
                return indicator;
            }
        }
        GameObject newIndicator = CreateNewIndicator();
        newIndicator.SetActive(true);
        newIndicator.transform.parent = parent.transform;
        return newIndicator;
    }
    public void ReturnIndicator(GameObject indicator)
    {
        if (indicator != null)
        {
            indicator.SetActive(false);
        }
    }
    IEnumerator InitializeIndicatorTexts(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateNewIndicatorText();
            yield return null;
        }
    }
    GameObject CreateNewIndicatorText()
    {
        GameObject newIndicatorText = Instantiate(IndicatorTextPrefab);
        newIndicatorText.SetActive(false);
        indicators.Add(newIndicatorText);
        newIndicatorText.transform.SetParent(indicatorTextParent,false);
        return newIndicatorText;
    }
    public GameObject GetIndicatorText()
    {
        foreach (var textObj in indicatorTexts)
        {
            if (!textObj.activeInHierarchy)
            {
                textObj.SetActive(true);
                textObj.GetComponent<TextMeshProUGUI>().text = "";
                return textObj;
            }
        }
        GameObject newTextObj = CreateNewIndicatorText();
        newTextObj.SetActive(true); 
        return newTextObj;
    }
    public void ReturnTMPText(GameObject textObj)
    {
        if (textObj != null)
        {
            textObj.GetComponent<TextMeshProUGUI>().text = "";
            textObj.SetActive(false);
        }
    }
}

