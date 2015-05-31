using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Envisage.Model;
using System.Reflection;
using Envisage.Behaviour;

namespace Envisage.Behaviours
{
    public class NotificationHub : MonoBehaviour
    {

        private static NotificationHub _instance = new NotificationHub();
        public static NotificationHub Instance
        {
            get
            {
                return _instance;
            }
        }

        private Dictionary<String, List<Func<String, String, IEnumerator>>> _hub = new Dictionary<String, List<Func<String, String, IEnumerator>>>();

        public void Start()
        {
            Application.ExternalCall("gameReady");

            // Ensure that the NotificationHub object is persisted across scenes
            UnityEngine.Object.DontDestroyOnLoad(this);

            MessageAction messageAction = new MessageAction();

            NotificationHub.Instance.Subscribe("HandleBrowserMessage", this.HandleBrowserMessage);
            NotificationHub.Instance.Subscribe("CreateSphere", messageAction.CreateSphere);
        }

        private IEnumerator HandleBrowserMessage(String json, String callback)
        {
            // process on the next frame
            yield return null;

            Debug.Log(String.Format("{0}: HandleBrowserMessage({1})", this.GetType().ToString(), json));

            var complex = JavaScriptConvert.DeserializeObject<BrowserMessage>(json);

            if (complex != null)
            {
                Debug.Log(complex.Colour);
            }

            if (!String.IsNullOrEmpty(callback))
            {
                // Test Data
                var result = "5";
                this.NotifyBrowser<String>(new Notification<String>()
                {
                    Callback = null,
                    Data = result,
                    MessageName = callback
                });
            }
        }

        public void Publish(String message)
        {
            Debug.Log(String.Format("{0}: Publish message [{1}]", this.GetType().ToString(), message));
            var notification = JavaScriptConvert.DeserializeObject<BrowserMessage>(message);
            Debug.Log(String.Format("{0}: Publish notification [{1}]", this.GetType().ToString(), notification));
            if (Instance._hub.ContainsKey(notification.MessageName))
            {
                var delegates = Instance._hub[notification.MessageName];

                if (delegates != null && delegates.Count > 0)
                {
                    foreach (var func in delegates)
                    {
                        StartCoroutine(func(notification.Data, notification.Callback));
                    }
                }
                else
                {
                    Debug.Log(String.Format("No delegates found for: {0}", notification.MessageName));
                }
            }
            else
            {
                Debug.Log(String.Format("Could not find: {0}", notification.MessageName));
            }
        }

        public void Subscribe(String messageName, Func<String, String, IEnumerator> function)
        {
            Debug.Log(String.Format("{0}: Subscribe to [{1}]", this.GetType().ToString(), messageName));
            if (!Instance._hub.ContainsKey(messageName))
            {
                Instance._hub.Add(messageName, new List<Func<String, String, IEnumerator>>());
            }
            Instance._hub[messageName].Add(function);
        }

        public void NotifyBrowser<T>(Notification<T> notification)
        {
            var data = JavaScriptConvert.SerializeObject(notification);
            Debug.Log("Data: " + data);
            Application.ExternalCall("publish", data);
        }
    }
}
