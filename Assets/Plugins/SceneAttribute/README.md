## Scene Attribute / Scene Reference README.
Thank you for downloading the Scene Reference.
This attribute was designed to give a user-friendly interface for dealing with scenes rather than strings while editing.

### How to Access:
This script is an all-in-one script containing the class and PropertyDrawer. It can be moved wherever you require it within your project thanks to it's all inclusive nature. Tested to run without any issues during editor and after building.

### How to reference:
By default the SceneAttribute is apart of the default namespace meaning you can reference it wherever. This can be changed freely depending on your project's requirements and codebase organisation.

By using the attribute [Scene] above any string related type, the string will be transformed into an object field for SceneAssets. this will allow you to select any and all of your scenes as a dropdown in the inspector as well as give you the ability to drag and drop.

Since it's just a string, you can reference it in scripts without any changes to Unity's SceneManagement workflow.

### Recommended Path:
- "Assets/Runtime/Scripts/Utility/SceneAttribute.cs"