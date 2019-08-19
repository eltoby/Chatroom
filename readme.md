Chatroom allows many users to chat in a Chat room.
Users can also ask for stock quotes using commands and a decoupled bot responds to their commands.

Implementation Details:

- ChatroomApi should be running when StockBot starts so Stockbot can connect to the chat and receive the messages.
- ChatroomApi should be running in order to use ChatroomWeb.

- ChatroomApi configurations are found in the file ChatroomApi/ChatroomApi/appsettings.json:
  - BaseUrl is the Url where ChatroomApi is deployed
  - MaxMessages is the number of messages the api will retrieve when asked for the last messages
  - TokenExpirationMinutes is the number of minutes the authentication token is alive
  - MqUrl is the hostname where RabbitMq is running
  - MqUser is the RabbitMq user name
  - MqPass is the RabbitMq password
  - MqQueue is the name of the Queue to use in RabbitMq
