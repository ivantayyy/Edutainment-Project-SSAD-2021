using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class MainMenuUIManager : MonoBehaviour
    {
        public static MainMenuUIManager instance;
        public GameObject mainMenuUI;
        public GameObject leaderBoardUI;
        public GameObject backbtn;
        private void Awake()
        {
            if (instance == null)
            {
                mainUI();
                instance = this;
            }

        }

        /**
         * Displays main menu UI
         */
        public void mainUI()
        {
            clearscreen();
            mainMenuUI.SetActive(true);
            backbtn.SetActive(true);
        }

        /**
         * Displays leaderboard UI
         */
        public void leaderBoard()
        {
            clearscreen();
            leaderBoardUI.SetActive(true);
        }

        /**
         * Returns to main menu UI
         */
        public void back()
        {
            clearscreen();
            mainMenuUI.SetActive(true);
        }

        /**
         * Clears all screens
         */
        public void clearscreen()
        {
            leaderBoardUI.SetActive(false);
            mainMenuUI.SetActive(false);
        }
    }
}