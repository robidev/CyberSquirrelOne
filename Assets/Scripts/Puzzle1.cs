using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle1 : MonoBehaviour
{
    public Text LCD;
    private string _input = "";

    private string welcomeText = "Enter code:";
    private bool PuzzleSolved = false;
    private bool KeypadEnabled = true;
    private bool EEPROM_WP_enable = false;
    private int[] buffer;
    private int PASSWORD_ADDRESS = 1;
    private int PASSWORD_LENGTH_ADDRESS = 0;
    private int TRIES_ADDRESS = 64;
    public int passwordLength = 4;
    public int MaxTries = 10;
    public Image keypad;

    public Sprite granted;
    public Sprite denied;
    public Sprite normal;

    // Start is called before the first frame update
    void Start()
    {
        LCD.text = welcomeText;
        keypad.sprite = normal;
        buffer = new int[256];
        EEPROM_write(PASSWORD_LENGTH_ADDRESS, 4);
        EEPROM_write(TRIES_ADDRESS, 0);
        SetPassword();
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
        if(PuzzleSolved == false && KeypadEnabled == true)//only respond to keypad if puzzle is not solved, and keypad is enabled
        {
            //Debug.Log(input);
            if(input == "x") //backspace
            {
                //Debug.Log("rem: " + welcomeText.Length);
                if(LCD.text.Length > welcomeText.Length)
                {
                    LCD.text = LCD.text.Remove(LCD.text.Length-1);
                    _input = _input.Remove(_input.Length-1);
                }
            }
            else //if input is ok or a number
            {
                int passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
                if(input != "v" && LCD.text.Length < welcomeText.Length + passwordLength) // add number if its below password length
                {
                    LCD.text += "*";
                    _input += input;
                }
                if(input == "v" ) //pressed ok, so check password
                {
                    if(LCD.text.Length < welcomeText.Length + passwordLength) //pad the password if it is not matching length
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
        LCD.text = "Validating";
        yield return new WaitForSecondsRealtime(1f);

        int passwordLength = EEPROM_read(PASSWORD_LENGTH_ADDRESS);
        for(int i = 0; i < passwordLength; i++)//check each character
        {
            if(_input[i] != EEPROM_read(PASSWORD_ADDRESS + i).ToString()[0]) //if character does not match password, stop with checking and show error
            {
                //yield return new WaitForSecondsRealtime(1f);

                LCD.text = "Incorrect password";
                keypad.sprite = denied; //enable red led
                yield return new WaitForSecondsRealtime(1f);

                int tries = EEPROM_read(TRIES_ADDRESS);//retrieve tries from EEPROM
                if(tries < MaxTries)//check if we are still allowed to try. if so increment try, if not, lock puzzle
                {

                    EEPROM_write(TRIES_ADDRESS, tries + 1);//increment tries in EEPROM
                    LCD.text = (MaxTries - tries) + " tries left";//display the amount of tries left
                    yield return new WaitForSecondsRealtime(1f);

                    //reset the lock again for input
                    _input = "";
                    LCD.text = welcomeText; //default text
                    keypad.sprite = normal; //disable leds
                    KeypadEnabled = true; //enable keypad
                }
                else
                {
                    //lock out the puzzle
                    LCD.text = "Too many failed attemts";
                    keypad.sprite = denied; //enable red led
                    KeypadEnabled = false; //keep keypad disabled
                    SetPassword(); //reset password
                    //kick out of lock-puzzle
                }
                yield break;
            }
            LCD.text += ".";
            yield return new WaitForSecondsRealtime(1f); 
        }

        Debug.Log("code OK");
        LCD.text = "Access granted";
        keypad.sprite = granted; //enable green led
        yield return new WaitForSecondsRealtime(1f);

        keypad.sprite = normal; //disable leds
        EEPROM_write(TRIES_ADDRESS, 0); //reset tries
        KeypadEnabled = false; //keep keypad disabled
        PuzzleSolved = true;
    }

    private int EEPROM_read(int address)
    {
        return buffer[address % 256];
    }

    private void EEPROM_write(int address, int value)
    {
        if(EEPROM_WP_enable == false) //if EEPROM was not modified
        {
            buffer[address % 256] = value;
        }
    }
}
