#include <stdio.h>  // printf() - Print formatted string to stdio
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/select.h>
#include <termios.h>
#include <time.h>

typedef unsigned char bool;
#define true 1
#define false 0
// Initialisation routines for the peripherals
void init_LCD();                            // set pins for LCD
void init_EEPROM();                         // set pins for serial EEPROM
void init_Keypad();                         // set pins for keypad
void init_LEDS();                           // set pins to drive leds
void init_IO();                             // set pins to drive lock/unlock IO

// Read from the keypad, 1 character at a time, buffered. the keys are mapped to characters as:
// +-----+
// |1 2 3|
// |4 5 6|
// |7 8 9|
// |x 0 v|
// +-----+
int fd;
unsigned char ReadKeyPad();

// Read and write to external EEPROM
unsigned char EEPROM_buffer[256];
void EEPROM_write(unsigned char address, unsigned char data); // write char to address
unsigned char EEPROM_read(unsigned char address);             // read from address

// Set the red and green leds
void LED_set(bool green_led, bool red_led);

// LCD routines
unsigned char LCD_buffer[20];
void LCD_write(unsigned char * text, bool clearLCD); // write the LCD, and optionally clear before writing
void LCD_backspace();                       // remove the last character from the LCD

//send serial data over IO pins
void serial_out(unsigned char * data);

// Iinitialisation routines for the peripheral
void init_LCD()                             // set pins for LCD
{
	printf("LCD initialised\r\n");
}

void init_EEPROM()                          // set pins for serial EEPROM
{
	time_t t;
	srand((unsigned) time(&t));
	printf("EEPROM initialised\r\n");
}

struct termios orig_termios;
void reset_terminal_mode() {
    tcsetattr(0, TCSANOW, &orig_termios);
}
void init_Keypad()                          // set pins for keypad
{
	struct termios new_termios;
    /* take two copies - one for now, one for later */
    tcgetattr(0, &orig_termios);
    memcpy(&new_termios, &orig_termios, sizeof(new_termios));

    /* register cleanup handler, and set the new terminal mode */
    atexit(reset_terminal_mode);
    cfmakeraw(&new_termios);
    tcsetattr(0, TCSANOW, &new_termios);
	setvbuf(stdout, (char *)NULL, _IONBF, 0); 
	printf("Keypad initialised\r\n");
}
int kbhit() {
    struct timeval tv = { 0L, 0L }; fd_set fds;
    FD_ZERO(&fds); FD_SET(0, &fds);
    return select(1, &fds, NULL, NULL, &tv);
}
int getch() {
    int r; unsigned char c;
    if ((r = read(0, &c, sizeof(c))) < 0) {
        return r;
    } else {
        return c;
    }
}

void init_LEDS()                            // set pins to drive leds
{
	printf("LEDS initialised\r\n");
}

void init_IO()                              // set pins to drive lock/unlock IO
{
	printf("IO initialised\r\n");
}

// Read from the keypad, 1 character at a time, buffered. the keys are mapped to characters as:
// +-----+
// |1 2 3|
// |4 5 6|
// |7 8 9|
// |x 0 v|
// +-----+
unsigned char ReadKeyPad()
{
	if (kbhit()) 
	{
		int ch = getch();
		if(ch == 'q')
		{
			exit(0);
		}
		if(ch >= '0' && ch <= '9')
		{
			return ch;
		}
		if(ch == 'x' || ch == 'v')
		{
			return ch;
		}
    }
	return 0;
}

// Read and write to external EEPROM
void EEPROM_write(unsigned char address, unsigned char data)  // write char to address
{
	printf("write EEPROM: address: %i, value: %i (%c)\r\n", address, data, data);
	EEPROM_buffer[address] = data;
}

unsigned char EEPROM_read(unsigned char address)              // read from address
{
	printf("read EEPROM: address: %i, value: %i\r\n", address, EEPROM_buffer[address]);
	return EEPROM_buffer[address];
}

// Set the red and green leds
void LED_set(bool green_led, bool red_led)
{
	printf("LED set green:%i, red:%i\r\n", green_led, red_led);
}

// LCD routines
void LCD_write(unsigned char * text, bool clearLCD)  // write the LCD, and optionally clear before writing
{
	if(clearLCD)
	{
		strcpy(LCD_buffer,text);
	}
	else
	{
		int offset = strlen(LCD_buffer);
		strcpy(LCD_buffer+offset,text);
	}
	printf("LCD:%s\r\n",LCD_buffer);
}

void LCD_backspace()                        // remove the last character from the LCD
{
	int offset = strlen(LCD_buffer);
	LCD_buffer[offset-1] = 0;
	printf("LCD:%s\r\n",LCD_buffer);
}

//send serial data over IO pins
void serial_out(unsigned char * data)
{
	printf("serial out: %s\r\n", data);
}

unsigned char serial_in()
{
	return 'C';
}

