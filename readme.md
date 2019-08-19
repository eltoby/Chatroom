Chatroom allows many users to chat in a Chat room.
Users can also ask for stock quotes using commands and a decoupled bot responds to their commands.

Implementation Details:

- A RabbitMq Server must be installed.
- Microsoft SqlServer must be installed. 
- .Net Core 2.2+ must be installed.

- ChatroomApi should be running when StockBot starts so Stockbot can connect to the chat and receive the messages.
- ChatroomApi should be running in order to use ChatroomWeb.

- ChatroomApi configurations are found in the file ChatroomApi/ChatroomApi/appsettings.json:
  - ChatConnectionString is the SQL Server connection string to the database. If the DB doesn't exists it is created on runtime, this may take some time on the first run.
  - BaseUrl is the Url where ChatroomApi is deployed.
  - MaxMessages is the number of messages the api will retrieve when asked for the last messages.
  - TokenExpirationMinutes is the number of minutes the authentication token is alive.
  - MqUrl is the hostname where RabbitMq is running.
  - MqUser is the RabbitMq user name.
  - MqPass is the RabbitMq password.
  - MqQueue is the name of the Queue to use in RabbitMq.

- StockBot configurations are found in the file StockBot/StockBot/appsettings.json
  - ChatApiUrl is the ChatroomApi chat hub url. It should be specified as [ChatroomApiUrl]/chat
  - UseMq indicates if the bot uses RabbitMq to send messages back to the Chatroom. If it's false messsages are sent by SignalR Hub.
  - MqUrl is the hostname where RabbitMq is running. Should be the same as in ChatroomApi.
  - MqUser is the RabbitMq user name.
  - MqPass is the RabbitMq password.
  - MqQueue is the name of the Queue to use in RabbitMq. Should be the same ass in ChatroomApi.

- ChatroomWeb configurations are found int the file ChatroomWeb/src/environments/environment.ts
  - apiUrl is the ChatroomApi url.
  - maxMessages is the maximum number of messages the chat window can show.