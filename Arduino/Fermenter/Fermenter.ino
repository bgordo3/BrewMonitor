#include <U8glib.h>
#include <EEPROM.h>

//Max mem = 2048 bytes

float Temperature = 0.f;

//Communication constants
#define CONNECT "<c"
#define ACCEPT  "<a"
#define KEEP_ALIVE  "<k"
#define SET_TARGET  "<t"
#define SET_ID  "<i"
#define GET_TEMP  "<g"
#define END_OF_LINE  '\n'
#define END_OF_CSTR  '\0'
#define TIMEOUT  5000

#define DEBUG_MODE true
char debugStr[3];

//Relay pins
#define HSWITCH1_RELAY 3
#define HSWITCH2_RELAY 4

//display
U8GLIB_SSD1306_128X64 u8g(U8G_I2C_OPT_NONE);	// I2C / TWI
#define TEMP_CHAR '°'

//Data save
#define CONFIG_VERSION "ls1"
#define CONFIG_START 32
#define ID_LENGTH 20

void setup()
{
  // start serial port at 9600 bps:
  Serial.begin(9600);
  Serial.setTimeout(TIMEOUT);

  loadPersistentConfig();

  setupDisplay();

  // if analog input pin 0 is unconnected, random analog
  // noise will cause the call to randomSeed() to generate
  // different seed numbers each time the sketch runs.
  // randomSeed() will then shuffle the random function.
  randomSeed(analogRead(0));

  pinMode(HSWITCH1_RELAY, OUTPUT);
  pinMode(HSWITCH2_RELAY, OUTPUT);
  digitalWrite(HSWITCH1_RELAY, LOW);
  digitalWrite(HSWITCH2_RELAY, HIGH);
}

void loop()
{
  // Get temperature and start cooling if needed.
  handleCooling();

  // Show sensor values (temperature) on display.
  displayLoop();

  // Respond to request
  handleServerRequest();

  //delay(1000);
}

//--------------------------------------------------------------
//                 ID Storing - Settings

struct StoreStruct
{
  // This is for mere detection if they are your settings
  char version[4];
  float TargetTemperature;
  // The variables of your settings
  char ID[ID_LENGTH];
}
storage =
{
  CONFIG_VERSION,
  21.f,
  // The default values
  "000000000000000000\0"
};

void loadPersistentConfig() {
  // To make sure there are settings, and they are YOURS!
  // If nothing is found it will use the default settings.
  if (EEPROM.read(CONFIG_START + 0) == CONFIG_VERSION[0] &&
      EEPROM.read(CONFIG_START + 1) == CONFIG_VERSION[1] &&
      EEPROM.read(CONFIG_START + 2) == CONFIG_VERSION[2])
    for (unsigned int t = 0; t < sizeof(storage); t++)
      *((char*)&storage + t) = EEPROM.read(CONFIG_START + t);
}

void saveConfig() {
  for (unsigned int t = 0; t < sizeof(storage); t++)
    EEPROM.write(CONFIG_START + t, *((char*)&storage + t));
}

//--------------------------------------------------------------
//                          Cooling

void handleCooling()
{
  GetTemperature();
  if (Temperature > storage.TargetTemperature + 0.5f)
  {
    digitalWrite(HSWITCH1_RELAY, HIGH);
    digitalWrite(HSWITCH2_RELAY, HIGH);
  }
  if (Temperature < storage.TargetTemperature - 0.5f)
  {

    digitalWrite(HSWITCH1_RELAY, LOW);
    digitalWrite(HSWITCH2_RELAY, LOW);
  }
}

//--------------------------------------------------------------
//                    Serial connexion

void handleServerRequest()
{
  
  memcpy(debugStr, "\000", 3);
  //Get the request
  String request = Serial.readStringUntil(END_OF_LINE);
  Serial.write(("ack : " + request + END_OF_LINE).c_str());
  //Handle it.
  if (request == KEEP_ALIVE)
    return;
  else if (request == SET_ID)
  {
    String new_ID = Serial.readStringUntil(END_OF_LINE);
    if (new_ID != "" && new_ID.length() <= ID_LENGTH)
    {
      new_ID.toCharArray(storage.ID, ID_LENGTH);
      Serial.write((new_ID + END_OF_LINE).c_str());
      Serial.write((String(storage.ID) + END_OF_LINE).c_str());
      saveConfig();
      memcpy(debugStr, SET_ID, 3);
    }
  }
  else if (request == SET_TARGET)
  {
    String new_Temp = Serial.readStringUntil(END_OF_LINE);
    if (new_Temp != "" && new_Temp.toFloat() != 0.f)
    {
      storage.TargetTemperature = new_Temp.toFloat();
      writeFloatToSerial(storage.TargetTemperature);
      saveConfig();
      memcpy(debugStr, SET_TARGET, 3);
    }
  }
  else if (request == CONNECT)
  {
    Serial.write((String(ACCEPT) + END_OF_LINE).c_str());
    Serial.write((String(storage.ID) + END_OF_LINE).c_str());
    memcpy(debugStr, CONNECT, 3);
  }
  else if (request == GET_TEMP)
  {
    writeFloatToSerial(Temperature);
    memcpy(debugStr, GET_TEMP, 3);
  }
}

void writeFloatToSerial(const float& inFloat)
{
  char tmp[6];
  dtostrf(inFloat, 2, 1, tmp);
  tmp[4] = END_OF_LINE;
  tmp[5] = END_OF_CSTR;
  Serial.write(tmp);
}

//--------------------------------------------------------------
//                        Display

void setupDisplay()
{
  u8g.begin();
  u8g.setFont(u8g_font_fub30);
}

void displayLoop()
{

  char tmp[6];
  dtostrf(Temperature, 2, 1, tmp);
  tmp[4] = TEMP_CHAR;
  tmp[5] = END_OF_CSTR;
  int xpos = 30;
  int ypos = 50;
  // picture loop
  u8g.firstPage();
  do {
    if (DEBUG_MODE)
      u8g.drawStr( xpos, ypos, debugStr );
    u8g.drawStr( xpos, ypos, tmp );
  } while ( u8g.nextPage() );
}

//--------------------------------------------------------------
//                      Read Temperature

void GetTemperature()
{
  Temperature = (random(30) / 10.f) + 15.f;
}



