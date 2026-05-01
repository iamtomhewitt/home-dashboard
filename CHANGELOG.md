## Version 7.1.0
Released **01 May 2026 10:24:29am** - *3 commits*
- 1054a9d chore: close menu when entering full screen
- 1303ddd fix: package.json not being included in build
- 5136ae5 fix: add icon for heavy rain

## Version 7.0.1
Released **26 Apr 2026 10:03:08am** - *1 commits*
- aa6eacd fix: catch null items in shopping list

## Version 7.0.0
Released **04 Apr 2026 21:34:27pm** - *158 commits*
- bb03929 author
- c5afd5b put changleog back
- 3deee18 run deploy after release script so versions are shipped properly
- 6b5baac update commitlint, push changes
- b21bddf add deployment scripts
- ac8633d update release script
- ddc8c8a image
- 0429d56 readme
- 25ab739 update http response in backend
- a957956 Update commit checking for master only
- dd6d4ca chore: init husky
- 487cd50 Try adding husky to package
- 782166a try js
- 2892ac5 Add commitlint
- 01bbbd9 http log interceptor and ok modal
- c83c48e rename library file
- 6209024 modal to show latest version
- 61c6c61 rename http to api to allow for more generic requests
- 05143db use pubsub to target refresh a widget
- dc9bbf7 open article when clicking on it
- 2f6a671 remove an ingredient and tidy up
- 2622cf2 style input
- c002c50 delete a recipe
- 2cd8b25 show message
- 7fed2d4 updates to recipe editor
- 0829eca fix updating a recipe lambda
- ec6f09c tidy
- 49983a2 start working on recipe editor
- 2724dd4 fix gmail padding
- 0486e71 fix bin day
- b9ea507 weather icon
- 78743f7 save config via settings page
- b6fc203 filter recipes
- 0a0804d disable confirm button when loading
- ff07951 fix night weather icon
- 63725ff start on recipe manager page
- 0d6c8bc show app version
- cff6c6c update modal css
- 8a5050b finish of settings page
- e76f2cf tidy up css
- 5852e70 padding
- d1be6bc 10pm not am
- 26525be show different icon if it's a clear night
- 15edd35 close menu when pressing a navigation button
- 054d4b6 start implementing a settings page
- 7bdd0c3 weather icon
- e0ca299 update settings page
- dbd93f3 font and css tweaks
- e662d0e update icon
- 15e5bb8 style login error
- 2f64d7d use width and heights from config
- abd5232 menu tweaks
- 7e0b877 style the login screen
- 33b0e3f clear everything on logout
- fb12710 add shopping list to todoist
- 4571cdb populate shopping list
- 6c2b82a create a shopping list endpoint
- f0e6437 optional onNo for confirm modal
- f753d8f style the background of the app based on config
- 8ceafc5 background colour
- 586c1e5 dont open menu straight away
- eaae874 calculate isSelected
- efcc55a split into separate component
- 716940e update styling and button location of menu
- c9b3cd6 initial hamburger menu
- 27b6edc label
- 1634a43 compile
- 9dc216e fix splitwise widget when all settled up
- b8198b2 colouring buttons
- 8e7b1e0 show recipe details, rightsize and scroll large modals
- e3c8a9f save a recipe to the planner
- 42269cb replace pubsub modals with a stack of modals controlled by a hook
- 0f91aca css and show recipe details modal, need to find a better way to manage modals
- d9caac1 create an Icon component, rename api to http
- 65650fc rendering recipes
- a828f0e cookbook lambda and part of frontend
- 20273b5 fix bin day calculation
- bf4204d font size
- c2c102e pass onClose from modal to modal children
- c227314 css tweaks and rendering modals
- c32f699 show modal
- 4a8bb25 add a food planner widget
- 6cb4886 add a food planner lambda and share s3 functions in a library
- 517796f iterate for menu buttons, setup recipe manager page
- 8286a7e readme
- 144f417 update typings
- dd68996 splitwise widget
- 53c8360 clear weather
- 4ae3d50 splitwise api
- 555fe11 change widget css property so we can add gradients as background colours for widgets
- d7dc4bc finish off adding todoist items
- ed9bb0d Couple more icons for menu, tweak widget title z index
- 93b04fd Add a refresh button to menu
- 14d7166 Add a modal, and a todoist modal
- 2e48331 add and style an add button
- d3734fd delete a todoist task
- 831a2bc display of todoist items
- 2d536b4 create task
- 15b7d32 display todoist
- d4101b4 weather icons
- 92cb594 Add a todoist lambda
- bdba0d9 hide scroll bars for now (might style them later)
- e24bfe9 update error handling in backend
- 61ae666 finish off weather widget
- 9ddbd4e use the line based icons, add an icon page to view them all easily
- 41ea547 first pass at weather
- a2614a4 initial weather widget changes
- 1370321 folder
- 92583f4 add a weather api
- dd0537f add some cool weather icons from baz.dev
- c694fb6 correct time rate
- 13a53aa show a loading icon in the base widget
- e9e1dc6 tidy up and add gmail widget
- f8a9a17 wrap calendar with auth
- f57f53a move into lambda own infra folder
- b45d288 split out into function and resources yaml file
- 9a875d2 this seems to compile at least
- a003f42 google calendar api
- 0a5bb6e replace icons with fontawesome
- 00fa848 update favicon
- 98be88f font
- bd6585c add bin day widget
- 08240cd show date on clock
- 46df4a9 resize font on clock widget
- 586df95 replace hook with useEffect prop
- e3339e6 hook to refresh widgets
- 832c316 fix navigation on login
- 7929656 move the news api into a lambda as it does not allow browser requests
- 8465f9e update lambda response structure
- e0ca37f add a clock, fix navigation when logging in
- 188fed4 loop through list of widgets and render component
- cbd1357 bbc news widget, toggle full screen button
- 1c976de add gridstack
- cafb160 lgout on login page render
- 5e451ff navigate around when logged in
- b1f8b54 implement 'login'
- 4e1daa9 make backend always return a message
- b5cf0c3 tweak backend, connect to frontend
- d35dffd implement config api
- 2fb5347 full screen buttons for testing
- 07789bd extra http methods, add helper function
- 09ae044 IAM roles for lambdas
- 5f3b8a7 login page starter
- 2ed9648 tweaks
- d143c0c deploy frontend to aws
- b3cc1f0 deploy first lambda to aws!
- 1deefb3 start implementing config lambdas
- ac20119 split into frontend and backend with the view to house everything in a single repo
- a503e49 .env
- 68d1df7 routes
- edee026 add gridstack
- 31bc918 init as a react app
- ad7b3e1 chore(release): 6.10.4
- 430ff46 fix: malformed json
- 27e54dd fix: stop bbc news widget overlapping
- 6791def fix: todoist api has changed
- 3cd67e2 chore: add emma and will, update unity to 2022.3.7f1, tidy repo
- 9269745 chore(release): 6.10.3

## 6.10.3 (27/04/2023) 


### Issues in this release:

* [#254](https://github.com/iamtomhewitt/home-dashboard/issues/254) - Weather does not display when past a certain time
* [#253](https://github.com/iamtomhewitt/home-dashboard/issues/253) - Update weather codes



## 6.10.2 (09/04/2023) 


### Issues in this release:

* [#252](https://github.com/iamtomhewitt/home-dashboard/issues/252) - DarkSky api has ended



## 6.10.1 (14/09/2022) 


### Issues in this release:

* [#251](https://github.com/iamtomhewitt/home-dashboard/issues/251) - Fix BBC news title sizing



## 6.10.0 (17/05/2022) 


### Issues in this release:

* [#242](https://github.com/iamtomhewitt/home-dashboard/issues/242) - Add background colour to config
* [#239](https://github.com/iamtomhewitt/home-dashboard/issues/239) - Make widgets a rounded square
* [#237](https://github.com/iamtomhewitt/home-dashboard/issues/237) - Add name of dashboard to the bottom next to the logs
* [#235](https://github.com/iamtomhewitt/home-dashboard/issues/235) - NCTX Buses Widget
* [#234](https://github.com/iamtomhewitt/home-dashboard/issues/234) - New Dashboard (H + B)
* [#233](https://github.com/iamtomhewitt/home-dashboard/issues/233) - Update recipe manage endpoints and config



## 6.9.8 (14/10/2021) 


### Issues in this release:

* [#228](https://github.com/iamtomhewitt/home-dashboard/issues/228) - Download config does not work



## 6.9.7 (05/10/2021) 


### Issues in this release:

* [#224](https://github.com/iamtomhewitt/home-dashboard/issues/224) - Trains SSL CA certificate error



## 6.9.5 (03/05/2021) 


### Issues in this release:

* [#217](https://github.com/iamtomhewitt/home-dashboard/issues/217) - Copy Code button on settings button doesn’t change to config colour



## 6.9.4 (02/04/2021) 


### Issues in this release:

* [#213](https://github.com/iamtomhewitt/home-dashboard/issues/213) - Add a copy to clipboard button for config api key



## 6.9.3 (25/02/2021) 


### Issues in this release:

* [#209](https://github.com/iamtomhewitt/home-dashboard/issues/209) - Update settings page



## 6.9.2 (07/11/2020) 


### Issues in this release:

* [#203](https://github.com/iamtomhewitt/home-dashboard/issues/203) - Last day of weather isn't updating



## 6.9.1 (01/11/2020) 


### Issues in this release:

* [#198](https://github.com/iamtomhewitt/home-dashboard/issues/198) - Weather starts from one day too far
* [#197](https://github.com/iamtomhewitt/home-dashboard/issues/197) - Make settings key copiable 
* [#195](https://github.com/iamtomhewitt/home-dashboard/issues/195) - Planner entries don’t refresh



## 6.9.0 (20/10/2020) 


### Issues in this release:

* [#191](https://github.com/iamtomhewitt/home-dashboard/issues/191) - Add api key to calendar endpoint
* [#189](https://github.com/iamtomhewitt/home-dashboard/issues/189) - Update calendar endpoint
* [#188](https://github.com/iamtomhewitt/home-dashboard/issues/188) - Use a CMS
* [#183](https://github.com/iamtomhewitt/home-dashboard/issues/183) - Journeys duplicate on start up



## 6.8.3 (07/10/2020) 


### Issues in this release:

* [#184](https://github.com/iamtomhewitt/home-dashboard/issues/184) - Update recipe manager endpoint



## 6.8.2 (19/08/2020) 


### Issues in this release:

* [#179](https://github.com/iamtomhewitt/home-dashboard/issues/179) - Refactor / code tidy
* [#177](https://github.com/iamtomhewitt/home-dashboard/issues/177) - Use new shopping list endpoint



## 6.8.1 (08/08/2020) 


### Issues in this release:

* [#173](https://github.com/iamtomhewitt/home-dashboard/issues/173) - Splitwise data should remain if there is an error



## 6.8.0 (28/07/2020) 


### Issues in this release:

* [#169](https://github.com/iamtomhewitt/home-dashboard/issues/169) - Add to shopping button should show please wait
* [#168](https://github.com/iamtomhewitt/home-dashboard/issues/168) - Food planner clear button is not working



## 6.7.4 (13/07/2020) 


### Issues in this release:

* [#164](https://github.com/iamtomhewitt/home-dashboard/issues/164) - Use github releaser   