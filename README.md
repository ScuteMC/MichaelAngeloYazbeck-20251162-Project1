# Lanternwake

Michael Angelo Yazbeck | Student ID: 20251162 | CSC 231 Project 1

A moth courier carries fallen starlight to amber lantern shrines in a moonlit botanical garden. Gather one light, deliver it, then find the newly relocated pair. Light ten lanterns before dawn.

## Open and play

- Unity version: 6000.5.1f1. Universal 2D render pipeline.
- Main scene: Assets/Lanternwake/Scenes/MainMenu.unity.
- Gameplay scene: Assets/Lanternwake/Scenes/MoonGarden.unity.
- Both scenes are enabled in Build Settings, with MainMenu first.
- Open MainMenu, enter Play Mode, and click PLAY / ENTER THE GARDEN.
- WASD or arrow keys: direct top-down movement. Touch objects to interact.
- R: restart after a win or loss, without leaving Play Mode.
- Escape: return to the main menu.
- White map marker: courier; pale yellow: starlight; orange: destination.

## Submitted Inspector values

| Setting | Value | Component |
|---|---:|---|
| Delivery goal | 10 | RoundController |
| Starting time | 180 seconds | RoundController |
| Normal movement speed | 6 units/second | MothPilot |
| Boosted movement speed | 10 units/second | MothPilot |
| Boost duration | 5 seconds | MothPilot |
| Additional rule: non-final delivery bonus | 5 seconds | RoundController |
| Minimum pair separation | 8 units | RoutePlanner |
| Minimum distance from player | 3 units | RoutePlanner |
| Spawn obstacle clearance | 1 unit | RoutePlanner |

The courtyard is 36 x 28 units. It contains eight solid fern/rock beds, four solid boundaries, 21 accessible spawn markers, and four one-use moonblooms. The garden ground is walkable, including moss between paths. The four moonblooms return on restart. Repeated boosts refresh the timer; they never stack speed. Delivery cancels a boost.

The initial timer is intentionally generous for learning the routes. A crossing takes roughly six seconds at normal speed; 180 seconds plus at most 45 bonus seconds supports ten deliveries with room for navigation and obstacles.

## Exactly one assessed additional mechanic

**Delivery bonus:** each successful non-final delivery adds five seconds exactly once. Empty visits do not count or award time. The final winning delivery gives no bonus. Finished rounds cannot receive a bonus. This rewards successful navigation while keeping one authoritative timer.

## Project structure and explanations

- MothPilot: reads the new Input System's saved Move action, normalizes diagonal input, and assigns Rigidbody2D velocity in FixedUpdate. Velocity is in units per second; the physics engine applies its fixed timestep. Multiplying velocity by deltaTime would incorrectly slow it down. Update reduces the boost timer using Time.deltaTime even when stationary. Inspector fields hold normal/boost speed and duration.
- FollowCamera: smoothly follows the courier in LateUpdate and clamps its center to the courtyard bounds. It moves only the camera, never the level. Snap resets the camera on a new round.
- PickupTrigger, ShrineTrigger and BoostFlower: use OnTriggerEnter2D and the Player tag to identify the courier. Trigger radii and the courier's Rigidbody2D provide actual physics interactions.
- DeliveryRules: owns the single carrying flag. Collection hides the current pickup and displays a carried-light sprite plus particles. Delivery clears cargo before asking the round controller to count, so repeated callbacks cannot count the same item twice.
- RoutePlanner: owns the collection of marker references and previous pair indices. It enumerates all valid ordered pairs, rejects both prior indices for both objects, checks pair/player separation and solid-layer clearance, then randomly selects one valid pair. It never silently relaxes safety conditions. The shipped markers were also checked with a clearance-aware flood fill.
- RoundController: owns remaining time, completed count and Active/Won/Lost state. Update calls Tick with Time.deltaTime; Tick clamps to zero. Delivery is accepted only while active with positive time. The final delivery changes state before relocation or bonus code can run. ResetRound restores player, cargo, particles, timer, score, boost availability, camera and a new pair.
- GardenHud: reads authoritative state and presents it using TextMeshPro. It formats non-negative time, progress, collect/deliver instructions, speed-boost status, map markers and distinct results. It does not own a second gameplay timer or score.
- MenuController: the Play button loads MoonGarden using SceneManager.LoadScene.
- GardenControls.inputactions and action-reference assets: saved new Input System bindings. No legacy Input.GetAxis / Input.GetKey is used.
- Prefabs: FallenStarlight, AmberShrine and Moonbloom. Scene instances have their gameplay references assigned in the Inspector. A newly placed prefab instance needs its scene-specific controller references assigned.
- GardenBuilder (Editor): reproducible authoring helper; creates and wires the scene objects and assets through Unity APIs. The saved scenes run independently of the builder.
- LanternwakeCliBootstrap (Editor): compatibility helper for Pipeline 0.9.0-exp.1 on Unity 6000.5. It retries descriptor publication after reload restrictions end. It is unrelated to gameplay.
- GardenVerification and GardenDemo: editor-only automation, excluded from player compilation and not saved into the scenes. Verification drives virtual keyboard input and real physics callbacks as well as direct boundary-condition checks. Demo recording uses temporary test values in memory.

## Testing and demonstration

See TEST_RECORDS.txt for ten executed test records, including a 1,000-pair randomization check. Both R-key restart paths, timer expiry, zero-time delivery rejection, physical collision and trigger behavior were exercised in Unity Play Mode.

Lanternwake-Demonstration.mp4 is a captioned, scripted recording of the actual Unity game. It uses a temporary goal of two for the win demonstration and an eight-second remaining clock for the loss demonstration, as allowed by the brief. These temporary values are not saved into MoonGarden. The submitted settings remain ten deliveries and 180 seconds.

## Credits and assistance disclosure

This is a new concept and implementation. No Delivery Dash source code, class artwork, or engagement-exercise code was reused.

Implementation, testing automation, documentation and art direction were produced with OpenAI Codex assistance. All seven original raster artwork assets were generated using the native OpenAI ImageGen tool for this project: MenuGarden, Ground, Moth, Starlight, Shrine, Moonbloom and GardenRock. Transparent alpha is preserved for sprites. ASSET_PROMPTS.md records the generation prompts. UI panels, text, map markers and particles are Unity components, not stock art.

Unity, the Input System, Universal Render Pipeline and TextMeshPro are Unity Technologies packages under their supplied licenses. Liberation Sans SDF is supplied in TextMeshPro Essential Resources; its included license files remain in Assets/TextMesh Pro. FFmpeg was used only to encode the demonstration and is not a game dependency.

The author should study the scripts and explanations above and be able to explain and modify them. The rubric includes individual understanding; no automated check can guarantee an instructor's grade.

## Consulted resources

- Supplied CSC231 Project 1 Build a 2D Collection and Delivery Game (1).pdf: all three pages, core requirements, checklist and 100-mark rubric.
- Unity CLI built-in `unity skill show`, including `references/playmode-verification-loop.md` and live command schemas.
- Installed com.unity.pipeline 0.9.0-exp.1 source/documentation, particularly PipelineDescriptorPublisher, CaptureCommands and RuntimeInputCommand.
- Installed com.unity.inputsystem 1.19.0 source, including InputSettings.
- Unity API reference entry points from the assignment: https://docs.unity3d.com/ScriptReference/Time-deltaTime.html and https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html
- Package documentation: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.19/manual/index.html

## Known limitations

No audio is included. The demonstration is captioned rather than narrated. Test and recording helpers contain paths from the development machine; these are optional editor tools and are not needed to open or play the saved scenes. Reusable pickup prefabs require scene-specific references when adding new instances. Unity may need to resolve the listed packages the first time an extracted project opens.

Submission excludes Library, Temp, Logs, UserSettings, .git and generated IDE files. The project does not require a standalone executable.
