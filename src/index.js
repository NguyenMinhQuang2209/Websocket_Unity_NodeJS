const express = require("express");
const http = require("http");
const WebSocket = require("ws");

const app = express();
const server = http.createServer(app);
const wss = new WebSocket.Server({ server });

let clientId = 0;
let clients = [];
// WebSocket server
wss.on("connection", (ws) => {

    // start join server
  clientId++;
  clients.push({ id: clientId, client: ws, position: [0, 0, 0], name: "helo" });
  let id = clients.find((c) => c.client == ws).id;

  let datas = clients.map((item) => {
    return {
      id: item.id,
      name: item.name,
      position: item.position,
    };
  });

  let finalData = {
    eventName: "JoinRoom",
    clientId: id,
    data: datas,
  };

  let newData = {
    eventName: "AddPlayerRoom",
    clientId: id,
    data: [
      {
        id: clientId,
        name: "new",
        position: [0, 0, 0],
      },
    ],
  };

  wss.clients.forEach((client) => {
    if (client.readyState === WebSocket.OPEN) {
      if (client == ws) {
        client.send(JSON.stringify(finalData));
      }
      else{
        client.send(JSON.stringify(newData));
      }
    }
  });

  // end join server

  ws.on("message", (message) => {
    
  });


  //start leave server
  ws.on("close", () => {
    let currentClient = clients.find((item) => item.client == ws);
    if (currentClient != null) {
      wss.clients.forEach((client) => {
        if (client.readyState === WebSocket.OPEN) {
          client.send(JSON.stringify({
            eventName:"Leave",
            clientId: currentClient.id,
            data:[]
          }));
        }
      });
    }
    clients = clients.filter((item) => item.client != ws);
  });
});

server.listen(3000, () => {
  console.log("Express server listening on port 3000");
});
