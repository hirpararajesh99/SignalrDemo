let connection = new signalR.HubConnectionBuilder()
    .withUrl("/stocks?emp=1")    
    .build();


connection.start().then(function () {
    console.log('connected to hub');
}).catch(function (err) {
    return console.error(err.toString());
});


connection.on("GetDataBanknifty", function () {       
    loadData();    
});
connection.on("GetDataNifty", function () {     
    loadNiftyData();
});
