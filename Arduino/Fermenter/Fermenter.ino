#include <U8glib.h>
#include <EEPROM.h>

//Max mem = 2048 bytes

float Temperature = 0.f;

//Communication constants
#define CONNECT = "<c";
#define ACCEPT = "<a";
#define KEEP_ALIVE = "<k";
#define SET_ID = "<i";
#define END_OF_LINE = '\n';
#define TIMEOUT = 5;

//Relay pins
#define COOLER_RELAY 3
#define LIGHT_RELAY 4

//display
U8GLIB_SSD1306_128X64 u8g(U8G_I2C_OPT_NONE);	// I2C / TWI 

char TEMP_CHAR = '°';

void setup()
{
  // start serial port at 9600 bps:
  Serial.begin(9600); 
  Serial.setTimeout(10000); 
  
  loadConfig();
  
  setupDisplay();
  establishContact();  // send a byte to establish contact until receiver responds 
  
    // if analog input pin 0 is unconnected, random analog
  // noise will cause the call to randomSeed() to generate
  // different seed numbers each time the sketch runs.
  // randomSeed() will then shuffle the random function.
  randomSeed(analogRead(0));
  
  pinMode(COOLER_RELAY, OUTPUT);     
  pinMode(LIGHT_RELAY, OUTPUT);
  digitalWrite(LIGHT_RELAY, LOW);     
}

void loop()
{
  handleCooling();
 
  // show sensor values
  pictureLoop();            
  
  delay(1000);
  renegociateConnexion();
  
}

//-----------------------
//     Cooling
void handleCooling()
{
  GetTemperature();
  if (Temperature > 17.f)
    digitalWrite(COOLER_RELAY, LOW);   
   if (Temperature < 16.f)
    digitalWrite(COOLER_RELAY, HIGH);   
}

//----------------------
//    Serial connexion
void establishContact() 
{
  /*while (Serial.available() <= 0) 
  {
    delay(1000);
  }
  
  if (CONNECT == Serial.readStringUntil(END_OF_LINE))
  {
      String tmp = (ACCEPT + END_OF_LINE);
      Serial.write(tmp);
      tmp = (storage.ID + END_OF_LINE);
      Serial.write(tmp);
  }
  else
  {
    establishContact();
  }*/
}

void renegociateConnexion()
{
 /* String request = Serial.readStringUntil(END_OF_LINE);
  if (request == KEEP_ALIVE)
      return;
  else if (request == SET_ID)
  {
    String new_ID = Serial.readStringUntil(END_OF_LINE);
    if (new_ID != "")
    {
      storage.ID = new_ID;
      saveConfig();
    }
    
    renegociateConnexion();
  }*/
     
}

//-------------------------------
//        Display Connexion
void setupDisplay()
{
  u8g.begin();
  u8g.setFont(u8g_font_fub30);
}

void pictureLoop()
{
  char tmp[6];
  dtostrf(Temperature,2,1,tmp);
  tmp[4] = TEMP_CHAR;
  tmp[5] = '\0';
  int xpos = 30;
  int ypos = 50;
  // picture loop
  u8g.firstPage();  
  do {
    u8g.drawStr( xpos, ypos, tmp );
  } while( u8g.nextPage() );
  
  
}

//------------------------------
//        ReadTemperature
void GetTemperature()
{
  Temperature = (random(30) / 10.f) + 15.f;
}

//---------------------------------
// ID Storing
// ID of the settings block

#define CONFIG_VERSION "ls1"

// Tell it where to store your config data in EEPROM
#define CONFIG_START 32
#define ID_LENGTH 20

// Example settings structure
struct StoreStruct {
 
  // This is for mere detection if they are your settings
  char version[4];
  // The variables of your settings
  char ID[ID_LENGTH];
} storage = {
  CONFIG_VERSION,
  // The default values
  "͡° ͜ʖ ͡°0000\0"
};

void loadConfig() {
  // To make sure there are settings, and they are YOURS!
  // If nothing is found it will use the default settings.
  if (EEPROM.read(CONFIG_START + 0) == CONFIG_VERSION[0] &&
      EEPROM.read(CONFIG_START + 1) == CONFIG_VERSION[1] &&
      EEPROM.read(CONFIG_START + 2) == CONFIG_VERSION[2])
    for (unsigned int t=0; t<sizeof(storage); t++)
      *((char*)&storage + t) = EEPROM.read(CONFIG_START + t);
}

void saveConfig() {
  for (unsigned int t=0; t<sizeof(storage); t++)
    EEPROM.write(CONFIG_START + t, *((char*)&storage + t));
}

