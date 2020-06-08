using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Networking;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Linq;

public class APIHelper : MonoBehaviour
{

    public class Container
    {
        public User user;
    }

    static HttpClient client = new HttpClient();

    // Start is called before the first frame update
    async void Start()
    {
        //string constr = "Server=remotemysql.com ;Database=Lf5NzlwgGk;User ID=Lf5NzlwgGk;Password=ZBHoteu5F2;Pooling=true;CharSet=utf8;Port=3306";

        #region comments
        //Container c = new Container();
        //StartCoroutine(GetUser(c, "http://127.0.0.1:8000/users/2/"));
        //Debug.Log(c.user);

        //User user = new User("http://127.0.0.1:8000/users/2/", "ExampleUser", "example_email@dml.com");
        //StartCoroutine(PutUser("http://127.0.0.1:8000/users/2/", user));
        #endregion
        client.BaseAddress = new Uri("http://127.0.0.1:8000/");
        User user = new User();
        

        var oneUser = await GetUserAsync("users/1/");
        User u = new User();
        JsonUtility.FromJsonOverwrite(oneUser, u);

        UserDetails userDetails = new UserDetails(u.Name, new DateTime(1992, 03, 15, 00, 00, 00), true, "Engineer", "Free", "Ivanovich");

        //PostUserAsync("users/", user);

        //GetAsync("user_details/");
        CancellationToken token;
        PostUserDetailsAsync("user_details/", userDetails, token);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator  GetUser(Container c, string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            string test = www.downloadHandler.text;
            c.user = JsonUtility.FromJson<User>(test);
            Debug.Log(test.ToString());
            Debug.Log(test);
            Debug.Log(c.user.Name);
        }
    }

    IEnumerator PostUser(string url, User user)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user.Name);
        form.AddField("email", user.Email);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form POST complete!");
            }
        }
    }

    IEnumerator PutUser(string url, User user)
    {
        string dataToPut = JsonUtility.ToJson(user, true);
        Debug.Log(dataToPut);
        UnityWebRequest www = UnityWebRequest.Put(url, dataToPut);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Error While Sending: " + www.error);
        }
        else
        {
            Debug.Log("Received: " + www.downloadHandler.text);
        }
    }

    static async Task<string> GetUserAsync(string url)
    {
        HttpResponseMessage response = await client.GetAsync(url);
        HttpContent content = response.Content;
        User u = new User();
        string GetResult = null;
        if (response.IsSuccessStatusCode)
        {
            if (response.IsSuccessStatusCode)
            {
                GetResult = await content.ReadAsStringAsync();
            }
        }
        //JsonUtility.FromJsonOverwrite(GetResult, u);
        //u = (User)JsonUtility.FromJson(GetResult, typeof(User));
        return GetResult;
    }

    static async void PostUserAsync(string url, User user)
    {
        var userValues = new Dictionary<string, string>
        {
            { "username", user.Name },
            { "email", user.Email }
        };

        var content = new FormUrlEncodedContent(userValues);

        var response = await client.PostAsync(url, content);

        var responseString = await response.Content.ReadAsStringAsync();
    }

    static async void PutUserAsync(string url, User user)
    {
        var userValues = new Dictionary<string, string>
        {
            { "username", user.Name },
            { "email", user.Email }
        };

        var content = new FormUrlEncodedContent(userValues);

        var response = await client.PutAsync(url, content);

        var resposeString = await response.Content.ReadAsStringAsync();
    }

    static async void DeleteAsync(string url)
    {
        var response = await client.DeleteAsync(url);
    }

    static async void PostUserDetailsAsync(string url, UserDetails userDetails, CancellationToken token)
    {

        //string httpDate = userDetails.birthday.ToShortDateString();
        //var userDetailsValues = new Dictionary<string, string>
        //{
        //    //{ "user", userDetails.user },
        //    { "birthday", httpDate },
        //    //{ "sex", userDetails.sex.ToString() },
        //    { "position", userDetails.position },
        //    { "status", userDetails.status },
        //    { "third_name", userDetails.third_name }
        //};
        string json = JsonUtility.ToJson(userDetails);

        //DropdownProcessing(client.BaseAddress + url, userDetails.user);
        
        //CheckboxProcessing(client.BaseAddress + url, userDetails.sex);

        var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);
        Debug.Log(json);
        var responseString = await response.Content.ReadAsStringAsync();
        Debug.Log(json);
    }

    void DropdownProcessing(string url, string value)
    {
        HtmlDocument HD = new HtmlDocument();
        var web = new HtmlWeb
        {
            AutoDetectEncoding = false,
            OverrideEncoding = Encoding.UTF8,
        };
        HD = web.Load(url);
        
        var select = HD.DocumentNode.Descendants("select").Where(n => n.Attributes["name"] != null && n.Attributes["name"].Value == "user").ToArray();
 
        if (select != null)
        {
            
            foreach (var select_element in select)
            {
                if (select_element.Attributes["name"] != null && select_element.Attributes["name"].Value == "user")
                {
                    var option = select_element.SelectNodes("option");
                    for (int i = 0; i < option.Count; i++)
                    {
                        if (option[i].Attributes["value"].Value == value)
                        {
                            option[i].SetAttributeValue("selected", "selected");
                            
                        }
                    }
                }
            }
        }
    }

    void CheckboxProcessing(string url, bool value)
    { 
        HtmlDocument HD = new HtmlDocument();
        var web = new HtmlWeb
        {
            AutoDetectEncoding = false,
            OverrideEncoding = Encoding.UTF8,
        };
        HD = web.Load(url);

        var input = HD.DocumentNode.SelectNodes("input");

        foreach (var input_element in input)
        {
            if (input_element.Attributes["type"] != null && input_element.Attributes["type"].Value == "checkbox")
            {
                if (input_element.Attributes["name"].Value == "sex")
                {
                    input_element.SetAttributeValue("value", value.ToString());
                }
            }
        }
    }

    static async void PutUserDetailsAsync(string url, UserDetails userDetails)
    {
        var userDetailsValues = new Dictionary<string, string>
        {
            { "user", "http://127.0.0.1:8000/users/" + userDetails.user.ToString() + "/" },
            { "birthday", userDetails.birthday.ToString() },
            { "sex", userDetails.sex.ToString() },
            { "position", userDetails.position },
            { "status", userDetails.status },
            { "third_name", userDetails.third_name }
        };

        var content = new FormUrlEncodedContent(userDetailsValues);

        var response = await client.PutAsync(url, content);

        var resposeString = await response.Content.ReadAsStringAsync();
    }
}

//{"count":1,"next":null,"previous":null,"results":[{"url":"http://127.0.0.1:8000/users/1/","username":"DML_admin","email":"dml_admin@dml.com","groups":[]}]}