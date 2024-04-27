const express = require("express");
const http = require("http");
const WebSocket = require("ws");
const dotenv = require("dotenv");
const cors = require("cors");
const mongoose = require("mongoose");
const bodyParser = require("body-parser");
const app = express();

app.use(express.json());
app.use(
  cors({
    origin: "*",
  })
);
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());
dotenv.config();

const server = http.createServer(app);
const wss = new WebSocket.Server({ server });
const { handleMovement, handleAnimator } = require("./playerAction");

const router = require("./router/index");

router(app);

let clientId = 0;
let clients = [];

// WebSocket server
wss.on("connection", (ws) => {
  // start join server

  // end join server

  ws.on("message", (message) => {
    let data = message.toString();
    try {
      let jsonData = JSON.parse(data);
      if (jsonData.eventName == "JoinServer") {
        clientId++;
        clients.push({
          id: clientId,
          client: ws,
          position: jsonData.position,
          characterName: jsonData.characterName,
          name: jsonData.name,
          rotation: jsonData.rotation,
        });
        let id = clients.find((c) => c.client == ws).id;

        let datas = clients.map((item) => {
          return {
            id: item.id,
            name: item.name,
            position: item.position,
            rotation: item.rotation,
            characterName: item.characterName,
          };
        });

        let finalData = {
          eventName: "Player-JoinRoom",
          clientId: id,
          data: datas,
        };

        let newData = {
          eventName: "Player-AddPlayerRoom",
          clientId: id,
          data: [
            {
              id: clientId,
              position: jsonData.position,
              characterName: jsonData.characterName,
              name: jsonData.name,
              rotation: jsonData.rotation,
            },
          ],
        };

        wss.clients.forEach((client) => {
          if (client.readyState === WebSocket.OPEN) {
            if (client == ws) {
              client.send(JSON.stringify(finalData));
            } else {
              client.send(JSON.stringify(newData));
            }
          }
        });
      } else {
        messageType[jsonData.eventName](wss, jsonData);
      }
    } catch (error) {
      console.log(error);
    }
  });

  //start leave server
  ws.on("close", () => {
    let currentClient = clients.find((item) => item.client == ws);
    if (currentClient != null) {
      wss.clients.forEach((client) => {
        if (client.readyState === WebSocket.OPEN) {
          client.send(
            JSON.stringify({
              eventName: "Player-Leave",
              clientId: currentClient.id,
              data: [],
            })
          );
        }
      });
    }
    clients = clients.filter((item) => item.client != ws);
  });
});
mongoose
  .connect(process.env.DATABASE_URL)
  .then(() => {
    console.log("Connected to database.");
  })
  .catch((err) => {
    console.log(`Your error ${err}`);
  });

server.listen(3000, () => {
  console.log("Express server listening on port 3000");
});

let messageType = {
  "Player-Movement": handleMovement,
  "Player-PlayerAnimator": handleAnimator,
};
