using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecisionsMenu : MonoBehaviour
{
    //get the decisions from the game manager
    private GameManager _gameManager;
    private List<Decision> _decisionList;
    public new AudioSource audio;


    void Awake()
    {
        gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _decisionList = _gameManager.DecisionList;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (GameObject.Find("DecisionCanvas") != null)
        {
            Destroy(GameObject.Find("DecisionCanvas"));
        }
    }

    public void ShowMenu()
    {
        audio.Play();

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            CreateUI();
        }
    }

    void PopUpDecision(Decision decision)
    {
        if (GameObject.Find("DecisionCanvas") != null)
        {
            Destroy(GameObject.Find("DecisionCanvas"));
        }
        GameObject UISize = GameObject.Find("DecisionsBody");

        //create canvas
        GameObject canvas = new GameObject("DecisionCanvas", typeof(RectTransform));
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvas.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        canvas.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        canvas.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(UISize.GetComponent<RectTransform>().rect.width - 50, UISize.GetComponent<RectTransform>().rect.height - 20);
        canvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);

        //create panel
        GameObject panel = new GameObject("Panel", typeof(RectTransform));
        panel.transform.SetParent(canvas.transform, false);
        panel.AddComponent<Image>();
        panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        panel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
        panel.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(UISize.GetComponent<RectTransform>().rect.width - 50, UISize.GetComponent<RectTransform>().rect.height - 20);
        panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 0);
        panel.GetComponent<Image>().color = 0.75f * Color.white;

        GameObject slider = new GameObject("Slider", typeof(RectTransform));
        slider.transform.SetParent(panel.transform, false);
        slider.AddComponent<Slider>();
        slider.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        slider.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        slider.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        slider.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
        slider.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25);
        slider.GetComponent<Slider>().minValue = 0;
        slider.GetComponent<Slider>().maxValue = decision.MaxCost;
        slider.GetComponent<Slider>().value = 0;
        slider.GetComponent<Slider>().interactable = true;
        slider.GetComponent<Slider>().direction = Slider.Direction.LeftToRight;
        slider.GetComponent<Slider>().wholeNumbers = true;


        //slider background
        GameObject sliderBackground = new GameObject("SliderBackground", typeof(RectTransform));
        sliderBackground.transform.SetParent(slider.transform, false);
        sliderBackground.AddComponent<Image>();
        sliderBackground.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        sliderBackground.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        sliderBackground.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        sliderBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 10);
        sliderBackground.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        sliderBackground.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        //slider handle area
        GameObject sliderHandleArea = new GameObject("SliderHandleArea", typeof(RectTransform));
        sliderHandleArea.transform.SetParent(slider.transform, false);
        sliderHandleArea.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        sliderHandleArea.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        sliderHandleArea.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        sliderHandleArea.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 20);
        sliderHandleArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        //slider handle
        GameObject sliderHandle = new GameObject("SliderHandle", typeof(RectTransform));
        sliderHandle.transform.SetParent(sliderHandleArea.transform, false);
        sliderHandle.AddComponent<Image>();
        sliderHandle.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        sliderHandle.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        sliderHandle.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        sliderHandle.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 5);
        sliderHandle.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        sliderHandle.GetComponent<Image>().color = new Color32(128, 128, 128, 255);

        slider.GetComponent<Slider>().targetGraphic = slider.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        slider.GetComponent<Slider>().handleRect = slider.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();


        //create title
        GameObject title = new GameObject("Title", typeof(RectTransform));
        title.transform.SetParent(panel.transform, false);
        title.AddComponent<TextMeshProUGUI>();
        title.GetComponent<TextMeshProUGUI>().text = decision.Title;
        title.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        title.GetComponent<TextMeshProUGUI>().fontSize = 16;
        title.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        title.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        title.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        title.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
        title.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
        title.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        //create description
        GameObject description = new GameObject("Description", typeof(RectTransform));
        description.transform.SetParent(panel.transform, false);
        description.AddComponent<TextMeshProUGUI>();
        description.GetComponent<TextMeshProUGUI>().text = decision.Description;
        description.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        description.GetComponent<TextMeshProUGUI>().fontSize = 16;
        description.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        description.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        description.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        description.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
        description.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25);
        description.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        //create health effect text
        GameObject healthEffect = new GameObject("HealthEffect", typeof(RectTransform));
        healthEffect.transform.SetParent(panel.transform, false);
        healthEffect.AddComponent<TextMeshProUGUI>();
        //healthEffect.GetComponent<TextMeshProUGUI>().text = "Health Effect: " + decision.HealthEffect;
        healthEffect.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        healthEffect.GetComponent<TextMeshProUGUI>().fontSize = 16;
        healthEffect.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        healthEffect.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        healthEffect.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        healthEffect.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
        healthEffect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        healthEffect.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        //create happiness effect text
        GameObject happinessEffect = new GameObject("HappinessEffect", typeof(RectTransform));
        happinessEffect.transform.SetParent(panel.transform, false);
        happinessEffect.AddComponent<TextMeshProUGUI>();
        //happinessEffect.GetComponent<TextMeshProUGUI>().text = "Happiness Effect: " + decision.HappyEffect;
        happinessEffect.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        happinessEffect.GetComponent<TextMeshProUGUI>().fontSize = 16;
        happinessEffect.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        happinessEffect.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        happinessEffect.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        happinessEffect.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
        happinessEffect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        happinessEffect.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        GameObject politicalPower = new GameObject("PoliticalPower", typeof(RectTransform));
        politicalPower.transform.SetParent(panel.transform, false);
        politicalPower.AddComponent<TextMeshProUGUI>();
        politicalPower.GetComponent<TextMeshProUGUI>().text = "Political Power Cost: " + slider.GetComponent<Slider>().value;
        slider.GetComponent<Slider>().onValueChanged.AddListener(delegate
        {
            politicalPower.GetComponent<TextMeshProUGUI>().text = "Political Power Cost: " + slider.GetComponent<Slider>().value;
        });
        politicalPower.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        politicalPower.GetComponent<TextMeshProUGUI>().fontSize = 16;
        politicalPower.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        politicalPower.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        politicalPower.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        politicalPower.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 50);
        politicalPower.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        politicalPower.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        //create button
        GameObject button = new GameObject("Button", typeof(RectTransform));
        button.transform.SetParent(panel.transform, false);
        button.AddComponent<Button>();
        button.AddComponent<Image>();
        if (decision.IsEnacted)
        {
            button.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            slider.GetComponent<Slider>().interactable = false;
            slider.GetComponent<Slider>().gameObject.SetActive(false);

        }
        else
        {
            button.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
            slider.GetComponent<Slider>().interactable = true;
            slider.GetComponent<Slider>().gameObject.SetActive(true);


        }
        button.GetComponent<Button>().onClick.AddListener(delegate
        {
            decision.NotifyDecisionStatusChange(decision, slider.GetComponent<Slider>().normalizedValue);
            if (decision.IsEnacted)
            {
                decision.IsEnacted = false;
                GameObject DButton = GameObject.Find("DButton" + decision.Title);
                DButton.GetComponent<Image>().color = new Color32(0, 255, 0, 255);

                GameObject DButtonText = GameObject.Find("DButtonText" + decision.Title);
                DButtonText.GetComponent<TextMeshProUGUI>().text = "Enact";
                DButtonText.GetComponent<TextMeshProUGUI>().fontSize = 16;


            }
            else
            {
                decision.IsEnacted = true;

                GameObject DButton = GameObject.Find("DButton" + decision.Title);
                DButton.GetComponent<Image>().color = new Color32(255, 0, 0, 255);

                GameObject DButtonText = GameObject.Find("DButtonText" + decision.Title);
                DButtonText.GetComponent<TextMeshProUGUI>().text = "Revoke";
                DButtonText.GetComponent<TextMeshProUGUI>().fontSize = 16;

            }
            Destroy(canvas);
        });
        button.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        button.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 25);
        button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -55);

        //create button text
        GameObject buttonText = new GameObject("ButtonText", typeof(RectTransform));
        buttonText.transform.SetParent(button.transform, false);
        buttonText.AddComponent<TextMeshProUGUI>();
        if (decision.IsEnacted)
        {
            buttonText.GetComponent<TextMeshProUGUI>().text = "Revoke";
        }
        else
        {
            buttonText.GetComponent<TextMeshProUGUI>().text = "Enact";
        }
        buttonText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        buttonText.GetComponent<TextMeshProUGUI>().fontSize = 14;
        buttonText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        buttonText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        buttonText.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        buttonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        buttonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        GameObject exitButton = new GameObject("ExitButton", typeof(RectTransform));
        exitButton.transform.SetParent(panel.transform, false);
        exitButton.AddComponent<Button>();
        exitButton.AddComponent<Image>();
        exitButton.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
        exitButton.GetComponent<Button>().onClick.AddListener(delegate { Destroy(canvas); });
        exitButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        exitButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        exitButton.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
        exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(12.5f, -12.5f);

        GameObject exitButtonText = new GameObject("ExitButtonText", typeof(RectTransform));
        exitButtonText.transform.SetParent(exitButton.transform, false);
        exitButtonText.AddComponent<TextMeshProUGUI>();
        exitButtonText.GetComponent<TextMeshProUGUI>().text = "X";
        exitButtonText.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
        exitButtonText.GetComponent<TextMeshProUGUI>().fontSize = 14;
        exitButtonText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        exitButtonText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        exitButtonText.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        exitButtonText.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
        exitButtonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        exitButtonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
    }

    void CreateUI()
    {
        GameObject parent = GameObject.Find("Content");
        GameObject ScrollView = GameObject.Find("Scroll ViewDec");
        int buffer = 50;
        foreach (Decision decision in _decisionList)
        {
            GameObject panel = new GameObject("Panel", typeof(RectTransform));
            panel.transform.SetParent(parent.transform, false);
            panel.AddComponent<Image>();

            GameObject title = new GameObject("Title", typeof(RectTransform));
            title.transform.SetParent(panel.transform, false);
            title.AddComponent<TextMeshProUGUI>();
            title.GetComponent<TextMeshProUGUI>().text = decision.Title;
            title.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
            title.GetComponent<TextMeshProUGUI>().fontSize = 12;


            GameObject description = new GameObject("Description", typeof(RectTransform));
            description.transform.SetParent(panel.transform, false);
            description.AddComponent<TextMeshProUGUI>();
            description.GetComponent<TextMeshProUGUI>().text = decision.Description;
            description.GetComponent<TextMeshProUGUI>().fontSize = 12;
            description.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);

            GameObject button = new GameObject("DButton" + decision.Title, typeof(RectTransform));
            button.transform.SetParent(panel.transform, false);
            button.AddComponent<Button>();
            button.AddComponent<Image>();
            button.GetComponent<Button>().onClick.AddListener(() => PopUpDecision(decision));


            GameObject buttonText = new GameObject("DButtonText" + decision.Title, typeof(RectTransform));
            buttonText.transform.SetParent(button.transform, false);
            buttonText.AddComponent<TextMeshProUGUI>();
            buttonText.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);

            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(ScrollView.GetComponent<RectTransform>().rect.width - 100, 75);
            panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel.GetComponent<RectTransform>().anchoredPosition.y - buffer);
            buffer += 100;

            title.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
            title.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector2(120, 0);
            title.GetComponent<RectTransform>().sizeDelta = new Vector2(panel.GetComponent<RectTransform>().rect.width * 0.3f, 50);

            if (decision.IsEnacted)
            {
                button.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                buttonText.GetComponent<TextMeshProUGUI>().text = "Revoke";
                buttonText.GetComponent<TextMeshProUGUI>().fontSize = 16;

            }
            else
            {
                button.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
                buttonText.GetComponent<TextMeshProUGUI>().text = "Enact";
                buttonText.GetComponent<TextMeshProUGUI>().fontSize = 16;
            }

            button.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
            button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-85, 0);
            button.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 50);

            buttonText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            buttonText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            buttonText.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 50);
            buttonText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        }
    }
}
