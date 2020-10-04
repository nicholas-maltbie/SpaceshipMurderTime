using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
namespace SpaceShipMurderTime.Settings
{
    public class SettingsHelper : MonoBehaviour
    {
        [SerializeField] InputField usernameField;
        [SerializeField] int maxUsernameChars = 16;
        public void EnforceUsername()
        {
            var stripped = Regex.Replace(usernameField.text, "[^[A-Za-z0-9 ]", "");
            if (stripped.Length > maxUsernameChars)
            {
                stripped = stripped.Substring(0, maxUsernameChars);
            }
            usernameField.text = stripped;
            usernameField.textComponent.text = stripped;
        }
    }
}