# Dialogue Dreamland 🤖💬
A Unity project that uses ChatGPT to give NPCs personalities. 
 
<span style="color: red;">Important:</span>
The assets in this project are not contained in this repository. See
[Not included](#notincluded) for a list of assets used in this project.

## Unity version:
[2022.2.1f1](./ProjectSettings/ProjectVersion.txt)

## Goal
- Build a Unity project that uses [ChatGPT](https://openai.com/blog/chatgpt/) to give NPCs distinct personalities.
- Create a WebGL build that can be used in a browser.

## Plugin Contents

### Tools
- [RiderFlow](https://www.jetbrains.com/riderflow/) - Scenery tool to build and manage your 3D space

### Unity Packages
- [Cinemachine](https://unity.com/unity/features/editor/art-and-design/cinemachine) to manage the camera.
- [Unity Addressables](https://docs.unity3d.com/Manual/Addressables.html) to load assets at runtime.
- [Unity Input System](https://docs.unity3d.com/Manual/com.unity.inputsystem.html) to handle input.
- [Unity TextMeshPro](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html) to render text.
- [UI Toolkit](https://docs.unity3d.com/Manual/UIElements.html) to create the UI.


### Not included
- DDAssets
    - [Mega Animations Pack](https://assetstore.unity.com/packages/3d/animations/mega-animations-pack-162341)
    - [Character Controller Pro](https://assetstore.unity.com/packages/tools/physics/character-controller-pro-159150)
    - [POLYGON City - Low Poly 3D Art by Synty](https://assetstore.unity.com/packages/3d/environments/urban/polygon-city-low-poly-3d-art-by-synty-95214)
    - [POLYGON Icons Pack - Low Poly 3D Art by Synty](https://assetstore.unity.com/packages/3d/gui/polygon-icons-pack-low-poly-3d-art-by-synty-202117)
- Plugins
  - [Odin Inspector and Serializer](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)
  - [DOTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
## TODO:
- Build a UI that allows the user to interact with the NPCs.
    - Input field for the user to type a message.
    - Output field for the NPC to respond.
    - Button to send the message.
    - Button to clear the input field.
    - Button to stop chatting
- Build a Character system that can be used for both the player and NPCs.
- Build a Chat system that will handle the ChatGPT API calls.
- World UX
    - Fix colliders and layers so that camera doesn't clip through the floor or walls.

<br>
<div align="center">
  <p>Made by <a href="https://github.com/p-v-z">Petrie van Zyl</a></p>
</div>
