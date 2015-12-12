var exec = require('child_process').exec;

exec("mono build/HelloWorld.exe", function(err, stdout, stderr) {
    if (err) {
        console.log("child processes failed with error code: " +
            err.code);
    }
    console.log(stdout);
});