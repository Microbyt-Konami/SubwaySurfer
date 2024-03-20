using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Fields
    [SerializeField] private int countDownSecs;
    [SerializeField] private bool isLevelCurve;

    // Components
    private PlayerController playerController;
    private ShaderController shaderController;

    // Variables
    private bool isCountDown;
    private bool isGameObject = false;
    private int secondCountDown;

    public void GameOver()
    {
        playerController.NoMove = true;
        isGameObject = true;
        shaderController?.DesActivateChangeValues();
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        shaderController = FindFirstObjectByType<ShaderController>();
        StartCoroutine(DoCountDown());
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    IEnumerator DoCountDown()
    {
        playerController.NoMove = true;
        isCountDown = true;
        for (secondCountDown = 3; secondCountDown >= 1; secondCountDown--)
        {
            yield return new WaitForSecondsRealtime(1);
        }
        isCountDown = false;
        playerController.NoMove = false;
        if (isLevelCurve)
            shaderController?.ActivateChangeValues();
    }

    private void OnGUI()
    {
        // if (!isCountDown && !isGameObject)
        //     return;

        if (isCountDown)
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.FlexibleSpace();

            var styleText = GUI.skin.GetStyle("label");

            styleText.fontSize = 50 * 2;
            styleText.fontStyle = FontStyle.Bold;

            GUI.color = Color.black;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label(secondCountDown.ToString(), styleText);

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
        }

        // if (isGameObject)
        // {
        var styleButton = GUI.skin.GetStyle("button");

        styleButton.fontSize = 50;
        styleButton.fontStyle = FontStyle.Bold;

        GUI.color = Color.white;
        GUI.backgroundColor = Color.red;

        /*
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("RESTART", styleButton, GUILayout.Width(295), GUILayout.Height(150)))
                    SceneManager.LoadScene(0);
                GUILayout.EndHorizontal();
                */
        if (GUI.Button(new Rect(Screen.width - 295, 0, 295, 70), "RESTART", styleButton))
            SceneManager.LoadScene(0);
        // }
    }
}
