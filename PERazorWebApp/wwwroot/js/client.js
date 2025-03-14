"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start().then(function () {
    console.log("SignalR connected successfully!");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("RemoveMedicine", function (id) {
    var row = document.getElementById(id);
    document.getElementById("bodyId").removeChild(row);
});