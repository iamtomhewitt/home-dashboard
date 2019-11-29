# 🛠 Getting Started

Follow these steps to build and run your own dashboard.

Requirements:
1. [Unity3D](https://unity.com) (with the Android module installed).
2. An Android tablet.

## 💻 Install Unity
1. Install the latest version of Unity. Easiest way is to install via Unity Hub.
2. When installing, make sure you select the Android Build Module from the list of modules.

## 🔑 API Keys & Apps
In order for the widgets to work, some need API keys, or external apps.

### 📰 BBC News
* Go to [News API website](https://newsapi.org/) and register to get an API key. Save it for later.

### 📅 Google Calendar
* Go to [Google Calendar API](https://developers.google.com/calendar/quickstart/js) and follow the instructions.
* You will need to be signed into Google, and may have to make your Google Calendar public.
* Make a note of your API Key and Calendar ID, you will need it later.
* [This section requires further updates]

### 📝 Online Lists (Todoist)
* Signup to [Todoist](https://todoist.com/). Use the web version for the following steps, but you can use the app to update your dashboard whilst on the move!
* Create two new projects, one called 'TODO' and one called 'Shopping'.
* Click on each of the projects you have just created. Find out the project ID for each one by inspecting the URL. For example:

	* https://todoist.com/app#project%2F3222176134%2Ffull

	* Your project ID is everything between `%2F`, for example:

	* https://todoist.com/app#project%2F***3222176134***%2Ffull

	* Your project ID is: `3222176134`

	Make a note of these as we need them later.

* Get an API key. You can do this by navigating to `Settings>Integration>API Token` from the Todoist web page. Save your API key for later.

### 🚂 Trains
* Go to [Huxley API](https://huxley.apphb.com/) and register for an account to get an API key. For 'proposed usage' select software. Save it for later.
* Find out your train station code from [here](https://www.nationalrail.co.uk/stations_destinations/48541.aspx)

### ☀️ Weather 
* Go to [Darksky](https://darksky.net/dev) and register for an API key.
* Find out your latitude and longitude for where you want your weather. Save the values for later.

## 👷🏻‍♂️ Build Your Dashboard

### ⚙️ Config File
* Using the `config-template.json' provided, fill out the config file with the API keys you have retrieved. 
* Optionally, save it as a new file.

### 📊 Dashboard
* Open the `Dashboard.unity` scene.
* Find the `Config` GameObject, and drag your config file into the `Config File` variable slot in the inspector.
* In the hierarchy, select each widget. Some variables will need updating to match your config file. For example, the Google Calendar widget has a variable `apiKeyConfigKeyName` which needs to match your config file. This is to help distinguish between multiple calendars.
* Run it! Press the play button and see your dashboard populate.

### 📱 Deploy to Android
* Plug in your Android device to your computer. Make sure you have all your drivers updated and the Android SDK.
* Open the Build Settings in Unity, and click Build & Run.
* Any issues, consider the [troubleshooting](https://docs.unity3d.com/Manual/TroubleShootingAndroid.html) guide from Unity.
