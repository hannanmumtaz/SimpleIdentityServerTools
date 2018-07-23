var express = require('express');
var app = express();

app.engine('html', require('ejs').renderFile);
app.set('views', __dirname);

app.use(express.static(__dirname + '/wwwroot'));
app.get('/', function(req, res) {
    res.render('index.html');
});

console.log("Listen 4201");
app.listen(4201);
