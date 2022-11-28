package fr.unice.polytech;

import fr.unice.polytech.server.ArrayOfstring;
import fr.unice.polytech.server.ILetsGoBiking;
import fr.unice.polytech.server.LetsGoBiking;

import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;
import java.util.logging.Logger;

public class Application {

    static Logger logger = Logger.getLogger(Application.class.getName());

    public static void main(String[] args) {
        LetsGoBiking letsGoBiking = new LetsGoBiking();
        ILetsGoBiking proxy = letsGoBiking.getBasicHttpBindingILetsGoBiking();

        String originAddress = askForAddress("origin");
        String destinationAddress = askForAddress("destination");

        //2 Place de Paris, 69009 Lyon
        //1 Bd Jard. Zoologique, 13004 Marseille

        logger.info("Calling the web service...");
        ArrayOfstring arrayOfstring = proxy.getItinerary(originAddress, destinationAddress);
        logger.info("Web service called.");
        List<String> instructions = new ArrayList<>(arrayOfstring.getString());
        logger.info("Parsing the instructions...");
        parseInstructions(instructions);

    }

    private static void parseInstructions(List<String> instructions) {
        for (String instruction : instructions) {
            String[] split = instruction.split("§");
            String direction = split[0];
            String distance = split[1];
            System.out.println("Direction: " + direction + " - Distance: " + distance + "m");
        }
    }

    private static String askForAddress(String askedAddress) {
        String address = "";
        Scanner scanner = new Scanner(System.in);
        while (address.isEmpty()) {
            System.out.print("Please enter the " + askedAddress + " address: ");
            address = scanner.nextLine();
        }
        return address;
    }
}
