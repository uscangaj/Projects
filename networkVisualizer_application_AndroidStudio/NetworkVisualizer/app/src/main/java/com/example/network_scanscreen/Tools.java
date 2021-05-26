package com.example.network_scanscreen;

import android.Manifest;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Network;
import android.net.TrafficStats;
import android.os.Build;
import android.os.StrictMode;

import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.appcompat.widget.Toolbar;

import android.util.Log;
import android.view.View;

import com.google.android.material.snackbar.Snackbar;

import org.jsoup.Connection;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;

import java.io.*;
import java.net.*;
import java.lang.StringBuilder;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.locks.ReentrantLock;

public class Tools {

    //TODO
    // static void traceRT(*computer);

    // 1 = common; port 22, 80, 443, 8008, 9000, 9001
    // Scans all potential hosts on a network.


    // SCAN IS JUST FOR PICKING UP IP addresses
    // Picks up IP, sends to category, which then creates Computer object and
    // finds rest of the information.
    //Possible entries include
    // default: 192.168.0.1/24 --> MEANS 192.168.0.0-255
    // 192.168.0.1/16 --> MEANS 192.168.0-255.0-255
    // etc
    // Scans each to check if open and then creates a Object Computer
    static void scanPorts(Computer computer) {
        int ports[] = {21, 22, 80, 311, 443, 3389, 8080};
        String temp= "";
        int i = 0;
        while (i < ports.length) {
            try {
                Log.d("TestingPort", "Testing Port: " + ports[i]);
                // open socket
                Socket socket = new Socket();
                SocketAddress address = new InetSocketAddress(computer.get_ip(), ports[i]);
                socket.connect(address, 1000);
                if (socket.isConnected()) {
                    Log.d("Success", "IP: " + computer.get_ip() + " at port " + ports[i] + " established a connection. ");
                    computer.ports[ports[i]] = true;
                    temp = ports[i] + " ";
                    computer.openports = computer.openports + temp;
                }
                // close socket
                socket.close();
            } catch (IOException e) {
                Log.d("Failure", "IP: " + computer.get_ip() + " at port " + ports[i] + " timed out! ");
            } i++;
        }
    };

    static void scan()//String ipv4)
    {
        final Scan obj = new Scan();
        //we need to a way to find the subnet mask and put it in here, this is fine for now
        obj.listip("192.168.0.1");

        // obj.wscan();

//         i fixed it where we dont have to divide the the ip into four parts and hard code it in and run it in each thread
//        now the threads will take care of grabbing the ips
        int size = 4;
        Thread[] thread = new Thread[size];
        thread[0] = new Thread(new Runnable() {
            @Override
            public void run() {

                obj.scan();
            }
        });
        thread[1] = new Thread(new Runnable() {
            @Override
            public void run() {

                obj.scan();
            }
        });
        thread[2] = new Thread(new Runnable() {
            @Override
            public void run() {

                obj.scan();
            }
        });
        thread[3] = new Thread(new Runnable() {
            @Override
            public void run() {

                obj.scan();
            }
        });
        short i = 0;

        while (i < size) { thread[i].start(); i++; }
    };

    static public String ManufacturerID(String macAddress){
        String page = "https://api.macvendors.com/"+ macAddress;
        //Connecting to the web page
        Connection conn = Jsoup.connect(page).ignoreHttpErrors(true);
        //executing the get request
        Document doc = null;
        String result;
        try {
            doc = conn.get();
        } catch (IOException e) {
            e.printStackTrace();
            result = "Unknown";
            Log.d("Please", "Error: " + macAddress);
            return result;
        }
        //Retrieving the contents (body) of the web page
        result = doc.body().text();
        Log.d("Please", result + ": " + macAddress);

        return result;
    }

    // Just used as a refresh function for computer.
    // Does rescan of all ports.
    static public void check_alive(Computer comp)
    {
        short i = 0;
        while (i < Computer.NUM_OF_PORTS) {
            if (comp.ports[i]) {
                try {
                    // open socket
                    Socket socket = new Socket();
                    SocketAddress address = new InetSocketAddress(comp.get_ip(), i);
                    socket.connect(address, 30);
                    if (socket.isConnected()) {
                        comp.alive = true;
                        comp.ports[i] = true;
                        Log.d("Success", "IP: " + comp.get_ip() + " at port " + i + " established a connection.");
                    }
                    // close socket
                    socket.close();
                } catch (Exception e) {
                    comp.ports[i] = false;
                    Log.d("Failure", "IP: " + comp.get_ip() + " at port " + i + " timed out! ");
                }
            }
            i++;
        }
    };

    static public void UploadDownload(){
        long BeforeTime = System.currentTimeMillis();
        // return number of bytes transmitted since device boot
        long TotalTxBeforeTest = TrafficStats.getTotalTxBytes();
        // return number of bytes received since device boot
        long TotalRxBeforeTest = TrafficStats.getTotalRxBytes();

        try {
            URL url = new URL("https://ddragon.leagueoflegends.com/cdn/img/champion/splash/Bard_0.jpg");
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.connect();
            InputStream inputStream = connection.getInputStream();
            Bitmap bitmap = BitmapFactory.decodeStream(inputStream);
            // return number of bytes transmitted since device boot
            long TotalTxAfterTest = TrafficStats.getTotalTxBytes();
            // return number of bytes received since device boot
            long TotalRxAfterTest = TrafficStats.getTotalRxBytes();
            long AfterTime = System.currentTimeMillis();

            double TimeDifference = AfterTime - BeforeTime;

            // total number of bytes received from the network exchange
            double rxDiff = TotalRxAfterTest - TotalRxBeforeTest;
            // total number of bytes transmitted from the network exchange
            double txDiff = TotalTxAfterTest - TotalTxBeforeTest;

            // if there was change
            if((rxDiff != 0 ) && (txDiff != 0)){
                // total rx bytes per second
                double rxBPS = (rxDiff /(TimeDifference / 1000));
                // total tx bytes per second
                double txBPS = (txDiff / (TimeDifference / 1000));
                // Log the testing inside of doInBackground
                Log.d("Upload/Download", "Download speed:  " + rxBPS + "\n" + "Upload Speed: " + txBPS);
            }
            else{
                Log.d("Error", "");
            }


        } catch (MalformedURLException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return;
    };

        // Credit: https://stackoverflow.com/questions/51685723/how-to-scan-ip-and-mac-address-of-all-device-connected-to-wifi-in-android-accura
    static public String getMacAddress(String ipFinding)
    {

        Log.i("IPScanning","Scan was started!");

        BufferedReader bufferedReader = null;

        try {
            bufferedReader = new BufferedReader(new FileReader("/proc/net/arp"));

            String line;
            while ((line = bufferedReader.readLine()) != null) {
                String[] splitted = line.split(" +");
                if (splitted != null && splitted.length >= 4) {
                    String ip = splitted[0];
                    String mac = splitted[3];
                    if (mac.matches("..:..:..:..:..:..")) {

                        if (ip.equalsIgnoreCase(ipFinding))
                        {
                            Log.i("Mac Address", "Mac: " + mac);
                            return mac;
                        }
                    }
                }
            }

        } catch (FileNotFoundException e) {
            Log.wtf("Mac Address", "arp file not found");

            e.printStackTrace();
        } catch (IOException e) {
            Log.wtf("Mac Address", "Exception");
            Log.wtf("Mac Address", "ARP not found.");

            e.printStackTrace();
        } finally{
            try {
                bufferedReader.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        return "00:00:00:00:00:00";
    };
	
	
    // Only works on a network with a router that supports DNS. Otherwise displays IP
    static public String getHostName(String ip){
        try {
            InetAddress ipInet = Inet4Address.getByName(ip);
            String hostName = ipInet.getHostName();

            Log.d("Hostname Address", "Hostname: " + hostName);

            return hostName;
        }
        catch (Exception e)
        {
            Log.d("Hostname", "Hostname not found: " + ip);
        }
        return "Hostname Error";
    };

    /*
    * 0 = unreachable
    * 1 = good
    * 2 = decent
    * 3 = bad
    */
    static public int checkPingConnection(String ipv4){
        try {
            InetAddress ip = Inet4Address.getByName(ipv4);
            if (ip.isReachable(600)) { return 1; }
            else if (ip.isReachable(1300)) { return 2; }
           // else if (ip.isReachable(1300)) { return 3; }
            else { return 0; }
        }
        catch (Exception e) { Log.wtf("CheckPing", "Error"); return 0; }
    };
}
