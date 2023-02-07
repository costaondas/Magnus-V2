
#include <ezButton.h>

ezButton button1(2);
ezButton button2(3);
ezButton button3(4);
ezButton button4(5);
char mychar;
String inputString = "";
bool stringComplete = false;

void setup() {
  inputString.reserve(200);
  Serial.begin(9600);
  pinMode(6, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(9, OUTPUT);
  pinMode(10, OUTPUT);
  pinMode(11, OUTPUT);
  pinMode(12, OUTPUT);
  pinMode(13, OUTPUT);

  digitalWrite(6, LOW);
  digitalWrite(7, LOW);
  digitalWrite(8, LOW);
  digitalWrite(9, LOW);
  digitalWrite(10, LOW);
  digitalWrite(11, LOW);
  digitalWrite(12, LOW);
  digitalWrite(13, LOW);
  button1.setDebounceTime(50);
  button2.setDebounceTime(50);
  button3.setDebounceTime(50);
  button4.setDebounceTime(50);
}
void comand(String _comand)
    {


              

              char Buf[50];
              _comand.toCharArray(Buf, 50);

            if(Buf[0] == '0'){
              digitalWrite(6, LOW);
            }
            else{ digitalWrite(6, HIGH);}

            if(Buf[1] == '0'){
              digitalWrite(7, LOW);
            }
            else{ digitalWrite(7, HIGH);}


            if(Buf[2] == '0'){
              digitalWrite(8, LOW);
            }
            else{ digitalWrite(8, HIGH);}


            if(Buf[3] == '0'){
              digitalWrite(9, LOW);
            }
            else{ digitalWrite(9, HIGH);}


            if(Buf[4] == '0'){
              digitalWrite(10, LOW);
            }
            else{ digitalWrite(10, HIGH);}


            if(Buf[5] == '0'){
              digitalWrite(11, LOW);
            }
            else{ digitalWrite(11, HIGH);}


            if(Buf[6] == '0'){
              digitalWrite(12, LOW);
            }
            else{ digitalWrite(12, HIGH);}


            if(Buf[7] == '0'){
              digitalWrite(13, LOW);
            }
            else{ digitalWrite(13, HIGH);}
            
      }

void loop() {
  delay(10);
  /*
  button1.loop();
  button2.loop();
  button3.loop();
  button4.loop();

  //Serial.println("ALIVE");

  int btn1State = button1.getState();
  int btn2State = button2.getState();
  int btn3State = button3.getState();
  int btn4State = button3.getState();

  if(button1.isPressed())
    Serial.println("inPut1;low");

  if(button1.isReleased())
    Serial.println("inPut1;high");

  if(button2.isPressed())
    Serial.println("inPut2;low");

  if(button2.isReleased())
    Serial.println("inPut2;high");

  if(button3.isPressed())
    Serial.println("inPut3;low");

  if(button3.isReleased())
    Serial.println("inPut3;high");
    
  if(button4.isPressed())
    Serial.println("inPut4;low");

  if(button4.isReleased())
    Serial.println("inPut4;high");


*/
 
if (stringComplete) {
    Serial.println("RECEBIDO<" + inputString + ">");
    comand(inputString);
    
    inputString = "";
    stringComplete = false;
  }
  
  String analRead = "";
  analRead = "INPUT:";
  for(int a = 0;a < 6; a++)
  {
    
    if(a != 5)
    {
      analRead +=String(analogRead(a)) + ";" ;
    }
    else
    {
      analRead += String(analogRead(a));
    }
    
  }
 // Serial.println(analRead);
  

    
}







void serialEvent() {
  while (Serial.available()) 
  
  {
    inputString = Serial.readStringUntil('\r\n');
      stringComplete = true;
      //Serial.println("Received<" + inputString + ">"); //com essa linha funciona
    }
    
  }
