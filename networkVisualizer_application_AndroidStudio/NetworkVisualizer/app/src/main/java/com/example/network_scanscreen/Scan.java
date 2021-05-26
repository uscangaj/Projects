package com.example.network_scanscreen;
import android.util.Log;

import java.net.SocketException;
import java.net.UnknownHostException;
import java.io.*;
import java.net.*;
import java.util.*;
import static java.lang.System.out;
import static java.lang.System.setOut;
import java.io.*;
import java.net.*;
import java.lang.StringBuilder;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.locks.ReentrantLock;


public class Scan {


    private int counter=0;
    private int step =10;
    // public int i=0;

    //list all ips according to the subnet mask
    public ArrayList<String> computerlist = new ArrayList<>();


    /*
    The following method increments the IP address by 1.
    Example:
        - Input: 192.16.2.15
        - Output: 192.16.2.16
     */
    public synchronized String incrementIP(String ip) {
        Log.d("incrementIP","IP Address: " + ip);
        final String [] bits = ip.split("\\.");
        if(bits.length != 4 ) {
            Log.wtf("Error", "The IP does not contain 4 octets");
        }
        for(int i = bits.length - 1; i >= 0; i--){
            int finalBit = Integer.parseInt(bits[i]);
            if(finalBit < 255){
                bits[i] = String.valueOf(finalBit + 1);
                bits[i] = String.valueOf(finalBit + 1);
                for(int j = i + 1; j < 4; j++) {
                    bits[j] = "0";
                }
                break;
            }
        }
        String finalIp = bits[0].concat(".").concat(bits[1]).concat(".").concat(bits[2]).concat(".")
                .concat(bits[3]);
        Log.d("address", "The address is  " + finalIp);
        return finalIp;
    }

    //lets try this we create a list of all the possible ips. makes it easier for threading
    public void listip(String ip) {
        System.out.println("we are inside the litip");
        //left it to 100 for debugging purposes but it should probably be 256
        for(int i=0;i<255;i++){
            computerlist.add(ip);
            ip=incrementIP(ip);
        }
        for(String s: computerlist){
            System.out.println(s);
        }
    }

    public String resetLastOctet(String ip) {
        final String [] octets = ip.split("\\.");
        if(octets.length != 4 ){
            System.out.println("Error The IP does not contain 4 octets"); }
        octets[3] = "0";
        String resetIP = octets[0].concat(".").concat(octets[1]).concat(".").concat(octets[2]).concat(".").concat(octets[3]);
        return resetIP;
    }

    void scan() {
        int i = 0;

        boolean flag = true;
        while (flag) {
            synchronized (this) { i = counter; counter = counter + step; }
            if (i < computerlist.size()) {
                int istep = i;
                while (i < (istep + step) && (i < computerlist.size())) {
                    String ipv4 = computerlist.get(i);
                    try {
                        // open socket
                        InetAddress ip = Inet4Address.getByName(ipv4);
                        if (ip.isReachable(1300)) {
                            // Add try to create specific computer and add port
                            synchronized (this) { Category.insertDevices(ipv4); }
                           Log.d("Success IP: ", ipv4 + " established a connection.");
                        } else {
                            Log.wtf("Ping Timeout", "Ping Timeout: " + ipv4);
                        }
                    } catch (Exception ex) {
                        ex.printStackTrace();
                        Log.wtf("Failure IP: ", ipv4 + " timed out! ");
                    }
                    i++;
                }
            } else {

                flag = false;
            }
        }
    }
};






