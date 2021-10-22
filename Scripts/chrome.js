//var applicationName = window.location.href.split('/')[3];
//openInChrome("http://web-0108a.iaf/" + applicationName);

openInChrome(window.location.href);

//peasants, start using chrome ffs

function openInChrome(url) {
    //check if IE
    if (false || !!document.documentMode) {
        //if (navigator.appName.indexOf("Microsoft") > -1) {
        var pathA = "\"C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe\"";
        var pathB = "\"C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe\"";

        //window.open('http://web-0108a.iaf/close', '_self', '');
        //window.close();

        try {
            var active = new ActiveXObject("WScript.shell");
            active.run("Chrome " + url + "");
            setTimeout(function () {
                window.open('', '_self', '');
                window.close();
            }, 2000);
        } catch(e){
            
        }

        // active.Run(pathB + " " + url);

        // active.Run(pathA + " " + url);

    }
}