# 💻 Development

Follow these steps to build and run your own dashboard.

Requirements:
1. [Unity3D](https://unity.com) (with the Android module installed).
2. An Android tablet.

## 💻 Install Unity
1. Install the latest version of Unity. Easiest way is to install via Unity Hub.
2. When installing, make sure you select the Android Build Module from the list of modules.

## 🔑 API Keys & Apps
You'll need a set of API keys for each fo the widgets, which can be found [here](GETTING&#32;STARTED.md).

## 👷🏻‍♂️ Build Your Dashboard

### ⚙️ Config File
* Using the `config-template.json' provided, fill out the config file with the API keys you have retrieved. 
* Optionally, save it as a new file.

### 📊 Dashboard
* Open the `Dashboard.unity` scene.
* Find the `Config` GameObject, and drag your config file into the `Config File` variable slot in the inspector.
* In the hierarchy, select each widget. Some variables will need updating to match your config file. For example, the Google Calendar widget has a variable `gmailAddress` which needs to match your config file. This is to help distinguish between multiple calendars.
* Run it! Press the play button and see your dashboard populate.

### 📱 Deploy to Android
* Plug in your Android device to your computer. Make sure you have all your drivers updated and the Android SDK.
* Open the Build Settings in Unity, and click Build & Run.
* Any issues, consider the [troubleshooting](https://docs.unity3d.com/Manual/TroubleShootingAndroid.html) guide from Unity.
