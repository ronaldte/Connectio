"use strict";

// Returns timer for 10 seconds to autohide notification bubble.
function get_notification_timer() {
    return setTimeout(() => {
        document.getElementById("notification-bubble").hidden = true
    }, 10000);
}

// Global reference to timer on notification bubble.
var notification_timer = null;

var connection = new signalR.HubConnectionBuilder().withUrl("/messageHub").build();

// Updates notification bubble on new message and runs countdown to autohide.
connection.on("Notify", function (user, message, conversationId) {
    let params = new URLSearchParams(document.location.search);

    if (params.get("conversationId") == conversationId) {
        location.reload();
        return;
    }
    clearTimeout(notification_timer);

    var bubble = document.getElementById("notification-bubble");
    bubble.hidden = false;

    var user_element = document.getElementById("notification-from");
    user_element.innerHTML = user;

    var message_element = document.getElementById("notification-message");
    const short_message = ((message.length < 51) ? message : message.substring(0, 50) + "...");
    message_element.innerHTML = short_message;

    var link = document.getElementById("notification-link");
    link.href = "Conversation/Read?conversationId=" + conversationId;

    notification_timer = get_notification_timer();
});

connection.start();