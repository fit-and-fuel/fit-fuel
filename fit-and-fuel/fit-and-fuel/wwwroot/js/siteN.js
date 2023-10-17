var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//connection.on("ReceiveMessage", function (fromUser, message) {
//    var msg = fromUser + " : " + message;
//    var li = document.createElement("li");
//    li.textContent = msg;
//    $("#list").prepend(li);
//});
//connection.on("ReceiveMessage", function (message) {
//    var msg = message;
//    var li = document.createElement("li");
//    li.textContent = msg;

//    var list = document.getElementById("list");
//    list.appendChild(li);

//});
connection.on("SendMessage", function (message) {
    var msg = message;

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

    // Append the new message structure at the end of both list and list1
    var list = document.getElementById("list");
    list.appendChild(messageStructure);

});


connection.on("ReceiveMessage", function (message) {
    var msg = message;

    // Create a new message structure
    var messageStructure = document.createElement("div");
    messageStructure.className = "media";

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

    // Append the new message structure at the end of both list and list1
    var list = document.getElementById("list");
    list.appendChild(messageStructure);

});
connection.start().catch(function (err) {
    return console.error(err.toString());
});

//$("#btnSend").on("click", function () {
//    var message = $("#txtMessage").val();
//    connection.invoke("SendMessage",  message);
//});

$("#btnSend").on("click", function () {

    var message = $("#txtMessage").val();
    var toUser = $("#toUser").val();
    connection.invoke("SendMessageNut", message, toUser);
});
