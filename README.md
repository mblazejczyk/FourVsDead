# Four vs dead

## Description
Four vs Dead is an advanced multiplayer co-op game. Alone or with up to three additional players, you must defeat enemies coming in waves. The game offers:
- A complex skill tree system
- Various types of weapons with different properties
- Different types of armor providing unique benefits
- The ability to purchase up to two upgrades for the entire team
- Several different types of enemies with unique abilities – including melee fighters, ranged attackers, and fire-based enemies
- An inventory system featuring unique badges earned throughout the game
- A ban system
- A chat system
- A reporting system
and much more

Below is a list and description of the scripts, as well as installation instructions for the project.

## Warning! ⚠️
This game was not created for commercial purposes. Integrating scripts on your own servers may expose them to attacks, for which the author bears no responsibility.
The game was developed before learning proper security systems and may be vulnerable to basic attacks such as SQL injection.

## List of Main Scripts

| Script Name         | Description |
|---------------------|-------------|
| ArmorController      | Manages player armor, including changes and its effect on character stats. |
| BarycadeSystem      | Handles barricades in the game – repairing, damage, and interactions with players and enemies. |
| BossBorderControl   | Deals damage to a player when they are in a designated zone. |
| Bullet              | Handles projectile destruction upon impact or after a set time. |
| ButtonSfxController | Plays sound effects for UI buttons. |
| BuyZone            | Manages purchase zones, allowing players to buy weapons, armor, and upgrades. |
| ChatSystem         | Implements the multiplayer chat system. |
| checkingYsServer   | Checks the connection status to the database server. |
| CursorController   | Manages the mouse cursor and pause menu handling. |
| DestroyMe         | Automatically removes an object after a set time. |
| DoorSystem        | Manages doors in the game, including opening, closing, and activating enemy waves. |
| EnemyTracker      | Tracks a random enemy and rotates the object toward them. |
| EqItemHandler     | Handles interactions with inventory items. |
| EqSystem          | Implements the player’s inventory system, allowing item browsing and usage. |
| Fire             | Creates a fire effect that deals damage to the player. |
| Firework         | Generates a fireworks effect with random colors and sounds. |
| GameTimer        | Counts game time and displays it on screen. |
| GameUpgradesController | Manages the game upgrade system and purchases. |
| GunController    | Handles shooting, ammo, and weapon changes for the player. |
| Launcher        | Manages game server connections and multiplayer room handling. |
| loginHandler    | Stores player login data and progress. |
| LoginProfileManager | Displays the player's profile, experience level, and stats. |
| loginSystem     | Manages player login and registration. |
| MatchAudioController | Plays sound effects related to enemy waves and area unlocking. |
| MatchController | Manages enemy waves and their generation. |
| MaxPlayerCheck  | Controls the maximum number of players in a lobby. |
| Menu           | Handles the in-game menu system. |
| MenuManager    | Manages opening and closing the game menu. |
| OnBadgeHover   | Handles interactions with badges. |
| OpenURL        | Opens specified links in a web browser. |
| PingAndFps     | Displays the current ping and frames per second. |
| PlayerController | Manages player movement and interaction. |
| PlayerInfoController | Manages the display of player information. |
| PlayerListitem | Represents players on the lobby list. |
| PlayerManager  | Handles player data management in the game. |
| Referencer     | Facilitates access to various components in the game. |
| ReportSystem   | Allows players to report other users. |
| RescueSystem   | Manages player revival mechanics. |
| RewardSaver    | Stores player progress and rewards. |
| RewardSystem   | Manages the in-game rewards system. |
| RoomListitem   | Represents rooms in the game lobby. |
| RoomManager    | Manages multiplayer rooms in the game. |
| SettingsMenu   | Handles the game settings menu. |
| ShootLineController | Manages shooting animations for the player. |
| SmoothCamMovement | Provides smooth camera movement. |
| SpawnerColider | Handles collisions related to object spawning. |
| SqlController  | Manages connections to the SQL database. |
| UpgradeMenager | Manages the game’s upgrade system. |
| UpgradeUiController | Manages the upgrade interface display. |

## List of Secondary Scripts

| Script Name                  | Description |
|------------------------------|-------------|
| StartNextTutorial             | Starts the next tutorial stage upon entering a trigger. |
| TutorialController            | Manages the tutorial, plays sounds, and displays text. |
| ArmorSO                       | ScriptableObject storing armor stats. |
| Gun                           | ScriptableObject storing weapon information. |
| ItemSO                        | ScriptableObject storing item data. |
| Waves                         | ScriptableObject defining enemy waves. |
| IBarycadeDmg                  | Interface defining the method for changing barricade HP. |
| IDamagable                    | Interface defining the method for taking damage. |
| IPlayerDamage                 | Interface for modifying player HP. |
| EnemyController_archer        | Controls the archer enemy, managing movement and attacks. |
| EnemyController_boss          | Controls the boss enemy, handling abilities and attacks. |
| EnemyController_firerunner    | Controls the fast-moving enemy that ignites areas. |
| EnemyController               | Basic enemy controller, managing movement and damage. |

## Database Description

| Table Name | Description |
|------------|-------------|
| **accounts** | Stores player logins, passwords, and progress data. |
| **bans** | List of bans imposed on users, including reasons and expiration dates. |
| **connectionCheck** | Stores connection check information. |
| **inviteCodes** | Contains game invite codes. |
| **itemsOwned** | List of items owned by players. |
| **lastLogin** | Stores users' last login dates. |
| **moderators** | Contains the list of game moderators. |
| **reports** | Stores user reports with reasons and chat logs. |
| **saves** | Saves game statistics, such as zombies killed, coins collected, and damage dealt. |

## Installation Instructions
1. Clone the repository
2. Open the `Unity project` folder in Unity 2021.3 or newer
3. Register and log in to [PhotonNetwork](https://www.photonengine.com/)
4. Create a new Multiplayer Pun application
5. Copy the App ID
6. In the Unity project, go to `Photon/PhotonUnityNetworking/Resources`
7. In the Photon Server Settings object, change `App Id Pun` to the copied ID from the dashboard
8. Import the SQL file from the `Database` folder to your SQL server
9. Upload the PHP file from the `PHP` folder to your server
10. In the `sqlmanager.php` file, enter your username, password, and database in the first four lines in the appropriate places
11. In the Unity project, in the `sqlController.cs` file at line `24` and in `loginSystem.cs` at line `61`, change the URL to match your `sqlmanager.php` file on your server
12. At this point, the project should work correctly.

Due to GDPR regulations, user creation has been removed; you must create such a system yourself or manually add players to the database. The `password` column contains a standard PHP hash, and `UpgradesSave` should contain 25 zeros.

## Requirements
- Unity 2021 or newer
- Photon PUN for multiplayer functionality
- SQL Server
- PHP 7 or newer server support

## Author
Project created by Mateusz Błażejczyk.

