#include <stdio.h>  // sprintf() - Print formatted string to buffer
#include <random.h> // rand()    - Random number from 0 to 32767
#include <time.h>   // sleep()   - Sleep for x milliseconds
#include <string.h> // strlen()  - Measure string length up until, but not including a 0-character


/*
 * HARDWARE INTERFACE ROUTINES
 */

// EEPROM addresses
#define PASSWORD_LENGTH_ADDRESS = 0
#define PASSWORD_ADDRESS        = 1
#define TRIES_ADDRESS           = 64

// Iinitialisation routines for the peripheral
extern void init_LCD();                            // set pins for LCD
extern void init_EEPROM();                         // set pins for serial EEPROM
extern void init_Keypad();                         // set pins for keypad
extern void init_LEDS();                           // set pins to drive leds
extern void init_IO();                             // set pins to drive lock/unlock IO
. 
// Read from the keypad, 1 character at a time, buffered. the keys are mapped to characters as:
// +-----+
// |1 2 3|
// |4 5 6|
// |7 8 9|
// |x 0 v|
// +-----+
extern char ReadKeyPad();

// Read and write to external EEPROM
extern void EEPROM_write(char address, char data); // write char to address
extern char EEPROM_read(char address);             // read from address

// Set the red and green leds
extern void LED_set(bool green_led, bool red_led);

// LCD routines
extern void LCD_write(char * text, bool clearLCD); // write the LCD, and optionally clear before writing
extern void LCD_backspace();                       // remove the last character from the LCD

// Unlock the lock via IO
extern void Unlock();


/*
 * MAIN CODE
 */

// Generate a random password of defined length
void SetPassword(char length);
// Process keypresses
void KeyInput(char input);
// Check the password entered with the one in the EEPROM, and unlock if correct
void CheckPassword();

// Global variables
char welcomeText[] = "Enter code:";

// Preset values and buffer
char passwordLength = 4;
char _input[passwordLength+1] = "";
char MaxTries = 10;


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
    LCD_write(welcomeText,true);
    LED_set(false,false);
    EEPROM_write(PASSWORD_LENGTH_ADDRESS, passwordLength);
    EEPROM_write(TRIES_ADDRESS, 0);
    SetPassword(passwordLength);

    while(true) // loop forever
    {
        char key = ReadKeyPad();    // retrieve 1 keypress from keypad
        KeyInput(key);              // process the keypress
    }
}


void SetPassword(char length)
{
    for(int i = 0; i < length; i++) // perform this operation for the amount of characters in the password
    {
        char digit = rand() % 10; //get a random value from 0 to 9
        EEPROM_write(PASSWORD_ADDRESS + i, digit + '0'); // store the character value instead of the number
    } 
}

void KeyInput(char input)
{
    if(input == 'x') //backspace pressed
    {
        if(strlen(_input) > 0)
        {
            LCD_backspace();//remove last character
            _input[strlen(_input) -1]] = 0;//zero terminate the string
        }
    }
    else //if input is v or a number
    {
        int _passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
        if(input != 'v' && strlen(_input) < _passwordLength) // add number if its below password length, else it is ignored
        {
            LCD_write("*",false);//display a '*' and do not clear the LCD

            int len = strlen(_input);
            _input[len-1] = input;//add the key to the input buffer
            _input[len] = 0; //zero terminate the string
        }
        if(input == 'v' ) //pressed ok, so check password
        {
            for(int i = strlen(_input); i < _passwordLength); i++) //pad the password if it is not matching length
            {
                _input[i] = 'A';//pad it with character A, to prevent underflow
            }
            _input[passwordLength] = 0;//zero terminate string

            CheckPassword();//call checking routine
        }
    }
}

void CheckPassword()
{
    LCD_write("Validating", true);
    sleep(1000);

    char _passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
    if(_passwordLength == 0)// a 0 lenght password is not allowed
    {
        LCD_write("No password set!",true);
        LED_set(false,true); //enable red led
        sleep(1000);

        _input[0] = 0;
        LCD_write(welcomeText,true); //default text
        LED_set(false,false); //disable leds 
        return          
    }

    // Check the password, one character a a time
    for(int i = 0; i < _passwordLength; i++)
    {
        if(_input[i] != EEPROM_read(PASSWORD_ADDRESS + i)) //if character of input buffer does not match password, stop with checking and show error
        {
            LCD_write("Incorrect password",true);
            LED_set(false,true); //enable red led
            sleep(1000);

            char tries = EEPROM_read(TRIES_ADDRESS);//retrieve tries from EEPROM
            if(tries < MaxTries)//check if we are still allowed to try. if so increment try, if not, lock 
            {
                EEPROM_write(TRIES_ADDRESS, tries + 1);//increment tries in EEPROM

                char text[32];
                sprintf(text,"%i tries left",MaxTries - tries);
                LCD_write(text,true);//display the amount of tries left
                sleep(1000);

                //reset the lock again for input
                _input[0] = 0;
                LCD_write(welcomeText,true); //default text
                LED_set(false,false); //disable leds
            }
            else
            {
                LCD_write("Too many failed attemts",true);
                LED_set(false,true);//enable red led
                SetPassword(passwordLength); //reset password after too many failed tries
                sleep(1000); 

                _input[0] = 0;
                LED_set(false,false);
                LCD_write(welcomeText,true);
                EEPROM_write(TRIES_ADDRESS, 0);
            }
            return;
        }
        LCD_write(".",false);
        sleep(1000);
    }

    LCD_write("Access granted",true);
    LED_set(true,false);//enable green led
    Unlock();
    sleep(1000);

    _input[0] = 0;
    LED_set(false,false); //disable leds
    EEPROM_write(TRIES_ADDRESS, 0); //reset tries
    LCD_write(welcomeText,true);
}
