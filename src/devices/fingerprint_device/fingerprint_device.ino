#include <Arduino_JSON.h>
#include <JSON.h>
#include <JSONVar.h>
#include <WiFi.h>
#include <HardwareSerial.h>
#include <Adafruit_Fingerprint.h>
#include <HTTPClient.h>

char *ssid = "UTT-CUERVOS"; //"test-ard";
char *pass = "CU3RV@S2022"; //"12345678";
char deviceName[] = "ESP-FG-TNT";

String host = "http://172.17.5.194:81/api/";
// int port = 81;

int wifiStatus = WL_IDLE_STATUS;

int intervalLoop = 5000;
int current = 0;

JSONVar deviceProps;
JSONVar fingerProps;

Adafruit_Fingerprint finger = Adafruit_Fingerprint(&Serial1, 0x00000000);

String combine(String urlExtension) {
  return host + urlExtension;
}

void connectWifi() {
  WiFi.begin(ssid, pass);

  while (wifiStatus != WL_CONNECTED) {
    printResult("[WIFI] Connecting to WIFI SSID: ", ssid);
    wifiStatus = WiFi.waitForConnectResult();    
    printResult("[WIFI] Status: ", wifiStatus);    
  }

  WiFi.setAutoReconnect(true);
  WiFi.persistent(true);

  Serial.println("[WIFI] WiFi connected!");
  Serial.print("[WIFI] IP address: ");
  Serial.println(WiFi.localIP());
}

void scanWifis() {
  int n = WiFi.scanNetworks();
  Serial.println("[WIFI] scan done!");
  if (n == 0) {
    Serial.println("[WIFI] no networks found!");
  } else {
    Serial.print("[WIFI] ");
    Serial.print(n);
    Serial.println(" networks found");
    for (int i = 0; i < n; ++i) {
      Serial.print("[WIFI] ");
      Serial.print(i + 1);
      Serial.print(": ");
      Serial.print(WiFi.SSID(i));
      Serial.print(" (");
      Serial.print(WiFi.RSSI(i));
      Serial.println(")");      
      delay(10);
    }
  }  
}

bool connectFingerprint(bool showMsg = true) {
  if (finger.verifyPassword()) {
    if(showMsg) Serial.println("[FINGER] Finger print Attached!");
    return true;
  } else {
    Serial.println("[FINGER] Finger print NOT FOUNDED!");    
    return false;
  }
}

void loadDetails() {   
  finger.getParameters();

  fingerProps["sensorStatus"] = finger.status_reg;
  fingerProps["sysId"] = finger.status_reg;
  fingerProps["capacity"] = finger.capacity;
  fingerProps["securityLevel"] = finger.security_level;
  fingerProps["address"] = (int)finger.device_addr;
  fingerProps["packetLen"] = finger.packet_len;
  fingerProps["baudRate"] = finger.baud_rate;

  deviceProps["deviceName"] = deviceName;
  deviceProps["ip"] = WiFi.localIP().toString().c_str();;
  deviceProps["fingerPrint"] = fingerProps;
}

void showDetails() {
  Serial.println(F("[STATUS] Sensor Details"));  

  printResult("[STATUS] Device name: ", deviceName);
  printResult("[STATUS] WiFi SSID: ", ssid);
  printResult("[STATUS] Finger Status: ", finger.status_reg);
  printResult("[STATUS] Sys ID: ", finger.system_id);
  printResult("[STATUS] Capacity: ", finger.capacity);
  printResult("[STATUS] Device Address: ", finger.device_addr);
  printResult("[STATUS] Security level: ", finger.security_level);
  printResult("[STATUS] Packet len: ", finger.packet_len);
  printResult("[STATUS] Baud rate: ", finger.baud_rate);
  printResult("[STATUS] WiFi Status: ", wifiStatus);
}

void printResult(char *name, char *value) { Serial.print(F(name)); Serial.println(value);}
void printResult(char *name, uint16_t value) { Serial.print(F(name)); Serial.println(value);}
void printResult(char *name, uint32_t value) { Serial.print(F(name)); Serial.println(value); }
void printResult(char *name, int value) { Serial.print(F(name)); Serial.println(value); }
void printResult(char *name, String value) { Serial.print(F(name)); Serial.println(value); }

String getResult(HTTPClient * client, int httpCode) {
  String payload = "";

  if (httpCode > 0) {
    payload = client->getString();
    printResult("[HTTP] Payload from request: ", payload);      
  } else {
    printResult("[HTTP] Error: ", client->errorToString(httpCode).c_str());
  }

  return payload;
}

String getRequest(String url) {
  HTTPClient client;

  client.begin(url);  
  String payload = getResult(&client, client.GET());
  client.end();

  return payload;
}

String postRequest(String url, JSONVar content) {
  HTTPClient client;  
  String contentStr = JSONVar::stringify(content);

  client.begin(url);
  client.addHeader("Content-Type", "application/json");
  client.addHeader("Content-Length", String(sizeof(contentStr)));  
  String payload = getResult(&client, client.POST(contentStr));
  client.end();

  return payload;
}

void setup() {
  Serial1.begin(57600, SERIAL_8N1, 16, 17);
  Serial.begin(115200);
  finger.begin(57600);
  
  WiFi.mode(WIFI_STA);
  
  Serial.println("[STARTUP] Starting!...");

  scanWifis();
  connectFingerprint();
  connectWifi();
  showDetails();
}

void loop() {  
  wifiStatus = WiFi.status();  
  if(wifiStatus != WL_CONNECTED) connectWifi();
  connectFingerprint(false);

  if (current > intervalLoop) {
    loadDetails();
    // printResult("Payload to send: ", JSONVar::stringify(deviceProps));
    postRequest(combine("Device"), deviceProps);        
    current = 0;
  }

  delay(100);
  current += 100;
}