﻿// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Specialized;
using System.Xml.Linq;
using TerminalLibrary;

Console.WriteLine("Hello, World!");

PointOfSaleTerminal terminal = new PointOfSaleTerminal();

terminal.SetPricing("A", volume: 1, price: 1.25);
terminal.SetPricing("A", volume: 3, price: 3.00);
terminal.SetPricing("B", volume: 1, price: 4.25);
terminal.SetPricing("C", volume: 1, price: 1.00);
terminal.SetPricing("C", volume: 6, price: 5.00);
terminal.SetPricing("D", volume: 1, price: 0.75);
//terminal.Scan("A");
//terminal.Scan("C");

//Scan these items in this order: AAAABCDAAA; Verify the total price is $13.25.
//Scan these items in this order: CCCCCCC; Verify the total price is $6.00.
//Scan these items in this order: ABCD; Verify the total price is $7.25

//terminal.ScanSplitted("AAAABCDAAA");
//terminal.ScanSplitted("CCCCCCC");
terminal.ScanSplitted("ABCD");

//terminal.ScanSplitted("AAAAAAA");

//... etc.

double result = terminal.CalculateTotal();

Console.WriteLine("result:");
Console.WriteLine(result);
Console.ReadLine();