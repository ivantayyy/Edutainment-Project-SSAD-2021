# SSAD_Project :pray: :100:

## Steps for installing Dependencies
1. Download Firebase Android SDK [here](https://firebase.google.com/docs/unity/setup) and follow download instructions
     * Note that our firebase Console is already set up. Can ask samuel add u to database
     * Note that the google-services.json script is already set up under `Assets` folder
     * After download. import `FirebaseAuth.Unity` and `FirebaseDatabase.Unity` files from the `DotNet4` folder in your Firebase SDK
2. From the Unity's Asset store search for `JSON.NET For Unity`. Download and import everything
3. From the Unity's Asset store searchh for `proyecto 26 Rest Client` for Unity. Download and import everything
4. Connect to Photon Network with the app id `e2e174c6-2fcd-46ed-9d98-4a96995d254e`


## Info about code
### Main Menu
*MainMenuScript(object)* – contains MainMenu(script)   
  * MainMenu(script)
    1. Turns MainMenuScript(object) to `dontdestroyonload` object (so it can be referenced in other scenes)
    1. Connects to photon on `awake()`
    1. singlePlayer() – assigned to singleplayerbutton
    1. sets `isMultiplayer` and `isCustom` to `false`
    1. Loads ChooseCharacter scene
    1. `multiPlayer()` – assigned to `multiplayerbutton`
    1. sets `isMultiplayer` to `true` and `isCustom` to `false`
    1. Loads Multiplayer scene
    1. `custom()` – assigned to custombutton
    1. sets `isMultiplayer` to `false` and `isCustom` to `true`
    1. Loads `CustomLobbyCreation` scene
    1. `summaryReport()` – assigned to summaryreportbutton
    1. displays `summaryreportUI`
    1. `leaderBoard()` – assigned to `leaderBoardButton`
      * displays leaderboard UI

*SinglePlayer Mode*   
1. Scenes: MainMenu -> ChooseCharacters -> Level_select -> map   

*MultiPlayer Mode*
  * Scenes: MainMenu -> Multiplayer -> Lobby -> ChooseCharacters -> Level_select -> map   
CustomLobby Mode
  * Scenes: MainMenu -> CustomLobbyCreation -> Lobby -> ChooseCharacters -> Level_select -> map   
*Multiplayer*   
1. MultiplayerMenuController(object) – contains Multiplayer Menu(script)   
1. Multiplayer Menu(script)
1. createGame() – assigned to creategame button
1. PhotonNetwork.createroom based on createGameInput.text for room name
1. Set player who created room as masterclient (it is normally automatic)
1. Load lobby scene 
1. joinGame() – assigned to joingame button
1. PhotonN PhotonNetwork.joinroom based on joinGameInput.text for room name
1. Load lobby scene
1. CustomLobbyCreation
1. customLobby(object) – contains CustomQuestions(script)
1. CustomQuestions(script)
1. Start()
1. Get idToken and localID on start from hardcoded email, password and URL
1. Update()
1. If all questions created, display continueButton
1. nextScene() – assigned to continueButton
1. PhotonNetwork.CreateRoom based on getLobbyName.text for room name
1. Load lobby scene
1. OnSubmit() – assigned to submit button
1. PostToDatabse() called
1. questionTypeChecker() called
1. reset inputfields to “”
1. submitQuestionChange() – assigned to getQuestion button
1. retrieveCustomInfo() called
1. PostToDatabase()
1. Create mcqData object from inputfields
1. Reset quizcounterholder/quizcountertext and questioncounterholder/questioncountertext back to 1 when 15 questions reached.
1. Reset questioncounterholder/questioncountertext every 3 questions
1. Update quizcounterholder/quizcountertext and questioncounterholder/questioncountertext otherwise when new questions submitted 
1. Insert question into database
1. If 15 questions submitted, allQuestionsCreated = true
1. questionTypeChecker()
1. Check that all 3 questions of a quiz is of a single type(mcq or saq)
1. retrieveCustomInfo()
1. get previously entered questions from database for edits, based on selectQuestion and selectQuiz dropdown
1. reInsertQuestion() called
1. reInsertQuestion()
1. display question from database on inputfield
