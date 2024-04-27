using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AuthenticationController : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public GameObject loading;
    public Button registerBtn;
    public Button loginBtn;

    private string URL = "https://authentication-three-kappa.vercel.app";
    private void Start()
    {
        loading.SetActive(false);
        registerBtn.onClick.AddListener(() =>
        {
            Register();
        });
        loginBtn.onClick.AddListener(() =>
        {
            Login();
        });
        usernameField.text = "minhquang";
        passwordField.text = "12345";
    }
    public void Register()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        StartCoroutine(RegisterAPI(username, password));
    }
    public IEnumerator RegisterAPI(string username, string password)
    {
        loading.SetActive(true);

        Auth auth = new(username, password);
        string jsonData = JsonConvert.SerializeObject(auth);

        string registerURL = URL + "/auth/register";

        using (UnityWebRequest www = new(registerURL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.responseCode == 200)
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
        loading.SetActive(false);
    }
    public void Login()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        StartCoroutine(LoginAPI(username, password));
    }
    public IEnumerator LoginAPI(string username, string password)
    {
        loading.SetActive(true);
        Auth auth = new(username, password);
        string jsonData = JsonConvert.SerializeObject(auth);

        string loginURL = URL + "/auth/login";

        using (UnityWebRequest www = new(loginURL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            Debug.Log(www.error);

            if (www.responseCode == 200)
            {
                PlayerPrefs.SetString("username", usernameField.text);
                DataStore.instance.SetData("username", usernameField.text);
                SceneController.instance.LoadNewScene(SceneController.SceneName.Main_UI, true);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }

        loading.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(GetUsers());
        }
    }
    public IEnumerator GetUsers()
    {
        string getURL = URL + "/auth/get";
        using (UnityWebRequest www = new(getURL, UnityWebRequest.kHttpVerbGET))
        {
            DownloadHandlerBuffer dH = new();
            www.downloadHandler = dH;
            yield return www.SendWebRequest();
            if (www.responseCode == 200)
            {
                List<Auth> data = JsonConvert.DeserializeObject<List<Auth>>(www.downloadHandler.text);
                for (int i = 0; i < data.Count; i++)
                {
                    Debug.Log(data[i].GetAuth());
                }
            }
            else
            {
                Debug.Log(www.error);
            }
        }
    }
}
public class Auth
{
    public string username;
    public string password;
    public Auth(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
    public string GetAuth()
    {
        return username + "-" + password;
    }
}