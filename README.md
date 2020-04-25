# OneHundredAndEighty 
* [Archived], not deleted bacause of history =)
* Manual darts score calculator with some cool stuff...
  
# OneHundredAndEightyCore
* All-in-one steel tip darts desktop app
* Version [2.1 released](https://github.com/YellowFive5/OneHundredAndEighty/releases/tag/v.2.1)
* Main community, united this idea is [here](https://www.facebook.com/groups/281778298914107/)
* ❗ Applies to automatic throw detection with [[DartboardRecognition]](https://github.com/YellowFive5/DartboardRecognition) 
  - Please keep in mind, that all of this stuff is under heavy development now. Problems of performance and optimization stays not on first place for me now. So I can't guarantee perfect work of this stuff on your "piece of sh*t" notebook. On my system (4.2 Ghz, 6 cores, 16RAM) all works correctly (with near 100% load ;-P )
* 🐞  Known problems / bug reports:
  - When you start new game, make sure there are no darts in dartboard, because if you will start a new game and go to extract darts from dartboard - detection process brokes =( - will be fixed someday
  
![180app2 0](https://user-images.githubusercontent.com/42347722/78453424-4bfaf300-769a-11ea-9ddf-bf09598b37e4.jpg)

## Release notes:
timeline ↓
- [x] Automatic throw detection using [[DartboardRecognition]](https://github.com/YellowFive5/DartboardRecognition)
- [x] Database storage
- [x] Single player free-throws/free-points game type
- [x] Double players free-throws/free-points game type
- [x] Single player free-throws/write off points game type
- [x] Double players free-throws/write off points game type

✅ [[**v.2.0 Release**](https://github.com/YellowFive5/OneHundredAndEighty/releases/tag/v2.0)]
- [x] Database version check with maigrating to new versions
- [x] Some telemetry write
- [x] Full quick start guide

✅ [[**v.2.1 Release**](https://github.com/YellowFive5/OneHundredAndEighty/releases/tag/v.2.1)]

- [x] Classic game type
- [ ] Runtime crossing debug mode
- [ ] Throw manual correction
- [ ] Throw undo
- [ ] Manual hand end
- [ ] UI prettyfy

⭕ [**v.2.2 Release**]
- [ ] ...
- **Unsorted todo's:**
- [ ] CamCalibrator itegration
- [ ] Around the clock game type
- [ ] Shanghai game type
- [ ] Cricket game type
- [ ] Killer game type
- [ ] Training game types
- [ ] Player statistics browse
- [ ] Players statistics comparison
- [ ] Leaderboards by statistic items
- [ ] Player achieves
- [ ] Detection performance improve
- [ ] Detection accuracy improve
- [ ] ...

P.S. - If you like this stuff and if you want you can **donate** me for some beer 🍻 (click 💜"**Sponsor**" button in the top of a page) 

# Quick start guide 🎯
## 1. General advices:
If you decided to build and run this stuff - you need some required things:
1. **Classic dartboard**
2. **PC**
   
   Please keep in mind, that all of this stuff is still under heavy development now. Problems of performance and optimization stays not on first place for me now. So, I can't guarantee perfect work of this stuff on your "piece of sh*t" notebook. On my system (4.2 Ghz, 6 cores, 16RAM) all works correctly (with near 100% load)
3. **Cams**
![cams](https://user-images.githubusercontent.com/42347722/79052015-3bb7ba80-7c3c-11ea-88a4-70306da8e7c3.JPEG)

You need 4 cams with same model. (In theory you can use only 2 or 3 cameras, but detection quality will be worse in nearly-sticked darts situations) Recommended resolution of cameras is 1280x720.

"What the best cams?" It is opened question. I'm not tried anything instead of OV2710 modules (2MP 1080P HD, 85 HFOW, distortionless). Many of people says that Logitech C920 is the best choice (you bet, with its price...)

4. **Lights**

   I use 5 meters of 12V cold-light LED-strip. Enough for 2 rounds inside of cabinet.
5. **Usb hubs, extension cables**

   I use x2 5 meters usb extension cables and x2 quad usb hubs without external power.
6. **Some materials and instruments**

### Cams setup scheme
Classic positions for cams setup:

![Скриншот 2019-11-09 13 14 59](https://user-images.githubusercontent.com/42347722/79052084-90f3cc00-7c3c-11ea-9194-f0896e8a25e7.jpg)

### Cams connection scheme

![Untitled Diagram](https://user-images.githubusercontent.com/42347722/79052102-ac5ed700-7c3c-11ea-8d13-7ed0de6bfa70.jpg)

Notice, that it can be situation, when all your connected cams will not be work together at same time. It happens because there are not enough power in 1 PC usb port for working 2+ cams at same time. You can see all 4 cams connected via Control Panel but they don't work under load simultaneously. To check this situation, you can use [CamCalibrator](https://github.com/YellowFive5/DartboardRecognition/releases/tag/1.3). Just run it 4 times simultaneously and run in on all 4 cams. If not, all cams translate image - you need to reconnect cams different way.
  
### [CamCalibrator](https://github.com/YellowFive5/DartboardRecognition/releases/tag/1.3)

CamCalibrator is simple project to presetup camera when you fix it on stand. The idea is to stick dart into bull and run calibrator. You need to set camera like next screenshot. Blue line goes through dart, red line lies on dartboard surface. Then you fix camera tightly. Setup all your cameras this way.

![Скриншот 2019-11-09 18 38 54](https://user-images.githubusercontent.com/42347722/79052109-bd0f4d00-7c3c-11ea-88f2-b65c852e4f68.png)

All connected to PC cameras will be print in textbox. Each of camera have ID, which you can check this way. This ID's will be need for main app setup.

Keep in mind one thing - when you plug off cams and plug it again, but in another usb - ID of cam will change. So, try to connect cams one way to not change setups every time.

## 2. Some things to DIY:

You are free to choose materials and instruments to build necessary darts cabinet. So, I just post my photos and I hope you will understand concept and idea.

![2020-04-11 21_03_09-Clipboard](https://user-images.githubusercontent.com/42347722/79052124-e7610a80-7c3c-11ea-9a07-4210e5820733.png)

![IMG_2256](https://user-images.githubusercontent.com/42347722/79052130-f34ccc80-7c3c-11ea-8c45-e9dd2af50586.JPEG)

![IMG_2258](https://user-images.githubusercontent.com/42347722/79052132-f9db4400-7c3c-11ea-8d15-b0097e044a59.JPEG)

![IMG_2259](https://user-images.githubusercontent.com/42347722/79052134-01025200-7c3d-11ea-8946-dbe92b3c8d0b.JPEG)

## 3. App setup

Now, when you build cabinet, you can setup remaining things in main app.

### General Setup tab values:
- **Cameras HFOV** - Horizontal field of view of your cameras [default is 85]. 
- **Cam resolution width**  - [default is 1028]
- **Cam resolution height** - [default is 720]
- **Moves extraction value** - calculated after two images difference find value, with the help of which system understand, that you move your hand to extract darts from dartboard. [default is 8000]
- **Move detected sleep time** - delay in seconds after moves on cam detected. Need to wait until dart fully sticks in dartboard after flight. [default is 0.75]
- **Moves noise value** - calculated after two images difference find value to reduce noise moves. [default is 175]
- **Smooth gaussian value** - value to smooth images in process of work. Not need to change it. [default is 5]
- **Threshold sleep time** - delay in seconds after all cams working circle. [default is 0.3]
- **Extraction sleep time** - delay in seconds to stop detection process, when you extract darts from dartboard. [default is 4]
- **Minimal contour arc** - value, with the help of which system search dart contour after throw detected. [default is 105]
- **Moves dart value** - calculated after two images difference find value, with the help of which system detects dart-moves. [default is 600]

Most of this values you don’t need to change, but if you want you can experiment.

### Cams setup

![IMG_2259](https://user-images.githubusercontent.com/42347722/79062894-ac93bc80-7ca6-11ea-8736-e73ee95ef7eb.png)

1. Set general values
2. Set cams ID's (use [CamCalibrator](https://github.com/YellowFive5/DartboardRecognition/releases/tag/1.3) to find cam ID)
3. Set cams setup sectors (9-5-1-4 is classics)

![91749790_2282946622009939_7528721286386679808_o](https://user-images.githubusercontent.com/42347722/79063125-67708a00-7ca8-11ea-81da-fc6652e40a87.jpg)

4. Set distance to cams in cm.
5. Click "Calibrate" button - X/Y will be calculated

Go to each cams tab

![91749790_2282946622009939_7528721286386679808_o](https://user-images.githubusercontent.com/42347722/79063258-27f66d80-7ca9-11ea-90a9-b1187ada2bf5.png)

1. Stick one dart in bull.
2. Click "Start"
3. Setup using sliders all like on image above. Red line surface, Center point, ROI region, Threshold. Also dont set ROI region close to surface. Use this image as ethalon.
4. Click "Stop"
5. Go to next cam
6. Setup all cams this way
7. All app setups complete

## 4. Database.db

All setup values and all information about player, game or each throw stores in database.

### Database migration to new app version

Version of database must be same that version of app. In app development process it's clear that new versions of app releases sometimes. So, I add possibility for you to use your first database with all your collected information in new versions of app. When new version of app releases - copy your old version Database.db to new app folder with replace and run new version of app. App will offer you to migrate your database to new version. After that you can use new version of app with your old (now fresh) database.

## 5. Detection working process

...

1. Cams taking pictures one by one and compare this image with previous taken images to find difference
2. You throw next dart
3. Dart flight captured by one of cams
4. System calculates enough difference and understand, that dart-move detected
5. Some delay for dart to fully sticks in dartboard
6. Taking pictures from all 4 cams
7. Calculating pictures difference with previous pictures with previous dart
8. Finding new dart contour on each of 4 images
9. Choosing 2 best dart contours
10. Calculating point of impact on projection with line intersections
11. Calculating and return throw data
12. repeat

...

![process](https://user-images.githubusercontent.com/42347722/79052152-2e4f0000-7c3d-11ea-8035-5b56e650b045.jpg)


