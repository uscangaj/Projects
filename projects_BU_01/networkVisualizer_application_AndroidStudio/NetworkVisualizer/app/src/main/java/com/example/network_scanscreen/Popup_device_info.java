package com.example.network_scanscreen;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.DisplayMetrics;
import android.view.View;
import android.widget.TextView;
import android.content.Intent;


public class Popup_device_info extends AppCompatActivity {
    //variables
    double ratioW = 0.9;    //popup window width ratio
    double ratioH = 0.8;    //popup window height ratio

    Intent newIntent;
    Bundle bundle;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.popup_device_info);
        newIntent = getIntent();
        bundle = newIntent.getExtras();

        //get window size and reduce for popup effect
        DisplayMetrics dm = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getRealMetrics(dm);
        int screenW = dm.widthPixels;
        int screenH = dm.heightPixels;
        getWindow().setLayout((int)(screenW*ratioW), (int)(screenH*ratioH));

        
        //Title
        TextView text_device_Name = findViewById(R.id.text_DeviceName);
        String sName = newIntent.getStringExtra("device");
        text_device_Name.setText(sName);

        //IP
        TextView text_device_IP = findViewById(R.id.text_device_info_ip);
        String sIP = newIntent.getStringExtra("ip");
        text_device_IP.setText(sIP);

        //MAC
        TextView text_device_MAC = findViewById(R.id.text_device_info_mac);
        String sMAC = newIntent.getStringExtra("mac");
        text_device_MAC.setText(sMAC);

        //upload
        TextView text_device_upload = findViewById(R.id.text_device_info_upload);
        String sUpload = newIntent.getStringExtra("upload");
        text_device_upload.setText(sUpload);

        //download
        TextView text_device_download = findViewById(R.id.text_device_info_download);
        String sDownload = newIntent.getStringExtra("download");
        text_device_download.setText(sDownload);

        //usage
        TextView text_device_usage = findViewById(R.id.text_device_info_usage);
        String sUsage = newIntent.getStringExtra("usage");
        text_device_usage.setText(sUsage);
    }

    public void Button_Tooltip_IP(View view){
        Intent intentIP = new Intent(Popup_device_info.this, Popup_tooltip_ip.class);
        startActivity(intentIP);
    }

    public void Button_Tooltip_MAC(View view){
        Intent intentMAC = new Intent(Popup_device_info.this, Popup_tooltip_mac.class);
        startActivity(intentMAC);
    }
}