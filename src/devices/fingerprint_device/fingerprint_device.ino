#include <Arduino_JSON.h>
#include <JSON.h>
#include <JSONVar.h>
#include <WiFi.h>
#include <WebSocketsClient.h>
#include <HardwareSerial.h>
#include <Adafruit_Fingerprint.h>
#include <HTTPClient.h>

#define DOWNLOAD_IMAGE 0x0b;
#define UPLOAD_IMAGE 0x0a;

/* on send -> { "deviceName": "", "data": { "action": "" } } */
/* on receive/response -> { "status": "OK", "msg": "COMPLETE", "data": { "action": "" } } */

const String TYPE_FINGER_DETECTED = "FINGER_DETECTED";
const String TYPE_FINGER_MODEL = "FINGER_MODEL";
//const char * TYPE_FINGER_DETECTED = "FINGER_DETECTED";

char *ssid = "UTT-CUERVOS"; // "IZZI-99D0"; // "test-ard";
char *pass = "CU3RV@S2022"; // "VBBPMSZNNEJV"; // "12345678";
String deviceName = "ESP-FG-TNT";
String model = "ESP-32";

String host = "http://172.17.4.85:81/api/"; // "http://192.168.0.125:81/api/"; //
String domain = "172.17.4.85"; // "192.168.0.125"; // 
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

void printHex(int num, int precision) {
  char tmp[16];
  char format[128];  


  sprintf(format, "%%.%dX", precision);

  sprintf(tmp, format, num);
  Serial.print(tmp);
}

String getPayloadData(JSONVar data) {
  JSONVar body;

  body["deviceName"] = deviceName;
  body["data"] = data;

  return JSONVar::stringify(body);
}

String getPayloadDataMsg(String msg, String type) {
  JSONVar data;

  data["action"] = "INFO";
  data["msg"] = msg;
  data["type"] = type;

  return getPayloadData(data);
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
  String fingerMsg;
  while (p != FINGERPRINT_OK) {    
    p = finger.getImage();
    switch (p) {
      case FINGERPRINT_OK:
        fingerMsg = "[FINGER] Image taken!";      
        break;
      case FINGERPRINT_NOFINGER:
        fingerMsg = "[FINGER] Waiting to finger...";      
        break;
      case FINGERPRINT_PACKETRECIEVEERR:
        fingerMsg = "[FINGER] Communication error!";      
        break;
      case FINGERPRINT_IMAGEFAIL:
        fingerMsg = "[FINGER] Imaging error!";      
        break;
      default:
        fingerMsg = "[FINGER] Unknown error...";      
        break;
    }    
    
    Serial.println(fingerMsg);
    String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
    webSocket.sendTXT(jsonMsg);
  }  

  return p;
}

int takeFingerImage(int type = 1) {
  int p = finger.image2Tz(type);
  String fingerMsg;
  
  switch (p) {
    case FINGERPRINT_OK:
      fingerMsg = "[FINGER] Image converted";
      break;
    case FINGERPRINT_IMAGEMESS:
      fingerMsg = "[FINGER] Image too messy";
      break;
    case FINGERPRINT_PACKETRECIEVEERR:
      fingerMsg = "[FINGER] Communication error";
      break;
    case FINGERPRINT_FEATUREFAIL:
      fingerMsg = "[FINGER] Could not find fingerprint features";
      break;
    case FINGERPRINT_INVALIDIMAGE:
      fingerMsg = "[FINGER] Invalid image";
      break;
    default:
      fingerMsg = "[FINGER] Unknown error";
      break;
  }

  Serial.println(fingerMsg);
  String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
  webSocket.sendTXT(jsonMsg);

  return p;
}

int removeFinger() {  
  String fingerMsg;
  delay(2000);
  int p = 0;
  while (p != FINGERPRINT_NOFINGER) {
    fingerMsg = "[FINGER] Remove finger";
    Serial.println(fingerMsg);
    String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
    webSocket.sendTXT(jsonMsg);
    p = finger.getImage();
    delay(100);
  }

  return p;
}

int createModelFinger() {
  String fingerMsg;
  int p = finger.createModel();

  if (p == FINGERPRINT_OK) {
    fingerMsg = "[FINGER] Prints matched!";
  } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
    fingerMsg = "[FINGER] Communication error";
  } else if (p == FINGERPRINT_ENROLLMISMATCH) {
    fingerMsg = "[FINGER] Fingerprints did not match";
  } else {
    fingerMsg = "[FINGER] Unknown error";
  }

  Serial.println(fingerMsg);
  String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
  webSocket.sendTXT(jsonMsg);

  return p;
}

int storeModelFinger(int id) {
  String fingerMsg;
  int p = finger.storeModel(id);

  Serial.print("[FINGER] ID "); Serial.println(id);  
  if (p == FINGERPRINT_OK) {
    fingerMsg = "[FINGER] Stored!";
  } else if (p == FINGERPRINT_PACKETRECIEVEERR) {
    fingerMsg = "[FINGER] Communication error";    
  } else if (p == FINGERPRINT_BADLOCATION) {
    fingerMsg = "[FINGER] Could not store in that location";    
  } else if (p == FINGERPRINT_FLASHERR) {
    fingerMsg = "[FINGER] Error writing to flash";
    return p;
  } else {
    fingerMsg = "[FINGER] Unknown error";    
  }

  Serial.println(fingerMsg);
  String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
  webSocket.sendTXT(jsonMsg);

  return p;
}

int loadFingerModel(int id) {
  String fingerMsg;
  int p = finger.loadModel(id);

  switch (p) {
    case FINGERPRINT_OK:      
      fingerMsg = "[FINGER] Template " + String(id) + " loaded";
      break;
    case FINGERPRINT_PACKETRECIEVEERR:
      fingerMsg = "[FINGER] Communication error";
      break;
    default:
      fingerMsg = "[FINGER] Unknown error...";
      break;
  }

  Serial.println(fingerMsg);
  String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
  webSocket.sendTXT(jsonMsg);

  return p;
}

int getFingerModel() {
  String fingerMsg;
  int p = finger.getModel();
  switch (p) {
    case FINGERPRINT_OK:
      fingerMsg = "[FINGER] Transferring Template";
      break;
    default:
      fingerMsg = "[FINGER] Unknown error";
      break;
  }

  Serial.println(fingerMsg);
  String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
  webSocket.sendTXT(jsonMsg);

  return p;
}

int registerFinger(int tokenKey) {  
  int p = getFingerImage();
  p = takeFingerImage(1);  
  p = removeFinger();
  p = getFingerImage();
  p = takeFingerImage(2);
  p = createModelFinger();
  p = storeModelFinger(tokenKey);

  // downloadFingerprintTemplate(tokenKey);

  return p;
}

int downloadFingerprintTemplate(int id)
{
  int p = loadFingerModel(id);
  p = getFingerModel();

  // uint8_t bytesReceived[534];

  // Adafruit_Fingerprint_Packet imagePacket(DOWNLOAD_IMAGE, sizeof(data), data);       

  // // one data packet is 267 bytes. in one data packet, 11 bytes are 'usesless' :D
  // uint8_t bytesReceived[534]; // 2 data packets
  // memset(bytesReceived, 0xff, 534);

  // uint32_t starttime = millis();
  // int i = 0;
  // while (i < 534 && (millis() - starttime) < 20000) {
  //   if (Serial1.available()) {
  //     bytesReceived[i++] = Serial1.read();
  //   }
  // }  

  // uint8_t fingerTemplate[512]; // the real template
  // memset(fingerTemplate, 0xff, 512);

  // // filtering only the data packets
  // int uindx = 9, index = 0;
  // memcpy(fingerTemplate + index, bytesReceived + uindx, 256);   // first 256 bytes
  // uindx += 256;       // skip data
  // uindx += 2;         // skip checksum
  // uindx += 9;         // skip next header
  // index += 256;       // advance pointer
  // memcpy(fingerTemplate + index, bytesReceived + uindx, 256);   // second 256 bytes

  // for (int i = 0; i < 512; ++i) {    
  //   printHex(fingerTemplate[i], 2);    
  // }

  // Serial.println("\ndone.");

  return p;
}

void bindAction(JSONVar data) {
  if(data.hasOwnProperty("action")) {
    String action = String((const char *)data["action"]);       
    if( action == "REGISTER_FINGER") {
      int employeeId = data["employeeId"];      
      int tokenKey = data["hintKey"];

      if (tokenKey == 0) {
        tokenKey = finger.getTemplateCount() + 1;
      }

      int result = registerFinger(tokenKey);

      if (result == FINGERPRINT_OK) {        
        data["hintKey"] = tokenKey;
        postRequest(combine("device/setHint"), data);
      }

    } else if ( action == "GET_HINTS") {
      // This one is kinda difficult to do...
    } else if ( action == "DELETE_HINT") {
      int tokenKey = data["hintKey"];
      int result = finger.deleteModel(tokenKey);
      
      if (result == FINGERPRINT_OK) {
        String fingerMsg = "[FINGER] Delete Model for finger ID: " + String(tokenKey);
        Serial.println(fingerMsg);
        String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_MODEL);
        webSocket.sendTXT(jsonMsg);
      }

    } else if ( action == "DELETE_HINTS") {

      finger.emptyDatabase();
      String fingerMsg = "[FINGER] Empty Fingers Data Base";
      Serial.println(fingerMsg);
      String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_MODEL);
      webSocket.sendTXT(jsonMsg);

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
  deviceProps["deviceModel"] = model;
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

  connectFingerprint();
  scanWifis();  
  connectWifi();
  connectSocket();
  showDetails();
}

void loop() {
  connectFingerprint(false);
  wifiStatus = WiFi.status();  
  if(wifiStatus != WL_CONNECTED) connectWifi();
  webSocket.loop();  

  if (current > intervalLoop) {
    loadDetails();    
    postRequest(combine("Device"), deviceProps);
    current = 0;
  }

  int p = finger.getImage();

  if (p == FINGERPRINT_OK) {
    String fingerMsg;
    JSONVar fingerPost;
    
    p = getFingerImage();
    p = takeFingerImage(1);
    p = finger.fingerSearch();
    removeFinger();

    if( p == FINGERPRINT_OK ) {      
      fingerPost["deviceName"] = deviceName;
      fingerPost["hintKey"] = finger.fingerID;
      fingerPost["confidence"] = finger.confidence;
      fingerPost["status"] = "OK";

      fingerMsg = "[FINGER] Finger Match ID: " + String(finger.fingerID) + ". Confidence: " + String(finger.confidence);
    } else {
      fingerPost["deviceName"] = deviceName;
      fingerPost["status"] = "NOT_MATCH";
      fingerMsg = "[FINGER] Finger did not MATCH!";
    }  

    Serial.println(fingerMsg);
    String jsonMsg = getPayloadDataMsg(fingerMsg, TYPE_FINGER_DETECTED);
    webSocket.sendTXT(jsonMsg);

    postRequest(combine("device/signal"), fingerPost);

  }

  delay(100);
  current += 100;
}