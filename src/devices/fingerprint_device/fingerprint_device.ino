#include <Arduino_JSON.h>
#include <JSON.h>
#include <JSONVar.h>
#include <WiFi.h>
#include <WebSocketsClient.h>
#include <HardwareSerial.h>
#include <Adafruit_Fingerprint.h>
#include <HTTPClient.h>

/* on send -> { "deviceName": "", "data": { } } */
/* on receive/response -> { "status": "OK", "msg": "COMPLETE", "data": { "action": "" } } */

char *ssid = "IZZI-99D0"; // "UTT-CUERVOS"; //"test-ard";
char *pass = "VBBPMSZNNEJV"; // "CU3RV@S2022"; // "12345678";
String deviceName = "ESP-FG-TNT";

String host = "http://192.168.0.125:81/api/"; // "http://172.18.7.153:81/api";
String domain = "192.168.0.125";

int port = 81;

int wifiStatus = WL_IDLE_STATUS;

int intervalLoop = 5000;
int current = 0;

JSONVar deviceProps;
JSONVar fingerProps;

WebSocketsClient webSocket;
Adafruit_Fingerprint finger = Adafruit_Fingerprint(&Serial1, 0x00000000);

String combine(String urlExtension) {
  return host + urlExtension;
}

String getPayloadData(JSONVar data) {
  JSONVar body;

  body["deviceName"] = deviceName;
  body["data"] = data;

  return JSONVar::stringify(body);
}

JSONVar getReceivedData(JSONVar data) {
  JSONVar received;

  if(data.hasOwnProperty("status")) {
    String status = String((const char *)data["status"]);

    if (status != "OK") {
      Serial.println("[WSc][ERROR RESPONSE] " + status + " - MSG: " + String((const char *)data["msg"]));
    } else {
      received = data["data"];
    }
  }

  return received;
}

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

int getFingerImage() {
  int p = -1;
  while (p != FINGERPRINT_OK) {
    p = finger.getImage();
    switch (p) {
    case FINGERPRINT_OK:
      Serial.println("[FINGER] Image taken!");
      break;
    case FINGERPRINT_NOFINGER:
      Serial.println("[FINGER] Waiting to finger...");
      break;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("[FINGER] Communication error!");
      break;
    case FINGERPRINT_IMAGEFAIL:
      Serial.println("[FINGER] Imaging error!");
      break;
    default:
      Serial.println("[FINGER] Unknown error...");
      break;
    }
  }

  return p;
}

int takeFingerImage(int type = 1) {
  int p = finger.image2Tz(type);
  switch (p) {
    case FINGERPRINT_OK:
      Serial.println("[FINGER] Image converted");
      return p;
    case FINGERPRINT_IMAGEMESS:
      Serial.println("[FINGER] Image too messy");
      return p;
    case FINGERPRINT_PACKETRECIEVEERR:
      Serial.println("[FINGER] Communication error");
      return p;
    case FINGERPRINT_FEATUREFAIL:
      Serial.println("[FINGER] Could not find fingerprint features");
      return p;
    case FINGERPRINT_INVALIDIMAGE:
      Serial.println("[FINGER] Could not find fingerprint features");
      return p;
    default:
      Serial.println("[FINGER] Unknown error");
      return p;
  }
}

int removeFinger() {
  Serial.println("[FINGER] Remove finger");
  delay(2000);
  int p = 0;
  while (p != FINGERPRINT_NOFINGER) {
    p = finger.getImage();
  }
  return p;
}

int createModelFinger() {
  int p = finger.createModel();
  if (p == FINGERPRINT_OK) {
    Serial.println("[FINGER] Prints matched!");
    return p;
  } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
    Serial.println("[FINGER] Communication error");
    return p;
  } else if (p == FINGERPRINT_ENROLLMISMATCH) {
    Serial.println("[FINGER] Fingerprints did not match");
    return p;
  } else {
    Serial.println("[FINGER] Unknown error");
    return p;
  }
}

int storeModelFinger(int id) {
  Serial.print("[FINGER] ID "); Serial.println(id);
  int p = finger.storeModel(id);
  if (p == FINGERPRINT_OK) {
    Serial.println("[FINGER] Stored!");
    return p;
  } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
    Serial.println("[FINGER] Communication error");
    return p;
  } else if (p == FINGERPRINT_BADLOCATION) {
    Serial.println("[FINGER] Could not store in that location");
    return p;
  } else if (p == FINGERPRINT_FLASHERR) {
    Serial.println("[FINGER] Error writing to flash");
    return p;
  } else {
    Serial.println("[FINGER] Unknown error");
    return p;
  }
}

int registerFinger() {
  int hintId = finger.getTemplateCount();
  int p = getFingerImage();
  p = takeFingerImage(1);
  p = removeFinger();
  p = getFingerImage();
  p = takeFingerImage(2);
  p = createModelFinger();
  p = storeModelFinger(hintId + 1);

  return p;
}

void bindAction(JSONVar data) {
  if(data.hasOwnProperty("action")) {
    String action = String((const char *)data["action"]);       
    if( action == "REGISTER_FINGER") {
      int employeeId = data["employeeId"];      
      int tokenKey = data["tokenKey"];

      if (tokenKey == 0) {
        tokenKey = finger.getTemplateCount() + 1;
      }

      

      registerFinger();
    } else if ( action == "GET_HINTS") {

    } else if ( action == "SET_HINTS") {

    } else {
      Serial.println("[ACTION] not recognized! " + action);
    }
                    
    
    
  } else {
    Serial.println("[ACTION] No action key was found in payload response!");
  }
}

void webSocketEvent(WStype_t type, uint8_t * payload, size_t length) {
  String sPayload;
	switch(type) {
		case WStype_DISCONNECTED:
			Serial.printf("[WSc] Disconnected!\n");
			break;
		case WStype_CONNECTED:
			Serial.printf("[WSc] Connected to url: %s\n", payload);
			// send message to server when Connected
			// webSocket.sendTXT("CONNECTED");
			break;
		case WStype_TEXT:
			Serial.printf("[WSc] get text: %s\n", payload);
      sPayload = String((char*) payload);      
      bindAction( 
        getReceivedData(JSONVar::parse(sPayload)) 
      );
			break;
		case WStype_BIN:
			Serial.printf("[WSc] get binary length: %u\n", length);
			//hexdump(payload, length);

			// send data to server
			// webSocket.sendBIN(payload, length);
			break;
		case WStype_ERROR:			
		case WStype_FRAGMENT_TEXT_START:
		case WStype_FRAGMENT_BIN_START:
		case WStype_FRAGMENT:
		case WStype_FRAGMENT_FIN:
			break;
	}
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

void connectSocket() {
  webSocket.begin(domain, port, "/webSocket/" + deviceName);
	webSocket.onEvent(webSocketEvent);
  webSocket.setReconnectInterval(3000);
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

void setup() {
  Serial1.begin(57600, SERIAL_8N1, 16, 17);
  Serial.begin(115200);
  finger.begin(57600);
  
  WiFi.mode(WIFI_STA);
  
  Serial.println("[STARTUP] Starting!...");

  scanWifis();
  connectFingerprint();
  connectWifi();
  connectSocket();
  showDetails();
}

void loop() {  
  wifiStatus = WiFi.status();  
  if(wifiStatus != WL_CONNECTED) connectWifi();
  webSocket.loop();
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