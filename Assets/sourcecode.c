#include <stdio.h>  // sprintf() - Print formatted string to buffer
#include <stdlib.h> //<random.h> // rand()    - Random number from 0 to 32767
#include <unistd.h> //<time.h>   // sleep()   - Sleep for x milliseconds
#include <string.h> // strlen()  - Measure string length up until, but not including a 0-character

//bool definitions, as c does not have them
typedef unsigned char bool;
#define true 1
#define false 0

/*
 * HARDWARE INTERFACE ROUTINES
 */

// EEPROM addresses
#define PASSWORD_LENGTH 4
#define PASSWORD_LENGTH_ADDRESS 0
#define PASSWORD_ADDRESS        1
#define TRIES_ADDRESS           64
#define UNLOCKSEQUENCE          "++AA&$aa0000000"
#define RESETLOCKSEQUENCE       "++AA&$bb0000000" 


// Initialisation routines for the peripherals
extern void init_LCD();                            // set pins for LCD
extern void init_EEPROM();                         // set pins for serial EEPROM
extern void init_Keypad();                         // set pins for keypad
extern void init_LEDS();                           // set pins to drive leds
extern void init_IO();                             // set pins to drive lock/unlock IO

// Read from the keypad, 1 character at a time, buffered. the keys are mapped to characters as:
// +-----+
// |1 2 3|
// |4 5 6|
// |7 8 9|
// |x 0 v|
// +-----+
extern unsigned char ReadKeyPad();

// Read and write to external EEPROM
extern void EEPROM_write(unsigned char address, unsigned char data); // write char to address
extern unsigned char EEPROM_read(unsigned char address);             // read from address

// Set the red and green leds
extern void LED_set(bool green_led, bool red_led);

// LCD routines
extern void LCD_write(unsigned char * text, bool clearLCD); // write the LCD, and optionally clear before writing
extern void LCD_backspace();                       // remove the last character from the LCD

//send serial data over IO pins
extern void serial_out(unsigned char * data);

extern unsigned char serial_in();

/*
 * MAIN CODE
 */

// Generate a random password of defined length
void SetPassword(unsigned char length);
// Process keypresses
void KeyInput(unsigned char input);
// Check the password entered with the one in the EEPROM, and unlock if correct
void CheckPassword();
// Unlock the lock via IO
void Unlock();

// Global variables
unsigned char WelcomeText[] = "Enter code:";
// Preset values and buffer
unsigned char InputBuffer[PASSWORD_LENGTH+1] = "";
unsigned char MaxTries = 10;


//the main routine, should run forever
void main()
{
    //initialise all peripheral
    init_LCD();
    init_EEPROM();
    init_Keypad();
    init_LEDS();
    init_IO();

    //get ready for normal operation
    LCD_write(WelcomeText,true);
    LED_set(false,false);
    EEPROM_write(PASSWORD_LENGTH_ADDRESS, PASSWORD_LENGTH);
    EEPROM_write(TRIES_ADDRESS, 0);
    SetPassword(PASSWORD_LENGTH);

    while(true) // loop forever
    {
        unsigned char check = serial_in();//check if we are connected to the lock
        if(check != 'C') // no keepalive received
        {
            LCD_write("Bus error!",true);
        }
        else
        {
            unsigned char key = ReadKeyPad();    // retrieve 1 keypress from keypad
            if(key != 0)
            {
                KeyInput(key);              // process the keypress
            }
        }
        usleep(100000); // have loop execute every 100ms
    }
}

// Set a random password
void SetPassword(unsigned char length)
{
    for(int i = 0; i < length; i++) // perform this operation for the amount of characters in the password
    {
        unsigned char digit = rand() % 10; //get a true random value from 0 to 9 (random is not determinstic)
        EEPROM_write(PASSWORD_ADDRESS + i, digit + '0'); // store the character value instead of the number in EEPROM
    } 
}

// Routine to process keypresses
void KeyInput(unsigned char input)
{
    if(input == 'x') //backspace pressed
    {
        if(strlen(InputBuffer) > 0)
        {
            LCD_backspace();//remove last character
            InputBuffer[strlen(InputBuffer) -1] = 0;//zero terminate the string
        }
    }
    else //if input is v or a number
    {
        unsigned char _passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
        if(input != 'v' && strlen(InputBuffer) < _passwordLength) // add number if its below password length, else it is ignored
        {
            LCD_write("*",false);//display a '*' and do not clear the LCD

            int len = strlen(InputBuffer);
            InputBuffer[len] = input;//add the key to the input buffer
            InputBuffer[len+1] = 0; //zero terminate the string
        }
        if(input == 'v' ) //pressed ok, so check password
        {
            for(int i = strlen(InputBuffer); i < _passwordLength; i++) //pad the password if it is not matching length
            {
                InputBuffer[i] = 'A';//pad it with character A, to prevent underflow
            }
            InputBuffer[_passwordLength] = 0;//zero terminate string

            CheckPassword();//call checking routine
        }
    }
}

// Routine to check the password for validity
void CheckPassword()
{
    LCD_write("Validating", true);
    sleep(1);

    unsigned char _passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
    if(_passwordLength == 0)// a 0 lenght password is not allowed
    {
        LCD_write("No password set!",true);
        LED_set(false,true); //enable red led
        sleep(1);

        //reset the lock again for input
        InputBuffer[0] = 0;
        LCD_write(WelcomeText,true); //default text
        LED_set(false,false); //disable leds 
        return;         
    }

    // Check the password, one character a a time
    for(unsigned char i = 0; i < _passwordLength; i++)
    {
        if(InputBuffer[i] != EEPROM_read(PASSWORD_ADDRESS + i)) //check if character of input buffer does not match password
        {
            // if so, stop with checking and show error
            LCD_write("Incorrect password",true);
            LED_set(false,true); //enable red led
            sleep(1);

            unsigned char tries = EEPROM_read(TRIES_ADDRESS);//retrieve tries from EEPROM
            if(tries < MaxTries)//check if we are still allowed to try. if so increment try, if not, lock 
            {
                EEPROM_write(TRIES_ADDRESS, tries + 1);//increment tries in EEPROM

                unsigned char text[32];
                sprintf(text,"%i tries left",MaxTries - tries);
                LCD_write(text,true);//display the amount of tries left
                sleep(1);

                //reset the lock again for input
                InputBuffer[0] = 0;
                LCD_write(WelcomeText,true); //default text
                LED_set(false,false); //disable leds
            }
            else
            {
                LCD_write("Too many failed attemts",true);
                LED_set(false,true);//enable red led
                SetPassword(_passwordLength); //reset password after too many failed tries
                sleep(1); 

                //reset the lock again for input
                InputBuffer[0] = 0;
                LED_set(false,false);
                LCD_write(WelcomeText,true);
                EEPROM_write(TRIES_ADDRESS, 0);
            }
            return;
        }
        LCD_write(".",false);
        sleep(1);
    }
    //if we arrive here, the code was correct
    LCD_write("Access granted",true);
    LED_set(true,false);//enable green led
    Unlock();//perform the unlock sequence

    //reset the lock again for input
    InputBuffer[0] = 0;
    LED_set(false,false); //disable leds
    EEPROM_write(TRIES_ADDRESS, 0); //reset tries
    LCD_write(WelcomeText,true);
}

// Routine to unlock the door
void Unlock()
{
    serial_out(UNLOCKSEQUENCE);
    sleep(3);
    serial_out(RESETLOCKSEQUENCE);
}
