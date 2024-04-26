const WebSocket = require("ws");
// handle with message
const handleMovement = (wss, data) => {
  let tempData = {
    eventName: "Player-Movement",
    clientId: data.clientId,
    data: [
      {
        id: data.clientId,
        name: "",
        position: data.position,
        rotation: data.rotation,
      },
    ],
  };
  wss.clients.forEach((client) => {
    if (client.readyState === WebSocket.OPEN) {
      client.send(JSON.stringify(tempData));
    }
  });
};

const handleAnimator = (wss,data) => {
  wss.clients.forEach((client) => {
    if (client.readyState === WebSocket.OPEN) {
      client.send(JSON.stringify(data));
    }
  });
}

module.exports = {
  handleMovement,
  handleAnimator,
};
