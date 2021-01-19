#include "common.h"

bool pumpState = false;
bool servoState = false;
bool waitingForResponse = false;
int waterLevel;
int requestType;

Servo servo;

void setup(){  
  Serial.begin(115200);

  pinMode(enA, OUTPUT);
  pinMode(in1, OUTPUT);
  pinMode(in2, OUTPUT);
  pinMode(LIGHT_PIN, OUTPUT);

  digitalWrite(in1, LOW);
  digitalWrite(in2, LOW);
  digitalWrite(LIGHT_PIN, LOW);

  servo.attach(SERVO_PIN);
  servo.write(4);

  Wire.begin(I2CAddressESPWifi);
  Wire.onReceive(espWifiReceiveEvent);
  Wire.onRequest(espWifiRequestEvent);

  delay(1000);
  Serial.println("Started");
}

void espWifiReceiveEvent(int count){
  int value = Wire.read();    
  process(value);
}
void process(int messageType){
  switch(messageType)
  {
    case LIGHT_ON:
      digitalWrite(LIGHT_PIN, HIGH);
      waitingForResponse = true;
      break;
    case LIGHT_OFF:
      digitalWrite(LIGHT_PIN, LOW);
      waitingForResponse = true;
      break;
    case FEEDER_START:
      servoState = true;
      break;
    case PUMP_START:
      pumpState = true;
      break;
    case GET_VALUES:
      requestType = GET_VALUES;
      break;
    default:
      break;
  }
}
void espWifiRequestEvent(){
  if(waitingForResponse)
  {
    Wire.write(DONE);
    waitingForResponse = false;
  }
  else if(requestType == (int)GET_VALUES)
  {
    Wire.write(waterLevel);              
    Wire.write((waterLevel >> 8));  
  }
  else
  {
    -1;
  }    
}
void startPump(){  
  analogWrite(enA, 255);
  digitalWrite(in1, HIGH);
  digitalWrite(in2, LOW);
  
  while(waterLevel >= WATER_LEVEL_HIGH)
  {
    waterLevel = analogRead(A3);
    delay(200);
  }
  
  digitalWrite(in1, LOW);
  digitalWrite(in2, LOW);

  pumpState = false;
  waitingForResponse = true;
}
void startServo(){

  servo.write(180);
  delay(700);
  servo.write(4);
  servoState = false;
  waitingForResponse = true;
}
void loop(){ 
  waterLevel = analogRead(A3);
  delay(10);
  
  if(pumpState || waterLevel > WATER_LEVEL_LOW)
  {
    startPump();
  }
  
  if(servoState)
  {
    startServo();
  }
}
