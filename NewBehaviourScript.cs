using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Timers;
using System;

public class NewBehaviourScript : MonoBehaviour
{

    private float moveSpeed;
    //private long timer;
    private double previousTime;
    private int rotationCounter;
    private int sampleSize = 50;
    int[] samplingArray = new int[50];
    private int RPM;
    private int originalMillis;

    public SerialPort sp = new SerialPort("/dev/cu.usbserial-1410", 9600);
    DateTime original = DateTime.Now;
    DateTime current = DateTime.Now;


    // Start is called before the first frame update
    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 500;
        moveSpeed = 0;
        previousTime = 4000;
        //timer=0;
    }

    // Update is called once per frame
    void Update()
    {
        current = DateTime.Now;
        TimeSpan timer = current - original;

        //timer = update.Millisecond - originalMillis;

        //print(timer.TotalMilliseconds);

        if (sp.IsOpen)
        {
            try
            {
                //MoveObject((char)sp.ReadByte());
                //print((char)sp.ReadByte());

                //in the first 5 seconds
                
                if (timer.TotalMilliseconds < 5000)
                {

                    for (int i = 0; i < sampleSize; i++)
                    {
                        //print((char)sp.ReadByte());
                        print(timer.TotalMilliseconds);
                        if ((char)sp.ReadByte() == 1)
                        {
                            samplingArray[i] = 1;
                            print("1");
                            //print(samplingArray[i]);
                        }
                        else if ((char)sp.ReadByte() == 0)
                        {
                            samplingArray[i] = 0;
                            print("0");
                            //print(samplingArray[i]);
                        }
                        
                        System.Threading.Thread.Sleep(100);
                    }
                    print("First 5 Seconds Passed");
                }

                //after first 5 seconds
                //shift samples to the left
                for (int i = 1; i < sampleSize; i++)
                {
                    
                    samplingArray[i - 1] = samplingArray[i];
                }
                //record 50th sample
                if ((char)sp.ReadByte() == 1)
                {
                    samplingArray[49] = 1;
                    
                    //    Serial.println("Reed sensor triggered");
                    //    Serial.println(samplingArray[50]);
                }
                else
                {
                    samplingArray[49] = 0;
                    
                    //    Serial.println("Reed sensor not triggered");
                    //    Serial.println(samplingArray[50]);
                }




                //  Serial.println(previousTime);
                //  //measure RPM every second
                if (timer.TotalMilliseconds - 1000 > previousTime)
                {
                        print("RPM: Calculating");
                    previousTime = timer.TotalMilliseconds;
                    for (int i = 0; i < sampleSize; i++)
                    {
                        
                        rotationCounter = rotationCounter + samplingArray[i];
                    }
                    print("rotation Counter: " + rotationCounter);
                    RPM = rotationCounter * 12;
                    print("RPM: " + RPM);
                    rotationCounter = 0;
                  
                    
                    //Serial.print("MPH: ");
                    //Serial.println((RPM * 60 * 2.155)/1609);
                }

                System.Threading.Thread.Sleep(100);
            }


            catch (System.Exception)
            {
                throw;
            }

        }





        void MoveObject(int speed)
        {
            //moveSpeed = speed * 1.0f;
            //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //transform.Translate(distance * Time.deltaTime, 0f, 0f);


        }
    }
}
        