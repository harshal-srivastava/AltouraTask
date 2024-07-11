using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Class responsible for saving the new user information to the json file in the resources folder
/// Handling the user information locally by saving the usernames and passwords in a local json file present in resources folder
/// </summary>
public class UserLoadSaveManager : MonoBehaviour
{
    [SerializeField]
    private string saveFileName;

    private string saveFilePath;

    private Dictionary<string, string> availableUserDictionary;

    public Users testData;

    /// <summary>
    /// Delegate coupled with static event to send application wide event
    /// When a new user information is saved successfully
    /// </summary>
    /// <param name="user"></param>
    public delegate void NewUserInfoSaved(UserData user);
    public static NewUserInfoSaved NewUserInfoSavedSuccessEvent;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.dataPath, "Resources", saveFileName + ".json");
        LoadAvailableUsersFile();
    }

    /// <summary>
    /// Function to read the json file, create a new one if it doesn't exists
    /// Transfer the information retrieved to the availableUserDictionary
    /// Using dictionary helps in fast user information fetching
    /// </summary>
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

    /// <summary>
    /// Transfer the data retreived from the json file to the availableUserDictionary dictionary
    /// Would help in fast user data fetching
    /// Username is key, password is value in the dictionary
    /// </summary>
    /// <param name="usersFetched"></param>
    void PopulateAvailableUserDictionary(List<UserData> usersFetched)
    {
        availableUserDictionary = new Dictionary<string, string>();
        for (int i=0;i<usersFetched.Count;i++)
        {
            availableUserDictionary.Add(usersFetched[i].userName, usersFetched[i].userPassword);
        }
    }

    /// <summary>
    /// Function to save the new user information to the json file
    /// </summary>
    /// <param name="dataToSave"></param>
    /// <param name="defaultSave"></param>
    public void SaveUserToFile(UserData dataToSave, bool defaultSave = false)
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
        if (defaultSave) // condition added to avoid an infinite recursive call when a default user is being created
        {
            return;
        }
        NewUserInfoSavedSuccessEvent?.Invoke(dataToSave);
        Debug.Log("Saved users to path : " + saveFilePath);
    }

    /// <summary>
    /// Function to create default user data and save to file
    /// Added to handle the case if the users file does not exists or has been deleted
    /// </summary>
    private void CreateDefaultData()
    {
        UserData defaultUserData = new UserData();
        defaultUserData.userName = "DefaultUser";
        defaultUserData.userPassword = "DefaultPassword";
        SaveUserToFile(defaultUserData, true);
    }

    /// <summary>
    /// Function to check if the user already exists in the json file
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckUser(string name)
    {
        return availableUserDictionary.ContainsKey(name);
    }

    /// <summary>
    /// Function to verify the password entered for a particular user
    /// Fetch the username as key and match for the particular key value
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
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



