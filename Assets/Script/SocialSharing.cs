using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class SocialSharing : MonoBehaviour
    {
        public string FBNameParam = "Try out this questions!?";
        public string FBDescriptionParam = "";
        private const string FB_ADDRESS = "https://www.facebook.com/SSAD-100602375489555";
        private const string FB_LANG = "en";

        public string twitterNameParam = "Try out this questions!?";
        public string twitterDescriptionParam = "";
        private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
        private const string TWITTER_LANG = "en";

        /**
         * Redirects user to twitter website.
         * Requires user to log in if they are not logged in yet.
         * Opens create post dialog with set message.
         */
        public void PressedTwitter()
        {
            Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(twitterNameParam + "\n" +
                twitterDescriptionParam));

        }

        /**
         * Redirects user to a public facebook page.
         * Users can create post if they are logged in to facebok.
         */
        public void PressedFB()
        {
            Application.OpenURL(FB_ADDRESS + "?text=" + WWW.EscapeURL(FBNameParam + "\n" +
                FBDescriptionParam));
        }

    }
}