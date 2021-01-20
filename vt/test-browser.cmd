rd  /s /q "C:\Temp\Chrome dev session"
"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" --incognito --user-data-dir="C://Temp//Chrome dev session" --allow-file-access-from-files
rd  /s /q "C:\Temp\Chrome dev session"
rem https://stackoverflow.com/questions/23256830/crossorigin-errors-when-loading-vtt-file
rem The problem was that I was loading my html file in browser directly from disk so when it tries to access the vtt file then browser gets a feeling of cross origin request and hence the error.
rem You can get rid of this error simply by hosting your web page inside a web server like IIS.
rem The moment you start fetching your html page like a website all the files like video, or *.vtt are all relative so issue of CORS gets resolved.
rem Configure the MIME type for your web application in IIS with below details:
rem File Name Extension: .vtt
rem MIME type: text/vtt
rem https://stackoverflow.com/questions/15268604/html5-track-captions-not-showing
rem Disable same origin policy in Chrome
rem https://stackoverflow.com/questions/3102819/disable-same-origin-policy-in-chrome
rem https://stackoverflow.com/questions/18586921/how-to-launch-html-using-chrome-at-allow-file-access-from-files-mode
rem --allow-file-access-from-files is less dangerous than --disable-web-security
rem https://chrome.google.com/webstore/detail/web-server-for-chrome/ofhbbkphhbklhfoeikjpcbhemlocgigb
rem SubRip to WebVTT converter
rem https://atelier.u-sub.net/srt2vtt/
