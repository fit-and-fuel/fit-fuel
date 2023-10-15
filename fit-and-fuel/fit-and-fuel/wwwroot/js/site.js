var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//connection.on("ReceiveMessage", function (fromUser, message) {
//    var msg = fromUser + " : " + message;
//    var li = document.createElement("li");
//    li.textContent = msg;
//    $("#list").prepend(li);
//});
connection.on("ReceiveMessage", function (message) {
    var msg = message;
    var li = document.createElement("li");
    li.textContent = msg;
    $("#list").prepend(li);
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
    
    connection.invoke("SendMessage", message);
});
