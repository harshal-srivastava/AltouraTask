using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserLoadSaveManager : MonoBehaviour
{
    [SerializeField]
    private string saveFileName;

    private string saveFilePath;

    private Dictionary<string, string> availableUserDictionary;

    public Users testData;

    public delegate void NewUserInfoSaved(UserData user);
    public static NewUserInfoSaved NewUserInfoSavedSuccessEvent;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.dataPath, "Resources", saveFileName + ".json");
        LoadAvailableUsersFile();
    }

    void LoadAvailableUsersFile()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            testData = new Users();
            testData = JsonUtility.FromJson<Users>(json);
            PopulateAvailableUserDictionary(testData.users);
            Debug.Log("Data loaded from " + saveFilePath);
            
        }
        else
        {
            Debug.LogError("file not found, creating default user");
            CreateDefaultData();
            LoadAvailableUsersFile();
        }
    }

    void PopulateAvailableUserDictionary(List<UserData> usersFetched)
    {
        availableUserDictionary = new Dictionary<string, string>();
        for (int i=0;i<usersFetched.Count;i++)
        {
            availableUserDictionary.Add(usersFetched[i].userName, usersFetched[i].userPassword);
        }
    }

    public void SaveUserToFile(UserData dataToSave)
    {
        Users userList = new Users();
        // Load existing users from the JSON file
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            userList = JsonUtility.FromJson<Users>(json);
        }
        else
        {
            userList.users = new List<UserData>();
        }


        // Add the new user to the list
        userList.users.Add(dataToSave);

        // Convert back to JSON
        string updatedJson = JsonUtility.ToJson(userList);

        // Save the updated JSON to the file
        File.WriteAllText(saveFilePath, updatedJson);
        LoadAvailableUsersFile();
        NewUserInfoSavedSuccessEvent?.Invoke(dataToSave);
        Debug.Log("Saved users to path : " + saveFilePath);
    }

    void CreateDefaultData()
    {
        UserData defaultUserData = new UserData();

        defaultUserData.userName = "DefaultUser";
        defaultUserData.userPassword = "DefaultPassword";

        SaveUserToFile(defaultUserData);

    }

    public bool CheckUser(string name)
    {
        return availableUserDictionary.ContainsKey(name);
    }

    public bool IsPasswordCorrect(UserData data)
    {
        if (availableUserDictionary.ContainsKey(data.userName))
        {
            string password = "";
            if (availableUserDictionary.TryGetValue(data.userName, out password))
            {
                if (data.userPassword == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    CreateDefaultData();
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    LoadAvailableUsersFile();
        //}
    }
}

[System.Serializable]
public class Users
{
    public List<UserData> users;
}

[System.Serializable]
public class UserData
{
    public string userName;
    public string userPassword;
}



