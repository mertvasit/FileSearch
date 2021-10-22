//function openInChrome(url) {
//    if (navigator.appName.indexOf("Microsoft") > -1) {
//        var active = new ActiveXObject("WScript.shell");
//        //var path = "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"";
//        //active.Run(path + " " + url);
//        active.run("Chrome " + url + "");
//    }
//}

function openInChrome(url) {
    if (navigator.appName.indexOf("Microsoft") > -1) {
        var active = new ActiveXObject("WScript.shell");
        var path = "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"";
        try {
            active.Run(path + " " + url);
        }
        catch (e) {
            var path2 = "\"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe\"";;
            active.Run(path2 + " " + url);
        }
        //window.close();
    }
}

//open current site in chrome browser.
openInChrome(window.location.href);
