![home-dashboard](https://socialify.git.ci/iamtomhewitt/home-dashboard/image?description=1&forks=1&issues=1&logo=https%3A%2F%2Fraw.githubusercontent.com%2Fiamtomhewitt%2Fhome-dashboard%2Fmaster%2FAssets%2FImages%2FIcon.png&pattern=Floating%20Cogs&stargazers=1&theme=Light)
<p align="center"><img src="Github/demo.gif" width="700"></p>
<p align="center">Built with ❤︎ by <a href="https://github.com/iamtomhewitt">Tom Hewitt</a></p>

------------

Originally created using the [Smashing](https://github.com/Smashing/smashing) dashboard in Ruby, but moved to a [Unity3D](https://unity.com/) project as I was after some interactivity (such as adding and removing from a shopping list) instead of just displaying information (And also I could customize the widgets further than the Smashing dashboard).

## 🎯 Widgets
The current set of widgets on my dashboard are:

### 📰 BBC News
A widget that cycles headlines every minute. New headlines are fetched over time. Tapping on the BBC news widget opens up the displayed headline on the BBC news website.

### 🚮 Bin Day
An alert to show if the bin needs putting out. Changes colour based on which bin to put out.

### 🕒 Clock
A clock showing the date and time.

### 🥘 Food Planner
An editable set of 'cards' to show what we're having for our evening meal for the week.
I've also incorporated a set of predefined recipes from what we eat often. Pressing the button at the bottom of the planner will add every single ingredient from all the predefined recipes for that week to the shopping list widget, so you can plan your weekly shop based on what you are eating. Duplicate ingredients are also removed, so if you have a recipe of 200g of chicken, and another recipe with 400g of chicken, then 600g of chicken is added to your shopping list.

The recipes and planner is stored on an [api I built](https://github.com/iamtomhewitt/home-dashboard-recipe-manager).

### 📆 Google Calendars
A scrollable widget showing mine and my partners calendar for the next three months. Any edits to calendar events are done through the Google Calendar app itself and not the dashboard.

### 🚗 Journey Planner
Shows the journey time from my house to a certain destination. Changes colour based on the amount of traffic. Can list multiple journeys.

### 📝 Online Lists
Scrollable list widgets - one for TODOs, and one for the shopping list. The lists are integrated with the [Todoist](https://todoist.com) app, so I can add items to the shopping list via the app on my mobile.

### 💰 Splitwise
A widget that shows the amounts owed in our Splitwise group. It uses an [api I built](https://github.com/iamtomhewitt/home-dashboard-splitwise-manager).

### 🚂 Trains
A scrollable widget showing the latest trains from my local stations, showing any cancellations or delays. It uses a [Huxley](https://github.com/jpsingleton/Huxley) for its data.

### ☀ Weather
Forecast for now, and the next four days. It uses [Darksky](https://darksky.net/dev) for its data.