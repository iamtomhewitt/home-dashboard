# 👷‍♂️ Getting Started

## 🔑 API Keys & Apps
In order for the widgets to work, some need access keys (also known as API keys), or apps from the App Store.

Here's a list of what you need to do to get your dashboard up and running.

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

	* https://todoist.com/app#project%2F ***3222176134*** %2Ffull

	* Your project ID is: `3222176134`

	Make a note of these as we need them later.

* Get an API key. You can do this by navigating to `Settings>Integration>API Token` from the Todoist web page. Save your API key for later.

### 🚂 Trains
* Go to [Huxley API](https://huxley.apphb.com/) and register for an account to get an API key. For 'proposed usage' select software. Save it for later.
* Find out your train station code from [here](https://www.nationalrail.co.uk/stations_destinations/48541.aspx)

### ☀️ Weather 
* Go to [Darksky](https://darksky.net/dev) and register for an API key.
* Find out your latitude and longitude for where you want your weather. Save the values for later.

## 📊 Add to Dashboard
* Open the settings page from your dashboard by clicking the '⚙' icon.
* Fill out the settings with the access keys from above.
* Click save.
* Your dashboard will pick up the changes the next time the widgets refresh. If you can't wait that long, just restart the dashboard app.