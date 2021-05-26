package com.example.network_scanscreen;

import android.content.Context;
import android.net.wifi.WifiManager;

import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.NetworkInterface;
import java.net.Socket;
import java.net.SocketAddress;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.util.Collections;
import java.util.List;

public class Computer {
    static public final int NUM_OF_PORTS = 65535;

    // If true, then the device doesn't disappear
    private boolean favorite;       public void set_favorite() { if (favorite) { favorite = false; }
    else { favorite = true; } };
    // Checks if the connection is alive or not;
    // If not, AND favorited, shoiuld appear greyed out
    // else, delete computer object from memory
    public boolean alive;


    private static String localmac;   public static String getLocalmac() { return localmac; }

    private String nickname;        public String get_nickname() { return nickname; }
    private String hostname;
    private String manufacterer_ID; public String get_manufacterer_ID() { return manufacterer_ID; }
    private String ipv4;            public String get_ip() { return ipv4; };
    private String mac;             public String get_mac() { return mac; };

    public String get_hostname() { if (hostname != ipv4) { return hostname; } return "Error"; }

    // if port[index] == true; then comp has port 'index' open
    public boolean[] ports = new boolean[NUM_OF_PORTS];

    public String openports ="";


    Computer(String ip){
        nickname = "";
        ipv4 = ip;
        mac = Tools.getMacAddress(ip);
        hostname = Tools.getHostName(ip);
        manufacterer_ID = Tools.ManufacturerID(mac);
        Tools.scanPorts(this);
    };

    public int getConnectionQuality() { return Tools.checkPingConnection(ipv4); }


    //TODO
    //public static void tutorial(string)

//    this functions finds the local address of the device and sets it in the static variable localmac, finding other mac address on a network seems like it requires other
//     custom libraries (not apart of the standard java library), so its something we can look more into -seda kai
/*
    public static void findLocalmac() {

        StringBuilder sb = new StringBuilder();
        String macd="";

        try {
            List<NetworkInterface> all = Collections.list(NetworkInterface.getNetworkInterfaces());
            for (NetworkInterface nif : all) {
                if (!nif.getName().equalsIgnoreCase("wlan0")) continue;

                byte[] macBytes = nif.getHardwareAddress();
                if (macBytes == null) {
                    macd= "";
                }

                StringBuilder res1 = new StringBuilder();
                for (byte b : macBytes) {
                    res1.append(String.format("%02X:",b));
                }

                if (res1.length() > 0) {
                    res1.deleteCharAt(res1.length() - 1);
                }
                macd=res1.toString();
            }
        } catch (Exception ex) {
        }


        localmac=macd;
    }*/


    //TODO
    //change hostname to new name entered by user
    //Determine how the newName is acquired
    public String Change_name(String newName)
    {
        //allowed string length for nickname
        //int strlen = 16;

        //if (newName.length > 16)
        //Snackbar.make(view[0], "", Snackbar.LENGTH_LONG)
        //        .setAction("Action", null).show();

        //TODO
        //Security checks (string length, only numbers and letters)
        if (nickname != newName)
            return nickname = newName;

        //Snackbar.make(view[0], Nickname has been changed to: " + newName, Snackbar.LENGTH_LONG)
        //        .setAction("Action", null).show();

        //System.out.println("Nickname has been changed to: " + newName);

        return nickname;
    };

    //TODO
    //public void Change_category()

    //TODO
    //public void displayInfo(int code_for_how_much_information_to_display_,_yo)

}
