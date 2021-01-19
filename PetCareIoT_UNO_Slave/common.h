#include <Servo.h>
#include <SoftwareSerial.h>
#include <Wire.h>
#include <ArduinoJson.h>

//Mesage codes
#define FEEDER_START      1
#define PUMP_START        2
#define LIGHT_ON          3
#define LIGHT_OFF         4
#define GET_VALUES        5
#define GET_TEMPERATURE   6
#define GET_HUMIDITY      7
#define DONE              8

#define WATER_LEVEL_LOW 750  // Over than 750 is near empty, must start pump,
#define WATER_LEVEL_HIGH 350 // Under 320 is near full, must stop pumo.

#define SERVO_PIN 3
#define LIGHT_PIN 12
#define I2CAddressESPWifi 8
#define enA  9
#define in1  8
#define in2  7
