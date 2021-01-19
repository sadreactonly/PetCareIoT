#include <ESP8266WiFi.h>
#include <ESP8266WiFiMulti.h>
#include <WiFiClient.h>
#include <Wire.h>
#include <ESP8266HTTPClient.h>
#include <WebSocketsClient_Generic.h>
#include <Hash.h>
#include <ArduinoJson.h>

#define FEEDER_START      1
#define PUMP_START        2
#define LIGHT_ON          3
#define LIGHT_OFF         4
#define GET_VALUES        0x05
#define GET_TEMPERATURE   6
#define GET_HUMIDITY      7
#define DONE              8

#define I2CAddressESPWifi 8

const char* ssid     = "WIFI_NAME";
const char* pass = "WIFI_PASSWORD";

String serverLoginGlobal = "http://server_url/api/authenticate/login";
String websocketGlobal = "server_url";
String serverLoginLocal = "http://192.168.0.132/api/authenticate/login";

String websocketLocal = "192.168.0.132";

int portLocal = 61955;
int portGlobal = 80;
