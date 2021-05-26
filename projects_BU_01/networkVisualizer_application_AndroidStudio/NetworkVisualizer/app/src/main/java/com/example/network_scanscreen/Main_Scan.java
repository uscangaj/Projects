package com.example.network_scanscreen;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.os.CountDownTimer;
import android.view.View;
import android.widget.Button;
import android.widget.ProgressBar;
import android.content.Intent;


public class Main_Scan extends AppCompatActivity {

    /* Initialization */
    //Button scanButton;
    //ProgressBar scanProgressBar;
    Intent newIntent;
    ProgressBar scanProgressBar;
    Button scanButton;

    /* Variables */
    private boolean progressVisibility = false;
    


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.screen_main_scan);

        scanButton = findViewById(R.id.button_scan);
        scanProgressBar = findViewById(R.id.progressBar_Scan);

        scanProgressBar.setVisibility(View.GONE);

        Category.createSubCategories();
        Tools.scan();
    }


    public void ButtonScan_OnClick(View view) {
        if (progressVisibility == false) {
            CountDownTimer progressCountDown = new CountDownTimer(5000, 1000) {
                public void onTick(long timeUntilFinished) {
                    //enable progress bar animation
                    scanProgressBar.setVisibility(View.VISIBLE);
                    progressVisibility = true;

                    //disable scan button while "scanning"
                    scanButton.setEnabled(false);

                    //Screen transition
                    newIntent = new Intent(Main_Scan.this, Scanned_Devices.class);

                      /*  int size = 1;
                        Thread[] thread = new Thread[size];
                        thread[0] = new Thread(new Runnable() {
                            @Override
                            public void run() {
                                Tools.scan("192.168.0.0");
                            }
                        });
                       /* thread[1] = new Thread(new Runnable() {
                            @Override
                            public void run() {
                                Tools.scan("192.168.0.128");
                            }
                        });
                        short i = 0;

                        //test push

                        while (i < size) { thread[i].start(); i++; }*/
                }


                public void onFinish() {
                    ProgressBar scanProgressBar = findViewById(R.id.progressBar_Scan);
                    Button scanButton = findViewById(R.id.button_scan);
                    Intent newIntent = new Intent(Main_Scan.this, Scanned_Devices.class);

                    //disable progress bar animation
                    scanProgressBar.setVisibility(View.GONE);
                    progressVisibility = false;

                    //enable scan button afterwards
                    scanButton.setEnabled(true);

                    startActivity(newIntent);
                }
            }.start();
        }
    }
}