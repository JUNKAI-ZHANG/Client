using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Proto;

public class ComponmentScripts : MonoBehaviour
{
    public InputField UsernameInputField;
    public InputField PasswordInputField;

    public static int willLoadingScene = 0;
    public static int willModifyUI = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public string getUsername()
    {
        return UsernameInputField.text;
    }
    public string getPassword()
    {
        return PasswordInputField.text;
    }
    public void OnClickLogin()
    {
        Debug.Log(getUsername());
        Debug.Log(getPassword());

        if (getUsername() == "" && getPassword() == "")
        {
            Debug.LogError("Input NULL");
            return;
        }
        SendToServer.SendLoginInfo(getUsername(), getPassword());

        UsernameInputField.text = "";
        PasswordInputField.text = "";
    }
    void Update()
    {
        if (willLoadingScene != 0)
        {
            SceneManager.LoadScene(willLoadingScene);
            willLoadingScene = 0;
        }
    }
}
