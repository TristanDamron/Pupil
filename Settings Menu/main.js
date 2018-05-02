const {app, BrowserWindow} = require('electron');
const path = require('path');
const url = require('url');
const fs = require('fs');

function createWindow () {
  win = new BrowserWindow({width: 800, height: 700});
  
  win.loadURL(url.format({
    pathname: path.join(__dirname, 'index.html'),
    protocol: 'file:',
    slashes: true
  }));
}

app.on('ready', createWindow);

function dumpToFile(json) {
    fs.writeFile("../PupilData.json", json, function(err) {
        if (err) {
            return console.error("ERROR: Cannot open file.");
        }  
        console.log("dumpin");
    });
}

// @TODO: Do I need to make the json file look more like a serialized Unity object?
function unityifyJson(json) {
    var ret = json.replace('[', '');
    ret = ret.replace(']', '');
    ret = ret.replace('{', '');
    ret = ret.replace('}', '');
    ret = ret.replace('\"name\":', '');
    ret = "{" + ret + "}";
    console.log(ret);
    return ret;
}
