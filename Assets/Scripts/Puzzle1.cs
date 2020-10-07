using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Puzzle1 : MonoBehaviour
{
    //TODO: buzzer?
    public Text LCD;
    private string _input = "";
    private string welcomeText = "Enter code:";
    private bool PuzzleSolved = false;
    private bool KeypadEnabled = true;
    private int[] buffer;
    private int PASSWORD_ADDRESS = 1;
    private int PASSWORD_LENGTH_ADDRESS = 0;
    private int TRIES_ADDRESS = 64;
    public int passwordLength = 4;
    public int MaxTries = 10;
    public Image keypad;

    public Sprite both;
    public Sprite granted;
    public Sprite denied;
    public Sprite normal;
    public UnityEvent UnlockEvent;
    public ModifyChip modifyChip;
    // Start is called before the first frame update
    void Start()
    {
        LCD_write(welcomeText,true);
        LED_set(false,false);
        buffer = new int[256];
        EEPROM_write(PASSWORD_LENGTH_ADDRESS, 4);
        EEPROM_write(TRIES_ADDRESS, 0);
        SetPassword();
    }

    private void Awake()
    {
        _input = "";
        KeypadEnabled = true; //enable keypad
        LED_set(false,false);
        LCD_write(welcomeText,true);
        buffer = new int[256];
        EEPROM_write(PASSWORD_LENGTH_ADDRESS, 4);
        EEPROM_write(TRIES_ADDRESS, 0);
        Time.timeScale = 0;
        SetPassword();
    }
    
    private void Update()
    {
        Time.timeScale = 0;
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // has backspace/delete been pressed?
            {
                ButtonInput("x");
            }
            else if ((c == '\n') || (c == '\r')) // enter/return
            {
                ButtonInput("v");
            }
            else if(c == 'q')
            {
                ButtonInput("q");
            }
            else
            {
                if(c >= '0' && c <= '9')
                {
                    ButtonInput(c.ToString());
                }
            }
        }
    }

    public void SetPassword()
    {
        string password = "";
        for(int i = 0; i < passwordLength; i++)
        {
            int digit = Random.Range(1,10);
            EEPROM_write(PASSWORD_ADDRESS + i, digit);
            password += digit.ToString();
        } 
        Debug.Log(password);
    }

    public void ButtonInput(string input)
    {
        //
        //scancode override logic
        //
        if(input == "q")
        {
            _input = "";
            KeypadEnabled = true; //enable keypad
            LED_set(false,false);
            LCD_write(welcomeText,true);
            gameObject.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        if(PuzzleSolved == false && KeypadEnabled == true)//only respond to keypad if puzzle is not solved, and keypad is enabled
        {
            //Debug.Log(input);
            if(input == "x") //backspace
            {
                //Debug.Log("rem: " + welcomeText.Length);
                if(LCD_length() > welcomeText.Length)
                {
                    LCD_backspace();
                    _input = _input.Remove(_input.Length-1);
                }
            }
            else //if input is ok or a number
            {
                int passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
                if(input != "v" && LCD_length() < welcomeText.Length + passwordLength) // add number if its below password length
                {
                    LCD_write("*",false);
                    _input += input;
                }
                if(input == "v" ) //pressed ok, so check password
                {
                    if(LCD_length() < welcomeText.Length + passwordLength) //pad the password if it is not matching length
                    {
                        _input = _input.PadRight(passwordLength,'A');//pad it with character A, as it will never match a code
                    }
                    StartCoroutine("CheckPassword");//call checking coroutine
                }
            }
        }
    }

    private IEnumerator CheckPassword()
    {
        KeypadEnabled = false;//disable the keypad input
        Debug.Log("checking:" + _input);
        LCD_write("Validating", true);
        yield return new WaitForSecondsRealtime(1f);

        int passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
        if(passwordLength == 0)
        {
            LCD_write("No password set!",true);
            LED_set(false,true); //enable red led
            yield return new WaitForSecondsRealtime(1f);

            //reset the lock again for input
            _input = "";
            LCD_write(welcomeText,true); //default text
            LED_set(false,false); //disable leds
            KeypadEnabled = true; //enable keypad  
            yield break;          
        }
        for(int i = 0; i < passwordLength; i++)//check each character
        {
            if(_input[i] != EEPROM_read(PASSWORD_ADDRESS + i).ToString()[0]) //if character does not match password, stop with checking and show error
            {
                //yield return new WaitForSecondsRealtime(1f);

                LCD_write("Incorrect password",true);
                LED_set(false,true); //enable red led
                yield return new WaitForSecondsRealtime(1f);

                int tries = EEPROM_read(TRIES_ADDRESS);//retrieve tries from EEPROM
                if(tries < MaxTries)//check if we are still allowed to try. if so increment try, if not, lock puzzle
                {

                    EEPROM_write(TRIES_ADDRESS, tries + 1);//increment tries in EEPROM
                    LCD_write((MaxTries - tries) + " tries left",true);//display the amount of tries left
                    yield return new WaitForSecondsRealtime(1f);

                    //reset the lock again for input
                    _input = "";
                    LCD_write(welcomeText,true); //default text
                    LED_set(false,false); //disable leds
                    KeypadEnabled = true; //enable keypad
                }
                else
                {
                    //lock out the puzzle
                    LCD_write("Too many failed attemts",true);
                    LED_set(false,true);//enable red led
                    
                    //KeypadEnabled = false; //keep keypad disabled
                    SetPassword(); //reset password
                    yield return new WaitForSecondsRealtime(1f); 

                    _input = "";
                    KeypadEnabled = true; //enable keypad
                    LED_set(false,false);
                    LCD_write(welcomeText,true);
                    EEPROM_write(TRIES_ADDRESS, 0);
                    //kick out of lock-puzzle
                    gameObject.SetActive(false);
                    Time.timeScale = 1;
                }
                yield break;
            }
            LCD_write(".",false);
            yield return new WaitForSecondsRealtime(1f); 
        }

        Debug.Log("code OK");
        LCD_write("Access granted",true);
        LED_set(true,false);//enable green led
        PuzzleSolved = true;
        UnlockEvent.Invoke();
        yield return new WaitForSecondsRealtime(1f);

        LED_set(false,false); //disable leds
        EEPROM_write(TRIES_ADDRESS, 0); //reset tries
        KeypadEnabled = false; //keep keypad disabled
        yield return new WaitForSecondsRealtime(1f);
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private int EEPROM_read(int address)
    {
        //TODO: add address pins
        int SDA = modifyChip.getState(45);
        int SCL = modifyChip.getState(46);
        if(SCL != 0 || SDA == 2)
            return 0;
        if(SDA == 1)
            return 0xff;

        return buffer[address % 256];
    }
    private void EEPROM_write(int address, int value)
    {
        //TODO: add address pins
        int SDA = modifyChip.getState(45);
        int SCL = modifyChip.getState(46);
        if(SCL != 0 || SDA != 0)
            return;

        int EEPROM_WP_enable = modifyChip.getState(47);
        if(EEPROM_WP_enable != 1) //if EEPROM was not forced high
        {
            buffer[address % 256] = value;
        }
    }
    //https://www.electronicsforu.com/resources/learn-electronics/16x2-lcd-pinout-diagram
    private void LCD_write(string value, bool clear)
    {
        
        int RS = modifyChip.getState(15);// pin 15 = RS
        
        int RW = modifyChip.getState(16);// pin 16 = RW,
        if(RS == 2 || RW == 1) // if command register forced(low), or RW forced read(high); just return
            return;
        
        //1-8 = D0-D7
        string newVal = "";
        foreach(char letter in value)
        {
            int modifiedLetter = letter;
            int[] a = new int[8];
            for(int i = 0; i < 9; i++)
            {
                int bit = modifyChip.getState(i+1);
                if(bit == 1)
                {
                    modifiedLetter |= (1 << i);
                }
                else if(bit == 2)
                {
                    modifiedLetter &= ~(1 << i);
                }
            }
            newVal += (char)modifiedLetter;
        }
        
        if(clear && RS == 0) // we cannot clear LCD if command register is forced
            LCD.text = newVal;
        else
            LCD.text += newVal;
    }
    private void LCD_backspace()
    {
        int RS = modifyChip.getState(15);// pin 15 = RS
        int RW = modifyChip.getState(16);// pin 16 = RW,
        if(RS == 1 || RW == 1) // if data register forced(high), or RW forced read(high); just return
            return;
        LCD.text = LCD.text.Remove(LCD.text.Length-1);
    }
    private int LCD_length()
    {
        return LCD.text.Length;
    }
    private void LED_set(bool green, bool red)
    {
        int state = modifyChip.getState(38); // green led
        if(state > 0){
            if(state == 1) green = true;
            else green = false;
        }
        state = modifyChip.getState(39); // red led
        if(state > 0){
            if(state == 1) red = true;
            else red = false;
        }

        if(green == true && red == true)
        {
            keypad.sprite = both; 
        }
        else if(green == true)
        {
            keypad.sprite = granted; 
        }
        else if(red == true)
        {
            keypad.sprite = denied; 
        }
        else
        {
            keypad.sprite = normal;
        }
    }

    private void LED_get(out bool green, out bool red)
    {
        if(keypad.sprite == both)
        {
            green = true;
            red = true;
        }
        else if(keypad.sprite == granted)
        {
            green = true;
            red = false;
        }
        else if(keypad.sprite == denied)
        {
            green = false;
            red = true;
        }
        else
        {
            green = false;
            red = false;
        }

    }
    public bool IsPuzzleSolved()
    {
        return PuzzleSolved;
    }

    public void chipModifier(int pin)
    {
        switch(pin)
        {
            case 38:
            case 39:
                bool red;
                bool green;
                LED_get(out green, out red);
                LED_set(green,red);
            break;
            default:
            break;
        }
    }
}
