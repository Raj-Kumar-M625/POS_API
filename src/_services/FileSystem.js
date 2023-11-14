
const axios = require("axios");
const fs = require("fs");
axios.get("https://jsonplaceholder.typicode.com/posts").then((res) => {
    let json = JSON.stringify(res.data)
    
    fs.writeFile("newfile.json",json , function (err) {
        if (err) throw err;
        console.log("File is created successfully.");
    });
})






