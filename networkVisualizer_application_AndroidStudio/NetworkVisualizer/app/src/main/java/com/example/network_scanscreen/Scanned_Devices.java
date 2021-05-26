package com.example.network_scanscreen;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.os.CountDownTimer;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ProgressBar;
import android.content.Intent;
import android.widget.Toast;

import com.google.android.material.navigation.NavigationView;



public class Scanned_Devices extends AppCompatActivity implements NavigationView.OnNavigationItemSelectedListener{
    Button button_rescan;
    ProgressBar rescanProgressBar;

    private boolean rescanProgressVisibility = false;
    private boolean navMenuVis = false;

    String upload = "Open Ports: ";
    String d;
    String ip = "IP Address: ";
    String mac = "MAC Address: ";

    public void computerString(String string) { upload = upload +  string; }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.screen_scanned__devices);

        button_rescan = findViewById(R.id.rescan_button);
        rescanProgressBar = findViewById(R.id.progressBar_reScan);
        rescanProgressBar.setVisibility(View.GONE);

        NavigationView navigationView = findViewById(R.id.nav_view);
        navigationView.setVisibility(View.GONE);
        navigationView.setNavigationItemSelectedListener(this);

        computerString(Category.devices[Category.router].openports);
        d = Category.devices[Category.router].get_manufacterer_ID();
        ip = ip + Category.devices[Category.router].get_ip();
        mac = mac + Category.devices[Category.router].get_mac();
    }

    // used for the items inside of the navigation bar
    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem item) {
        switch (item.getItemId()) {
            case R.id.nav_about:
                openAboutPage();
                break;

            case R.id.nav_help:
                openHelpPage();
                break;
        }

        return true;
    }

    public void openAboutPage() {
        Intent intent = new Intent(this, About_Page.class);
        startActivity(intent);
    }

    public void openHelpPage() {
        Intent intent = new Intent(this, Help_Page.class);
        startActivity(intent);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu){
        MenuInflater inflater = getMenuInflater();
        inflater.inflate(R.menu.toolbar_menu, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(@NonNull MenuItem item) {

        NavigationView navigationView = findViewById(R.id.nav_view);

        switch (item.getItemId()){
            case R.id.menu:
                if (navMenuVis == false) {
                    navigationView.setVisibility(View.VISIBLE);
                    navMenuVis = true;
                } else {
                    navigationView.setVisibility(View.GONE);
                    navMenuVis = false;
                }
        }
        return false;
    }

    public void ButtonRescan_OnClick(View view) {
        //can remove this code and replace it with the actual "scan" code for the button action
        // Intent newIntent = new Intent(Scanned_Devices.this, MainActivity.class);
        //startActivity(newIntent);

        if (rescanProgressVisibility == false) {
            CountDownTimer progressCountDown = new CountDownTimer(5000, 1000) {
                public void onTick(long timeUntilFinished) {
                    //enable progress bar animation
                    rescanProgressBar.setVisibility(View.VISIBLE);
                    rescanProgressVisibility = true;

                    //disable scan button while "scanning"
                    button_rescan.setEnabled(false);
                }

                @Override
                public void onFinish() {
                    ProgressBar scanProgressBar = findViewById(R.id.progressBar_reScan);
                    Button scanButton = findViewById(R.id.rescan_button);

                    //disable progress bar animation
                    scanProgressBar.setVisibility(View.GONE);
                    rescanProgressVisibility = false;

                    //enable scan button afterwards
                    scanButton.setEnabled(true);
                }
            }.start();
        }
    }

    String r = "";

    String download = "";
    String usage = "";
    public void Button_ConnectedTo(View view){
        String t = "Scanned Devices:";
        Intent newIntent = new Intent(Scanned_Devices.this, Popup_device_info.class);

        newIntent = newIntent.putExtra("router", r);
        newIntent = newIntent.putExtra("type", t);
        newIntent = newIntent.putExtra("device", d);
        newIntent = newIntent.putExtra("ip", ip);
        newIntent = newIntent.putExtra("mac", mac);
        newIntent = newIntent.putExtra("download", download);
        newIntent = newIntent.putExtra("upload", upload);
        newIntent = newIntent.putExtra("usage", usage);
        startActivity(newIntent);
    }


    /** index: 0 = Phones/Tablets  1 = PCs/Servers  2 = SecCam  3 = Misc  4 = Router **/
    public void Button_PhonesTablets(View view){
        String t = "Phones & Tablets:";
        Intent newIntent = new Intent(Scanned_Devices.this, PhonesTablets_device_list.class);

        for (int j = 0; j < Category.all_categories[0].sizeSub(); j++) {
            int i = Category.all_categories[0].indexFront; }

        newIntent = newIntent.putExtra("router", r);
        newIntent = newIntent.putExtra("type", t);
        newIntent = newIntent.putExtra("device", d);
        newIntent = newIntent.putExtra("ip", ip);
        newIntent = newIntent.putExtra("mac", mac);
        newIntent = newIntent.putExtra("download", download);
        newIntent = newIntent.putExtra("upload", upload);
        newIntent = newIntent.putExtra("usage", usage);
        startActivity(newIntent);
    }

    public void Button_Favorites(View view){
        String t = "Favorited Devices:";
        Intent newIntent = new Intent(Scanned_Devices.this, Popup_device_list.class);

        newIntent = newIntent.putExtra("router", r);
        newIntent = newIntent.putExtra("type", t);
        newIntent = newIntent.putExtra("device", d);
        newIntent = newIntent.putExtra("ip", ip);
        newIntent = newIntent.putExtra("mac", mac);
        newIntent = newIntent.putExtra("download", download);
        newIntent = newIntent.putExtra("upload", upload);
        newIntent = newIntent.putExtra("usage", usage);
        startActivity(newIntent);
    }

    public void Button_PCsServers(View view){
        String t = "Computers & Servers:";
        Intent newIntent = new Intent(Scanned_Devices.this, PCServers_device_list.class);

        newIntent = newIntent.putExtra("router", r);
        newIntent = newIntent.putExtra("type", t);
        newIntent = newIntent.putExtra("device", d);
        newIntent = newIntent.putExtra("ip", ip);
        newIntent = newIntent.putExtra("mac", mac);
        newIntent = newIntent.putExtra("download", download);
        newIntent = newIntent.putExtra("upload", upload);
        newIntent = newIntent.putExtra("usage", usage);
        startActivity(newIntent);
    }

    public void Button_CamsSensors(View view){
        String t = "Cameras & Sensors:";
        Intent newIntent = new Intent(Scanned_Devices.this, CamerasSensors_device_list.class);

        newIntent = newIntent.putExtra("router", r);
        newIntent = newIntent.putExtra("type", t);
        newIntent = newIntent.putExtra("device", d);
        newIntent = newIntent.putExtra("ip", ip);
        newIntent = newIntent.putExtra("mac", mac);
        newIntent = newIntent.putExtra("download", download);
        newIntent = newIntent.putExtra("upload", upload);
        newIntent = newIntent.putExtra("usage", usage);
        startActivity(newIntent);
    }

    public void Button_Miscellaneous(View view){
        String t = "Miscellaneous Devices:";
        Intent newIntent = new Intent(Scanned_Devices.this, Misc_device_list.class);

        newIntent = newIntent.putExtra("router", r);
        newIntent = newIntent.putExtra("type", t);
        newIntent = newIntent.putExtra("device", d);
        newIntent = newIntent.putExtra("ip", ip);
        newIntent = newIntent.putExtra("mac", mac);
        newIntent = newIntent.putExtra("download", download);
        newIntent = newIntent.putExtra("upload", upload);
        newIntent = newIntent.putExtra("usage", usage);
        startActivity(newIntent);
    }

    public void Button_Routers(View view){
        String t = "Routers:";
        Intent newIntent = new Intent(Scanned_Devices.this, Routers_device_list.class);

        newIntent = newIntent.putExtra("router", r);
        newIntent = newIntent.putExtra("type", t);
        newIntent = newIntent.putExtra("device", d);
        newIntent = newIntent.putExtra("ip", ip);
        newIntent = newIntent.putExtra("mac", mac);
        newIntent = newIntent.putExtra("download", download);
        newIntent = newIntent.putExtra("upload", upload);
        newIntent = newIntent.putExtra("usage", usage);
        startActivity(newIntent);
    }
}