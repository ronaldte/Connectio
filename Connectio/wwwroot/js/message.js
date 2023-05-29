"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();

connection.on("Notify", function (user, message) {
    alert(user + " says " + message);
});

connection.start();