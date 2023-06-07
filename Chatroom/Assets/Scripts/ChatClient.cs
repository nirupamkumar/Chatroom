using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using System;
using Unity.VisualScripting;

public class ChatClient : MonoBehaviour
{
    private WebSocket webSocketClient;

    [Header("Identifier")]
    public TMP_InputField identifierInput;

    [Header("Join/Create chatroom")]
    public TMP_InputField createRoomNameInput;
    public TMP_InputField joinRoomNameInput;
    public TMP_Dropdown roomListDropdown;

    [Header("Chartoom")]
    public TMP_InputField chatInputField;
    public TMP_Text displayChatText;

    private void Start()
    {
        webSocketClient = new WebSocket("ws://localhost:15001/chat");
        webSocketClient.OnMessage += OnMessageReceived;
        webSocketClient.Connect();

        chatInputField.onEndEdit.AddListener(OnEndEdit);

        roomListDropdown.ClearOptions();
        
    }

    private void OnMessageReceived(object sender, MessageEventArgs e)
    {
        //chatInputField.text = "Connection opened";
        //chatInputField.text = e.Data;

        string receivedMessages = e.Data;
        displayChatText.text += receivedMessages + "\n";
    }

    private void OnEndEdit(string inputText)
    {
        if (!string.IsNullOrEmpty(inputText) && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
            chatInputField.text = string.Empty;
        }
    }

    public void SendMessage()
    {
        string playerIdentifier = identifierInput.text;
        string message = "[" + playerIdentifier + "]: " + chatInputField.text;

        // send the message through websockets
        webSocketClient.Send(message);

        // clear the chat input field
        chatInputField.text = string.Empty;

        // Add the sent message to the Textmeshpro Text display
        displayChatText.text += message + "\n";
    }
   
    public void Leavechat()
    {
        if (webSocketClient != null)
        {
            string leaveMessage = identifierInput.text + " left the chat";
            displayChatText.text += leaveMessage + "\n";
            webSocketClient.Send(leaveMessage);
            webSocketClient.Send("Server: " + leaveMessage);
            identifierInput.text = string.Empty;

            webSocketClient.Close();
            webSocketClient = null;
        }
    }

    public void CreateChatRoom()
    {
        string chatRoomName = createRoomNameInput.text;

        if (!string.IsNullOrEmpty(chatRoomName))
        {
            webSocketClient.Send($"CreateChatRoom: {chatRoomName}");
            //roomListDropdown.options.Add(new TMP_Dropdown.OptionData(chatRoomName));
            //roomListDropdown.RefreshShownValue();

            createRoomNameInput.text = string.Empty;
        }  
    }

    private void UpdateChatRoomDropdown(string[] chatRoomNames)
    {
        roomListDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (string roomName in chatRoomNames)
        {
            dropdownOptions.Add(new TMP_Dropdown.OptionData(roomName));
        }

        roomListDropdown.AddOptions(dropdownOptions);
    }

    private void JoinChatRoom(string chatRoomName)
    {
        // Send a message to the server indicating that the client wants to join the chat room
        webSocketClient.Send($"JoinChatRoom: {chatRoomName}");
    }



    private void Update()
    {
        QuitApplication();
    }

    public void QuitApplication()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
