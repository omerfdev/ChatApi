import React, { useContext, useEffect, useRef } from "react";
import { useState, createContext } from "react";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import AuthContext from "./AuthProvider";
import AlertContext from "./AlertProvider";
import axios from "axios";

export const ChatContext = createContext();

const ChatContextProvider = ({ children }) => {
  const [selectedUser, setSelectedUser] = useState(null);
  const selectedUserRef = useRef(selectedUser);
  const [messages, setMessages] = useState([]);
  const [connectionState, setConnectionState] = useState(null);
  const { token, user } = useContext(AuthContext);
  const { openAlert } = useContext(AlertContext);
  const [isThereMoreMessages, setIsThereMoreMessages] = useState(false);
  const [activeUsers, setActiveUsers] = useState([]);
  const [newMessage, setNewMessage] = useState(null);

  useEffect(() => {
    const fetch = async () => {
      const connection = new HubConnectionBuilder()
        .withUrl("https://localhost:7271/chat", {
          accessTokenFactory: () => token,
        })
        .configureLogging(LogLevel.Information)
        .build();
      connection.on("ReceiveMessage", (receivedMessage, username) => {
        const currentSelectedUser = selectedUserRef.current;
        if (
          currentSelectedUser &&
          receivedMessage.senderId === currentSelectedUser.id
        ) {
          setMessages((messages) => [...messages, receivedMessage]);
        } else {
          openAlert("success", `you received a message from ${username}`);
          setNewMessage(receivedMessage);
        }
      });
      connection.on("ReceiveActiveUsers", (newActiveUsers) => {
        setActiveUsers(newActiveUsers);
      });

      try {
        await connection.start();
        console.log("SignalR connection started.");
        setConnectionState(connection);
      } catch (error) {
        console.error("Error starting SignalR connection:", error);
      }
    };

    fetch();
  }, [token, user.id, openAlert, selectedUserRef]);

  useEffect(() => {
    selectedUserRef.current = selectedUser;
    setMessages([]);
    selectedUser && loadMessages(true);
  }, [selectedUser]);

  const loadMessages = async (isNewUser) => {
    var data = {
      pageDate: messages[0] && !isNewUser ? messages[0].creationDate : null,
      pageSize: 10,
      firstUserId: user.id,
      secoundUserId: selectedUser.id,
    };
    try {
      const response = await axios.get("https://localhost:7271/api/private-messages", {
        params: data,
        headers: {
          Accept: "application/json",
          "Content-Type": "application/json",
          Authorization: `bearer ${token}`,
        },
      });

      isNewUser
        ? setMessages(response.data.messages)
        : setMessages([...response.data.messages, ...messages]);

      setIsThereMoreMessages(response.data.isThereMore);
    } catch (error) {
      console.error("Error loading messages:", error);
    }
  };

  const handleSendMessage = async (message) => {
    if (selectedUser && message !== "" && connectionState) {
      var newMessage = {
        senderId: user.id,
        receiverId: selectedUser.id,
        creationDate: Date.now(),
        textBody: message,
      };

      setMessages([...messages, newMessage]);
      setNewMessage(newMessage);

      try {
        await connectionState.invoke("SendMessageToUser", selectedUser.id, message);
      } catch (error) {
        console.error("Error sending message:", error);
      }
    }
  };

  return (
    <ChatContext.Provider
      value={{
        messages,
        selectedUser,
        setSelectedUser,
        handleSendMessage,
        isThereMoreMessages,
        loadMessages,
        activeUsers,
        newMessage,
      }}
    >
      {children}
    </ChatContext.Provider>
  );
};

export default ChatContextProvider;
