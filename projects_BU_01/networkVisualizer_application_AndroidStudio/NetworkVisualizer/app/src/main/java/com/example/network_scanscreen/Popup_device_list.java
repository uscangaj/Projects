package com.example.network_scanscreen;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.DisplayMetrics;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.content.Intent;
import java.util.ArrayList;

public class Popup_device_list extends AppCompatActivity {
    //variables
    double ratioW = 0.9;    //popup window width ratio
    double ratioH = 0.8;    //popup window height ratio

    Intent newIntent;
    Bundle bundle;

    private RecyclerView mRecyclerView;
    private Recycle_Adapter mAdapter;
    private RecyclerView.LayoutManager mLayoutManager;
    private ArrayList<Card_java> card_javas;
    private EditText editText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.popup_device_list);
        newIntent = getIntent();
        bundle = newIntent.getExtras();

        //get window size and reduce for popup effect
        DisplayMetrics dm = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getRealMetrics(dm);
        int screenW = dm.widthPixels;
        int screenH = dm.heightPixels;
        getWindow().setLayout((int)(screenW*ratioW), (int)(screenH*ratioH));

        TextView title = findViewById(R.id.text_DeviceType);
        String n = newIntent.getStringExtra("type");
        title.setText(n);

        createList();
        buildRecycler();

        editText = findViewById(R.id.list_search);
        editText.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                filter(s.toString());
            }
        });
    }

    private void filter(String text) {
        ArrayList<Card_java> filteredList = new ArrayList<>();

        for (Card_java item : card_javas) {
            if (item.getText1().toLowerCase().contains(text.toLowerCase())) {
                filteredList.add(item);
            }
        }
        mAdapter.filterList(filteredList);
}

    private void createList() {
        card_javas = new ArrayList<>();

        int listsize = 13;
        String ntext;
        String iptext = "     IP:   192.168.2.34";
        String mactext = "MAC:   b4:0a:6f:94:c3:p0";
        int wifi_image;


        for (int i=1 ; i<=listsize ; i++) {
            if (i<10)
                ntext = "Device 0" + i;
            else
                ntext = "Device " + i;

            if(i%3 == 1)
                wifi_image = R.drawable.wifi_green;
            else if (i%3 == 2)
                wifi_image = R.drawable.wifi_yellow;
            else
                wifi_image = R.drawable.wifi_red;

           // card_javas.add(new Card_java(wifi_image, ntext, mactext,iptext));
        }

        /*
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name1", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name2", "MAC:   b4:0a:6f:94:c3:p0","     IP:   292.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name3", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name4", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name5", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name6", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name7", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name8", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name9", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name11", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name22", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name33", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name44", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name55", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name66", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name77", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        card_javas.add(new Card_java(R.drawable.wifi_20, "Network_Name88", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        */
    }

    private void buildRecycler() {
        mRecyclerView = findViewById(R.id.recycler_view);
        mRecyclerView.setHasFixedSize(true);
        mLayoutManager = new LinearLayoutManager(this);
        mAdapter = new Recycle_Adapter(card_javas);

        mRecyclerView.setLayoutManager(mLayoutManager);
        mRecyclerView.setAdapter(mAdapter);
    }

//    public void Button_DeviceInfo(View view){
//        String r = newIntent.getStringExtra("router");
//        String t = newIntent.getStringExtra("type");
//        String d = newIntent.getStringExtra("device");
//        String ip = newIntent.getStringExtra("ip");
//        String mac = newIntent.getStringExtra("mac");
//        String upload = newIntent.getStringExtra("upload");
//        String download = newIntent.getStringExtra("download");
//        String usage = newIntent.getStringExtra("usage");
//
//        Intent newIntent = new Intent(Popup_device_list.this, Popup_device_info.class);
//
//        newIntent = newIntent.putExtra("router", r);
//        newIntent = newIntent.putExtra("type", t);
//        newIntent = newIntent.putExtra("device", d);
//        newIntent = newIntent.putExtra("ip", ip);
//        newIntent = newIntent.putExtra("mac", mac);
//        newIntent = newIntent.putExtra("download", download);
//        newIntent = newIntent.putExtra("upload", upload);
//        newIntent = newIntent.putExtra("usage", usage);
//        startActivity(newIntent);
//    }
}