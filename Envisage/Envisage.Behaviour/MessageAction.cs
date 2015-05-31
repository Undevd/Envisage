using Envisage.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Envisage.Behaviour
{
    public class MessageAction : MonoBehaviour
    {
        public IEnumerator CreateSphere(String json, String callback)
        {
            yield return null;
            Debug.Log(String.Format("{0}: CreateSphere json [{1}]", this.GetType().ToString(), json));
            BrowserMessage bm = JavaScriptConvert.DeserializeObject<BrowserMessage>(json);
            Debug.Log(String.Format("{0}: CreateSphere bm Colour [{1}]", this.GetType().ToString(), bm.Colour));
            if (bm.State.Equals("Active"))
            {
                var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                ball.name = bm.Name;
                ball.transform.position = new Vector3(0.0f, 9.0f, 0.0f);
                ball.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                Rigidbody rigidBody = ball.AddComponent<Rigidbody>();
                rigidBody.mass = 2.0f;
                rigidBody.useGravity = true;
                rigidBody.AddTorque(1.0f, 0.0f, 1.0f, ForceMode.Force);

                Renderer render = ball.GetComponent<Renderer>();
                switch (bm.Colour)
                {
                    case "Red":
                        render.material = Resources.Load("Blue") as Material;
                        break;
                    case "Blue":
                        render.material = Resources.Load("Red") as Material;
                        break;
                    case "Purple":
                        render.material = Resources.Load("Purple") as Material;
                        break;
                    case "Black":
                        render.material = Resources.Load("Black") as Material;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                GameObject ball = GameObject.Find(bm.Name);
                if (ball != null)
                {
                    Destroy(ball.GetComponent<Rigidbody>());
                    GameObject spotLight = GameObject.Find("SpotLight");
                    ball.transform.parent = spotLight.transform;
                    ball.transform.position = spotLight.transform.position;
                    ball.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

                    StartCoroutine(DesSphere(ball));
                }
            }
        }

        private IEnumerator DesSphere(GameObject gameObject)
        {
            yield return new WaitForSeconds(2);

            Destroy(gameObject);
        }
    }
}
