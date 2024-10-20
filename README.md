# **Chatroom Application**  
A simple client-server chatroom application built using Unity and WebSocket-Sharp.

## **Overview**  
This project demonstrates real-time communication between a Unity client and a server using WebSocket. All messages are serialized using a custom serializer with code generation for efficiency and performance.

- **Socket Technology:** WebSocket (using `websocket-sharp`)  
- **Unity Version:** 2021.3.5f1 LTS

---

## **Features**

### Any user can:
- **Join/leave** the chat server.
- **List** all available chatrooms.
- **Create** a new chatroom.
- **Enter/leave** a chatroom.
- **Send/receive** messages in a chatroom.

---

## **Client-Side**

The client is built in Unity with a simple user interface. Below is a preview of the chatroom client UI:

![Chatroom Client UI](https://github.com/nirupamkumar/Chatroom/assets/63305439/61876fd5-769f-4edc-bce5-dd144a1aef91)

### Chatroom Actions:
- **Create/Join a Chatroom:**
  
  ![Create Join](https://github.com/nirupamkumar/Chatroom/assets/63305439/815527f9-b505-4692-b645-4fcc02a9eabe)
  
- **Chatroom Interface:**
  
  ![Chatroom](https://github.com/nirupamkumar/Chatroom/assets/63305439/b461b4e7-007c-4305-9b4f-3bdc265ea902)

---

## **Server-Side**

The server is responsible for handling client connections, managing chatrooms, and broadcasting messages. Below is a snapshot of the server interface:

![Server](https://github.com/nirupamkumar/Chatroom/assets/63305439/07a68da2-ccbf-4543-8b61-fd4080fd5343)

---

## **Technology Stack**
- **Client:** Unity (2021.3.5f1 LTS) with WebSocket-Sharp for communication.
- **Server:** Built using WebSocket-Sharp for handling real-time communication.
- **Serialization:** Custom serializer with code generation to serialize messages between client and server efficiently.

---

## **How to Run**

### **Server**
1. Clone the repository.
2. Open the server folder in Visual Studio.
3. Run the WebSocket server using the command:
   ```bash
   dotnet run
