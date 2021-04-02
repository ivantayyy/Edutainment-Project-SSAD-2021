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
    * Turns MainMenuScript(object) to `dontdestroyonload` object (so it can be referenced in other scenes)
    * Connects to photon on `awake()`
    * singlePlayer() – assigned to singleplayerbutton
      * sets `isMultiplayer` and `isCustom` to `false`
      * Loads ChooseCharacter scene
    * multiPlayer()` – assigned to `multiplayerbutton`
      * sets `isMultiplayer` to `true` and `isCustom` to `false`
      * Loads Multiplayer scene
    * custom()` – assigned to custombutton
      * sets `isMultiplayer` to `false` and `isCustom` to `true`
      * Loads `CustomLobbyCreation` scene
    * summaryReport()` – assigned to summaryreportbutton
      * displays `summaryreportUI`
    *`leaderBoard()` – assigned to `leaderBoardButton`
      * displays leaderboard UI

*SinglePlayer Mode*   
  * Scenes: MainMenu -> ChooseCharacters -> Level_select -> map   

*MultiPlayer Mode*
  * Scenes: MainMenu -> Multiplayer -> Lobby -> ChooseCharacters -> Level_select -> map   
 
*CustomLobby Mode*
  * Scenes: MainMenu -> CustomLobbyCreation -> Lobby -> ChooseCharacters -> Level_select -> map   
  
### Multiplayer  
*MultiplayerMenuController(object)* – contains Multiplayer Menu(script)   
  * Multiplayer Menu(script)
    * createGame() – assigned to creategame button
      * PhotonNetwork.createroom based on createGameInput.text for room name
      * Set player who created room as masterclient (it is normally automatic)
      * Load lobby scene 
    * joinGame() – assigned to joingame button
      * PhotonNetwork.joinroom based on joinGameInput.text for room name
      * Load lobby scene
      
### CustomLobbyCreation
*customLobby(object)* – contains CustomQuestions(script)
 * CustomQuestions(script)
   * Start()
     * Get idToken and localID on start from hardcoded email, password and URL
   * Update()
     * If all questions created, display continueButton
   * nextScene() – assigned to continueButton
     * PhotonNetwork.CreateRoom based on getLobbyName.text for room name
     * Load lobby scene
   * OnSubmit() – assigned to submit button
     * PostToDatabse() called
     * questionTypeChecker() called
     * reset inputfields to “”
   * submitQuestionChange() – assigned to getQuestion button
     * retrieveCustomInfo() called
   * PostToDatabase()
     * Create mcqData object from inputfields
     * Reset quizcounterholder/quizcountertext and questioncounterholder/questioncountertext back to 1 when 15 questions reached.
     * Reset questioncounterholder/questioncountertext every 3 questions
     * Update quizcounterholder/quizcountertext and questioncounterholder/questioncountertext otherwise when new questions submitted 
     * Insert question into database
     * If 15 questions submitted, allQuestionsCreated = true
   * questionTypeChecker()
     * Check that all 3 questions of a quiz is of a single type(mcq or saq)
   * retrieveCustomInfo()
     * get previously entered questions from database for edits, based on selectQuestion and selectQuiz dropdown
     * reInsertQuestion() called
   * reInsertQuestion()
     * display question from database on inputfield

### Lobby
*LobbyController(object)* – contains lobbyController(script)
 * lobbyController(script)
   * Start()
     * Set photon sync scene settings to true
     * Add “PlayerReady” state to player properties for every PhotonNetwork player
     * Call LateStart Coroutine to display room name
   * Update()
     * Display start button to masterclient only when all players are ready
     * When a player is ready, display ready text below their name
   * readyClick() - assigned to ready button
     * Set PhotonNetwork player’s “PlayerReady” state in player properties to true
   * characterSelection() - assigned to start button
     * Load ChooseCharacters Scene
   * allPlayersReady()
     * Check is all player’s “PlayerReady” state is true
*Emailer(object)* – contains EmailFactory(script)
 * EmailFactory(script)
   * Sends email based on inputfields

### ChooseCharacters
*CharacterSelectionController(object)* - contains CharacterSelection (script)
 * CharacterSelection(script)
   * Awake()
     * Find mainMenuScript to get isCustom and isMultiplayer values
   * Start()
     * Set PhotonNetwork to sync scenes for all players
     * if multiplayer or custom mode, reset PhotonNetwork player’s “PlayerReady” state in player properties to false
   * Update()
     * if multiplayer or custom mode, display start button to masterclient when all players are ready
     * When a player is ready, display ready text below their name
     * When a player has choosen a character, display text of chosen character
   * aPress() - assigned to alexis button
     * Assign SelectedCharacter(object)'s selection to alexis
   * cPress() - assigned to chubs button
     * Assign SelectedCharacter(object)'s selection to chubs
   * jPress() - assigned to alexis button
     * Assign SelectedCharacter(object)'s selection to john
   * startGame()
     * if singleplayer mode, PhotonNetwork.joinroom
     * Load Level_select scene
   * readyClick() - assigned to ready button
     * Set PhotonNetwork player’s “PlayerReady” state in player properties to true
   * allPlayersReady()
     * Check is all player’s “PlayerReady” state is true
*SelectedCharacter(object)* contains SelectedCharacter(script)  
 * SelectedCharacter (script)
   * Turns SelectedCharacter(object) to `dontdestroyonload` object (so it can be referenced in other scenes)
   * Stores character selection

### Questions
*QuestionSign(script)* - assigned to quiz sign (weapon or trash)   
 * Start()
   * restartQuiz(quizManager) called 
 * Update()
   * if player in range and 'spacebar' pressed
     * if questions screen is not open, display questions screen 
     * else close questions screen and call restartQuiz(quizManager)
 * OnTriggerEnter2D(Collider2D) 
   * Sense if player's and quiz sign's box collides
   * Sets playerInRange to true
 * OnTriggerExit2D(Collider2D) 
   * Sense if player leaves and quiz sign's box collider
   * Sets playerInRange to false
   * Close questions screen
 * restartQuiz(quizManager)
   * Set quiz animations to default
   * Rest quizManager's currentQuestion and number of correct questions to 0
   * Calls quizManager's generateQuestion() 
   
*AbstractQuizManager* 
* correct()
  * Increase numCorrect by 1
  * Increase current question by 1
  * if current question is not the last question, call generateQuestion()
* wrong()
  * Increase current question by 1
  * if current question is not the last question, call generateQuestion()
* generateQuestion()
  * Display next question from QnA(list of QuestionAndAnswer object)
  * SetAnswers() called
* SetAnswers() abstract method
  * MCQ (weaponsquizmanager) implementation  
    * Set submit button's WeaponAnswer(script)'s isCorrect variable to false
    * Display question options text on toggles
  * SAQ (trashquizmanager) implementation
    * Set submit button's WeaponAnswer(script)'s isCorrect variable to true or false base on correct variable logic
* checkAns() abstract method
  * MCQ (weaponsquizmanager) implementation  
    * Check if the correct toggle/answer is selected
    *  Set submit button's WeaponAnswer(script)'s isCorrect variable to true or false base on above
  * SAQ (trashquizmanager) implementation
    * Compare inputField's text to correct answer from QnA Answer
    * if text are the same, set correct to true, else set to false
    
*AbstractAnswer(script)* - attached to submit button
* answer()
  * if isCorrect variable is true
    * playAnimation() called
    * quizManager.correct() called
  * else 
    * quizManager.wrong() called

*LoadQuestions(script)*
* Start()
  * Get idToken and localID from database
  * StartCoroutine to load 15 questions
  * StartCoroutine within coroutine to assign questions to individual quizmanagers on the map 
