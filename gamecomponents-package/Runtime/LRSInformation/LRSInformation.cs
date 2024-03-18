using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousGameComponents.LRSComponent
{
    /// <summary>
    /// Simple yet essential class to manage the LRS Information that will be used by the components in a general way. This is a singleton, meaning that can exist only one LRSI in the scene. Any other will be automatically deleted
    /// </summary>
    public class LRSInformation : MonoBehaviour
    {
        [Tooltip("URL of the wished LRS")]
        public string LRSUrl = "";
        [Tooltip("LRS credential Key value")]
        public string remoteKey = "";
        [Tooltip("LRS credential Secret value")]
        public string remoteSecret = "";
        [Space]
        [Tooltip("Identification of the player for the xAPI to register. Usually it's an email address")]
        public string agent = "";

        private static LRSInformation singleton;
        public static string LRSILRSUrl { get; private set; }
        public static string LRSIremoteKey { get; private set; }
        public static string LRSIremoteSecret { get; private set; }
        public static string LRSIAgent { get; private set; }

        private void Awake()
        {
            LRSIremoteKey = remoteKey;
            LRSILRSUrl = LRSUrl;
            LRSIremoteSecret = remoteSecret;
            LRSIAgent = agent;
        }

        private void Update()
        {
            if (singleton == null)
                singleton = this;

            if (singleton != this)
                Destroy(this);

            LRSIremoteKey = remoteKey;
            LRSILRSUrl = LRSUrl;
            LRSIremoteSecret = remoteSecret;
        }


    }
}
