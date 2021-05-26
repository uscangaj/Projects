package com.example.network_scanscreen;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.DisplayMetrics;
import android.view.View;
import android.widget.TextView;
import android.content.Intent;

public class Popup_tooltip_ip extends AppCompatActivity {
    //variables
    double ratioW = 0.9;    //popup window width ratio
    double ratioH = 0.8;    //popup window height ratio

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.popup_tooltip_ip);

        //get window size and reduce for popup effect
        DisplayMetrics dm = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getRealMetrics(dm);
        int screenW = dm.widthPixels;
        int screenH = dm.heightPixels;
        getWindow().setLayout((int) (screenW * ratioW), (int) (screenH * ratioH));
    }
}