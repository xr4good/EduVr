using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginDataManagement : MonoBehaviour
{
    // Start is called before the first frame update
    public void SetEmail(string email)
    {
        UserLoginData.email = email;
    }

    // Update is called once per frame
    public string GetEmail()
    {
        return UserLoginData.email;
    }
}
