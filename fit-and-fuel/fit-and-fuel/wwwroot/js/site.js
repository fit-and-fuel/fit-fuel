var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// Initialize a variable to store the last received message
var lastReceivedMessage = null;

connection.on("SendMessage", function (message) {
    var msg = message;

    // Check if the message is the same as the last received message to prevent repetition
    if (msg !== lastReceivedMessage) {
        lastReceivedMessage = msg;

        var messageStructure = document.createElement("div");
        messageStructure.className = "media justify-content-end align-items-end ms-auto";

        var messageReceived = document.createElement("div");
        messageReceived.className = "message-sent w-auto";

        var p = document.createElement("p");
        p.className = "mb-1";
        p.textContent = msg;

        // Append the elements to build the structure
        messageReceived.appendChild(p);
        messageStructure.appendChild(messageReceived);

        // Append the new message structure to the chat container
        var list = document.getElementById("list");
        list.appendChild(messageStructure);
    }
});

connection.on("ReceiveMessage", function (message) {
    var msg = message;

    // Check if the message is the same as the last received message to prevent repetition
    if (msg !== lastReceivedMessage) {
        lastReceivedMessage = msg;

        // Create a new message structure
        var messageStructure = document.createElement("div");
        messageStructure.className = "media";
        messageStructure.style = "direction:rtl;";
        var messageReceived = document.createElement("div");
        messageReceived.className = "message-received w-auto";
       
        var dFlex = document.createElement("div");
        dFlex.className = "d-flex";

        var ms1Text = document.createElement("div");
        ms1Text.className = "ms-1 text";

        var p = document.createElement("p");
        p.className = "mb-1";
        p.textContent = msg;

        // Append the elements to build the structure
        ms1Text.appendChild(p);
        dFlex.appendChild(ms1Text);
        messageReceived.appendChild(dFlex);
        messageStructure.appendChild(messageReceived);

        // Append the new message structure to the chat container
        var list = document.getElementById("list");
        list.appendChild(messageStructure);
    }
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

$("#btnSend").on("click", function () {
    var message = $("#txtMessage").val();
    lastReceivedMessage = message; // Set the last received message to the sent message
    connection.invoke("SendMessage", message);
});
