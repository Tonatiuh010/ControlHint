#include <HardwareSerial.h>
#include <Adafruit_Fingerprint.h>

#define DEVICE_NAME "ESP-FG-TNT";

Adafruit_Fingerprint finger = Adafruit_Fingerprint(&Serial1, 0x00000000);
//Utils utils = Utils();//Utils(&Serial);

void setup() {  
  Serial1.begin(57600, SERIAL_8N1, 16, 17);
  Serial.begin(115200); 
  finger.begin(57600);

  Serial.println("Arduino finger Print");


  if (finger.verifyPassword()) {
    Serial.println("Found fingerprint sensor!");
  } else {
    Serial.println("Did not find fingerprint sensor :(");
    while (1) { delay(1); }
  }

  finger.getParameters();
  setFingerprintParams();  

}

void loop() {
  // put your main code here, to run repeatedly:

}
// ,"prebuild": "python ./prebuild/prebuild.py"

void setFingerprintParams() {  
  Serial.println(F("Reading sensor parameters"));  
  Serial.print(F("Status: 0x")); Serial.println(finger.status_reg, HEX);
  Serial.print(F("Sys ID: 0x")); Serial.println(finger.system_id, HEX);
  Serial.print(F("Capacity: ")); Serial.println(finger.capacity);
  Serial.print(F("Security level: ")); Serial.println(finger.security_level);
  Serial.print(F("Device address: ")); Serial.println(finger.device_addr, HEX);
  Serial.print(F("Packet len: ")); Serial.println(finger.packet_len);
  Serial.print(F("Baud rate: ")); Serial.println(finger.baud_rate);
  //utils.PrintResult("test", "123");
  //Serial.print(F("test: ")); Serial.println(template.Id);

}
