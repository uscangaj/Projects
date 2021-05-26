package com.example.network_scanscreen;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.TextView;
import android.content.Intent;
import java.util.ArrayList;

public class Routers_device_list extends AppCompatActivity {
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

    // private void createList(Category)
    /*
        card_javas.add(new Card_java(R.drawable.wifi_20, , "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));


     */
    private void createList() {
        card_javas = new ArrayList<>();
        Category point = Category.all_categories[4];

        for (int i = 0; i < point.sizeSub(); i++) {
            Computer device = Category.devices[point.indexFront + i];
            Log.d("Added", "this device " + device.get_manufacterer_ID());
            int connection = device.getConnectionQuality();

            int wifi;
            switch (connection) {
                case 1:     wifi = R.drawable.wifi_green; break;
                case 2:     wifi = R.drawable.wifi_yellow; break;
                //case 3:     wifi = R.drawable.wifi_red; break;
                default:    wifi = R.drawable.wifi_red; break;
            }

            String hostname = device.get_hostname();
            if (hostname != "Error") { hostname = device.get_manufacterer_ID(); }
            if (device.get_nickname() != "") { hostname = device.get_nickname(); }

            card_javas.add(new Card_java(wifi, hostname, "Mac: " + device.get_mac(), "IPv4: " + device.get_ip(), "Ports: " + device.openports));
        }
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
//        Intent newIntent = new Intent(Routers_device_list.this, Popup_device_info.class);
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