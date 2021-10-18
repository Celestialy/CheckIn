#include <Arduino.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <SPI.h>
#include <MFRC522.h>
#include <EEPROM.h>

String APIURL = "<Backend api url>";
 
int ledRed = 2; // D4
int ledGreen = 0; // D3
int ledYellow = 16; // D0

int RFID_RST = 5; // D1
int RFID_SS = 4; // D2

MFRC522 mfrc522(RFID_SS, RFID_RST);

#define buzzer 15

void setup() {
  EEPROM.begin(512);
  pinMode(ledRed, OUTPUT);
  pinMode(ledGreen, OUTPUT);
  pinMode(ledYellow, OUTPUT);
  digitalWrite(ledRed, HIGH);
  digitalWrite(ledGreen, LOW);
  digitalWrite(ledYellow, LOW);

  Serial.begin(115200);
  SPI.begin();

  //clearEEPROM();
  
  checkNetwork(); 

  mfrc522.PCD_Init();
  digitalWrite(ledRed, LOW);
  Serial.println(("RFID Scanner ready.."));
}

void loop() {
  if (WiFi.status() == WL_CONNECTED) {
    digitalWrite(ledRed, LOW);
    digitalWrite(ledGreen, HIGH);
    if (!mfrc522.PICC_IsNewCardPresent()) { // Look for new cards
      return;
    }

    if (!mfrc522.PICC_ReadCardSerial()) { // Select one of the cards
      return;
    }

    String content = "";
    for (byte i = 0; i < mfrc522.uid.size; i++) {
      content.concat(String(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " "));
      content.concat(String(mfrc522.uid.uidByte[i], HEX));
    }
    content.toUpperCase();
    postCheckInOut(content.substring(1));
    delay(500);
  } else {
    digitalWrite(ledRed, HIGH);
    digitalWrite(ledGreen, LOW);
    Serial.println("Reconnecting to Wifi..");
    checkNetwork();
    delay(3000);
  }
}

void postCheckInOut(String givenCardID) { // Post checkin to API
  digitalWrite(ledYellow, HIGH);
  HTTPClient http;
  http.setTimeout(10000);
  http.begin(APIURL);
  http.addHeader("Content-Type", "application/json");
  String MAC = WiFi.macAddress();
  int httpCode = http.POST("{\"cardID\":\"" + givenCardID + "\",\"macAddress\":\"" + MAC + "\"}");
  Serial.print("Http Code: ");
  Serial.println(httpCode);
  http.end();
  Serial.println(givenCardID);
  tone(buzzer, 2500);
  digitalWrite(ledYellow, LOW);
  if (httpCode == 200) {
    cardAccepted();
  } else {
    cardFailed();
  }

}

void cardAccepted() { // Led and Buzzer on card accept
  Serial.println("Card Accepted");
  digitalWrite(ledGreen, HIGH);
  delay(250);
  noTone(buzzer);
  digitalWrite(ledGreen, LOW);
}

void cardFailed() { // Led and Buzzer on card fail
  Serial.println("Card Failed");
  digitalWrite(ledRed, HIGH);
  delay(50);
  noTone(buzzer);
  tone(buzzer, 150);
  delay(50);
  noTone(buzzer);
  delay(100);
  tone(buzzer, 150);
  delay(50);
  noTone(buzzer);
  delay(100);
  tone(buzzer, 150);
  delay(50);
  noTone(buzzer);
  digitalWrite(ledRed, LOW);
}

void firstTimeSetup() { // First time setup
  wifiScan();
  Serial.println("Please type SSID");
  while (Serial.available() == 0) {

  }
  String userInputSSID = Serial.readString();


  Serial.println("Please type Password");
  while (Serial.available() == 0) {

  }
  String userInputPassword = Serial.readString();

  
  String clear = Serial.readString();

  for (int i = 0; i < userInputSSID.length() + userInputPassword.length() + 1; i++) {
    EEPROM.write(i, 0);
  }
  writeStringToEEPROM(0, userInputSSID);
  writeStringToEEPROM(userInputSSID.length() + 1, userInputPassword);
  EEPROM.commit();
  delay(2000);
  Serial.println("Network saved..");
  checkNetwork();
}

void checkNetwork() {
  Serial.println("Checking for saved network in EEPROM...");
  String SSID = readStringFromEEPROM(0);
  String Password = readStringFromEEPROM(SSID.length() + 1);

  if(SSID != "" || Password != "") {
    Serial.println("Found SSID/Password: " + SSID + "/" + Password);
    wifiConnect(SSID, Password);
  } else {
    Serial.println("Couldnt find a network, running first time setup..");
    firstTimeSetup();
  }
}

void wifiScan() { // Scan for nearby networks
  int networks = WiFi.scanNetworks();
  Serial.println("First time setup, scanning nearby networks");

  if (networks == 0) {
    Serial.println("no networks found");
  } else {
    Serial.print(networks);
    Serial.println(" networks found");
    for (int i = 0; i < networks; ++i) { // Print SSID and RSSI for each network found
      Serial.print(i + 1);
      Serial.print(": ");
      Serial.print(WiFi.SSID(i));
      Serial.print(" (");
      Serial.print(WiFi.RSSI(i));
      Serial.print(")");
      Serial.println((WiFi.encryptionType(i) == ENC_TYPE_NONE) ? " " : "*");
      delay(10);
    }
  }
}

void wifiConnect(String SSID, String Password) { // Connect to given network
  WiFi.disconnect();
  WiFi.mode(WIFI_STA);
  WiFi.begin(SSID, Password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.print(".");
  }

  delay(1000);

  if(WiFi.status() == WL_CONNECTED) {
    Serial.println("");
    Serial.println("WiFi connected");
    Serial.println("MAC address: " + WiFi.macAddress());
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP());
  } else {
    Serial.print("IP address: ");
  }
}

String readStringFromEEPROM(int address) { // Read string at given start address
  char data[100];
  int len = 0;
  unsigned char k;
  k = EEPROM.read(address);
  while((k != '\0' || k != '') && len < 100)   // Read until null character
  {
    k = EEPROM.read(address + len);
    data[len] = k;
    len++;
  }
  data[len] = '\0';
  return String(data);
}

void writeStringToEEPROM(int addrOffset, const String &strToWrite) { // Write string to EEPROM at given start address
  byte len = strToWrite.length();
  for (int i = 0; i < len; i++)
  {
    EEPROM.write(addrOffset + i, strToWrite[i]);
  }
}

void clearEEPROM() {
  Serial.println("");
  Serial.println("type clear to clear EEPROM");
  while (Serial.available() == 0) {

  }
  String inputClear = Serial.readString();
  if (inputClear == "clear") {
    Serial.println("EEPROM Cleared");
    for (int i = 0; i < 1024; i++) {
          EEPROM.write(i, 0);
      };
  }
}
