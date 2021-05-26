package com.example.network_scanscreen;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

public class Help_Page extends AppCompatActivity {
    private Button button;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.screen_help_page);
    }

    public void openScannedDevices() {
        Intent intent = new Intent(this, Scanned_Devices.class);
        startActivity(intent);
    }
}