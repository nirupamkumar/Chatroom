using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WebSocketSharp;
using System;
using Unity.VisualScripting;
using System.Linq;

public class ChatClient : MonoBehaviour
{
    private WebSocket webSocketClient;

    [Header("Identifier")]
    public TMP_InputField identifierInput;

    [Header("Join/Create chatroom")]
    public TMP_InputField createRoomNameInput;
    public TMP_Dropdown roomListDropdown;
    private List<string> chatRoomNames = new List<string>();

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
        if (e.Data.StartsWith("JoinChatRoom: "))
        {
            string chatRoomList = e.Data.Substring("JoinChatRoom: ".Length);
            chatRoomNames = chatRoomList.Split(',').ToList();
            Debug.Log("Joined chat room: " + chatRoomNames);

            UpdateDropdownList();
        }
        else
        {
            chatInputField.text = "Connection opened";
            //chatInputField.text = e.Data;
            string receivedMessages = e.Data;
            displayChatText.text += receivedMessages + "\n";
        }       
    }

    private void OnEndEdit(string inputText)
    {
        if (!string.IsNullOrEmpty(inputText) && Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
            //chatInputField.text = string.Empty;
        }
    }

    public void SendMessage()
    {
        if (chatInputField.text == "Connection opened")
        {
            chatInputField.text = string.Empty;
            return;
        }

        string playerIdentifier = identifierInput.text;
        string message = "[" + playerIdentifier + "]: " + chatInputField.text;
        displayChatText.text += message + "\n";
    
        webSocketClient.Send(message);
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
            roomListDropdown.options.Add(new TMP_Dropdown.OptionData(chatRoomName));
            roomListDropdown.RefreshShownValue();

            JoinChatRoom(chatRoomName);
            createRoomNameInput.text = string.Empty;
        }  
    }

    private void UpdateDropdownList()
    {
        // Clear the existing dropdown options
        //roomListDropdown.ClearOptions();

        // Add the updated chat room names to the dropdown options
        roomListDropdown.AddOptions(chatRoomNames);

        // Update the displayed value
        roomListDropdown.RefreshShownValue();
    }

    private void JoinChatRoom(string chatRoomName)
    {
        // Send a message to the server indicating that the client wants to join the chat room
        webSocketClient.Send($"JoinChatRoom: {chatRoomName}");
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

    public void ContinueButton()
    {
        if (string.IsNullOrEmpty(identifierInput.text))
        {
            identifierInput.text = "player";
        }

        Debug.Log(identifierInput.text);

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
