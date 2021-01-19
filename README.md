# PetCareIoT
## Take care of your pet from anywhere in world in a real-time with Arduino, Android and and ASP.NET Core.

The project was completed using the following technologies:

- **Master/Slave I2C connection between ESP8266 and Arduino Uno** ( ESP-01 and Android Nano)
- **Xamarin.Android** for Android application
- **Asp.Net Core Web API with WebSocket as middleware** for real-time communication.

## Features
**Project includes:**
- Pet feeder
- Watering system
- Light control
- Real-Time communication
- Reports and stats

## Communication
**There are 3 parts of communication here:**

- Client 1 ( Android or PC) - Send commands to ESP8266 and Arduino trough middleware.

- Middleware (Asp.Net Core app with websockets) - routes messages between clients in real-time and stores those messages in database.

- Client 2 (ESP8266 and Arduino) - responds to Client 1 requests and periodically sends sensors reading.

![Communication flow](https://github.com/sadreactonly/PetCareIoT/blob/master/PetCareIoT.Images/communication_flow.jpg?raw=true)

## Android app
![Main screen](https://github.com/sadreactonly/PetCareIoT/blob/master/PetCareIoT.Images/mainscreen.png?raw=true)
