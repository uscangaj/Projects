package com.example.network_scanscreen;

import android.util.Log;

import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;

public class Category {
    /******************************************************
                          Big Daddy
    *******************************************************/
    static private int size = 255;                   // size of the array
    static private int amount_of_computers = 0;    // how many computers are in the array

    static public Computer[] devices = new Computer[size]; //Originally Static
    static public Category[] all_categories;
    static public int router;
    /** index: 0 = Phones/Tablets  1 = PCs/Servers  2 = SecCam  3 = Misc  4 = Router **/


    /******************************************************
                         SUB CLASS PARTS
    *******************************************************/
    protected int indexFront;
    protected int indexBack;   // Is what gets incremented
    public int subCatID;
    public int sizeSub() { return indexBack - indexFront;}

    Category(int multiplier){ indexFront = indexBack = multiplier * 30; subCatID = multiplier; };

    static void createSubCategories() {
        int categoryAmt = 5; all_categories = new Category[categoryAmt];
        for (int i = 0; i < categoryAmt; i++) { all_categories[i] = new Category(i); }
    };

    /*public void DisplayDevices(Card_java card_javas)
    {
        for (int i = 0; i < sizeSub; i++)
        {
            card_javas.add(new Card_java(R.drawable.wifi_green, "Network_Name1", "MAC:   b4:0a:6f:94:c3:p0","     IP:   192.168.2.34"));
        }
    };*/

    // To insert into Sub Category: NOTE: takes Computer instead of String
    public void insertDevices(Computer computer) {
        int i = 0;
        boolean exists = false;
        while (i < sizeSub() && !exists && devices[i + indexFront] != null)
        {
            // if match
            if (computer.get_ip() == devices[i + indexFront].get_ip()) { exists = true; break; }
            i ++ ;
        }
        if (!exists) { devices[indexBack] = computer;
            if (computer.get_ip() == "192.168.0.1") { router = indexBack; }
            indexBack++; }
        Log.d("Success Insert", "device: " + computer.get_ip());

    };

    /** index: 0 = Phones/Tablets  1 = PCs/Servers  2 = SecCam  3 = Misc  4 = Router **/
    static public void sortDevices(Computer computer) {
        // Add ports for prior
        //if (computer.ports[80] || computer.ports[443] || computer.ports[8080]) {
          //  all_categories[1].insertDevices(computer);
       // }
        /**else if (computer.ports[] || computer.ports[443] || computer.ports[8080]) {
            all_categories[1].insertDevices(computer);
        }*/
        //else {
            switch (computer.get_manufacterer_ID()) {
                case "Apple, Inc.": { all_categories[0].insertDevices(computer); break; }
                case "XEROX CORPORATION": { all_categories[1].insertDevices(computer); break; }
                case "Cisco Systems, Inc": {
                    all_categories[4].insertDevices(computer); break;
                }
                case "Technicolor CH USA Inc.": { all_categories[4].insertDevices(computer); break; }
                case "Roku, Inc.": {
                    all_categories[3].insertDevices(computer); break;
                }
                case "Micro-Star INTL CO., LTD.": {
                    all_categories[1].insertDevices(computer); break;
                }
                default: {
                    Log.wtf("Categorize", "what in the hell? " + computer.get_manufacterer_ID());
                    all_categories[1].insertDevices(computer);
                }
            }
       // }
    };


    // Check if device exists already,                  // DOES NOT SAVE INFO YET 10/19
    // if so: update whatever information.
    // Else: add new information
    static public void insertDevices(String ip){
        Computer insertMe = new Computer(ip);

        int i = 0;
        boolean exists = false;
        while (i < amount_of_computers && !exists && devices[i] != null)
        {
            // if match
            if (insertMe.get_ip() == devices[i].get_ip()) { exists = true; break; }
            i ++ ;
        }
        if (!exists) { amount_of_computers++; }

        if (size < amount_of_computers)
        {
            Computer[] newDevices = new Computer[size*2];
            size = size * 2;

            int j = 0;
            while (j < size)
            {
                newDevices[j] = devices[j];
                j++;
            }
            devices = newDevices;
            newDevices = null;
        }
        sortDevices(insertMe);
    };


    public void deleteDevices(){
        //Computer deleteMe = createComputer(ip);

        //int i = 0;
        //boolean exists = false;

        /*
        while (i < amount_of_computers) {

        }
         */

    };
}
