#include "common.h"

ESP8266WiFiMulti WiFiMulti;
WebSocketsClient webSocket;
IPAddress serverIP(192, 168, 0, 132);

bool alreadyConnected = false;
String token = "token:";
unsigned long messageTimestamp = 0;

void webSocketEvent(WStype_t type, uint8_t * payload, size_t length){
  switch (type){
  case WStype_DISCONNECTED:
    if (alreadyConnected)
    {
      Serial.println("Disconnected from websocket!");
      alreadyConnected = false;
    }   
    break;
  case WStype_CONNECTED:
    {
      alreadyConnected = true;  
      Serial.print("Connected to websocket. ");
    }
    break;
  case WStype_TEXT:
    processPayload((char *) payload);
    break;
  case WStype_PING:
    Serial.printf("[WSc] get ping\n");
    break;
  case WStype_PONG:
    Serial.printf("[WSc] get pong\n");
    break;
  default:
    break;
  }
}

void setup(){
  Serial.begin(115200);
  Serial.setDebugOutput(false);
  
  Wire.begin(0,2); //Wire.begin(D1, D2);
  Serial.println("Booting");
  for (uint8_t t = 4; t > 0; t--)
  {
    Serial.print(".");
    delay(1000);
  }

  WiFiMulti.addAP(ssid, pass);
  Serial.println("Connecting to network");
  while (WiFiMulti.run() != WL_CONNECTED)
  {
    Serial.print(".");
    delay(100);
  }
  Serial.println();
  
  login();

  Serial.print("Connecting to WebSockets Server.");
  webSocket.setExtraHeaders(token.c_str());
  webSocket.begin(serverIP, 61955, "/");

  webSocket.onEvent(webSocketEvent);
  webSocket.setReconnectInterval(15000);
  webSocket.enableHeartbeat(60000, 10000, 2);
}

void loop(){
  webSocket.loop();
  
  uint64_t now = millis();
  if (now - messageTimestamp > 30000) 
  {
    messageTimestamp = now;
    getWaterLevel();  
  }
}

void processPayload(char * payload){
  StaticJsonDocument<300> doc;
  String jsonObject = String(payload);
  
  auto error = deserializeJson(doc, jsonObject);
  if (error) {
    Serial.println("Error parsing object.");
    return;
  }
  
  int messageType = doc["Type"];

  writeToArduino(messageType);      
  readFromArduino(doc);  
}

void getWaterLevel(){
  writeToArduino((int)GET_VALUES);
  Wire.requestFrom(8, 16); 

  int level;
  if(Wire.available()>0){
    level = (Wire.read() | Wire.read() << 8);    
    sendWaterLevel();
  }   
}

void readFromArduino(StaticJsonDocument<300> doc){
  int c;
  Wire.requestFrom(8, 4); 
  if(Wire.available()>0){
    c = Wire.read();
  }       
  if(c == DONE)
  {
    String myOutput ="";
    doc["Type"] = DONE;
    serializeJson(doc, myOutput);
    sendAckMessage(myOutput);
  }
  
}

void sendAckMessage(String output){
  String sendTo = String("{\"From\":\"Arduino\", \"Data\" :") + output + ", \"To\":\"\"}";
  webSocket.sendTXT(sendTo);
  Serial.println("ACK sended.");
}

void writeToArduino(int code){
  Wire.beginTransmission(I2CAddressESPWifi);
  Wire.write(code);
  Wire.endTransmission();
  if(code == FEEDER_START)
  {
    delay(1000);
  }

  if(code == PUMP_START)
  {
    delay(5000);
  }
}

void login(){
  StaticJsonDocument<600> doc;
  HTTPClient http;
  http.begin(serverLoginGlobal);
  
  http.addHeader("Content-Type", "application/json");
  http.POST("{\"username\" : \"arduino-esp\", \"password\" : \"Dragan.Mance##9\"}");
  delay(5000);  
  
  String payload = http.getString();

  auto error = deserializeJson(doc, payload);
  if (error) {
    return;
  }
  
  String tokenValue =  doc["token"];
  token += tokenValue;
  http.end();
}
